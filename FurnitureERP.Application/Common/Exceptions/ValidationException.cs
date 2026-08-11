namespace FurnitureERP.Application.Common.Exceptions;

public class ValidationExceptionApp : AppException
{
    public ValidationExceptionApp(string message)
        : base(message)
    {
    }
}