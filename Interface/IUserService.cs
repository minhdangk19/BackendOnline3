using BackendOnline3.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendOnline3.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync(string? searchQuery, int page, int pageSize);
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(UserRequestDto userDto);
        Task<UserDto> UpdateAsync(int id, UserRequestDto userDto);
        Task<bool> DeleteAsync(int id);
    }
}