using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 宣传类型栏目
    /// </summary>
    [Table("PublicityCategorys")]
    public class PublicityCategory
    {
        public int ID { get; set; }

        public int PublicityTypesID { get; set; }

        /// <summary>
        /// 宣传栏目
        /// </summary>
        public string PublicityCategoryName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderID { get; set; }

        public string CreateIP { get; set; }

        public string ModifyIP { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUser { get; set; }

        public DateTime? ModifyTime { get; set; }

        public int? ModifyUser { get; set; }

        [ForeignKey("PublicityTypesID")]
        public virtual PublicityType PublicityType {get;set;}
    }
}
