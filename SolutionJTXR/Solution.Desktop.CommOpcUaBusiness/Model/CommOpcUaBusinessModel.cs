using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.CommOpcUaBusiness.Model
{
    /// <summary>
    /// OpcUa业务数据模型
    /// </summary>
    public class CommOpcUaBusinessModel : ModelBase, IAudited
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

        //#region Opc Ua 业务编号

        //public int _businessId;

        //[Required(ErrorMessage = "OpcUa业务编号必填")]
        //public int BusinessId
        //{
        //    get { return _businessId; }
        //    set { Set(ref _businessId, value); }
        //}
        //#endregion

        #region Opc Ua 业务名称

        private string _businessName;

        [Required(ErrorMessage = "必填,长度小于20个字符"), MaxLength(20, ErrorMessage = "长度小于20个字符")]
        public string BusinessName
        {
            get { return _businessName; }
            set { Set(ref _businessName, value); }
        }
        #endregion

        //#region 数据点ID

        //public Guid _nodeId;

        //public Guid NodeId
        //{
        //    get { return _nodeId; }
        //    set { Set(ref _nodeId, value); }
        //}
        //#endregion

        //#region 数据点名称

        //public string _nodeName;

        //public string NodeName
        //{
        //    get { return _nodeName; }
        //    set { Set(ref _nodeName, value); }
        //}
        //#endregion

        #region 描述 
        private string _description;
        [MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
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
            Description = null;
        }

    }

}
