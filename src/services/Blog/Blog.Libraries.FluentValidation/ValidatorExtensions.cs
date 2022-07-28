using Blog.Libraries.FluentValidation.Validators;
using FluentValidation;

namespace Blog.Libraries.FluentValidation
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> MustBeLowercaseOrDigit<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
            => ruleBuilder
                .SetValidator(new MustBeLowercaseOrDigitValidator<T, TProperty>())
                .WithMessage("Must be lowercase or digit.");
    }
}