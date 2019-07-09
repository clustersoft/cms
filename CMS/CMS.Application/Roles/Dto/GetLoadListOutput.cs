using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Navgations.Dto;

namespace CMS.Application.Roles.Dto
{
    public class GetLoadListOutput
    {
        public List<GetLoadNavgationListOutput> navlist { get; set; }

        public List<GetLoadCategoryListOutput> categorylist { get; set; }
    }
}
