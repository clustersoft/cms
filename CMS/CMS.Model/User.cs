using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("Users")]
    public class User
    {
        public int ID { get; set; }

        /// <summary>
        /// 部门表关联ID
        /// </summary>
        public int? DeptID { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        /// <summary>
        /// 用户类型：0管理员，1普通用户
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 用户状态：0未启用，1启用
        /// </summary>
        public int Status { get; set; }

        public int OrderID { get; set; }

        public string Remark { get; set; }

        public string AgreeIP { get; set; }

        /// <summary>
        /// 用户来源（与文章表内容来源匹配）
        /// </summary>
        public string UserSourceFrom { get; set; }

        /// <summary>
        //，等级最高，grade=255，用于开发阶段，等交付后，可保留可删除。
        //-面向客户系统默认管理员，grade=254，客户不能删除，也不能编辑其权限及等级。
        //其他用户，grade取值为1-253，拥有添加用户权限的账号新增的账号，grade小于其等级。
        //权限控制的总逻辑：
        //grade>=254，拥有所有功能。
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后一次操作时间
        /// </summary>
        public string LastLoginIP { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public int CreateUser { get; set; }

        public int? ModifyUser { get; set; }

        public string CreateIP { get; set; }

        public string ModifyIP { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
