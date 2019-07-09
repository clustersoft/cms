using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Table("Departments")]
    public class Department
    {
        public int ID { get; set; }

        public int ParentID { get;set; }

        /// <summary>
        ///部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 部门名称简写
        /// </summary>
        public string SimpleName { get; set; }

        /// <summary>
        /// 部门职责或备注
        /// </summary>
        public string Duty { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public int? CreateUser { get; set; }

        public int? ModitfyUser { get; set; }
    }
}
