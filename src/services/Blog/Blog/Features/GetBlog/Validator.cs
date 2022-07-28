using Blog.Libraries.FluentValidation;
using FluentValidation;

namespace Blog.Features.GetBlog;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).MustBeLowercaseOrDigit();
        RuleFor(x => x.Organization).NotNull();
        RuleFor(x => x.Organization).NotEmpty();
        RuleFor(x => x.Organization).MustBeLowercaseOrDigit();
    }
}