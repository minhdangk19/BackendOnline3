using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendOnline3.Models
{
    [Table("AllowAccess")]
    public class AllowAccess
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string TableName { get; set; }
        public string AccessProperties { get; set; } // Lưu danh sách cột được phép, ví dụ: "InternName,DateOfBirth,University,Major"

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}