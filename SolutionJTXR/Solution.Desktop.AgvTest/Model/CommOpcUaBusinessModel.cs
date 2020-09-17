using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.AgvTest.Model
{
    /// <summary>
    /// OpcUa业务数据模型
    /// </summary>
    public class CommOpcUaBusinessModel : ModelBase, ILockable, IAudited, IRecyclable
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

        #region Opc Ua 业务编号

        public int _businessId;

        [Required(ErrorMessage = "OpcUa业务编号必填")]
        public int BusinessId
        {
            get { return _businessId; }
            set { Set(ref _businessId, value); }
        }
        #endregion

        #region Opc Ua 业务名称

        public string _businessName;

        [Required(ErrorMessage = "OpcUa业务名称必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string BusinessName
        {
            get { return _businessName; }
            set { Set(ref _businessName, value); }
        }
        #endregion

        #region 数据点ID

        public Guid _nodeId;

        public Guid NodeId
        {
            get { return _nodeId; }
            set { Set(ref _nodeId, value); }
        }
        #endregion

        #region 数据点名称

        public string _nodeName;

        public string NodeName
        {
            get { return _nodeName; }
            set { Set(ref _nodeName, value); }
        }
        #endregion

        #region 数据点值

        public string _value;

        public string Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }
        #endregion

        #region 描述 

        public string _description;

        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }
        #endregion

        #region 是否锁定
        private bool isLocked;

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked
        {
            get { return isLocked; }
            set { Set(ref isLocked, value); }
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

        #region 是否逻辑删除
        private bool isDeleted;

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { Set(ref isDeleted, value); }
        }
        #endregion

        protected override void Disposing()
        {
        }

    }

}
