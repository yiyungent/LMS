using Castle.ActiveRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：用户
    /// </summary>
    [ActiveRecord]
    public class SysUser : BaseEntity<SysUser>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [Display(Name = "登录账号")]
        [Property(Length = 30, NotNull = true)]
        public string LoginAccount { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Property(Length = 64, NotNull = true)]
        public string Password { get; set; }

        /// <summary>
        /// 状态
        ///     0: 正常
        ///     1: 禁用
        /// </summary>
        [Display(Name = "状态")]
        [Property]
        public int Status { get; set; }

        /// <summary>
        /// 担任角色列表
        ///     多对多关系
        /// </summary>
        [Display(Name = "担任角色列表")]
        [HasAndBelongsToMany(Table = "Role_User", ColumnKey = "UserId", ColumnRef = "RoleId")]
        public IList<SysRole> SysRoleList { get; set; }
    }
}