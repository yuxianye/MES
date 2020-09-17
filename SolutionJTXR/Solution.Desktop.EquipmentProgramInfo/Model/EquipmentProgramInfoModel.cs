using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.EquipmentProgramInfo.Model.ProgramTypeEnum;

namespace Solution.Desktop.EquipmentProgramInfo.Model
{
    /// <summary>
    /// 设备程序信息模型
    /// </summary>
    public class EquipmentProgramModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 设备ID
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion


        #region 程序名称
        public string programName;
        public string ProgramName
        {
            get { return programName; }
            set { Set(ref programName, value); }
        }
        #endregion

        #region 设备类别ID
        public Guid equipmentTypeId;
        [Required(ErrorMessage = "必选！")]
        public Guid EquipmentType_Id//Equipmenttype_Id
        {
            get { return equipmentTypeId; }
            set { Set(ref equipmentTypeId, value); }
        }
        #endregion

        #region 设备类别名称
        public string equipmentTypeName;
        public string EquipmentTypeName
        {
            get { return equipmentTypeName; }
            set { Set(ref equipmentTypeName, value); }
        }
        #endregion

        #region 程序编号
        private string programCode;

        /// <summary>
        /// 程序编号
        /// </summary>
        [Required(ErrorMessage = "必选！")]
        public string ProgramCode
        {
            get { return programCode; }
            set { Set(ref programCode, value); }
        }

        #endregion



        #region 程序地址
        private string programAddress;

        /// <summary>
        /// 程序地址
        /// </summary>
        public string ProgramAddress
        {
            get { return programAddress; }
            set { Set(ref programAddress, value); }
        }
        #endregion
        #region 程序地址
        private string programVersion;
        /// <summary>
        /// 程序版本号
        /// </summary>
        public string ProgramVersion
        {
            get { return programVersion; }
            set { Set(ref programVersion, value); }
        }
        #endregion

        #region 描述
        private string description;

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度小于200个字符")]
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }
        #endregion

        /// <summary>
        ///数量单位
        /// </summary>
        //EquRunningStateTypes
        #region 数量单位
        private ProgramType programType;

        /// <summary>
        /// 状态类型
        /// </summary>
        [Required(ErrorMessage = "数量单位类型必填")]
        public ProgramType ProgramType
        {
            get { return programType; }
            set { Set(ref programType, value); }
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
            ProgramName = null;
            ProgramCode = null;
            ProgramAddress = null;
            ProgramVersion = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
