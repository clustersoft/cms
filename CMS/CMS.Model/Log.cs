using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 日志表
    /// </summary>
    [Table("Logs")]
    public class Log
    {
        public int ID { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string ActionContent { get; set; }

        /// <summary>
        ///操作类型
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// 操作内容ID
        /// </summary>
        public int SourceID { get; set; }

        /// <summary>
        /// 添加人员ID
        /// </summary>
        public int SourceAddUserID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public int LogUserID { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 操作IP地址
        /// </summary>
        public string LogIPAddress { get; set; }

        /// <summary>
        /// 操作栏目
        /// </summary>
        public int CategoryID { get; set; }

        [ForeignKey("LogUserID")]
        public virtual User User { get; set; }
    }
}
