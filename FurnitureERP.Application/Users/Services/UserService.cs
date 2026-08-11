using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Security.DTOs;
using FurnitureERP.Application.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository repo,
            IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task Register(RegisterRequestdDTO request)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Login(LoginRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
