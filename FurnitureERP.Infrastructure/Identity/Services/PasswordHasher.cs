using BCrypt.Net;
using FurnitureERP.Application.Common.Interfaces;

namespace FurnitureERP.Infrastructure.Identity.Services;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string hash, string password)
        => BCrypt.Net.BCrypt.Verify(password, hash);
}
