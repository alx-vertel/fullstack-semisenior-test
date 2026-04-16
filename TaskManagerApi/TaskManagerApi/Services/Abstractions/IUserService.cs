using TaskManagerApi.Dtos.UserDto;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services.Abstractions
{
    public interface IUserService
    {
        Task<IEnumerable<ReadUserDto>> GetAllUsersAsync();

        Task<ReadUserDto?> GetUserByIdAsync(int id);
        Task<ReadUserDto> AddUserAsync(CreateUserDto userDto);
       
    }
}
