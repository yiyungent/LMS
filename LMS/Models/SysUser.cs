using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    /// <summary>
    /// 实体类：用户
    /// </summary>
    public class SysUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [Display(Name = "用户编号")]
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        public string Name { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [Display(Name = "登录账号")]
        public string LoginAccount { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 状态
        ///     0: 正常
        ///     1: 禁用
        /// </summary>
        [Display(Name = "状态")]
        public int Status { get; set; }
    }
}