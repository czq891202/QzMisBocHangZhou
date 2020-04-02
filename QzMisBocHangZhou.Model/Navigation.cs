using System.ComponentModel;

namespace QzMisBocHangZhou.Model
{
    /// <summary>
    /// 导航菜单
    /// </summary>
    [Description("导航菜单")]
    public class Navigation
    {
        /// <summary>
        /// Id
        /// </summary>
        [Description("Id")]
        public string Id { get; set; }


        /// <summary>
        /// 上级导航Id
        /// </summary>
        [Description("上级导航Id")]
        public string ParentId { get; set; } = "-1";


        /// <summary>
        /// 层级
        /// </summary>
        [Description("层级")]
        public int Level { get; set; }


        /// <summary>
        /// 导航名称
        /// </summary>
        [Description("导航名称")]
        public string Name { get; set; }


        /// <summary>
        /// 导航标题
        /// </summary>
        [Description("导航标题")]
        public string Title { get; set; }


        /// <summary>
        /// 导航地址
        /// </summary>
        [Description("导航地址")]
        public string LinkUrl { get; set; }


        /// <summary>
        /// 导航样式
        /// </summary>
        [Description("导航样式")]
        public string MenuClass { get; set; }


        /// <summary>
        /// 排序代码
        /// </summary>
        [Description("排序代码")]
        public string OrderCode { get; set; }


        /// <summary>
        /// 操作类型
        /// </summary>
        [Description("操作类型")]
        public string ActionType { get; set; }


        /// <summary>
        /// 是否隐藏
        /// </summary>
        [Description("是否隐藏")]
        public int IsLock { get; set; }


        /// <summary>
        /// 备注说明
        /// </summary>
        [Description("备注说明")]
        public string Remark { get; set; }


        
    }
}
