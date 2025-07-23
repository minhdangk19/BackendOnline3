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
    [Authorize] // Yêu cầu xác thực
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roles = await _service.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<RoleDto>>((int)HttpStatusCode.OK, "Lấy danh sách vai trò thành công", roles));
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
                var role = await _service.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy vai trò với ID = {id}", null));
                }
                return Ok(new ApiResponse<RoleDto>((int)HttpStatusCode.OK, "Lấy thông tin vai trò thành công", role));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")] // Chỉ ADMIN được tạo
        public async Task<IActionResult> Create([FromBody] RoleRequestDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ", ModelState));
            }

            try
            {
                var created = await _service.CreateAsync(roleDto);
                return CreatedAtAction(nameof(Get), new { id = created.RoleId }, new ApiResponse<RoleDto>((int)HttpStatusCode.Created, "Tạo vai trò thành công", created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")] // Chỉ ADMIN được cập nhật
        public async Task<IActionResult> Update(int id, [FromBody] RoleRequestDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ", ModelState));
            }

            try
            {
                var updated = await _service.UpdateAsync(id, roleDto);
                if (updated == null)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy vai trò với ID = {id}", null));
                }
                return Ok(new ApiResponse<RoleDto>((int)HttpStatusCode.OK, "Cập nhật vai trò thành công", updated));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")] // Chỉ ADMIN được xóa
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy vai trò với ID = {id}", null));
                }
                return Ok(new ApiResponse<object>((int)HttpStatusCode.OK, "Xóa vai trò thành công", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }
    }
}