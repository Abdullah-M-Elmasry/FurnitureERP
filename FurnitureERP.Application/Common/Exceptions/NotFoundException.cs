namespace FurnitureERP.Application.Common.Exceptions;

public class NotFoundExceptionApp : AppException
{
    public NotFoundExceptionApp(string message)
        : base(message)
    {
    }
}