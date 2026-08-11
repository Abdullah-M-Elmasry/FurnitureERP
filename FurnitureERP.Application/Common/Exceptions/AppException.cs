using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Common.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message)
            : base(message)
        {
        }
    }
}
