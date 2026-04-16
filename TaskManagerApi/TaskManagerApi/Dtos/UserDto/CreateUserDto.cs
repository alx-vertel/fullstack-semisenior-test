using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Dtos.UserDto
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; } = string.Empty;
    }
}
