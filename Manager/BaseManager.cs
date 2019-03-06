using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Domain;
using NHibernate.Criterion;
using Castle.ActiveRecord;

namespace Manager
{
    public class BaseManager<T> : ActiveRecordBase<T>
        where T : class
    {
        /// <summary>
        /// 新增实体
        /// </summary>
        public new void Create(T t)
        {
            ActiveRecordBase.Create(t);
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        public new void Delete(T t)
        {
            ActiveRecordBase.Delete(t);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        public void Delete(int id)
        {
            // 1.根据主键获取实体
            T t = GetEntity(id);
            // 2.根据实体删除
            if (t != null)
            {
                Delete(t);
            }
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        public void Edit(T t)
        {
            ActiveRecordBase.Update(t);
        }

        /// <summary>
        /// 查询
        /// </summary>
        public IList<T> Query(IList<ICriterion> condition)
        {
            // 1.IList --> Array
            // 2. Array --> IList
            // 3.强类型转换
            return (IList<T>)ActiveRecordBase.FindAll(typeof(T), condition.ToArray());
        }

        /// <summary>
        /// 获取全部实体
        /// </summary>
        public IList<T> GetAll()
        {
            // 强类型转换第二种写法
            return ActiveRecordBase.FindAll(typeof(T)) as IList<T>;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        public T GetEntity(int id)
        {
            return (T)ActiveRecordBase.FindByPrimaryKey(typeof(T), id);
        }
    }
}
