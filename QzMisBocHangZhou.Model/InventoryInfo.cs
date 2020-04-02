using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    public class InventoryInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        [Description("创建者Id")]
        public string UserId { get; set; }


        /// <summary>
        /// 盘点机构Id
        /// </summary>
        [Description("盘点机构Id")]
        public string OrgId { get; set; }


        /// <summary>
        /// 盘点名称
        /// </summary>
        [Description("盘点名称")]
        public string InventoryName { get; set; }


        /// <summary>
        /// 盘点开始时间
        /// </summary>
        [Description("盘点开始时间")]
        public DateTime? StartTime { get; set; }


        /// <summary>
        /// 盘点结束时间
        /// </summary>
        [Description("盘点结束时间")]
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }


        [Description("锁定")]
        /// <summary>
        /// 是否锁定(开始盘点后不可再修改)
        /// </summary>
        public int IsLocked { get; set; }


        public string StartTimeString
        {
            get
            {
                return StartTime?.ToString("yyyy-MM-dd");
            }
        }


        public string EndTimeString
        {
            get
            {
                return EndTime?.ToString("yyyy-MM-dd");
            }
        }

        public int SuccessCount { get; set; } = 0;


        public int Total { get; set; } = 0;


        public string OrgName { get; set; }

    }

    public class InventoryDetail
    {
        /// <summary>
        /// Id
        /// </summary>
        [Description("Id")]
        public string Id { get; set; }


        /// <summary>
        /// InventoryId
        /// </summary>
        [Description("盘点Id")]
        public string InventoryId { get; set; }


        /// <summary>
        /// 档案Id
        /// </summary>
        [Description("档案Id")]
        public string ArchiveId { get; set; }


        /// <summary>
        /// 盘点时间
        /// </summary>
        [Description("盘点时间")]
        public DateTime? InventoryTime { get; set; }


        /// <summary>
        /// 盘点状态
        /// </summary>
        [Description("盘点状态")]
        public VerifyType Status { get; set; } = VerifyType.未核对;


        public string QuotaNo { get; set; }


        public string LoanAccount { get; set; }


        public string LabelCode { get; set; }

    }

    /// <summary>
    /// 盘点核对状态
    /// </summary>
    [Description("盘点核对状态")]
    public enum VerifyType { 未核对 = -1, 失败 = 0, 成功 = 1 }
}
