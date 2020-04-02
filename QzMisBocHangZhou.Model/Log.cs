using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    [Description("日志")]
    public class Log
    {
        [Description("Id")]
        public string Id { get; set; }


        [Description("用户Id")]
        public string UserId { get; set; }


        [Description("用户名")]
        public string UserName { get; set; }


        [Description("菜单名称")]
        public string NavName { get; set; }


        [Description("操作类别")]
        public ActionType ActionType { get; set; }


        [Description("操作结果")]
        public string Remark { get; set; }


        [Description("用户IP")]
        public string UserIP { get; set; }


        [Description("操作时间")]
        public DateTime AddTime { get; set; }
    }


    public enum ActionType {[Description("显示")] Show = 1, [Description("查看")] View, [Description("添加")] Add, [Description("修改")] Edit, [Description("删除")] Delete, [Description("审核")] Audit, [Description("登录")] Login = 110, [Description("监管")] Supervision }
}
