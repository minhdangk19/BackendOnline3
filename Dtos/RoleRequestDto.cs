using System.ComponentModel.DataAnnotations;

namespace BackendOnline3.Dtos
{
    public class RoleRequestDto
    {
        [Required(ErrorMessage = "Tên vai trò không được để trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên vai trò phải từ 3 đến 100 ký tự.")]
        public string RoleName { get; set; }
    }
}