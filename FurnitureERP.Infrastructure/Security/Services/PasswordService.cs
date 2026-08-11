using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;

namespace FurnitureERP.Infrastructure.Security.Services;
public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var result = _hasher.VerifyHashedPassword(
            null!,
            hashedPassword,
            password);

        return result == PasswordVerificationResult.Success;
    }
}