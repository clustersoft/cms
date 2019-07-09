using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.Navgations
{
    public class NavgationService:BaseService<Navgation>,INavgationService
    {
        public NavgationService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }
    }
}
