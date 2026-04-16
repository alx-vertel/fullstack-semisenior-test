using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Dtos.UserTaskDto
{
    public class UpdateUserTaskStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
