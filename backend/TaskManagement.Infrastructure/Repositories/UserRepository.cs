using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;

namespace TaskManagement.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly TaskManagementContext _db;

    public UserRepository(TaskManagementContext db) => _db = db;

    public Task<List<User>> GetAllAsync(CancellationToken ct = default) =>
        _db.Users.AsNoTracking().ToListAsync(ct);

    public Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<bool> EmailExistsAsync(string email, int? excludeUserId = null, CancellationToken ct = default)
    {
        var q = _db.Users.AsQueryable().Where(u => u.Email == email);
        if (excludeUserId is not null)
            q = q.Where(u => u.Id != excludeUserId.Value);

        return q.AnyAsync(ct);
    }

    public Task AddAsync(User user, CancellationToken ct = default) =>
        _db.Users.AddAsync(user, ct).AsTask();

    public void Remove(User user) => _db.Users.Remove(user);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}
