using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TaskManagerApi.Data;
using TaskManagerApi.Dtos.UserTaskDto;
using TaskManagerApi.Models;
using TaskManagerApi.Services.Abstractions;

namespace TaskManagerApi.Services.Implementations
{
    public class UserTaskService(AppDbContext _context) : IUserTaskService
    {
        public async Task<ReadUserTaskDto> CreateUserTaskAsync(CreateUserTaskDto userTaskDto)
        {
            var task = new UserTask
            {
                Title = userTaskDto.Title,
                Description = userTaskDto.Description,
                UserId = userTaskDto.UserId,
                Status = UserTaskStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                AdditionalInfo = JsonSerializer.Serialize(userTaskDto.AdditionalInfo ?? new AdditionalInfoDto())
            };

            _context.UserTasks.Add(task);
            await _context.SaveChangesAsync();

            await _context.Entry(task).Reference(t => t.User).LoadAsync();
            return MapToReadDto(task);
        }

        public async Task<IEnumerable<ReadUserTaskDto>> GetAllUserTasksAsync(UserTaskStatus? status)
        {
            var query = _context.UserTasks
                .Include(t => t.User)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => MapToReadDto(t))
                .ToListAsync();
        }

        public async Task<ReadUserTaskDto?> GetUserTaskByIdAsync(int id)
        {
            var task = await _context.UserTasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return null;

            return MapToReadDto(task);
        }

        public async Task<IEnumerable<ReadUserTaskDto>> GetUserTasksByPriorityAsync(string priority)
        {
            var tasks = await _context.UserTasks
                 .FromSqlRaw("SELECT * FROM UserTasks WHERE JSON_VALUE(AdditionalInfo, '$.Priority') = {0}", priority)
                 .Include(t => t.User)
                 .ToListAsync();

            return tasks.Select(t => MapToReadDto(t));
        }

        public async Task<bool> UpdateUserTaskStatusAsync(int id, UserTaskStatus newStatus)
        {
            var task = await _context.UserTasks.FindAsync(id);
            if (task == null) return false;

            if (task.Status == UserTaskStatus.Pending && newStatus == UserTaskStatus.Done)
            {
                throw new InvalidOperationException("Can't change from 'Pending' to 'Done' directly");
            }

            task.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        private static ReadUserTaskDto MapToReadDto(UserTask userTask) => new()
        {
            Id = userTask.Id,
            Title = userTask.Title,
            Description = userTask.Description,
            Status = userTask.Status.ToString(),
            CreatedAt = userTask.CreatedAt,
            UserId = userTask.UserId,
            UserName = userTask.User?.Name ?? "No User",
            AdditionalInfo = !string.IsNullOrEmpty(userTask.AdditionalInfo)
                ? JsonSerializer.Deserialize<AdditionalInfoDto>(userTask.AdditionalInfo)
                : new AdditionalInfoDto()
        };

    }
}
