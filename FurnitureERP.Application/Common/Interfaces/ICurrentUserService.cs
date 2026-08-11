using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Common.Interfaces;


public interface ICurrentUserService
{
    int? UserId { get; }

    string? UserName { get; }

    IReadOnlyList<string> Permissions { get; }
}
