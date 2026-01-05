namespace TaskManagement.Core.Models;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; }
    public int UserId { get; set; }
    
    // Navigation property
    public User User { get; set; } = null!;
}
