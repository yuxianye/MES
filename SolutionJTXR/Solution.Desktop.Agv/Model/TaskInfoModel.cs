using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Agv.Model
{
    /// <summary>
    /// 任务模型
    /// </summary>
    public class TaskInfoModel : ModelBase, IAudited
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

        #region 任务名称
        private string taskName;

        /// <summary>
        /// 任务名称
        /// </summary>
        [Required(ErrorMessage = "必填，长度小于50个字符，不能重复！"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string TaskName
        {
            get { return taskName; }
            set { Set(ref taskName, value); }
        }
        #endregion

        #region 任务编号
        private int taskNo;

        /// <summary>
        /// 任务编号
        /// </summary>
        public int TaskNo
        {
            get { return taskNo; }
            set { Set(ref taskNo, value); }
        }
        #endregion

        //#region 任务类型
        //private int tasktype;

        ///// <summary>
        ///// 任务类型
        ///// </summary>
        //public int TaskType
        //{
        //    get { return tasktype; }
        //    set { Set(ref tasktype, value); }
        //}
        //#endregion

        #region 数据类型
        private TaskType taskType;
        [Required(ErrorMessage = "必填项")]
        public TaskType TaskType
        {
            get { return taskType; }
            set { Set(ref taskType, value); }
        }
        #endregion

        //#region 任务状态
        //private int taskStatus;

        ///// <summary>
        ///// 任务状态
        ///// </summary>
        //public int TaskStatus
        //{
        //    get { return taskStatus; }
        //    set { Set(ref taskStatus, value); }
        //}
        //#endregion

        #region 数据类型
        private TaskStatus taskStatus;
        [Required(ErrorMessage = "必填项")]
        public TaskStatus TaskStatus
        {
            get { return taskStatus; }
            set { Set(ref taskStatus, value); }
        }
        #endregion

        #region 目标地标点
        private Guid targetPointInfo_Id;

        /// <summary>
        /// 目标地标点
        /// </summary>
        [Required(ErrorMessage = "必选！")]
        public Guid TargetPointInfo_Id
        {
            get { return targetPointInfo_Id; }
            set { Set(ref targetPointInfo_Id, value); }
        }
        #endregion

        #region 目标地标点
        private string targetPointInfoName;

        /// <summary>
        /// 目标地标点
        /// </summary>
        public string TargetPointInfoName
        {
            get { return targetPointInfoName; }
            set { Set(ref targetPointInfoName, value); }
        }
        #endregion

        #region 任务小车
        private Guid taskCar_Id;

        /// <summary>
        /// 任务小车
        /// </summary>
        [Required(ErrorMessage = "必选！")]
        public Guid TaskCar_Id
        {
            get { return taskCar_Id; }
            set { Set(ref taskCar_Id, value); }
        }
        #endregion

        #region 任务小车
        private string taskCarName;

        /// <summary>
        /// 任务小车
        /// </summary>
        public string TaskCarName
        {
            get { return taskCarName; }
            set { Set(ref taskCarName, value); }
        }
        #endregion

        #region 路径库
        private Guid routeInfo_Id;

        /// <summary>
        /// 路径库
        /// </summary>
        [Required(ErrorMessage = "必选！")]
        public Guid RouteInfo_Id
        {
            get { return routeInfo_Id; }
            set { Set(ref routeInfo_Id, value); }
        }
        #endregion

        #region 路径库
        private string routeInfoName;

        /// <summary>
        /// 路径库
        /// </summary>
        public string RouteInfoName
        {
            get { return routeInfoName; }
            set { Set(ref routeInfoName, value); }
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
            taskName = null;
            TargetPointInfoName = null;
            TaskCarName = null;
            RouteInfoName = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
