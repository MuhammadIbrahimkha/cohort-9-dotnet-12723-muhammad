namespace TaskManagement.Application.DTOs.Tasks;

/// <summary>
/// Data required to update an existing task.
/// </summary>
public class UpdateTaskDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Status { get; set; }
    public required string Priority { get; set; }
    public int CategoryId { get; set; }
    public DateTime? DueDate { get; set; }
    public int AssignedToUserId { get; set; }
}