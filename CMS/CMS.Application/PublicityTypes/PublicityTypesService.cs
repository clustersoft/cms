using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application.PublicityTypes
{
    public class PublicityTypesService:BaseService<PublicityType>,IPublicityTypesService
    {
        public PublicityTypesService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
            {
        }
    }
}
