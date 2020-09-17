using Solution.Desktop.Core;
using Solution.Desktop.EquRepairsInfo.Model;
using Solution.Desktop.EquSparePartsInfo.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquipmentInfo.Model
{
    /// <summary>
    /// 设备维修信息模型
    /// </summary>
    public class EquRepairsInfoModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 设备ID
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 设备名称
        private string equipmentName;

        /// <summary>
        /// 设备名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string EquipmentName
        {
            get { return equipmentName; }
            set { Set(ref equipmentName, value); }
        }

        #endregion

        #region 设备编号
        private string equipmentInfo_Id;

        /// <summary>
        /// 设备编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string EquipmentInfo_Id
        {
            get { return equipmentInfo_Id; }
            set { Set(ref equipmentInfo_Id, value); }
        }
        #endregion

        #region 设备
        private EquipmentInfoModel equipmentInfo;

        /// <summary>
        /// 设备
        /// </summary>
        public EquipmentInfoModel EquipmentInfo
        {
            get { return equipmentInfo; }
            set { Set(ref equipmentInfo, value); }
        }
        #endregion

        #region 维修单号
        private string repairCode;

        /// <summary>
        /// 维修单号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string RepairCode
        {
            get { return repairCode; }
            set { Set(ref repairCode, value); }
        }
        #endregion

        #region 维修日期
        private DateTime? repairDate;

        /// <summary>
        /// 维修日期
        /// </summary>
        public DateTime? RepairDate
        {
            get { return repairDate; }
            set { Set(ref repairDate, value); }
        }
        #endregion

        #region 完成日期
        private DateTime? finishDate;

        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime? FinishDate
        {
            get { return finishDate; }
            set { Set(ref finishDate, value); }
        }
        #endregion

        #region 停机时长
        private double stopDuration;

        /// <summary>
        /// 停机时长
        /// </summary>
        public double StopDuration
        {
            get { return stopDuration; }
            set { Set(ref stopDuration, value); }
        }
        #endregion

        #region 诊断时长
        private double diagnosisDuration;

        /// <summary>
        /// 诊断时长
        /// </summary>
        public double DiagnosisDuration
        {
            get { return diagnosisDuration; }
            set { Set(ref diagnosisDuration, value); }
        }
        #endregion

        #region 技术支援时长
        private double supportDuration;

        /// <summary>
        /// 技术支援时长
        /// </summary>
        public double SupportDuration
        {
            get { return supportDuration; }
            set { Set(ref supportDuration, value); }
        }
        #endregion

        #region 排障时长
        private double faultRemovingDuration;

        /// <summary>
        /// 排障时长
        /// </summary>
        public double FaultRemovingDuration
        {
            get { return faultRemovingDuration; }
            set { Set(ref faultRemovingDuration, value); }
        }
        #endregion

        #region 备件等待时长
        private double sparePartDuration;

        /// <summary>
        /// 备件等待时长
        /// </summary>
        public double SparePartDuration
        {
            get { return sparePartDuration; }
            set { Set(ref sparePartDuration, value); }
        }
        #endregion

        #region 使用备件
        private EquSparePartsModel sparePartsInfo;

        /// <summary>
        /// 使用备件
        /// </summary>
        public EquSparePartsModel SparePartsInfo
        {
            get { return sparePartsInfo; }
            set { Set(ref sparePartsInfo, value); }
        }
        #endregion

        #region 使用备件Id
        private string sparePartsInfo_Id;

        /// <summary>
        /// 使用备件Id
        /// </summary>
        public string SparePartsInfo_Id
        {
            get { return sparePartsInfo_Id; }
            set { Set(ref sparePartsInfo_Id, value); }
        }
        #endregion

        #region 使用备件名称
        private string sparePartName;

        /// <summary>
        /// 使用备件名称
        /// </summary>
        public string SparePartName
        {
            get { return sparePartName; }
            set { Set(ref sparePartName, value); }
        }
        #endregion

        #region 备件数量
        private int sparePartQuantity;

        /// <summary>
        /// 备件数量
        /// </summary>
        public int SparePartQuantity
        {
            get { return sparePartQuantity; }
            set { Set(ref sparePartQuantity, value); }
        }
        #endregion

        #region 备件费用（元）
        private double sparePartCost;

        /// <summary>
        /// 备件费用（元）
        /// </summary>
        public double SparePartCost
        {
            get { return sparePartCost; }
            set { Set(ref sparePartCost, value); }
        }
        #endregion

        #region 故障类别
        private FaultType faultType;

        /// <summary>
        /// 故障类别
        /// </summary>
        public FaultType FaultType
        {
            get { return faultType; }
            set { Set(ref faultType, value); }
        }
        #endregion

        #region 故障现象描述
        private string faultDescription;

        /// <summary>
        /// 故障现象描述
        /// </summary>
        public string FaultDescription
        {
            get { return faultDescription; }
            set { Set(ref faultDescription, value); }
        }
        #endregion

        #region 故障分析
        private string faultAnalysis;

        /// <summary>
        /// 故障分析
        /// </summary>
        public string FaultAnalysis
        {
            get { return faultAnalysis; }
            set { Set(ref faultAnalysis, value); }
        }
        #endregion

        #region 故障原因
        private string faultReason;

        /// <summary>
        /// 故障原因
        /// </summary>
        public string FaultReason
        {
            get { return faultReason; }
            set { Set(ref faultReason, value); }
        }
        #endregion

        #region 工时费用
        private double manhoursCost;

        /// <summary>
        /// 工时费用
        /// </summary>
        public double ManhoursCost
        {
            get { return manhoursCost; }
            set { Set(ref manhoursCost, value); }
        }
        #endregion

        #region 故障损失
        private double faultLossCost;

        /// <summary>
        /// 故障损失
        /// </summary>
        public double FaultLossCost
        {
            get { return faultLossCost; }
            set { Set(ref faultLossCost, value); }
        }
        #endregion

        #region 维修人
        private string repairman;

        /// <summary>
        /// 维修人
        /// </summary>
        public string Repairman
        {
            get { return repairman; }
            set { Set(ref repairman, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
        }
        #endregion

        #region 记录创建时间
        private DateTime createdTime;

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { Set(ref createdTime, value); }
        }
        #endregion

        #region 创建者编号
        private string creatorUserId;

        /// <summary>
        /// 创建者编号
        /// </summary>
        public string CreatorUserId
        {
            get { return creatorUserId; }
            set { Set(ref creatorUserId, value); }
        }
        #endregion

        #region 最后更新时间
        private DateTime? lastUpdatedTime;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdatedTime
        {
            get { return lastUpdatedTime; }
            set { Set(ref lastUpdatedTime, value); }
        }
        #endregion

        #region 最后更新者编号
        private string lastUpdatorUserId;

        /// <summary>
        /// 最后更新者编号
        /// </summary>
        public string LastUpdatorUserId
        {
            get { return lastUpdatorUserId; }
            set { Set(ref lastUpdatorUserId, value); }
        }
        #endregion

        protected override void Disposing()
        {
            EquipmentName = null;
            EquipmentInfo_Id = null;
            EquipmentInfo = null;
            RepairCode = null;
            RepairDate = null;
            FinishDate = null;
            SparePartsInfo = null;
            SparePartsInfo_Id = null;
            SparePartName = null;
            SparePartsInfo = null;
            FaultDescription = null;
            FaultAnalysis = null;
            FaultReason = null;
            Repairman = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
