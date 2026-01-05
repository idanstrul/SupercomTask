namespace TaskManagement.Core.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // Navigation property
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}
