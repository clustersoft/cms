using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;

namespace CMS.Application
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private CMSContext dataContext;
        public CMSContext Get()
        {
            return dataContext ?? (dataContext = new CMSContext());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
