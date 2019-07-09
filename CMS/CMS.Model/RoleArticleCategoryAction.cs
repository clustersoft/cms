using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    ///发布文章栏目权限表
    /// </summary>
    [Table("Role_ArticleCategory_Action_Link")]
    public class RoleArticleCategoryAction
    {
        public int ID { get; set; }

        public int RoleID { get; set; }

        public int ArticleCategoryID { get; set; }

        public string ActionCode { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual Role Role { get; set; }
    }
}
