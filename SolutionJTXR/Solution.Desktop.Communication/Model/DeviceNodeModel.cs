using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Communication.Model
{
    /// <summary>
    /// 设备数据点数据模型
    /// </summary>
    public class DeviceNodeModel : ModelBase, IAudited/*, IRecyclable, ILockable*/
    {
        #region Id
        private Guid id = CombHelper.NewComb();

        /// <summary>
        /// Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        private Guid deviceServerInfoId;
        public Guid DeviceServerInfo_Id
        {
            get { return deviceServerInfoId; }
            set { Set(ref deviceServerInfoId, value); }
        }

        #region 设备服务器名称

        private string deviceServerInfoName;
        public string DeviceServerInfoName
        {
            get { return deviceServerInfoName; }
            set { Set(ref deviceServerInfoName, value); }
        }

        #endregion

        #region 数据点名称

        private string _nodeName;

        [Required(ErrorMessage = "必填项，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string NodeName
        {
            get { return _nodeName; }
            set { Set(ref _nodeName, value); }
        }
        #endregion

        #region 数据点URL

        private string _nodeUrl;

        [Required(ErrorMessage = "必填项，限200个字符"), MaxLength(200, ErrorMessage = "长度小于200个字符")]
        public string NodeUrl
        {
            get { return _nodeUrl; }
            set { Set(ref _nodeUrl, value); }
        }
        #endregion

        #region 订阅间隔
        private int _interval = 200;
        [Required(ErrorMessage = "必填，单位（毫秒）")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "无效的时间间隔,非0正整数，单位（毫秒）")]
        public int Interval
        {
            get { return _interval; }
            set { Set(ref _interval, value); }
        }
        #endregion

        #region 数据类型
        private DataType dataType;
        [Required(ErrorMessage = "必填项")]
        public DataType DataType
        {
            get { return dataType; }
            set { Set(ref dataType, value); }
        }
        #endregion

        #region 描述 

        private string _description;
        [MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }
        #endregion
        #region 是否被选中
        private bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                //Set(ref _isChecked, value);
                _isChecked = value;
                if (PropertyChangedHandler != null)
                    PropertyChangedHandler(this, new PropertyChangedEventArgs("IsChecked"));
            }
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
            DeviceServerInfoName = null;
            NodeName = null;
            NodeUrl = null;
            Description = null;
        }

    }

}
