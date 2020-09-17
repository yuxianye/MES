using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Agv.Model
{
    /// <summary>
    /// Agv车辆模型
    /// </summary>
    public class AgvInfoModel : ModelBase, IAudited
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

        #region 车辆编号
        private string carNo;

        /// <summary>
        /// 车辆编号
        /// </summary>
        [Required(ErrorMessage = "必填，长度小于50个字符，不能重复！"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string CarNo
        {
            get { return carNo; }
            set { Set(ref carNo, value); }
        }
        #endregion

        #region 车辆名称
        private string carName;

        /// <summary>
        /// 车辆名称
        /// </summary>
        [Required(ErrorMessage = "必填，长度小于50个字符，不能重复！"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string CarName
        {
            get { return carName; }
            set { Set(ref carName, value); }
        }
        #endregion

        #region 所属区域
        private int areaId;

        /// <summary>
        /// 所属区域
        /// </summary>
        public int AreaId
        {
            get { return areaId; }
            set { Set(ref areaId, value); }
        }
        #endregion

        #region 所属产线
        private int productLineId;

        /// <summary>
        /// 所属产线
        /// </summary>
        public int ProductLineId
        {
            get { return productLineId; }
            set { Set(ref productLineId, value); }
        }
        #endregion

        #region 设定速度
        private float settingSpeed;

        /// <summary>
        /// 设定速度
        /// </summary>
        public float SettingSpeed
        {
            get { return settingSpeed; }
            set { Set(ref settingSpeed, value); }
        }
        #endregion

        #region 通行优先级
        private int priority;

        /// <summary>
        /// 通行优先级
        /// </summary>
        public int Priority
        {
            get { return priority; }
            set { Set(ref priority, value); }
        }
        #endregion

        #region 报警电量
        private int alarmPowerLevel;

        /// <summary>
        /// 报警电量
        /// </summary>
        public int AlarmPowerLevel
        {
            get { return alarmPowerLevel; }
            set { Set(ref alarmPowerLevel, value); }
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
            CarNo = null;
            CarName = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
