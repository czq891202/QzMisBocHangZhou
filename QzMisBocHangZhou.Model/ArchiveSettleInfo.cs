using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    public class ArchiveSettleInfo
    {
        public string Id { get; set; }

        /// <summary>
        /// 文档Id
        /// </summary>
        public string ArchiveId { get; set; }


        /// <summary>
        /// 单位Id
        /// </summary>
        public string OrgId { get; set; }


        /// <summary>
        /// 结清人
        /// </summary>
        public string SettlePeople { get; set; }


        /// <summary>
        /// 结清日期
        /// </summary>
        public DateTime? SettleDate { get; set; }


        /// <summary>
        /// 状态(0:待审核, 1:审核通过, 2:已领取)
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 结清类型
        /// </summary>
        public string UsedBy { get; set; }


        public string SettleDateString
        {
            get
            {
                return SettleDate?.ToString("yyyy-MM-dd");
            }
        }


        public string OrgName { get; set; }


        public string OrgCode { get; set; }


        public string OrgContact { get; set; }


        public string QuotaNo { get; set; }


        public string LoanAccount { get; set; }


        public string LabelCode { get; set; }


        public int ArcStatus { get; set; }

        public string CustomerNo { get; set; }

        public string LoanBorrower { get; set; }
    }

    public class Removed_ArchiveSettleDetail
    {
        public string Id { get; set; }


        public string PId { get; set; }

        /// <summary>
        /// 文档Id
        /// </summary>
        public string ArchiveId { get; set; }

        
        public string QuotaNo { get; set; }


        public string LoanAccount { get; set; }


        public string LabelCode { get; set; }

    }
}
