using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquKnifeToolInfo.Model
{
    /// <summary>
    /// 刀具信息模型
    /// </summary>
    public class EquKnifeToolModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 刀具ID
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 刀具ID
        public Guid KnifeToolTypeInfoId;
        [Required(ErrorMessage = "必选！")]
        public Guid KnifeToolTypeInfo_Id//Equipmenttype_Id
        {
            get { return KnifeToolTypeInfoId; }
            set { Set(ref KnifeToolTypeInfoId, value); }
        }
        #endregion

        #region 刀具类别名称
        public string knifeToolTypeName;
        public string KnifeToolTypeName
        {
            get { return knifeToolTypeName; }
            set { Set(ref knifeToolTypeName, value); }
        }
        #endregion    

        #region 刀具名称
        private string knifeName;

        /// <summary>
        /// 刀具名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string KnifeName
        {
            get { return knifeName; }
            set { Set(ref knifeName, value); }
        }

        #endregion

        #region 刀具编码
        private string knifeCode;

        /// <summary>
        /// 刀具编码
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string KnifeCode
        {
            get { return knifeCode; }
            set { Set(ref knifeCode, value); }
        }

        #endregion

        #region 规格型号
        private string specifications;

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications
        {
            get { return specifications; }
            set { Set(ref specifications, value); }
        }
        #endregion

        //#region 安装时间
        //private DateTime installTime;
        ///// <summary>
        /////    安装时间
        ///// </summary>
        //public DateTime InstallTime
        //{
        //    get { return installTime; }
        //    set { Set(ref installTime, value); }
        //}
        //#endregion

        #region 安装时间
        private DateTime? installTime;

        /// <summary>
        /// 安装时间
        /// </summary>
        /// 
        [Required(ErrorMessage = "安装时间必填")]
        public DateTime? InstallTime
        {
            get { return installTime; }
            set { Set(ref installTime, value); }
        }
        #endregion

        #region 使用寿命
        private int serviceLife;

        /// <summary>
        ///  使用寿命
        /// </summary>
        [Required(ErrorMessage = "使用寿命必填")]
        public int ServiceLife
        {
            get { return serviceLife; }
            set { Set(ref serviceLife, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度小于200个字符")]
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

        //#region 是否逻辑删除
        ////private bool isDeleted;

        /////// <summary>
        /////// 是否逻辑删除
        /////// </summary>
        ////public bool IsDeleted
        ////{
        ////    get { return isDeleted; }
        ////    set { Set(ref isDeleted, value); }
        ////}
        //#endregion

        protected override void Disposing()
        {
            KnifeToolTypeName = null;
            KnifeCode = null;
            Specifications = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
            InstallTime = null;
        }

    }

}
