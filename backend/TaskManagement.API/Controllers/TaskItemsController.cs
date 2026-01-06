using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.DTOs;
using TaskManagement.Service.Services;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemsController : ControllerBase
{
    private readonly TaskItemService _service;
    private readonly IMapper _mapper;

    public TaskItemsController(TaskItemService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTaskItems(CancellationToken ct)
    {
        var tasks = await _service.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<TaskItemDto>>(tasks));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItemDto>> GetTaskItem(int id, CancellationToken ct)
    {
        var task = await _service.GetByIdAsync(id, ct);
        return Ok(_mapper.Map<TaskItemDto>(task));
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> CreateTaskItem(
        CreateTaskItemDto dto,
        CancellationToken ct)
    {
        var task = await _service.CreateAsync(
            dto.Title,
            dto.Description,
            dto.DueDate,
            dto.Priority,
            dto.UserId,
            ct);

        var taskDto = _mapper.Map<TaskItemDto>(task);

        return CreatedAtAction(
            nameof(GetTaskItem),
            new { id = taskDto.Id },
            taskDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTaskItem(
        int id,
        UpdateTaskItemDto dto,
        CancellationToken ct)
    {
        await _service.UpdateAsync(
            id,
            dto.Title,
            dto.Description,
            dto.DueDate,
            dto.Priority,
            ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTaskItem(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}
