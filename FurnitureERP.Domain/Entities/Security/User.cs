namespace FurnitureERP.Domain.Entities.Security;

public class User
{
    public int Id { get; private set; }

    public string Username { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string FullName { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    private User() { } // EF

    public User(string username, string passwordHash, string fullName)
    {
        Username = username;
        PasswordHash = passwordHash;
        FullName = fullName;
    }

    public void Deactivate() => IsActive = false;
}
