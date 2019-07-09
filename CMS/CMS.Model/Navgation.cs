using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 导航栏目表
    /// </summary>
    [Table("Navgation")]
    public class Navgation
    {
        public int ID { get; set; }

        public int ParentId { get; set; }

        /// <summary>
        /// 导航标识(唯一)
        /// </summary>
        [Column("Nav_Name")]
        public string NavName { get; set; }

        /// <summary>
        /// 导航名称
        /// </summary>
        [Column("Nav_Title")]
        public string NavTitle { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        [Column("Icon_Url")]
        public string IconUrl { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [Column("Link_Url")]
        public string LinkUrl { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 需分配的权限
        /// </summary>
        public string ActionTypes { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public int CreateUser { get; set; }

        public int? ModifyUser { get; set; }

        public string CreateIP { get; set; }

        public string ModifyIP { get; set; }
    }
}
