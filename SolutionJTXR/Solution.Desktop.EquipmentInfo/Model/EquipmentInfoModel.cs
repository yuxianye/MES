using Solution.Desktop.Core;
using Solution.Desktop.EntDepartmentInfo.Model;
using Solution.Desktop.EquFactoryInfo.Model;
using Solution.Desktop.EquipmentTypeInfo.Model;
using Solution.Desktop.EquSparePartTypeInfo.Model;
using Solution.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquipmentInfo.Model
{
    /// <summary>
    /// 设备信息模型
    /// </summary>
    public class EquipmentInfoModel : ModelBase, IAudited
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

        #region 设备名称
        private string equipmentName;

        /// <summary>
        /// 设备名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string EquipmentName
        {
            get { return equipmentName; }
            set { Set(ref equipmentName, value); }
        }

        #endregion

        #region 设备编号
        private string equipmentCode;

        /// <summary>
        /// 设备编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string EquipmentCode
        {
            get { return equipmentCode; }
            set { Set(ref equipmentCode, value); }
        }
        #endregion

        #region 设备类别ID
        private Guid equipmentType_Id;

        /// <summary>
        /// 设备类别ID
        /// </summary>
        public Guid EquipmentType_Id
        {
            get { return equipmentType_Id; }
            set { Set(ref equipmentType_Id, value); }
        }
        #endregion

        #region 设备类别名称
        private string equipmentType_Name;

        /// <summary>
        /// 设备类别名称
        /// </summary>
        public string EquipmentType_Name
        {
            get { return equipmentType_Name; }
            set { Set(ref equipmentType_Name, value); }
        }
        #endregion

        #region 设备类别
        public EquipmentTypeModel equipmenttype;

        /// <summary>
        /// 设备类别
        /// </summary>
        public EquipmentTypeModel Equipmenttype
        {
            get { return equipmenttype; }
            set { Set(ref equipmenttype, value); }
        }
        #endregion

        #region 规格
        private string specifications;

        /// <summary>
        /// 规格
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限100个字符"), MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string Specifications
        {
            get { return specifications; }
            set { Set(ref specifications, value); }
        }
        #endregion

        #region 型号
        private string mdelNumber;

        /// <summary>
        /// 型号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限100个字符"), MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string ModelNumber
        {
            get { return mdelNumber; }
            set { Set(ref mdelNumber, value); }
        }
        #endregion

        #region 部门ID
        private string departmentInfo_Id;

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentInfo_Id
        {
            get { return departmentInfo_Id; }
            set { Set(ref departmentInfo_Id, value); }
        }
        #endregion

        #region 部门名称
        private string departmentInfo_Name;

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentInfo_Name
        {
            get { return departmentInfo_Name; }
            set { Set(ref departmentInfo_Name, value); }
        }
        #endregion

        #region 部门
        private EntDepartmentInfoModel entDepartmentInfo;

        /// <summary>
        /// 部门
        /// </summary>
        public EntDepartmentInfoModel DepartmentInfo
        {
            get { return entDepartmentInfo; }
            set { Set(ref entDepartmentInfo, value); }
        }
        #endregion

        #region 设备状态
        private EquEquipmentState equipmentState;

        /// <summary>
        /// 设备状态
        /// </summary>
        public EquEquipmentState EquipmentState
        {
            get { return equipmentState; }
            set { Set(ref equipmentState, value); }
        }
        #endregion

        #region 启用时间
        private DateTime? startusingTime;

        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime? StartusingTime
        {
            get { return startusingTime; }
            set { Set(ref startusingTime, value); }
        }
        #endregion

        #region 负责人
        private string responsiblePerson;

        /// <summary>
        /// 负责人
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限20个字符"), MaxLength(20, ErrorMessage = "长度小于20个字符")]
        public string ResponsiblePerson
        {
            get { return responsiblePerson; }
            set { Set(ref responsiblePerson, value); }
        }
        #endregion

        #region 设备厂家ID
        private string equFactoryInfo_Id;

        /// <summary>
        /// 设备厂家ID
        /// </summary>
        public string EquFactoryInfo_Id
        {
            get { return equFactoryInfo_Id; }
            set { Set(ref equFactoryInfo_Id, value); }
        }
        #endregion

        #region 设备厂家名称
        private string equFactoryInfo_Name;

        /// <summary>
        /// 设备厂家名称
        /// </summary>
        public string EquFactoryInfo_Name
        {
            get { return equFactoryInfo_Name; }
            set { Set(ref equFactoryInfo_Name, value); }
        }
        #endregion

        #region 设备厂家名称
        private EquFactoryModel equFactoryInfo;

        /// <summary>
        /// 设备厂家名称
        /// </summary>
        public EquFactoryModel EquFactoryInfo
        {
            get { return equFactoryInfo; }
            set { Set(ref equFactoryInfo, value); }
        }
        #endregion

        #region 出厂编号
        private string factoryNumber;

        /// <summary>
        /// 出厂编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string FactoryNumber
        {
            get { return factoryNumber; }
            set { Set(ref factoryNumber, value); }
        }
        #endregion

        #region 出厂日期
        private DateTime? productionDate;

        /// <summary>
        /// 出厂日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return productionDate; }
            set { Set(ref productionDate, value); }
        }
        #endregion

        #region ABC分类
        private ABCType abcType;

        /// <summary>
        /// ABC分类
        /// </summary>
        public ABCType AbcType
        {
            get { return abcType; }
            set { Set(ref abcType, value); }
        }
        #endregion

        #region 原值
        private double originalValue;

        /// <summary>
        /// 原值
        /// </summary>
        //[RegularExpression(@"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$", ErrorMessage = "原值必须是浮点数")]
        public double OriginalValue
        {
            get { return originalValue; }
            set { Set(ref originalValue, value); }
        }
        #endregion

        #region 折旧年限
        private double depreciationYears;

        /// <summary>
        /// 折旧年限
        /// </summary>
        //[RegularExpression(@"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$", ErrorMessage = "原值必须是浮点数")]
        public double DepreciationYears
        {
            get { return depreciationYears; }
            set { Set(ref depreciationYears, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
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
            EquipmentName = null;
            EquipmentCode = null;
            EquipmentType_Name = null;
            Equipmenttype = null;
            Specifications = null;
            ModelNumber = null;
            DepartmentInfo_Id = null;
            DepartmentInfo_Name = null;
            DepartmentInfo = null;
            StartusingTime = null;
            ResponsiblePerson = null;
            EquFactoryInfo_Id = null;
            EquFactoryInfo_Name = null;
            EquFactoryInfo = null;
            FactoryNumber = null;
            ProductionDate = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
