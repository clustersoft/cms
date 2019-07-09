using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.PublicityCategories
{
    public class PublicityCategoryService:BaseService<PublicityCategory>,IPublicityCategoryService
    {
        public PublicityCategoryService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }
    }
}
