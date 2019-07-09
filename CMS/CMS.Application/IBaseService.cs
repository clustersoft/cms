using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application
{
    public interface IBaseService<T> where T:class ,new()
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Insert(T entity);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="whereLamda"></param>
        /// <returns></returns>
        T Get(Expression<Func<T,bool>> whereLamda);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="whereLamda"></param>
        /// <returns></returns>
        IQueryable<T> GetList(Expression<Func<T, bool>> whereLamda);

        /// <summary>
        /// 获取非跟踪列表
        /// </summary>
        /// <param name="whereLamda"></param>
        /// <returns></returns>
        IQueryable<T> GetNoTrackingList(Expression<Func<T, bool>> whereLamda);

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total"></param>
        /// <param name="whereLambda"></param>
        /// <param name="isAsc"></param>
        /// <param name="orderByLambda"></param>
        /// <returns></returns>
        IQueryable<T> GetPageList<S>(int limit, int offset, out int total,
          Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, S>> orderByLambda);

        /// <summary>
        /// 根据条件获取数量
        /// </summary>
        /// <param name="whereLamda"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> whereLamda);

        /// <summary>
        /// 获取第一个实体
        /// </summary>
        /// <returns></returns>
        T FirstOrDefault();

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="whereLamda"></param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> whereLamda);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereLamda"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> whereLamda);
    }
}
