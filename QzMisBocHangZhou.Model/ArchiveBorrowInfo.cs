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
        /// 状态(0:待审核, 1:审核通过, 2:已归还)
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
    }


    public class Removed_ArchiveBorrowDetails
    {
        public string Id { get; set; }


        public string PId { get; set; }


        public string ArchiveId { get; set; }


        public DateTime? PreReturnDate { get; set; }


        public DateTime? RealReturnDate { get; set; }


        public string ReturnPepole { get; set; }


        public string Remark { get; set; }


        /// <summary>
        /// 用途
        /// </summary>
        public string UsedBy { get; set; }


        /// <summary>
        /// 状态(0:未归还， 1:已归还)
        /// </summary>
        public int IsReturned { get; set; }

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

        public string LabelCode { get; set; }

        public string Borrower { get; set; }

        public string GuaranteeCrdNo { get; set; }

        /// <summary>
        /// 核验状态
        /// </summary>
        public VerifyType Status { get; set; } = VerifyType.未核对;
    }
}
