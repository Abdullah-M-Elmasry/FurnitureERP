using System;
using System.Collections.Generic;
using System.Text;
using FurnitureERP.Application.Common.Interfaces;

namespace FurnitureERP.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public int? UserId { get; private set; }

        public string? UserName { get; private set; }

        public IReadOnlyList<string> Permissions
        {
            get;
            private set;
        } = [];

        public void SetUser(
            int userId,
            string userName,
            List<string> permissions)
        {
            UserId = userId;
            UserName = userName;
            Permissions = permissions;
        }
    }
}
