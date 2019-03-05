using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.ActiveRecord;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类： 班级
    /// </summary>
    [ActiveRecord]
    public class Clazz : BaseEntity<Clazz>
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        [Display(Name = "班级名称")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 学生列表
        ///     一对多关系
        /// </summary>
        [Display(Name = "学生列表")]
        [HasMany(ColumnKey = "ClazzId")]
        public IList<Student> StudentList { get; set; }
    }
}
