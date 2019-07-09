using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.ArticleCategories.Dto
{
    public class GetCategoryOutput
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string ParentName { get; set; }

        public string Name { get; set; }

        public string RefNo { get; set; }

        public string Remark { get; set; }

        public int OrderID { get; set; }

        public int LinkType { get; set; }

        public string LinkPath { get; set; }

        public int AddArticlePermissions { get; set; }

        public int BeCategory { get; set; }

        public int State { get; set; }

        public int? TemplateId { get; set; }

        public string TemplatePreview { get; set; }

        public GetObjAttachOutput Attach { get; set; }
    }
}
