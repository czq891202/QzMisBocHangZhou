using System;

namespace QzMisBocHangZhou.Model
{
    public class ArchiveTransferInfo
    {
        public string Id { get; set; }

        /// <summary>
        /// 单位Id
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 移交人
        /// </summary>
        public string Handover { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 移交日期
        /// </summary>
        public DateTime? TransferDate { get; set; }


        /// <summary>
        /// 状态(0:待审核, 1:审核通过 2:驳回)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 档案Id
        /// </summary>
        public string ArchiveId { get; set; }

        public string TransferDateString
        {
            get
            {
                return TransferDate?.ToString("yyyy-MM-dd");
            }
        }

        public string OrgName { get; set; }

        public string OrgCode { get; set; }

        public string OrgContact { get; set; }
        
        public string QuotaNo { get; set; }

        public string LoanAccount { get; set; }

        public string LabelCode { get; set; }

        public string CustomerNo { get; set; }

        public string Borrower { get; set; }
    }

    /// <summary>
    /// 废弃
    /// </summary>
    public class Removed_ArchiveTransferDetails
    {
        public string Id { get; set; }


        public string PId { get; set; }

        /// <summary>
        /// 文档Id
        /// </summary>
        public string ArchiveId { get; set; }

        /// <summary>
        /// 一级档案份数
        /// </summary>
        public string LvlOneNum { get; set; }


        /// <summary>
        /// 二级档案卷数
        /// </summary>
        public string LvlTowNum { get; set; }


        public string QuotaNo { get; set; }

        public string LoanAccount { get; set; }

        public string LabelCode { get; set; }

        public string Borrower { get; set; }

        /// <summary>
        /// 核验状态
        /// </summary>
        public VerifyType Status { get; set; } = VerifyType.未核对;

    }
}
