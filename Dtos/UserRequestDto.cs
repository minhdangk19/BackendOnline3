using System.ComponentModel.DataAnnotations;

namespace BackendOnline3.Dtos
{
    public class UserRequestDto
    {
        [Required(ErrorMessage = "Họ tên không được để trống.")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Họ tên phải từ 3 đến 255 ký tự.")]
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "RoleId không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "RoleId không hợp lệ.")]
        public int RoleId { get; set; }
    }
}