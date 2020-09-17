using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Solution.Desktop.CommOpcUaNode.Model;

namespace Solution.Desktop.CommOpcUaBusiness.Model
{
    /// <summary>
    /// OpcUa设备业务数据点关联数据模型
    /// </summary>
    public class CommOpcUaBusinessNodeMapModel : ModelBase, IAudited
    {
        #region Id
        private Guid id ;

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

        private Guid _opcUaBusiness_Id;

        public Guid OpcUaBusiness_Id
        {
            get { return _opcUaBusiness_Id; }
            set { Set(ref _opcUaBusiness_Id, value); }
        }
        #endregion

        #region Opc Ua 业务名称

        private string _businessName;

        public string BusinessName
        {
            get { return _businessName; }
            set { Set(ref _businessName, value); }
        }
        #endregion

        #region 数据点ID

        private Guid _opcUaNode_Id;

        public Guid OpcUaNode_Id
        {
            get { return _opcUaNode_Id; }
            set { Set(ref _opcUaNode_Id, value); }
        }
        #endregion

        #region 数据点名称

        private string _nodeName;

        public string NodeName
        {
            get { return _nodeName; }
            set { Set(ref _nodeName, value); }
        }
        #endregion
        
        #region 设备ID

        private Guid _equipmentID;

        public Guid EquipmentID
        {
            get { return _equipmentID; }
            set { Set(ref _equipmentID, value); }
        }
        #endregion

        #region 设备名称

        private string _equipmentName;

        public string EquipmentName
        {
            get { return _equipmentName; }
            set { Set(ref _equipmentName, value); }
        }
        #endregion

        #region 数据点列表
        private ObservableCollection<CommOpcUaNodeModel> _commOpcUaNodeInfoList = new ObservableCollection<CommOpcUaNodeModel>();
        public ObservableCollection<CommOpcUaNodeModel> CommOpcUaNodeInfoList
        {
            get { return _commOpcUaNodeInfoList; }
            set { Set(ref _commOpcUaNodeInfoList, value); }
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
            BusinessName = null;
            NodeName = null;
            OpcUaBusiness_Id = Guid.Empty;
            OpcUaNode_Id = Guid.Empty;
            EquipmentID = Guid.Empty;
            EquipmentName = null;
        }

    }

}
