using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserName { get; }
    int? UserId { get; }
    List<string> Permissions { get; }

    void SetUser(int userId, string userName, List<string> permissions);
}
