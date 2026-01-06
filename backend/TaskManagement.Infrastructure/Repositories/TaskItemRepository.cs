using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;

namespace TaskManagement.Infrastructure.Repositories;

public sealed class TaskItemRepository : ITaskItemRepository
{
    private readonly TaskManagementContext _db;

    public TaskItemRepository(TaskManagementContext db)
    {
        _db = db;
    }

    public Task<List<TaskItem>> GetAllWithUserAsync(CancellationToken ct = default) =>
        _db.TaskItems
            .Include(t => t.User)
            .AsNoTracking()
            .ToListAsync(ct);

    public Task<TaskItem?> GetByIdWithUserAsync(int id, CancellationToken ct = default) =>
        _db.TaskItems
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.TaskItems.FirstOrDefaultAsync(t => t.Id == id, ct);

    public Task AddAsync(TaskItem taskItem, CancellationToken ct = default) =>
        _db.TaskItems.AddAsync(taskItem, ct).AsTask();

    public void Remove(TaskItem taskItem) =>
        _db.TaskItems.Remove(taskItem);

    public Task<bool> UserExistsAsync(int userId, CancellationToken ct = default) =>
        _db.Users.AnyAsync(u => u.Id == userId, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}
