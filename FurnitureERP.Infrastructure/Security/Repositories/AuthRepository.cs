//using FurnitureERP.Application.Identity.Interfaces;
//using FurnitureERP.Domain.Entities.Security;
//using FurnitureERP.Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;

//namespace FurnitureERP.Infrastructure.Security.Repositories;

//public class AuthRepository : IAuthRepository
//{
//    private readonly IDbContextFactory<AppDbContext> _contextFactory;

//    public AuthRepository(IDbContextFactory<AppDbContext> contextFactory)
//    {
//        _contextFactory = contextFactory;
//    }

//    public async Task<User?> GetByUsernameAsync(string username)
//    {
//        using var db = _contextFactory.CreateDbContext();

//        return await db.Users
//            .Include(u => u.UserRoles)
//            .ThenInclude(ur => ur.Role)
//            .FirstOrDefaultAsync(x => x.Username == username);
//    }

//    public async Task<List<string>> GetUserPermissionsAsync(int userId)
//    {
//        using var db = _contextFactory.CreateDbContext();

//        return await db.UserRoles
//            .Where(ur => ur.UserId == userId)
//            .Join(db.RolePermissions,
//                  ur => ur.RoleId,
//                  rp => rp.RoleId,
//                  (ur, rp) => rp)
//            .Join(db.Permissions,
//                  rp => rp.PermissionId,
//                  p => p.Id,
//                  (rp, p) => p.Name)
//            .ToListAsync();
//    }

//    public async Task SaveChangesAsync()
//    {
//        using var db = _contextFactory.CreateDbContext();
//        await db.SaveChangesAsync();
//    }
//}