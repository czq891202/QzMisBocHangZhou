using System;

namespace QzMisBocHangZhou.Model
{
    public class ArchiveInfo
    {
        /// <summary>
        /// 档案Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 押品标签号
        /// </summary>
        public string LabelCode { get; set; }

        /// <summary>
        /// 存放地
        /// </summary>
        /// <example>AAC20150000833</example>
        public string StorageLocation { get; set; }

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
        //1:已办妥正式抵押登记(他项权证)
        //2:已办妥预抵押登记
        //3:已办妥备案登记
        //4:未办妥抵押（抵押）登记
        //7:不需抵质押我行
        /// </example>
        public string GuaranteeType { get; set; }

        /// <summary>
        /// 押品状态
        /// </summary>
        public ArchiveStatusType Status { get; set; } = ArchiveStatusType.草稿;

        /// <summary>
        /// 担保品证件号(权证号)
        /// </summary>
        /// <example>
        /// 浙（2018）杭州市不动产证明第0050818号
        /// </example>
        public string GuaranteeCrdNo { get; set; }

        /// <summary>
        /// 总行担保品编号
        /// </summary>
        /// <example>
        /// 总行系统的押品编号, 30000023016425100
        /// </example>
        public string TPGuaranteeNo { get; set; }

        /// <summary>
        /// 总行档案编号
        /// </summary>
        /// <example>EJHC2015AA000885</example>
        public string TPArchiveNo { get; set; }

        /// <summary>
        /// 产品大类码
        /// </summary>
        /// <example>PLAA</example>
        public string ProductCode { get; set; }

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
        public string TPContractNo { get; set; }

        /// <summary>
        /// 押品信息(房产为地址信息)
        /// </summary>
        /// <remarks>
        /// 最近的有，以前的没有，有也不准(房产为地址信息)
        /// </remarks>
        public string MortgageDetailsInfo { get; set; }

        /// <summary>
        /// 客户经理
        /// </summary>
        /// <remarks>
        /// 在三级页面取不到，可考虑空着
        /// </remarks>
        public string AccountManager { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? LastEditDate { get; set; }

        /// <summary>
        /// 客户号
        /// </summary>
        public string CustomerNo { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        /// <example>
        /// 20180731
        /// </example>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 机构代码
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string SeqNo { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 额度用款编号
        /// </summary>
        public string CreditsNo { get; set; }

        /// <summary>
        /// 核心产品码
        /// </summary>
        public string CoreProductNo { get; set; }

        /// <summary>
        /// 五级分类(自动)
        /// </summary>
        public string LvlFiveAuto { get; set; }

        /// <summary>
        /// 五级分类(手工)
        /// </summary>
        public string LvlFiveManual { get; set; }

        /// <summary>
        /// 贷款余额
        /// </summary>
        public decimal? LoanBalance { get; set; }

        /// <summary>
        /// 贷款期限
        /// </summary>
        public string LoanTerm { get; set; }

        /// <summary>
        /// 贷款到期日
        /// </summary>
        public string LoanMaturityDate { get; set; }

        /// <summary>
        /// 贷款状态
        /// </summary>
        public string LoanStatus { get; set; }

        /// <summary>
        /// 贷款状态修改时间
        /// </summary>
        public string LoanStatusEditDate { get; set; }

        /// <summary>
        /// 额度状态
        /// </summary>
        public string CreditStatus { get; set; }

        /// <summary>
        /// 额度状态修改时间
        /// </summary>
        public string CreditStatusEditDate { get; set; }

        /// <summary>
        /// 核销标识
        /// </summary>
        public string WriteOffStatus { get; set; }

        /// <summary>
        /// 证券化标识
        /// </summary>
        public string SecuritizationStatus { get; set; }

        /// <summary>
        /// 关联账号（多笔贷款对应一个押品的情况）
        /// </summary>
        public string LinkedAccount { get; set; }
    }

    /// <summary>
    /// 档案状态
    /// </summary>
    public enum ArchiveStatusType
    {
        草稿 = 0,
        已入库 = 1,
        变更出库 = 3,
        借阅审核中 = 4,
        借阅出库 = 5,
        归还审核中 = 6,
        结清审核中=10,
        已结清 = 11,
        处置出库 = 19,
        变更结清出库 = 21,
        移交驳回 = 22,
        借阅驳回 = 23,
        归还驳回 = 24,
        结清驳回 = 25
    }
}
