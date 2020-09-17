using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Agv.Model
{
    /// <summary>
    /// 报警模型
    /// </summary>
    public class AlarmInfoModel : ModelBase, IAudited
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

        #region 报警车辆

        private Guid agvInfo_Id;

        /// <summary>
        /// 报警位置
        /// </summary>
        public Guid AgvInfo_Id
        {
            get { return agvInfo_Id; }
            set { Set(ref agvInfo_Id, value); }
        }
        #endregion

        #region 报警车辆

        private string agvName;

        /// <summary>
        /// 报警位置
        /// </summary>
        public string AgvName
        {
            get { return agvName; }
            set { Set(ref agvName, value); }
        }
        #endregion

        #region 报警位置

        private Guid carPosition_Id;

        /// <summary>
        /// 报警位置
        /// </summary>
        public Guid CarPosition_Id
        {
            get { return carPosition_Id; }
            set { Set(ref carPosition_Id, value); }
        }
        #endregion

        #region 报警位置

        private string carPosition_Name;

        /// <summary>
        /// 报警位置
        /// </summary>
        public string CarPosition_Name
        {
            get { return carPosition_Name; }
            set { Set(ref carPosition_Name, value); }
        }
        #endregion

        #region 报警码

        private int alarmCode;

        /// <summary>
        /// 报警码1：急停、2：防撞、3：障碍物、4：左驱动、5：右驱动、6脱轨、7低电压、8低电压停止、9：过流）
        /// </summary>
        public int AlarmCode
        {
            get { return alarmCode; }
            set { Set(ref alarmCode, value); }
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
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
