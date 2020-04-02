using System.ComponentModel;

namespace QzMisBocHangZhou.Model
{
    /// <summary>
    /// 角色
    /// </summary>
    [Description("角色")]
    public class Role
    {
        /// <summary>
        /// Id
        /// </summary>
        [Description("Id")]
        public string Id { get; set; }


        /// <summary>
        /// 角色名称
        /// </summary>
        [Description("角色名称")]
        public string RoleName { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }

    }
}
