using OSharp.Core.Data;
using Solution.EquipmentManagement.Models;
using System;

namespace Solution.EquipmentManagement.Dtos
{
    public class EquRepairsInfoInputDto : /* ILockable, IRecyclable,*/ IInputDto<Guid>, IAudited
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public virtual EquEquipmentInfo EquipmentInfo { get; set; }

        /// <summary>
        /// 维修单号
        /// </summary>
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
        public string FaultDescription { get; set; }

        /// <summary>
        /// 故障分析
        /// </summary>
        public string FaultAnalysis { get; set; }

        /// <summary>
        /// 故障原因
        /// </summary>
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
        public string Repairman { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>

        public DateTime? LastUpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        public string LastUpdatorUserId { get; set; }

    }
}
