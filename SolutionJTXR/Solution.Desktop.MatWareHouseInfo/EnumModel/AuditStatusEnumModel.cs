using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class AuditStatusEnumModel
    {
        public enum AuditStatus
        {
            /// <summary>
            /// 未审核
            /// </summary>
            [Description("未审核")]
            UnAuditStatus = 1,

            /// <summary>
            /// 审核中
            /// </summary>
            [Description("审核中")]
            AuditingStatus = 2,

            /// <summary>
            /// 审核通过
            /// </summary>
            [Description("审核通过")]
            AuditPassStatus = 3,

            /// <summary>
            /// 审核未通过
            /// </summary>
            [Description("审核未通过")]
            AuditUnPassStatus = 4,
        }
    }
}
