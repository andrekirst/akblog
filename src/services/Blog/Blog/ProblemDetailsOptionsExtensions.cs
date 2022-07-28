using FluentValidation;
using Hellang.Middleware.ProblemDetails;

namespace Blog;

public static class ProblemDetailsOptionsExtensions
{
    public static void MapFluentValidationException(this ProblemDetailsOptions options) =>
        options.Map<ValidationException>((ctx, ex) =>
        {
            var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            var errors = ex.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => y.ErrorMessage).ToArray());

            return factory.CreateValidationProblemDetails(ctx, errors, StatusCodes.Status400BadRequest);
        });
}