using System.ComponentModel.DataAnnotations;

namespace ATBS.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NotInThePastAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateTime date)
        {
            return true;
        }

        return date.Date >= DateTime.Today;
    }

    public override string FormatErrorMessage(string name) =>
        $"{name} must be today or a future date.";
}