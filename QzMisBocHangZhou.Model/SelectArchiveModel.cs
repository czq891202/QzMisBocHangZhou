using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QzMisBocHangZhou.Model
{
    public class SelectArchiveModel
    {
        /// <summary>
        /// 档案Id
        /// </summary>
        [Description("档案Id")]
        public string Id { get; set; }


        /// <summary>
        /// 机构Id
        /// </summary>
        [Description("机构Id")]
        public string OrgId { get; set; }

 
        /// <summary>
        /// 额度号
        /// </summary>
        /// <example>PF111410000000698689789</example>
        public string QuotaNo { get; set; }


        /// <summary>
        /// 贷款账号
        /// </summary>
        /// <example>368870055263</example>
        public string LoanAccount { get; set; }


        /// <summary>
        /// 押品编号
        /// </summary>
        [Description("标签代码")]
        public string LabelCode { get; set; }


        /// <summary>
        /// 借款人
        /// </summary>
        /// <example>何XX</example>
        [Description("借款人")]
        public string Borrower { get; set; }


        /// <summary>
        /// 押品crms登记类型(就是押品名称)
        /// </summary>
        /// <example>
        //1:已办妥正式抵押登记(他项权证)
        //2:已办妥预抵押登记
        //3:已办妥备案登记
        //4:未办妥抵押（抵押）登记
        //7:不需抵质押我行
        /// </example>
        [Description("押品crms登记类型")]
        public string GuaranteeType { get; set; }


        public string OrgCode { get; set; }


        public string OrgName { get; set; }

        public int IsChecked { get; set; }
    }
}
