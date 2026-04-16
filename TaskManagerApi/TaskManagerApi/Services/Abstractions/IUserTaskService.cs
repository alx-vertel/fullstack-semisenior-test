using TaskManagerApi.Dtos.UserTaskDto;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services.Abstractions
{
    public interface IUserTaskService
    {
        Task<IEnumerable<ReadUserTaskDto>> GetAllUserTasksAsync(UserTaskStatus? status);
        Task<ReadUserTaskDto?> GetUserTaskByIdAsync(int id);
        Task<ReadUserTaskDto> CreateUserTaskAsync(CreateUserTaskDto taskDto);
        Task<bool> UpdateUserTaskStatusAsync(int id, UserTaskStatus newStatus);
        Task<IEnumerable<ReadUserTaskDto>> GetUserTasksByPriorityAsync(string priority);
    }
}
