using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Navgations.Dto;

namespace CMS.Application.Roles.Dto
{
    public class GetRoleOutput
    {
        public string RoleName { get; set; }

        public string Remark { get; set; }

        public List<GetRoleCategoryListOutput> Categorylist { get; set; }

        public List<GetRoleNavgationListOutput> Navlist { get; set; }

        public int OrderID { get; set; }
    }
}
