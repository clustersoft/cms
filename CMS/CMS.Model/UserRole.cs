using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Table("User_Role_Link")]
    public class UserRole
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int RoleID { get; set; }

        //public DateTime CreateTime { get; set; }

        //public DateTime ModifyTime { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}
