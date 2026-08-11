namespace FurnitureERP.Application.Common.Exceptions;

public class ConflictExceptionApp : AppException
{
    public ConflictExceptionApp(string message)
        : base(message)
    {
    }
}