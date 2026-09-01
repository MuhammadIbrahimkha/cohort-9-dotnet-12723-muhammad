using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs.Tasks;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers;

/// <summary>
/// Handles CRUD operations for tasks.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Retrieves all tasks.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAll()
    {
        var tasks = await _taskService.GetAllAsync();
        return Ok(tasks);
    }

    /// <summary>
    /// Retrieves a single task by its identifier.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemDto>> GetById(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task is null)
        {
            return NotFound(new { message = $"Task with id {id} was not found." });
        }
        return Ok(task);
    }

    /// <summary>
    /// Creates a new task.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> Create([FromBody] CreateTaskDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");
        if (userIdClaim is null || !int.TryParse(userIdClaim, out var createdByUserId))
        {
            return Unauthorized();
        }

        var created = await _taskService.CreateAsync(dto, createdByUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var updated = await _taskService.UpdateAsync(id, dto);
        if (!updated)
        {
            return NotFound(new { message = $"Task with id {id} was not found." });
        }
        return NoContent();
    }

    /// <summary>
    /// Deletes a task. Restricted to Admins.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Task with id {id} was not found." });
        }
        return NoContent();
    }
}