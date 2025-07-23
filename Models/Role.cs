    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace BackendOnline3.Models
    {
        [Table("Roles")]
        public class Role
        {
            [Key]
            public int RoleId { get; set; }
            public string RoleName { get; set; }

            public virtual ICollection<AllowAccess> AllowAccesses { get; set; } = new List<AllowAccess>();
        }
    }