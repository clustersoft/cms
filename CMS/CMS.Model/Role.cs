using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("Roles")]
    public class Role
    {
        public int ID { get; set; }

        public string RoleName { get; set; }

        public string Remark{ get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public int CreateUser { get; set; }

        public int? ModitfyUser { get; set; }

        public int? OrderID { get; set; }

        public string CreateIP { get; set; }

        public string ModifyIP { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
