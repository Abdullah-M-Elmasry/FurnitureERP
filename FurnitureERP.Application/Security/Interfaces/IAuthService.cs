using FurnitureERP.Domain.Entities.Security;

namespace FurnitureERP.Application.Security.Interfaces;

public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
}
