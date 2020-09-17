using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    /// <summary>
    /// 仓库模型
    /// </summary>
    public class MatWareHouseInfoModel : ModelBase, IAudited
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

        /// ////////////////////////////////////////////////////////
        /// 


        #region 企业编号
        private string enterpriseCode;

        // <summary>
        // 企业编号
        // </summary>
        public string EnterpriseCode
        {
            get { return enterpriseCode; }
            set { Set(ref enterpriseCode, value); }
        }
        #endregion

        #region 企业名称
        private string enterpriseName;

        // <summary>
        // 企业名称
        // </summary>
        public string EnterpriseName
        {
            get { return enterpriseName; }
            set { Set(ref enterpriseName, value); }
        }
        #endregion


        #region 厂区编号
        private string entsiteCode;

        // <summary>
        // 厂区编号
        // </summary>
        public string EntSiteCode
        {
            get { return entsiteCode; }
            set { Set(ref entsiteCode, value); }
        }
        #endregion

        #region 厂区名称
        private string entsiteName;

        // <summary>
        // 厂区名称
        // </summary>
        public string EntSiteName
        {
            get { return entsiteName; }
            set { Set(ref entsiteName, value); }
        }
        #endregion

        #region 所属区域ID
        private Guid entarea_Id;
        public Guid EntArea_Id
        {
            get { return entarea_Id; }
            set { Set(ref entarea_Id, value); }
        }
        #endregion

        #region 区域编号
        private string entareaCode;

        // <summary>
        // 区域编号
        // </summary>
        public string EntAreaCode
        {
            get { return entareaCode; }
            set { Set(ref entareaCode, value); }
        }
        #endregion

        #region 区域名称
        private string entareaName;

        // <summary>
        // 区域名称
        // </summary>
        public string EntAreaName
        {
            get { return entareaName; }
            set { Set(ref entareaName, value); }
        }
        #endregion

        /// ////////////////////////////////////////////////////////

        #region 仓库类型ID
        private Guid matwarehousetype_Id;
        public Guid MatWareHouseType_Id
        {
            get { return matwarehousetype_Id; }
            set { Set(ref matwarehousetype_Id, value); }
        }
        #endregion

        #region 仓库类型编号
        private string warehousetypeCode;

        // <summary>
        // 仓库类型编号
        // </summary>
        public string WareHouseTypeCode
        {
            get { return warehousetypeCode; }
            set { Set(ref warehousetypeCode, value); }
        }
        #endregion

        #region 仓库类型名称
        private string warehousetypeName;

        // <summary>
        // 仓库类型名称
        // </summary>
        public string WareHouseTypeName
        {
            get { return warehousetypeName; }
            set { Set(ref warehousetypeName, value); }
        }
        #endregion      

        /// ////////////////////////////////////////////////////////

        #region 仓库编号
        private string warehouseCode;

        /// <summary>
        /// 仓库编号
        /// </summary>
        [Required(ErrorMessage = "仓库编号必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]

        public string WareHouseCode
        {
            get { return warehouseCode; }
            set { Set(ref warehouseCode, value); }
        }

        #endregion

        #region 仓库名称
        private string warehouseName;

        /// <summary>
        /// 仓库名称
        /// </summary>
        [Required(ErrorMessage = "仓库名称必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string WareHouseName
        {
            get { return warehouseName; }
            set { Set(ref warehouseName, value); }
        }
        #endregion

        #region 仓库描述
        private string description;

        /// <summary>
        /// 仓库描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }
        #endregion

        #region 仓库负责人
        private string manager;

        /// <summary>
        /// 仓库负责人
        /// </summary>
        public string Manager
        {
            get { return manager; }
            set { Set(ref manager, value); }
        }
        #endregion

        #region 仓库电话
        private string warehousePhone;

        /// <summary>
        /// 仓库电话
        /// </summary>
        //[Required(ErrorMessage = "电话号码必填"), MaxLength(11, ErrorMessage = "")]
        [RegularExpression(@"^[1][3,4,5,6,7,8,9][0-9]{9}$", ErrorMessage = "无效的电话号码")]
        public string WareHousePhone
        {
            get { return warehousePhone; }
            set { Set(ref warehousePhone, value); }
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
            EntArea_Id = Guid.Empty;
            EntAreaCode = null;
            EntAreaName = null;
            //
            MatWareHouseType_Id = Guid.Empty;
            WareHouseTypeCode = null;
            WareHouseTypeName = null;
            //
            WareHouseCode = null;
            WareHouseName = null;
            Description = null;
            Manager = null;
            WareHousePhone = null;
            Remark = null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
