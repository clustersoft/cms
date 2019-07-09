using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 留言表
    /// </summary>
    [Table("LeaveMessages")]
    public class LeaveMessage
    {
        public int ID { get; set; }

        public string Guid { get; set; }
        /// <summary>
        /// 留言种类
        /// </summary>
        public string LeaveType { get; set; }

        /// <summary>
        /// 留言内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 留言人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 留言时间
        /// </summary>
        public DateTime LeaveTime { get; set; }

        /// <summary>
        /// 留言人IP地址
        /// </summary>
        public string  LeaveIP { get; set; }
    }
}
