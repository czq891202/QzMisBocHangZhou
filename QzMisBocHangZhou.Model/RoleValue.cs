using System.ComponentModel;

namespace QzMisBocHangZhou.Model
{
    /// <summary>
    /// 角色权限
    /// </summary>
    [Description("角色权限")]
    public class RoleValue
    {
        /// <summary>
        /// Id
        /// </summary>
        [Description("Id")]
        public string Id { get; set; }


        /// <summary>
        /// 角色Id
        /// </summary>
        [Description("角色Id")]
        public string RoleId { get; set; }


        /// <summary>
        /// 菜单Id
        /// </summary>
        [Description("菜单Id")]
        public string NavigationId { get; set; }


        /// <summary>
        /// 操作类别
        /// </summary>
        [Description("操作类别")]
        public string ActionType { get; set; }
    }
}
