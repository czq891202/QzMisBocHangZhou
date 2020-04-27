using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    public class WorkSpaceInfo
    {
        public bool CanTransfer { get; set; }

        public bool CanApprovalTransfer { get; set; }
        


        public bool CanBorrow { get; set; }
        
        public bool CanApprovalBorrow { get; set; }

        public bool CanSettle { get; set; }
        public int ApprovalSettleOutNo { get; set; }

        public bool CanGiveBack { get; set; }
        public bool CanApprovalGiveBack { get; set; }

        public bool CanApprovalSettle { get; set; }
        public int ApprovalSettleNo { get; set; }
        /// <summary>
        /// 待移交数量
        /// </summary>
        public int TransferNo { get; set; }
        /// <summary>
        /// 待归还数量
        /// </summary>
        public int BorrowReturnNo { get; set; }
        /// <summary>
        /// 待移交审批数量
        /// </summary>
        public int ApprovalTransferNo { get; set; }
        /// <summary>
        /// 待借阅审批数量
        /// </summary>
        public int ApprovalBorrowNo { get; set; }
        /// <summary>
        /// 移交超期数量
        /// </summary>
        public int TransferExtendedNo { get; set; }
        /// <summary>
        /// 归还超期
        /// </summary>
        public int GiveBackExtendedNo { get; set; }
        /// <summary>
        /// 移交驳回
        /// </summary>
        public int TransferRollBackNo { get; set; }
        /// <summary>
        /// 借阅驳回
        /// </summary>
        public int BorrowRollBackNo { get; set; }
        /// <summary>
        /// 归还驳回
        /// </summary>
        public int GiveBackRollBackNo { get; set; }
        /// <summary>
        /// 结清驳回
        /// </summary>
        public int SettleRollBackNo { get; set; }
    }
}
