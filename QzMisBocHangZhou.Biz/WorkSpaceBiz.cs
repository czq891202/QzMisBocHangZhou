using QzMisBocHangZhou.DAL;
using QzMisBocHangZhou.Model;
using System;

namespace QzMisBocHangZhou.Biz
{
    public class WorkSpaceBiz
    {
        public static WorkSpaceInfo GetWorkSpaceInfo(UserInfo user)
        {
            var orgId = user.OrgId;
            if (string.IsNullOrWhiteSpace(user.OrgId)) orgId = OrgInfo.RootId;

            var data = WorkSpaceDAL.GetWorkSpaceInfo(orgId);

            var navList = NavigationBiz.GetByRole(user.RoleId);
            data.CanTransfer = navList.Exists(p => !string.IsNullOrWhiteSpace(p.LinkUrl) && p.LinkUrl.Equals("../Transfer/ListView", StringComparison.OrdinalIgnoreCase));
            data.CanBorrow = navList.Exists(p => !string.IsNullOrWhiteSpace(p.LinkUrl) && p.LinkUrl.Equals("../Borrow/ListView", StringComparison.OrdinalIgnoreCase));
            data.CanSettle = navList.Exists(p => !string.IsNullOrWhiteSpace(p.LinkUrl) && p.LinkUrl.Equals("../Settle/ListView", StringComparison.OrdinalIgnoreCase));

            data.CanApprovalTransfer = navList.Exists(p => !string.IsNullOrWhiteSpace(p.LinkUrl) && p.LinkUrl.Equals("../Transfer/ApprovalListView", StringComparison.OrdinalIgnoreCase));
            data.CanApprovalBorrow = navList.Exists(p => !string.IsNullOrWhiteSpace(p.LinkUrl) && p.LinkUrl.Equals("../Borrow/ApprovalListView", StringComparison.OrdinalIgnoreCase));
            data.CanApprovalSettle = navList.Exists(p => !string.IsNullOrWhiteSpace(p.LinkUrl) && p.LinkUrl.Equals("../Settle/ApprovalListView", StringComparison.OrdinalIgnoreCase));

            return data;
        }
    }
}
