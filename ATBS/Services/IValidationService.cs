using ATBS.Models;

namespace ATBS.Services;

public interface IValidationService
{
    List<FieldConstraint> DescribeConstraints<T>();

    List<string> Validate(object model);
}
