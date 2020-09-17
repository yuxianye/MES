using OSharp.Core.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.EquipmentManagement.Models
{
    [Description("设备维修信息")]
    public class EquRepairsInfo : EntityBase<Guid>, /*ILockable, IRecyclable, */IAudited
    {
        /// <summary>
        /// 设备信息
        /// </summary>
        public virtual EquEquipmentInfo EquipmentInfo{ get; set; }

        /// <summary>
        /// 维修单号
        /// </summary>
        [StringLength(50)]
        public string RepairCode { get; set; }

        /// <summary>
        /// 维修日期
        /// </summary>
        public DateTime RepairDate { get; set; }

        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// 停机时长
        /// </summary>
        public double StopDuration { get; set; }

        /// <summary>
        /// 诊断时长
        /// </summary>
        public double DiagnosisDuration { get; set; }

        /// <summary>
        /// 技术支援时长
        /// </summary>
        public double SupportDuration { get; set; }

        /// <summary>
        /// 排障时长
        /// </summary>
        public double FaultRemovingDuration { get; set; }

        /// <summary>
        /// 备件等待时长
        /// </summary>
        public double SparePartDuration { get; set; }

        /// <summary>
        /// 使用备件
        /// </summary>
        public virtual EquSparePartsInfo SparePartsInfo { get; set; }

        /// <summary>
        /// 备件数量
        /// </summary>
        public int SparePartQuantity { get; set; }

        /// <summary>
        /// 备件费用（元）
        /// </summary>
        public double SparePartCost { get; set; }

        /// <summary>
        /// 故障类别
        /// </summary>
        public FaultType faultType { get; set; }

        /// <summary>
        /// 故障现象描述
        /// </summary>
        [StringLength(100)]
        public string FaultDescription { get; set; }

        /// <summary>
        /// 故障分析
        /// </summary>
        [StringLength(100)]
        public string FaultAnalysis { get; set; }

        /// <summary>
        /// 故障原因
        /// </summary>
        [StringLength(50)]
        public string FaultReason { get; set; }

        /// <summary>
        /// 工时费用
        /// </summary>
        public double ManhoursCost { get; set; }

        /// <summary>
        /// 故障损失
        /// </summary>
        public double FaultLossCost { get; set; }

        /// <summary>
        /// 维修人
        /// </summary>
        [StringLength(50)]
        public string Repairman { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        #endregion

        #region Implementation of ICreatedAudited

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        [StringLength(50)]
        public string CreatorUserId { get; set; }

        #endregion

        #region Implementation of IUpdateAutited

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Index]

        public DateTime? LastUpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
