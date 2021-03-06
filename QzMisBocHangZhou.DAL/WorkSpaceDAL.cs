﻿using QzMisBocHangZhou.Model;
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
                            (select count(1) from ArchiveInfo where Status = 0 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as TransferNo,
                            (select count(1) from ArchiveTransferInfo where Status = 0 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  ApprovalTransferNo,
                            (select count(1) from ArchiveBorrowInfo where status = 0 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  ApprovalBorrowNo ,
                            (select count(1) from ArchiveBorrowInfo where status = 1 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  BorrowReturnNo ,
                            (select count(1) from ArchiveSettleInfo where status = 0 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  ApprovalSettleNo ,
                            (select count(1) from ArchiveSettleInfo where status = 1 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  ApprovalSettleOutNo ,
                            (select count(1) from ArchiveTransferInfo where status = 5 and TransferDate >= sysdate and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  TransferExtendedNo ,
                            (select count(1) from ArchiveBorrowInfo where status = 5 and PreReturnDate >= sysdate and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  GiveBackExtendedNo ,
                            (select count(1) from ArchiveInfo where status = 22 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  TransferRollBackNo ,
                            (select count(1) from ArchiveInfo where status = 23 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  BorrowRollBackNo ,
                            (select count(1) from ArchiveInfo where status = 24 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  GiveBackRollBackNo ,
                            (select count(1) from ArchiveInfo where status = 25 and OrgId in (select Id from OrgInfo where IsLock = 0 start with Id IN (select column_value from table (split (:OrgId))) connect by prior Id = ParentId)) as  SettleRollBackNo  
                        from dual";
            return DBCache.DataBase.ExecuteEntity<WorkSpaceInfo>(sql, DBCache.DataBase.CreatDbParameter("OrgId", orgId));
        }
    }
}
