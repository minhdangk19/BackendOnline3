using BackendOnline3.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendOnline3.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto> GetByIdAsync(int id);
        Task<RoleDto> CreateAsync(RoleRequestDto roleDto);
        Task<RoleDto> UpdateAsync(int id, RoleRequestDto roleDto);
        Task<bool> DeleteAsync(int id);
    }
}