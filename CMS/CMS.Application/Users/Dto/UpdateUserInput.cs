using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Users.Dto
{
    public class UpdateUserInput
    {
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 2)]
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
        [Required]
        public int Type { get; set; }

        /// <summary>
        /// 用户状态：0未启用，1启用
        /// </summary>
        [Required]
        [Range(0,1)]
        public int Status { get; set; }

        [Required]
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
        [Required]
        public int Grade { get; set; }

        [Required]
        public int ModifyUser { get; set; }

        public string RoleIDS { get; set; }
    }
}
