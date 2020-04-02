using QzMisBocHangZhou.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.DAL
{
    public class WorkSpaceDAL
    {
        public static WorkSpaceInfo GetWorkSpaceInfo(string orgId)
        {
            var sql = @"select 
                            (select count(1) from ArchiveInfo where Status = 0 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId)) as TransferNo,
                            (select count(1) from ArchiveTransferInfo where Status = 0 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId)) as  ApprovalTransferNo,
                            (select count(1) from ArchiveBorrowInfo where status = 0 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId)) as  ApprovalBorrowNo ,
                            (select count(1) from ArchiveBorrowInfo where status = 1 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId)) as  BorrowReturnNo ,
                            (select count(1) from ArchiveSettleInfo where status = 0 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId)) as  ApprovalSettleNo ,
                            (select count(1) from ArchiveSettleInfo where status = 1 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id = :OrgId connect by prior Id = ParentId)) as  ApprovalSettleOutNo 
                        from dual";

            return DBCache.DataBase.ExecuteEntity<WorkSpaceInfo>(sql, DBCache.DataBase.CreatDbParameter("OrgId", orgId));
        }
    }
}
