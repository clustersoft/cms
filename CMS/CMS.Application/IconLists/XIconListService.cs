using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.IconLists
{
    public class XIconListService:BaseService<IconList>,XIIconListService
    {
        public XIconListService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }
    }
}
