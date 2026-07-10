namespace FurnitureERP.Domain.Entities.Security;

public class UserRole
{
    public int UserId { get; private set; }
    public int RoleId { get; private set; }

    // 🔥 دول المهمين
    public User User { get; private set; } = null!;
    public Role Role { get; private set; } = null!;

    private UserRole() { }

    public UserRole(int userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}
