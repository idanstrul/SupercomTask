using TaskManagement.Core.Models;

namespace TaskManagement.API.DTOs;

public class TaskItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public int UserId { get; set; }
    public UserDto? User { get; set; }
}
