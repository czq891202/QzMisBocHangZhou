using System;

namespace QzMisBocHangZhou.Model
{
    public class TPArchiveInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 正式档案信息(对应ArchiveInfo表Id);
        /// </summary>
        public string OfficialArchiveId { get; set; }


        /// <summary>
        /// 担保品编号
        /// </summary>
        /// <example>
        /// 总行系统的押品编号, 30000023016425100
        /// </example>
        public string GuaranteeNo { get; set; }


        /// <summary>
        /// 贷款账号
        /// </summary>
        /// <example>368870055263</example>
        public string LoanAccount { get; set; }


        /// <summary>
        /// 额度号
        /// </summary>
        /// <example>PF111410000000698689789</example>
        public string QuotaNo { get; set; }


        /// <summary>
        /// 档案编号
        /// </summary>
        /// <example>EJHC2015AA000885</example>
        public string ArchiveNo { get; set; }


        /// <summary>
        /// 存放地
        /// </summary>
        /// <example>AAC20150000833</example>
        public string StorageLocation { get; set; }


        /// <summary>
        /// 机构代码
        /// </summary>
        /// <example>27005</example>
        public string OrgCode { get; set; }


        /// <summary>
        /// 机构名称
        /// </summary>
        /// <example>
        /// 浙江省分行营业中心
        /// </example>
        public string OrgName { get; set; }


        /// <summary>
        /// 产品码
        /// </summary>
        /// <example>PLAA</example>
        public string ProductCode { get; set; }


        /// <summary>
        /// 借款人
        /// </summary>
        /// <example>何XX</example>
        public string Borrower { get; set; }


        /// <summary>
        /// 贷款金额
        /// </summary>
        /// <example>1520000.00</example>
        public decimal? LoanAmount { get; set; }


        /// <summary>
        /// 押品crms登记类型(就是押品名称)
        /// </summary>
        /// <example>
        /// 1.已办妥正式抵押登记(他项权证)
        /// 2.已办妥预抵押登记
        /// 3.车辆登记证
        /// 4.保单
        /// 5.存单
        /// 6.公积金组合
        /// </example>
        public string GuaranteeType { get; set; }


        /// <summary>
        /// 押品crms入库状态
        /// </summary>
        /// <example>
        /// 入库
        /// 变更出库
        /// 处置出库
        /// 借阅出库
        /// 变更结清
        /// 结清
        /// </example>
        public string ArchiveStatus { get; set; }


        /// <summary>
        /// 担保品证件号(权证号)
        /// </summary>
        /// <example>
        /// 浙（2018）杭州市不动产证明第0050818号
        /// </example>
        public string GuaranteeCredentialNo { get; set; }
        

        /// <summary>
        /// 经办人
        /// </summary>
        /// <example>
        /// 薛寒
        /// </example>
        public string Transactor { get; set; }


        /// <summary>
        /// 贷款发放日
        /// </summary>
        /// <example>
        /// 20151209
        /// </example>
        public string LoanReleaseDate { get; set; }


        /// <summary>
        /// 押品价值
        /// </summary>
        /// <example>
        /// 2180000.00 
        /// </example>
        public decimal? MortgageValue { get; set; }


        /// <summary>
        /// 合同号
        /// </summary>
        /// <remarks>
        /// 在三级页面取不到，可考虑空着
        /// </remarks>
        public string ContractNo { get; set; }


        /// <summary>
        /// 房屋所在地
        /// </summary>
        /// <remarks>
        /// 最近的有，以前的没有，有也不准
        /// </remarks>
        public string HouseLocation { get; set; }


        /// <summary>
        /// 客户经理
        /// </summary>
        /// <remarks>
        /// 在三级页面取不到，可考虑空着
        /// </remarks>
        public string AccountManager { get; set; }


        /// <summary>
        /// 拟还日期
        /// </summary>
        /// <example>
        /// 20180915
        /// </example>
        public DateTime? PreReturnDate { get; set; }


        /// <summary>
        /// 归还日期
        /// </summary>
        /// <remarks>20180915</remarks>
        public DateTime? ReturnDate { get; set; }
        

        /// <summary>
        /// 修改日期
        /// </summary>
        /// <example>
        /// 20180731
        /// </example>
        public DateTime? EditDate { get; set; }


        /// <summary>
        /// 客户号
        /// </summary>
        public string CustomerNo { get; set; }
    }
}
