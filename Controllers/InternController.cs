using BackendOnline3.Dtos;
using BackendOnline3.Interface;
using BackendOnline3.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackendOnline3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InternController : ControllerBase
    {
        private readonly IInternService _service;

        public InternController(IInternService service)
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
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new ApiResponse<object>((int)HttpStatusCode.Unauthorized, "Không tìm thấy thông tin người dùng.", null));
                }
                var interns = await _service.GetAllAsync(userEmail, searchQuery, page, pageSize);
                return Ok(new ApiResponse<IEnumerable<dynamic>>((int)HttpStatusCode.OK, "Lấy danh sách thực tập sinh thành công", interns));
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
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new ApiResponse<object>((int)HttpStatusCode.Unauthorized, "Không tìm thấy thông tin người dùng.", null));
                }
                var intern = await _service.GetByIdAsync(id, userEmail);
                if (intern == null)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy thực tập sinh với ID = {id}", null));
                }
                return Ok(new ApiResponse<dynamic>((int)HttpStatusCode.OK, "Lấy thông tin thực tập sinh thành công", intern));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] InternRequestDto internDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ", ModelState));
            }

            try
            {
                var created = await _service.CreateAsync(internDto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, new ApiResponse<InternDto>((int)HttpStatusCode.Created, "Tạo thực tập sinh thành công", created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] InternRequestDto internDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ", ModelState));
            }

            try
            {
                var updated = await _service.UpdateAsync(id, internDto);
                if (updated == null)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy thực tập sinh với ID = {id}", null));
                }
                return Ok(new ApiResponse<InternDto>((int)HttpStatusCode.OK, "Cập nhật thực tập sinh thành công", updated));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy thực tập sinh với ID = {id}", null));
                }
                return Ok(new ApiResponse<object>((int)HttpStatusCode.OK, "Xóa thực tập sinh thành công", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }
    }
}