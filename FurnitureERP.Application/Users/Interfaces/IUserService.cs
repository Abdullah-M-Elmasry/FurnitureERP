using FurnitureERP.Application.Security.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task Register(RegisterRequestdDTO request);

        Task<string> Login(LoginRequestDto request);
    }
}
