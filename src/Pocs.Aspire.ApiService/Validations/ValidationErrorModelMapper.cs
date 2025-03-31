using Pocs.Aspire.Business.Users;

namespace Pocs.Aspire.ApiService.Validations;

public static class ValidationErrorModelMapper
{
    internal static ValidationErrorModel ToResponse(this FieldError fieldError) =>
           new(fieldError.Field, fieldError.Message);
}
