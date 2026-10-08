using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ATBS.Models;
using ATBS.Attributes;

namespace ATBS.Services;

public class ValidationService : IValidationService
{
    public List<FieldConstraint> DescribeConstraints<T>()
    {
        return typeof(T)
            .GetProperties()
            .Select(property => new FieldConstraint
            {
                FieldName = property.Name,
                Type = property.PropertyType.Name,
                Constraints = DescribeConstraints(property)
            })
            .Where(constraint => constraint.Constraints.Count > 0)
            .ToList();
    }
    
    private static List<string> DescribeConstraints(PropertyInfo property)
    {
        return property
            .GetCustomAttributes<ValidationAttribute>(inherit: true)
            .Select(Describe)
            .ToList();
    }
    
    private static string Describe(ValidationAttribute attribute) => attribute switch
    {
        RequiredAttribute => "Required",
        StringLengthAttribute length => $"Max length {length.MaximumLength}",
        NotInThePastAttribute => "Allowed Range (today → future)",
        ValidCabinPricesAttribute => "Requires all Cabin Classes to be defined and have a defined price",
        _ => attribute.GetType().Name.Replace("Attribute", string.Empty)
    };
    
    public List<string> Validate(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        // TryValidateObject(validateAllProperties: true) runs every attribute on every property.
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);

        return results
            .Select(result => result.ErrorMessage ?? "Invalid value.")
            .ToList();
    }
}