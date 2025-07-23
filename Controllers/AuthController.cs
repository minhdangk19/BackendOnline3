using BackendOnline3.Dtos;
using BackendOnline3.Interface;
using BackendOnline3.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace BackendOnline3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ", ModelState));
            }

            try
            {
                var token = await _authService.LoginAsync(loginDto);
                return Ok(new ApiResponse<string>(
                    (int)HttpStatusCode.OK,
                    "Đăng nhập thành công. Sử dụng token với 'Bearer <token>' trong header Authorization (ví dụ: Authorization: Bearer <token>)",
                    token));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse<object>((int)HttpStatusCode.Unauthorized, ex.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }
    }
}