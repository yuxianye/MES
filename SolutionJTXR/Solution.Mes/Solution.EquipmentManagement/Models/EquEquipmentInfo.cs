using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Solution.EnterpriseInformation.Models;

namespace Solution.EquipmentManagement.Models
{
    [Description("设备信息")]
    public class EquEquipmentInfo : EntityBase<Guid>, /*ILockable, IRecyclable, */IAudited
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备类别ID
        /// </summary>
        /// 
        public virtual EquipmentTypeInfo Equipmenttype { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [StringLength(100)]
        public string Specifications { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        [StringLength(100)]
        public string ModelNumber { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public virtual EntDepartmentInfo DepartmentInfo { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public EquEquipmentState EquipmentState { get; set; }

        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime StartusingTime { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [StringLength(20)]
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// 设备厂家
        /// </summary>
        public virtual EquFactoryInfo EquFactoryInfo{ get; set; }

        /// <summary>
        /// 出厂编号
        /// </summary>
        [StringLength(50)]
        public string FactoryNumber { get; set; }

        /// <summary>
        /// 出场日期
        /// </summary>
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// ABC分类
        /// </summary>
        public ABCType AbcType { get; set; }

        /// <summary>
        /// 原值
        /// </summary>
        public double OriginalValue { get; set; }

        /// <summary>
        /// 折旧年限
        /// </summary>
        public double DepreciationYears { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }
        //#region Implementation of ILockable
        ///// <summary>
        ///// 是否锁定
        ///// </summary>
        //public bool IsLocked { get; set; }
        //#endregion

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        #endregion

        #region Implementation of ICreatedAudited

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        [StringLength(50)]
        public string CreatorUserId { get; set; }

        #endregion

        #region Implementation of IUpdateAutited

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Index]

        public DateTime? LastUpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion

        //#region Implementation of IRecyclable
        ///// <summary>
        ///// 是否逻辑删除
        ///// </summary>
        //public bool IsDeleted { get; set; }
        //#endregion
    }
}
