using System;
using System.Collections.Generic;
using System.Text;
using FurnitureERP.Application.Common.Interfaces;

namespace FurnitureERP.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string? UserName { get; private set; }
        public int? UserId { get; private set; }
        public List<string> Permissions { get; private set; } = new List<string>();

        public void SetUser(int userId, string userName, List<string> permissions)
        {
            UserId = userId;
            UserName = userName;
            Permissions = permissions;
        }
    }
}
