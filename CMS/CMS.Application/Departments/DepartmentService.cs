using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.Departments
{
    public class DepartmentService:BaseService<Department>,IDepartmentSerivce
    {
        public DepartmentService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }

    }
}
