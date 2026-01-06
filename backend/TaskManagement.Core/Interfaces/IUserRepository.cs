using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken ct = default);
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<bool> EmailExistsAsync(string email, int? excludeUserId = null, CancellationToken ct = default);

    Task AddAsync(User user, CancellationToken ct = default);
    void Remove(User user);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
