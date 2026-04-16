using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Dtos.UserTaskDto;
using TaskManagerApi.Models;
using TaskManagerApi.Services.Abstractions;

namespace TaskManagerApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class UserTasksController(IUserTaskService userTaskService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadUserTaskDto>>> GetUserTasks([FromQuery] UserTaskStatus? status)
        {
            var userTasks = await userTaskService.GetAllUserTasksAsync(status);
            return Ok(userTasks);
        }

        [HttpPost]
        public async Task<ActionResult<ReadUserTaskDto>> CreateUserTask([FromBody] CreateUserTaskDto taskDto)
        {
            try
            {
                var createdTask = await userTaskService.CreateUserTaskAsync(taskDto);
                return CreatedAtAction(nameof(GetUserTasks), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500, title: "Error creating the task");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateUserTaskStatus(int id, [FromBody] UpdateUserTaskStatusDto statusUpdate)
        {
            try
            {
                if (!Enum.TryParse<UserTaskStatus>(statusUpdate.Status, out var newStatus))
                {
                    return BadRequest("Invalid status.");
                }

                var success = await userTaskService.UpdateUserTaskStatusAsync(id, newStatus);

                if (!success) return NotFound($"Task with ID {id} not found.");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<ReadUserTaskDto>>> GetUserTasksByPriority(string priority)
        {
            var tasks = await userTaskService.GetUserTasksByPriorityAsync(priority);
            return Ok(tasks);
        }
    }
}
