using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CMS.Model;
using CMS.Util;
using EntityFramework.Extensions;

namespace CMS.Application
{
    public class BaseService<T> :Disposable,IBaseService<T> where T : class, new()
    {
        protected readonly string conn = ConfigurationManager.ConnectionStrings["CMSConnect"].ToString();


        protected BaseService(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }
        private CMSContext _db;

        protected CMSContext db
        {
            get { return _db ?? (_db = DatabaseFactory.Get()); }
        }
        public int Count(Expression<Func<T, bool>> whereLamda)
        {
            return db.Set<T>().Where(whereLamda).Count();
        }

        public bool Delete(int id)
        {
            var entity = db.Set<T>().Find(id);
            db.Set<T>().Remove(entity);
            return db.SaveChanges() > 0;
        }

        public virtual T Get(int id)
        {
            return db.Set<T>().Find(id);
        }

        public virtual T Get(Expression<Func<T, bool>> whereLamda)
        {
            return db.Set<T>().FirstOrDefault(whereLamda);
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLamda)
        {
            return db.Set<T>().Where(whereLamda);
        }

        public IQueryable<T> GetNoTrackingList(Expression<Func<T, bool>> whereLamda)
        {
            return db.Set<T>().Where(whereLamda).AsNoTracking();
        }

        public IQueryable<T> GetPageList<S>(int limit, int offset, out int total,
          Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, S>> orderByLambda)
        {
            var tempData = db.Set<T>().Where(whereLambda);

            total = tempData.Count();

            if (isAsc)
            {
                tempData = tempData.OrderBy<T, S>(orderByLambda).
                    Skip<T>(offset).
                    Take<T>(limit).AsQueryable();
            }
            else
            {
                tempData = tempData.OrderByDescending<T, S>(orderByLambda).
                    Skip<T>(offset).
                    Take<T>(limit).AsQueryable();
            }
            return tempData.AsNoTracking();
        }

        public T Insert(T entity)
        {
            db.Set<T>().Add(entity);
            return db.SaveChanges() > 0 ? entity : null;
        }

        public virtual bool Update(T entity)
        {

            if (db.Entry<T>(entity).State == EntityState.Detached)
            {
                db.Set<T>().Attach(entity);
            }
            db.Entry<T>(entity).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }

        public virtual bool Delete(Expression<Func<T, bool>> whereLamda)
        {
            return db.Set<T>().Where(whereLamda).Delete() > 0;
        }

        public T FirstOrDefault()
        {
            return db.Set<T>().FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> whereLamda)
        {
            return db.Set<T>().Where(whereLamda).FirstOrDefault();
        }
    }
}
