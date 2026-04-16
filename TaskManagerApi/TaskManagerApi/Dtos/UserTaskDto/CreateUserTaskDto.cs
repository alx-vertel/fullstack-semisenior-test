using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Dtos.UserTaskDto
{
    public class CreateUserTaskDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        public AdditionalInfoDto? AdditionalInfo { get; set; }
    }
}
