using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.DTOs;
using TaskManagement.Core.Models;
using TaskManagement.Infrastructure;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly TaskManagementContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        TaskManagementContext context,
        IMapper mapper,
        ILogger<UsersController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        return Ok(userDtos);
    }

    /// <summary>
    /// Get a user by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }

        var userDto = _mapper.Map<UserDto>(user);
        return Ok(userDto);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        // Check if email already exists
        var emailExists = await _context.Users.AnyAsync(u => u.Email == createUserDto.Email);
        if (emailExists)
        {
            return Conflict(new { message = $"User with email {createUserDto.Email} already exists." });
        }

        var user = _mapper.Map<User>(createUserDto);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = _mapper.Map<UserDto>(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }

        // Check if email is being changed and if new email already exists
        if (user.Email != updateUserDto.Email)
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == updateUserDto.Email && u.Id != id);
            if (emailExists)
            {
                return Conflict(new { message = $"User with email {updateUserDto.Email} already exists." });
            }
        }

        _mapper.Map(updateUserDto, user);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await UserExists(id))
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a user (cascade delete will remove associated task items)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users
            .Include(u => u.TaskItems)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found." });
        }

        // Cascade delete is configured in DbContext, so task items will be deleted automatically
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> UserExists(int id)
    {
        return await _context.Users.AnyAsync(e => e.Id == id);
    }
}
