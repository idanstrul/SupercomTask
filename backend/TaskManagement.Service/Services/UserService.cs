using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;
using TaskManagement.Service.Exceptions;

namespace TaskManagement.Service.Services;

public sealed class UserService
{
    private readonly IUserRepository _users;

    public UserService(IUserRepository users)
    {
        _users = users;
    }

    public async Task<List<User>> GetAllAsync(CancellationToken ct = default)
    {
        return await _users.GetAllAsync(ct);
    }

    public async Task<User> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(id, ct);
        if (user is null)
            throw new NotFoundException($"User with ID {id} not found.");

        return user;
    }

    public async Task<User> CreateAsync(
        string fullName,
        string telephone,
        string email,
        CancellationToken ct = default)
    {
        if (await _users.EmailExistsAsync(email, excludeUserId: null, ct))
            throw new ConflictException($"User with email {email} already exists.");

        var user = new User
        {
            FullName = fullName,
            Telephone = telephone,
            Email = email
        };

        await _users.AddAsync(user, ct);
        await _users.SaveChangesAsync(ct);

        return user;
    }

    public async Task UpdateAsync(
        int id,
        string fullName,
        string telephone,
        string email,
        CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(id, ct);
        if (user is null)
            throw new NotFoundException($"User with ID {id} not found.");

        if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            if (await _users.EmailExistsAsync(email, excludeUserId: id, ct))
                throw new ConflictException($"User with email {email} already exists.");
        }

        user.FullName = fullName;
        user.Telephone = telephone;
        user.Email = email;

        await _users.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(id, ct);
        if (user is null)
            throw new NotFoundException($"User with ID {id} not found.");

        _users.Remove(user);
        await _users.SaveChangesAsync(ct);
    }
}
