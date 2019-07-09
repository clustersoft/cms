using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;
using CMS.Util;
using EntityFramework.Caching;

namespace CMS.Application.SystemConfigurations
{
    public class SystemConfigurationService : BaseService<SystemConfiguration>, ISystemConfigurationService
    {
        public SystemConfigurationService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public int GetPageSize()
        {
            //缓存
            var cachePageSize=CacheManager.Current.Get("pageSize");
            if (cachePageSize != null)
            {
                return (int)cachePageSize;
            }
            else
            {
                var systemConfiguration = db.SystemConfigurations.FirstOrDefault();
                if (systemConfiguration == null)
                {
                    return CMSConst.PageSiez;
                }
                else
                {
                    if (systemConfiguration.PageSizes <= 0)
                    {
                        return CMSConst.PageSiez;
                    }
                    else
                    {
                        CacheManager.Current.Add("pageSize", systemConfiguration.PageSizes);
                        return systemConfiguration.PageSizes;
                    }
                }
            }
        }

        public override bool Update(SystemConfiguration input)
        {
            if (db.Entry<SystemConfiguration>(input).State == EntityState.Detached)
            {
                db.Set<SystemConfiguration>().Attach(input);
            }
            db.Entry<SystemConfiguration>(input).State = EntityState.Modified;

            bool boolReturn = db.SaveChanges() > 0;

            if (boolReturn)
            {
                CacheManager.Current.Set("pageSize",input.PageSizes);
            }
            return boolReturn;
        }
    }
}
