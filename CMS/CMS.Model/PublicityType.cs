using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 宣传类型表
    /// </summary>
    [Table("PublicityTypes")]
    public class PublicityType
    {
        public int ID { get; set; }

        /// <summary>
        ///宣传种类名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUser { get; set; }

        public DateTime ModifyTime { get; set; }

        public int ModifyUser { get; set; }

        public int OrderID { get; set; }
    }
}
