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
        public int TransferNo { get; set; }

        public bool CanApprovalTransfer { get; set; }
        public int ApprovalTransferNo { get; set; }


        public bool CanBorrow { get; set; }
        public int BorrowReturnNo { get; set; }
        
        public bool CanApprovalBorrow { get; set; }
        public int ApprovalBorrowNo { get; set; }


        public bool CanSettle { get; set; }
        public int ApprovalSettleOutNo { get; set; }

        public bool CanApprovalSettle { get; set; }
        public int ApprovalSettleNo { get; set; }
        

    }
}
