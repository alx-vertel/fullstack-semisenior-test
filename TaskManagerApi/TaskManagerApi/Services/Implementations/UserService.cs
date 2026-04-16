using TaskManagerApi.Data;
using TaskManagerApi.Dtos.UserDto;
using TaskManagerApi.Models;
using TaskManagerApi.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace TaskManagerApi.Services.Implementations
{
    public class UserService(AppDbContext _context) : IUserService
    {
        public async Task<ReadUserDto> AddUserAsync(CreateUserDto userDto)
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == userDto.Email);
            if (emailExists)
            {
                throw new InvalidOperationException("Email is already registered.");
            }

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new ReadUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<IEnumerable<ReadUserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new ReadUserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                })
                .ToListAsync();
        }

        public async Task<ReadUserDto?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new ReadUserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();
        }
    }
}
