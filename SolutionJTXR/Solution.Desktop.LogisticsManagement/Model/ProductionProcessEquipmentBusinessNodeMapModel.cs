using Solution.Desktop.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.LogisticsManagement.Model
{
    /// <summary>
    /// 设备业务数据点关联数据模型
    /// </summary>
    public class ProductionProcessEquipmentBusinessNodeMapModel : ModelBase, IAudited
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

        #region 业务编号

        private Guid _businessNode_Id;

        public Guid BusinessNode_Id
        {
            get { return _businessNode_Id; }
            set { Set(ref _businessNode_Id, value); }
        }
        #endregion

        #region 工序ID

        private Guid _productionProcessInfo_Id;

        public Guid ProductionProcessInfo_Id
        {
            get { return _productionProcessInfo_Id; }
            set { Set(ref _productionProcessInfo_Id, value); }
        }
        #endregion

        #region 业务名称

        private string _businessName;

        public string BusinessName
        {
            get { return _businessName; }
            set { Set(ref _businessName, value); }
        }
        #endregion

        #region 业务描述

        private string _description;

        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }
        #endregion

        #region 数据点ID

        private Guid _deviceNode_Id;

        public Guid DeviceNode_Id
        {
            get { return _deviceNode_Id; }
            set { Set(ref _deviceNode_Id, value); }
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

        #region 数据点类型

        private DataType dataType;

        public DataType DataType
        {
            get { return dataType; }
            set { Set(ref dataType, value); }
        }
        #endregion

        #region 数据点地址

        private string nodeUrl;

        public string NodeUrl
        {
            get { return nodeUrl; }
            set { Set(ref nodeUrl, value); }
        }
        #endregion

        #region 设备ID

        private Guid? _equipment_Id;

        public Guid? Equipment_Id
        {
            get { return _equipment_Id; }
            set { Set(ref _equipment_Id, value); }
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

        //#region 数据点列表
        //private ObservableCollection<DeviceNodeModel> deviceNodeList = new ObservableCollection<DeviceNodeModel>();
        //public ObservableCollection<DeviceNodeModel> DeviceNodeList
        //{
        //    get { return deviceNodeList; }
        //    set { Set(ref deviceNodeList, value); }
        //}
        //#endregion

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
            BusinessNode_Id = Guid.Empty;
            DeviceNode_Id = Guid.Empty;
            Equipment_Id = Guid.Empty;
            EquipmentName = null;
        }

    }
}
