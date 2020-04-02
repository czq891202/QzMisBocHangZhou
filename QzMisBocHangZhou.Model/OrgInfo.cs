using System.ComponentModel;

namespace QzMisBocHangZhou.Model
{
    /// <summary>
    /// 机构
    /// </summary>
    public class OrgInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 父机构Id
        /// </summary>
        public string ParentId { get; set; }


        /// <summary>
        /// 机构名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 机构类型
        /// </summary>
        public string TypeName { get; set; }


        /// <summary>
        /// 机构编号
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// 是否禁用
        /// </summary>
        public int IsLock { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName { get; set; }


        public static readonly string RootId = "00000";
        public static readonly string RootCode = "00000";
    }
}
