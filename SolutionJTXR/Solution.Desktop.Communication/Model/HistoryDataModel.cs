using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Communication.Model
{
    /// <summary>
    /// 历史数据模型
    /// </summary>
    public class HistoryDataModel : ModelBase
    {
        #region Id
        private long id;

        /// <summary>
        /// Id
        /// </summary>

        public long Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 服务器ID
        public string deviceServer_Id;

        public string DeviceServer_Id
        {
            get { return deviceServer_Id; }
            set { Set(ref deviceServer_Id, value); }
        }
        #endregion

        #region 服务器名称
        public string deviceServerName;
        public string DeviceServerName
        {
            get { return deviceServerName; }
            set { Set(ref deviceServerName, value); }
        }
        #endregion

        #region 数据点ID
        public string deviceNode_Id;

        public string DeviceNode_Id
        {
            get { return deviceNode_Id; }
            set { Set(ref deviceNode_Id, value); }
        }
        #endregion

        #region 数据点名称
        public string nodeName;
        public string NodeName
        {
            get { return nodeName; }
            set { Set(ref nodeName, value); }
        }
        #endregion

        #region 数据值
        public string _dataValue;
        public String DataValue
        {
            get { return _dataValue; }
            set { Set(ref _dataValue, value); }
        }
        #endregion

        #region 数据质量
        private int? quality;
        /// <summary>
        /// 数据质量
        /// </summary>
        public int? Quality
        {
            get { return quality; }
            set { Set(ref quality, value); }
        }
        #endregion

        #region 记录创建时间
        private Nullable<DateTime> createdTime;

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public Nullable<DateTime> CreatedTime
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

        protected override void Disposing()
        {
        }

    }

}
