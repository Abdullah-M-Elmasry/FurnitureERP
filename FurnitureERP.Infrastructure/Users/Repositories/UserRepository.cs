using FurnitureERP.Application.Users.Interfaces;
using FurnitureERP.Domain.Entities.Security;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Users.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> UsernameExists(string username)
    {
        return await _context.Users
            .AnyAsync(x => x.Username == username);
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }
}