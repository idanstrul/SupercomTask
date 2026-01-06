using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.DTOs;
using TaskManagement.Service.Services;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _service;
    private readonly IMapper _mapper;

    public UsersController(UserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(CancellationToken ct)
    {
        var users = await _service.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id, CancellationToken ct)
    {
        var user = await _service.GetByIdAsync(id, ct);
        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(
        CreateUserDto dto,
        CancellationToken ct)
    {
        var user = await _service.CreateAsync(
            dto.FullName,
            dto.Telephone,
            dto.Email,
            ct);

        var userDto = _mapper.Map<UserDto>(user);

        return CreatedAtAction(
            nameof(GetUser),
            new { id = userDto.Id },
            userDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(
        int id,
        UpdateUserDto dto,
        CancellationToken ct)
    {
        await _service.UpdateAsync(
            id,
            dto.FullName,
            dto.Telephone,
            dto.Email,
            ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}
