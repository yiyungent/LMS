using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Domain;
using Service;
using Manager;
using Component;
using NHibernate.Criterion;

namespace Component
{
    public class BaseComponent<T, M> : BaseService<T>
        where T : BaseEntity<T>, new()
        where M : BaseManager<T>, new()
    {
        protected M manager = (M)typeof(M).GetConstructor(Type.EmptyTypes).Invoke(null);

        /// <summary>
        /// 新增实体
        /// </summary>
        public void Create(T t)
        {
            manager.Create(t);
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        public void Delete(T t)
        {
            manager.Delete(t);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        public void Delete(int id)
        {
            manager.Delete(id);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        public void Edit(T t)
        {
            manager.Edit(t);
        }

        /// <summary>
        /// 查询
        /// </summary>
        public IList<T> Query(IList<ICriterion> condition)
        {
            return manager.Query(condition);
        }

        /// <summary>
        /// 获取全部实体
        /// </summary>
        public IList<T> GetAll()
        {
            return manager.GetAll();
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        public T GetEntity(int id)
        {
            return manager.GetEntity(id);
        }
    }
}
