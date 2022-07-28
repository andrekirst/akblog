using FluentValidation;
using FluentValidation.Validators;

namespace Blog.Libraries.FluentValidation.Validators;

public interface IMustBeLowercaseOrDigitValidator : IPropertyValidator
{
}

public class MustBeLowercaseOrDigitValidator<T, TProperty> : PropertyValidator<T, TProperty>, IMustBeLowercaseOrDigitValidator
{
    public override bool IsValid(ValidationContext<T> context, TProperty value) => value is string stringValue && stringValue.All(c => char.IsLower(c) || char.IsDigit(c));

    public override string Name => "MustBeLowercaseValidator";
}