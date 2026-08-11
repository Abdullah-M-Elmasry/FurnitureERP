using FurnitureERP.Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}