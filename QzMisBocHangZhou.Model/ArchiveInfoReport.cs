using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    public class ArchiveInfoReport : ArchiveInfo
    {
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
        public string TransferDateString
        {
            get
            {
                return TransferDate?.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 移交状态(0:待审核, 1:审核通过 2:驳回)
        /// </summary>
        public int TransStatus { get; set; }

        /// <summary>
        /// 结清人
        /// </summary>
        public string SettlePeople { get; set; }

        /// <summary>
        /// 结清日期
        /// </summary>
        public DateTime? SettleDate { get; set; }
        public string SettleDateString
        {
            get
            {
                return SettleDate?.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 结清状态(0:待审核, 1:审核通过, 2:已领取, 3:驳回 )
        /// </summary>
        public int SettleStatus { get; set; }

        /// <summary>
        /// 结清类型
        /// </summary>
        public string SettleUsedBy { get; set; }

        /// <summary>
        /// 借阅人
        /// </summary>
        public string BorrowPeople { get; set; }

        /// <summary>
        /// 借阅时间
        /// </summary>
        public DateTime? BorrowDate { get; set; }
        public string BorrowDateString
        {
            get
            {
                return BorrowDate?.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 用途
        /// </summary>
        public string BorrowUsedBy { get; set; }

        /// <summary>
        /// 借阅状态(0:借阅待审核, 1:借阅审核通过, 2:借阅出库, 3:归还审核中, 4:已归还, 5:借阅驳回, 6:归还驳回)
        /// </summary>
        public int BorrowStatus { get; set; }

        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? PreReturnDate { get; set; }

        /// <summary>
        /// 归还时间
        /// </summary>
        public string PreReturnDateString
        {
            get
            {
                return PreReturnDate?.ToString("yyyy-MM-dd");
            }
        }
    }
}
