using System.ComponentModel.DataAnnotations;

namespace BackendOnline3.Dtos
{
    public class AllowAccessRequestDto
    {
        [Required(ErrorMessage = "RoleId không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "RoleId không hợp lệ.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Tên bảng không được để trống.")]
        public string TableName { get; set; }
        [Required(ErrorMessage = "Danh sách cột truy cập không được để trống.")]
        public string AccessProperties { get; set; }
    }
}