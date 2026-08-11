using FurnitureERP.Domain.Entities.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsername(string username);

        Task<User?> GetById(int id);

        Task<bool> UsernameExists(string username);

        Task Add(User user);
    }
}
