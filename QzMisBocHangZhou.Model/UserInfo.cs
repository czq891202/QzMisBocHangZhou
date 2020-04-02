using System;
using System.ComponentModel;

namespace QzMisBocHangZhou.Model
{
    /// <summary>
    /// 用户实体
    /// </summary>
    [Description("用户")]
    public class UserInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        [Description("Id")]
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Description("密码")]
        public string Password { get; set; }

        /// <summary>
        /// 所属组织机构Id
        /// </summary>
        [Description("所属组织机构Id")]
        public string OrgId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Description("角色Id")]
        public string RoleId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Description("姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Description("电话")]
        public string Mobile { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 最后一次登陆时间
        /// </summary>
        [Description("最后一次登陆时间")] 
        public DateTime? LastLandingTime { get; set; }
    }
}
