using BackendOnline3.Dtos;
using BackendOnline3.Interface;
using BackendOnline3.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace BackendOnline3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Yêu cầu xác thực cho toàn bộ controller
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchQuery, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Trang hoặc kích thước trang không hợp lệ.", null));
            }

            try
            {
                var users = await _service.GetAllAsync(searchQuery, page, pageSize);
                return Ok(new ApiResponse<IEnumerable<UserDto>>((int)HttpStatusCode.OK, "Lấy danh sách người dùng thành công", users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy người dùng với ID = {id}", null));
                }
                return Ok(new ApiResponse<UserDto>((int)HttpStatusCode.OK, "Lấy thông tin người dùng thành công", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")] // Chỉ Admin được tạo
        public async Task<IActionResult> Create([FromBody] UserRequestDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ", ModelState));
            }

            try
            {
                var created = await _service.CreateAsync(userDto);
                return CreatedAtAction(nameof(Get), new { id = created.UserId }, new ApiResponse<UserDto>((int)HttpStatusCode.Created, "Tạo người dùng thành công", created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")] // Chỉ Admin được cập nhật
        public async Task<IActionResult> Update(int id, [FromBody] UserRequestDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ", ModelState));
            }

            try
            {
                var updated = await _service.UpdateAsync(id, userDto);
                if (updated == null)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy người dùng với ID = {id}", null));
                }
                return Ok(new ApiResponse<UserDto>((int)HttpStatusCode.OK, "Cập nhật người dùng thành công", updated));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")] // Chỉ Admin được xóa
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy người dùng với ID = {id}", null));
                }
                return Ok(new ApiResponse<object>((int)HttpStatusCode.OK, "Xóa người dùng thành công", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }
    }
}