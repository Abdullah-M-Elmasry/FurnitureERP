using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Security.DTOs
{
    public class RegisterRequestdDTO
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
    }
}
