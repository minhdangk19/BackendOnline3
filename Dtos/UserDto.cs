namespace BackendOnline3.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}