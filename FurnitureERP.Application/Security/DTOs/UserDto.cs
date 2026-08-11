using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Security.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
    }
}
