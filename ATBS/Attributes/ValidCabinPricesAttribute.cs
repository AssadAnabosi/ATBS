using System.ComponentModel.DataAnnotations;
using ATBS.Models;

namespace ATBS.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ValidCabinPricesAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not Dictionary<CabinClass, float> cabinPrices)
        {
            return ValidationResult.Success;
        }

        foreach (CabinClass cabinClass in Enum.GetValues<CabinClass>())
        {
            if (!cabinPrices.TryGetValue(cabinClass, out float price))
            {
                return new ValidationResult($"{cabinClass} price is missing.");
            }

            if (price <= 0)
            {
                return new ValidationResult($"{cabinClass} price must be greater than 0.");
            }
        }

        return ValidationResult.Success;
    }
}