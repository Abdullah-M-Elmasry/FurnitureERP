using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Security.Interfaces;
using FurnitureERP.Domain.Entities.Security;

namespace FurnitureERP.Application.Security.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly ICurrentUserService _currentUser;

    public AuthService(
        IAuthRepository repo,
        IPasswordHasher hasher,
        ICurrentUserService currentUser)  // اضفنا CurrentUserService
    {
        _repo = repo;
        _hasher = hasher;
        _currentUser = currentUser;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username);

        if (user == null || !user.IsActive)
            return null;

        if (!_hasher.Verify(user.PasswordHash, password))
            return null;

        user.LastLoginAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync();

        // =========================
        // ضبط بيانات المستخدم الحالي + صلاحياته
        // =========================
        var permissions = await _repo.GetUserPermissionsAsync(user.Id);
        _currentUser.SetUser(user.Id, user.Username, permissions);

        return user;
    }
}
