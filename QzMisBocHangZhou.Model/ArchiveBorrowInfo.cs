using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    public class ArchiveBorrowInfo
    {
        public string Id { get; set; }

        /// <summary>
        /// 单位Id
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 借阅人
        /// </summary>
        public string Borrower { get; set; }

        /// <summary>
        /// 借阅时间
        /// </summary>
        public DateTime? BorrowDate { get; set; }

        public string ArchiveId { get; set; }

        public DateTime? PreReturnDate { get; set; }

        public DateTime? RealReturnDate { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        public string UsedBy { get; set; }

        /// <summary>
        /// 状态(0:借阅待审核, 1:借阅审核通过, 2:借阅出库, 3:归还审核中, 4:已归还, 5:借阅驳回, 6:归还驳回)
        /// </summary>
        public int Status { get; set; }

        public string BorrowDateString
        {
            get
            {
                return BorrowDate?.ToString("yyyy-MM-dd");
            }
        }

        public string OrgName { get; set; }

        public string OrgCode { get; set; }

        public string OrgContact { get; set; }

        public string PreReturnDateString
        {
            get
            {
                return PreReturnDate?.ToString("yyyy-MM-dd");
            }
        }

        public string RealReturnDateString
        {
            get
            {
                return RealReturnDate?.ToString("yyyy-MM-dd");
            }
        }

        public string QuotaNo { get; set; }

        public string LoanAccount { get; set; }

        public string CustomerNo { get; set; }

        public string LabelCode { get; set; }

        public string LoanBorrower { get; set; }

        public string StorageLocation { get; set; }

        public string ProductCode { get; set; }
    }
}
