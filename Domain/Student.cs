using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.ActiveRecord;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类： 学生
    /// </summary>
    [ActiveRecord]
    public class Student : BaseEntity<Student>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        ///     0: 男
        ///     1: 女
        /// </summary>
        [Display(Name = "性别")]
        [Property]
        public int Sex { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        [Display(Name = "学号")]
        [Property(Length = 10, NotNull = true)]
        public string StudyNumber { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        [Property(Length = 20, NotNull = true)]
        public string Mobile { get; set; }

        /// <summary>
        /// 所在班级
        ///     多对一关系
        /// </summary>
        [Display(Name = "所在班级")]
        [BelongsTo(Column = "ClazzId")]
        public Clazz Clazz { get; set; }
    }
}
