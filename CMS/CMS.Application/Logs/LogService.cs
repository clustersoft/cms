using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.Logs
{
    public class LogService:BaseService<Log>,ILogService
    {
        public LogService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }
    }
}
