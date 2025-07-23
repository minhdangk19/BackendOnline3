using BackendOnline3.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendOnline3.Interface
{
    public interface IAllowAccessService
    {
        Task<IEnumerable<AllowAccessDto>> GetAllAsync();
        Task<AllowAccessDto> GetByIdAsync(int id);
        Task<AllowAccessDto> CreateAsync(AllowAccessRequestDto allowAccessDto);
        Task<AllowAccessDto> UpdateAsync(int id, AllowAccessRequestDto allowAccessDto);
        Task<bool> DeleteAsync(int id);
    }
}