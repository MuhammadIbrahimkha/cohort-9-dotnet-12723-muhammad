namespace TaskManagement.Application.DTOs.Tasks;

/// <summary>
/// Represents a task returned to the client.
/// </summary>
public class TaskItemDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Status { get; set; }
    public required string Priority { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime? DueDate { get; set; }
    public int AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}