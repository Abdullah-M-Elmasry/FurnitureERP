namespace FurnitureERP.Domain.Entities.Security;

public class Role
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;

    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

    private Role() { }

    public Role(string name)
    {
        Name = name;
    }
}
