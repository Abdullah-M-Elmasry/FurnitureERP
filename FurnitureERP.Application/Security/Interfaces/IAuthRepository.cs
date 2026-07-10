using FurnitureERP.Domain.Entities.Security;

namespace FurnitureERP.Application.Security.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<List<string>> GetUserPermissionsAsync(int userId);
    Task SaveChangesAsync();
}
