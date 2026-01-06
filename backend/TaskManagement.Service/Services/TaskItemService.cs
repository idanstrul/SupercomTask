using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;
using TaskManagement.Service.Exceptions;

namespace TaskManagement.Service.Services;

public sealed class TaskItemService
{
    private readonly ITaskItemRepository _tasks;

    public TaskItemService(ITaskItemRepository tasks)
    {
        _tasks = tasks;
    }

    public async Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default)
    {
        return await _tasks.GetAllWithUserAsync(ct);
    }

    public async Task<TaskItem> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var task = await _tasks.GetByIdWithUserAsync(id, ct);
        if (task is null)
            throw new NotFoundException($"Task item with ID {id} not found.");

        return task;
    }

    public async Task<TaskItem> CreateAsync(
        string title,
        string description,
        DateTime dueDate,
        Priority priority,
        int userId,
        CancellationToken ct = default)
    {
        if (!await _tasks.UserExistsAsync(userId, ct))
            throw new NotFoundException($"User with ID {userId} not found.");

        var task = new TaskItem
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            Priority = priority,
            UserId = userId
        };

        await _tasks.AddAsync(task, ct);
        await _tasks.SaveChangesAsync(ct);

        // Load user for response mapping
        return await _tasks.GetByIdWithUserAsync(task.Id, ct)
               ?? throw new InvalidOperationException("Failed to load created task.");
    }

    public async Task UpdateAsync(
        int id,
        string title,
        string description,
        DateTime dueDate,
        Priority priority,
        CancellationToken ct = default)
    {
        var task = await _tasks.GetByIdAsync(id, ct);
        if (task is null)
            throw new NotFoundException($"Task item with ID {id} not found.");

        task.Title = title;
        task.Description = description;
        task.DueDate = dueDate;
        task.Priority = priority;

        await _tasks.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var task = await _tasks.GetByIdAsync(id, ct);
        if (task is null)
            throw new NotFoundException($"Task item with ID {id} not found.");

        _tasks.Remove(task);
        await _tasks.SaveChangesAsync(ct);
    }
}
