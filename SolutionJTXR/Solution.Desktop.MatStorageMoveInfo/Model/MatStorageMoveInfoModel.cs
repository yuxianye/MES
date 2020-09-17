using Solution.Desktop.Core;
using Solution.Desktop.MaterialBatchInfo.Model;
using Solution.Desktop.RoleManager.Model;
using Solution.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.AuditStatusEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.InStorageStatusEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.InStorageTypeEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.StorageMoveStateEnumModel;

namespace Solution.Desktop.MatStorageMoveInfo.Model
{
    /// <summary>
    /// 移库信息模型
    /// </summary>
    public class MatStorageMoveInfoModel : ModelBase, IAudited
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

        #region 移库编号
        private string storagemoveCode;

        /// <summary>
        /// 移库编号
        /// </summary>
        [Required(ErrorMessage = "移库编号必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string StorageMoveCode
        {
            get { return storagemoveCode; }
            set { Set(ref storagemoveCode, value); }
        }

        #endregion


        #region 移库原因
        private string storagemoveReason;

        /// <summary>
        /// 移库原因
        /// </summary>
        [Required(ErrorMessage = "移库原因必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string StorageMoveReason
        {
            get { return storagemoveReason; }
            set { Set(ref storagemoveReason, value); }
        }

        #endregion

        #region 原库位ID
        private Guid? fromlocationID;

        /// <summary>
        /// 原库位ID
        /// </summary>
        public Guid? FromLocationID
        {
            get { return fromlocationID; }
            set { Set(ref fromlocationID, value); }
        }
        #endregion

        #region 原库位编号
        private string fromwarehouselocationCode;

        /// <summary>
        /// 原库位编号
        /// </summary>
        public string FromWareHouseLocationCode
        {
            get { return fromwarehouselocationCode; }
            set { Set(ref fromwarehouselocationCode, value); }
        }

        #endregion

        #region 原库位名称
        private string fromwarehouselocationName;

        /// <summary>
        /// 原库位名称
        /// </summary>
        public string FromWareHouseLocationName
        {
            get { return fromwarehouselocationName; }
            set { Set(ref fromwarehouselocationName, value); }
        }
        #endregion

        #region 移动到库位ID
        private Guid? tolocationID;

        /// <summary>
        /// 移动到库位ID
        /// </summary>
        public Guid? ToLocationID
        {
            get { return tolocationID; }
            set { Set(ref tolocationID, value); }
        }
        #endregion

        #region 新库位编号
        private string towarehouselocationCode;

        /// <summary>
        /// 新库位编号
        /// </summary>
        public string ToWareHouseLocationCode
        {
            get { return towarehouselocationCode; }
            set { Set(ref towarehouselocationCode, value); }
        }

        #endregion

        #region 新库位名称
        private string towarehouselocationName;

        /// <summary>
        /// 新库位名称
        /// </summary>
        public string ToWareHouseLocationName
        {
            get { return towarehouselocationName; }
            set { Set(ref towarehouselocationName, value); }
        }
        #endregion

        #region 操作人员
        private string operatorPerson;

        /// <summary>
        /// 操作人员
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string Operator
        {
            get { return operatorPerson; }
            set { Set(ref operatorPerson, value); }
        }
        #endregion


        #region 移库状态
        private StorageMoveState storagemoveState;

        /// <summary>
        /// 移库状态
        /// </summary>
        public StorageMoveState StorageMoveState
        {
            get { return storagemoveState; }
            set { Set(ref storagemoveState, value); }
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
            Remark = null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
