using FurnitureERP.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Common.Exceptions;

public class UnauthorizedExceptionApp : AppException
{
    public UnauthorizedExceptionApp(string message)
        : base(message)
    {
    }
}

