using FluentValidation;
using MediatR;

namespace Blog;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);

        foreach (var validator in _validators)
        {
            var validationsResults = await validator.ValidateAsync(context, cancellationToken);
            var errors = validationsResults.Errors.Where(f => f != null).ToList();

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
        }

        return await next();
    }
}