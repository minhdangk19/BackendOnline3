using BackendOnline3.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendOnline3.Interface
{
    public interface IInternService
    {
        Task<IEnumerable<dynamic>> GetAllAsync(string userEmail, string? searchQuery, int page, int pageSize);
        Task<dynamic> GetByIdAsync(int id, string userEmail);
        Task<InternDto> CreateAsync(InternRequestDto internDto);
        Task<InternDto> UpdateAsync(int id, InternRequestDto internDto);
        Task<bool> DeleteAsync(int id);
    }
}