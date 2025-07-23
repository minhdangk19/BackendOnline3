using BackendOnline3.Dtos;
using System.Threading.Tasks;

namespace BackendOnline3.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequestDto loginDto);
    }
}