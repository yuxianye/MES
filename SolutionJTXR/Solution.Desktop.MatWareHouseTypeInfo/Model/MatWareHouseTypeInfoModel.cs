using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.MatWareHouseTypeInfo.Model
{
    /// <summary>
    /// 仓库类型模型
    /// </summary>
    public class MatWareHouseTypeInfoModel : ModelBase, IAudited
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

        #region 仓库类型编号
        private string warehousetypeCode;

        /// <summary>
        /// 仓库类型编号
        /// </summary>
        [Required(ErrorMessage = "仓库类型编号必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]

        public string WareHouseTypeCode
        {
            get { return warehousetypeCode; }
            set { Set(ref warehousetypeCode, value); }
        }

        #endregion

        #region 仓库类型名称
        private string warehousetypeName;

        /// <summary>
        /// 仓库类型名称
        /// </summary>
        [Required(ErrorMessage = "仓库类型名称必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string WareHouseTypeName
        {
            get { return warehousetypeName; }
            set { Set(ref warehousetypeName, value); }
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
            WareHouseTypeCode = null;
            WareHouseTypeName = null;
            Remark = null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
