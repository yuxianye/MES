using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.DisTaskDispatchInfo.Model
{
    /// <summary>
    /// 盘点明细信息模型
    /// </summary>
    public class DisTaskDispatchItemInfoModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 任务ID
        private Guid? disStepTeachingTaskDispatch_Id;

        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid? DisStepTeachingTaskDispatch_Id
        {
            get { return disStepTeachingTaskDispatch_Id; }
            set { Set(ref disStepTeachingTaskDispatch_Id, value); }
        }
        #endregion

        #region 分步教学任务类别名称
        private string tasktypename;

        /// <summary>
        /// 分步教学任务类别名称
        /// </summary>
        public string TaskTypeName
        {
            get { return tasktypename; }
            set { Set(ref tasktypename, value); }
        }
        #endregion

        #region 任务结果
        private string taskResult;

        /// <summary>
        /// 任务编号
        /// </summary>
        public string TaskResult
        {
            get { return taskResult; }
            set { Set(ref taskResult, value); }
        }

        #endregion

        #region 设备ID
        private Guid? equipment_Id;

        /// <summary>
        /// 设备ID
        /// </summary>
        public Guid? Equipment_Id
        {
            get { return equipment_Id; }
            set { Set(ref equipment_Id, value); }
        }
        #endregion

        #region 设备名称
        private string equipmentName;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return equipmentName; }
            set { Set(ref equipmentName, value); }
        }
        #endregion

        #region 设备编号
        private string equipmentCode;

        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode
        {
            get { return equipmentCode; }
            set { Set(ref equipmentCode, value); }
        }
        #endregion

        #region 设备动作ID
        private Guid? equipmentAction_Id;

        /// <summary>
        /// 设备动作ID
        /// </summary>
        public Guid? EquipmentAction_Id
        {
            get { return equipmentAction_Id; }
            set { Set(ref equipmentAction_Id, value); }
        }
        #endregion

        #region 动作类别编号
        private string equipmentActionCode;

        /// <summary>
        /// 动作类别编号
        /// </summary>
        public string EquipmentActionCode
        {
            get { return equipmentActionCode; }
            set { Set(ref equipmentActionCode, value); }
        }
        #endregion

        #region 动作类别名称
        private string equipmentActionName;

        /// <summary>
        /// 动作类别名称
        /// </summary>
        public string EquipmentActionName
        {
            get { return equipmentActionName; }
            set { Set(ref equipmentActionName, value); }
        }
        #endregion


        #region 动作顺序
        private string actionOrder;

        // <summary>
        // 动作顺序
        // </summary>
        public string ActionOrder
        {
            get { return actionOrder; }
            set { Set(ref actionOrder, value); }
        }
        #endregion      

        #region 任务明细编号
        private string taskItemCode;

        /// <summary>
        /// 任务明细编号
        /// </summary>
        [Required(ErrorMessage = "任务明细编号必填"), MaxLength(100, ErrorMessage = "长度大于100个字符")]
        public string TaskItemCode
        {
            get { return taskItemCode; }
            set { Set(ref taskItemCode, value); }
        }

        #endregion


        #region 任务描述
        private string description;

        /// <summary>
        /// 任务描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度大于200个字符")]
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }
        #endregion

        #region 任务结果
        private string taskItemResult;

        /// <summary>
        /// 任务结果
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string TaskItemResult
        {
            get { return taskItemResult; }
            set { Set(ref taskItemResult, value); }
        }
        #endregion

        #region 完成时间
        private DateTime? finishTime;

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? FinishTime
        {
            get { return finishTime; }
            set { Set(ref finishTime, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度大于200个字符")]
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
            TaskResult = null;
            TaskTypeName = null;
            EquipmentActionCode = null;
            EquipmentActionName = null;
            taskItemCode = null;
            Remark = null;
            Description = null;
            TaskItemResult = null;
            FinishTime = null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
