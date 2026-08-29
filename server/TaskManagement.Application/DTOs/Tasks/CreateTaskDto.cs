namespace TaskManagement.Application.DTOs.Tasks;

/// <summary>
/// Data required to create a new task.
/// </summary>
public class CreateTaskDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string Priority { get; set; } = "Medium";
    public int CategoryId { get; set; }
    public DateTime? DueDate { get; set; }
    public int AssignedToUserId { get; set; }
}