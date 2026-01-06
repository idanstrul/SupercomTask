using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.DTOs;
using TaskManagement.Core.Models;
using TaskManagement.Infrastructure;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemsController : ControllerBase
{
    private readonly TaskManagementContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TaskItemsController> _logger;

    public TaskItemsController(
        TaskManagementContext context,
        IMapper mapper,
        ILogger<TaskItemsController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Get all task items with user details
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTaskItems()
    {
        var taskItems = await _context.TaskItems
            .Include(t => t.User)
            .ToListAsync();

        var taskItemDtos = _mapper.Map<IEnumerable<TaskItemDto>>(taskItems);
        return Ok(taskItemDtos);
    }

    /// <summary>
    /// Get a task item by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemDto>> GetTaskItem(int id)
    {
        var taskItem = await _context.TaskItems
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (taskItem == null)
        {
            return NotFound(new { message = $"Task item with ID {id} not found." });
        }

        var taskItemDto = _mapper.Map<TaskItemDto>(taskItem);
        return Ok(taskItemDto);
    }

    /// <summary>
    /// Create a new task item
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> CreateTaskItem(CreateTaskItemDto createTaskItemDto)
    {
        // Verify user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == createTaskItemDto.UserId);
        if (!userExists)
        {
            return BadRequest(new { message = $"User with ID {createTaskItemDto.UserId} not found." });
        }

        var taskItem = _mapper.Map<TaskItem>(createTaskItemDto);
        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        // Load user for response
        await _context.Entry(taskItem).Reference(t => t.User).LoadAsync();

        var taskItemDto = _mapper.Map<TaskItemDto>(taskItem);
        return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.Id }, taskItemDto);
    }

    /// <summary>
    /// Update an existing task item
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTaskItem(int id, UpdateTaskItemDto updateTaskItemDto)
    {
        var taskItem = await _context.TaskItems.FindAsync(id);
        if (taskItem == null)
        {
            return NotFound(new { message = $"Task item with ID {id} not found." });
        }

        _mapper.Map(updateTaskItemDto, taskItem);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TaskItemExists(id))
            {
                return NotFound(new { message = $"Task item with ID {id} not found." });
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a task item
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskItem(int id)
    {
        var taskItem = await _context.TaskItems.FindAsync(id);
        if (taskItem == null)
        {
            return NotFound(new { message = $"Task item with ID {id} not found." });
        }

        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> TaskItemExists(int id)
    {
        return await _context.TaskItems.AnyAsync(e => e.Id == id);
    }
}
