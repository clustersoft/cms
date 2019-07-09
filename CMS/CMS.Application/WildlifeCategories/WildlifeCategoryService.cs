using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.WildlifeCategories
{
    public class WildlifeCategoryService:BaseService<WildlifeCategory>,IWildlifeCategoryService
    {
        public WildlifeCategoryService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
