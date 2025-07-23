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
    public class AllowAccessController : ControllerBase
    {
        private readonly IAllowAccessService _service;

        public AllowAccessController(IAllowAccessService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var allowAccesses = await _service.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<AllowAccessDto>>((int)HttpStatusCode.OK, "Lấy danh sách quyền truy cập thành công", allowAccesses));
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
                var allowAccess = await _service.GetByIdAsync(id);
                if (allowAccess == null)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy quyền truy cập với ID = {id}", null));
                }
                return Ok(new ApiResponse<AllowAccessDto>((int)HttpStatusCode.OK, "Lấy thông tin quyền truy cập thành công", allowAccess));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")] // Chỉ ADMIN được tạo
        public async Task<IActionResult> Create([FromBody] AllowAccessRequestDto allowAccessDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ", ModelState));
            }

            try
            {
                var created = await _service.CreateAsync(allowAccessDto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, new ApiResponse<AllowAccessDto>((int)HttpStatusCode.Created, "Tạo quyền truy cập thành công", created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")] // Chỉ ADMIN được cập nhật
        public async Task<IActionResult> Update(int id, [FromBody] AllowAccessRequestDto allowAccessDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>((int)HttpStatusCode.BadRequest, "Dữ liệu không hợp lệ", ModelState));
            }

            try
            {
                var updated = await _service.UpdateAsync(id, allowAccessDto);
                if (updated == null)
                {
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy quyền truy cập với ID = {id}", null));
                }
                return Ok(new ApiResponse<AllowAccessDto>((int)HttpStatusCode.OK, "Cập nhật quyền truy cập thành công", updated));
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
                    return NotFound(new ApiResponse<object>((int)HttpStatusCode.NotFound, $"Không tìm thấy quyền truy cập với ID = {id}", null));
                }
                return Ok(new ApiResponse<object>((int)HttpStatusCode.OK, "Xóa quyền truy cập thành công", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(500, "Đã có lỗi xảy ra.", ex.Message));
            }
        }
    }
}