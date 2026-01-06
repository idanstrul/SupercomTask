using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface ITaskItemRepository
{
    Task<List<TaskItem>> GetAllWithUserAsync(CancellationToken ct = default);
    Task<TaskItem?> GetByIdWithUserAsync(int id, CancellationToken ct = default);
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default);

    Task AddAsync(TaskItem taskItem, CancellationToken ct = default);
    void Remove(TaskItem taskItem);

    Task<bool> UserExistsAsync(int userId, CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
