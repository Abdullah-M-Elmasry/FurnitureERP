using FurnitureERP.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Infrastructure.Identity.Services
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly ICurrentUserService _currentUser;

        public PermissionChecker(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public bool HasPermission(string permission)
        {
            if (_currentUser.Permissions == null)
                return false;

            return _currentUser.Permissions.Contains(permission);
        }
    }
}
