using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Actions.Dto;
using CMS.Model;

namespace CMS.Application.Actions
{
    public class ActionService:BaseService<CMSAction>,IActionService
    {
        public ActionService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }
    }
}
