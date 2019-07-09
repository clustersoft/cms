using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    /// <summary>
    /// 权限动作码表
    /// </summary>
    [Table("Actions")]
    public class CMSAction
    {
        public int ID { get; set; }

        public string ActionName { get; set; }

        public string ActionCode { get; set; }
    }
}
