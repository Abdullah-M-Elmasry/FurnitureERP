namespace FurnitureERP.Domain.Entities.Security;

public class Permission
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;

    public ICollection<RolePermission> RolePermissions { get; private set; }
       = new List<RolePermission>();
    private Permission() { }

    public Permission(string name)
    {
        Name = name;
    }
}
