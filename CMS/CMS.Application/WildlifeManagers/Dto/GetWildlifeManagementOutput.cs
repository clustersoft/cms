using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.WildlifeManagers.Dto
{
    public class GetWildlifeManagementOutput
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Introduce { get; set; }

        public int OrderID { get; set; }

        public int WildlifeCategoryID { get; set; }

        public GetObjAttachOutput Attach { get; set; }
    }
}
