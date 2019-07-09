using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 角色导航栏目权限表
    /// </summary>
    [Table("Role_Nav_Action_Link")]
    public class RoleNavAction
    {
        public int ID { get; set; }

        public int RoleID { get; set; }

        public string Nav_Code { get; set; }

        public string Action_Code { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual Role Role { get; set; }
    }
}
