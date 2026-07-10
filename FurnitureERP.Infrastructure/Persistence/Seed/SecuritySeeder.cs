using FurnitureERP.Domain.Entities.Security;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FurnitureERP.Application.Common.Interfaces;

namespace FurnitureERP.Infrastructure.Persistence.Seed;

public static class SecuritySeeder
{
    public static async Task SeedAsync(AppDbContext db, IPasswordHasher hasher)
    {
       // Console.WriteLine("🔥 Security Seeder Running...");

        await db.Database.MigrateAsync();

        // =========================
        // Permissions
        // =========================
        var permissionNames = new[]
        {
            "Users.View",
            "Users.Create",
            "Users.Edit",
            "Users.Delete"
        };

        foreach (var name in permissionNames)
        {
            var exists = await db.Permissions
                .AnyAsync(p => p.Name == name);

            if (!exists)
            {
                await db.Permissions.AddAsync(new Permission(name));
            }
        }

        await db.SaveChangesAsync();

        // =========================
        // Admin Role
        // =========================
        var adminRole = await db.Roles
            .FirstOrDefaultAsync(r => r.Name == "Admin");

        if (adminRole == null)
        {
            adminRole = new Role("Admin");
            await db.Roles.AddAsync(adminRole);
            await db.SaveChangesAsync();
        }

        // =========================
        // Admin User
        // =========================
        var adminUser = await db.Users
            .FirstOrDefaultAsync(u => u.Username == "admin");

        if (adminUser == null)
        {
            adminUser = new User(
                "admin",
                hasher.Hash("123456"),
                "System Admin"
            );

            await db.Users.AddAsync(adminUser);
            await db.SaveChangesAsync();
        }

        // =========================
        // UserRole
        // =========================
        var userRoleExists = await db.UserRoles
            .AnyAsync(ur => ur.UserId == adminUser.Id &&
                            ur.RoleId == adminRole.Id);

        if (!userRoleExists)
        {
            db.UserRoles.Add(new UserRole(adminUser.Id, adminRole.Id));
            await db.SaveChangesAsync();
        }

        // =========================
        // RolePermissions
        // =========================
        var permissions = await db.Permissions.ToListAsync();

        foreach (var permission in permissions)
        {
            var rolePermissionExists = await db.RolePermissions
                .AnyAsync(rp => rp.RoleId == adminRole.Id &&
                                rp.PermissionId == permission.Id);

            if (!rolePermissionExists)
            {
                db.RolePermissions.Add(
                    new RolePermission(adminRole.Id, permission.Id));
            }
        }

        await db.SaveChangesAsync();

        Console.WriteLine("✅ Security Seeder Completed.");
    }
}
