using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EquipmentManagement.Models;
using Solution.EnterpriseInformation.Models;
using System.Collections.Generic;

namespace Solution.EquipmentManagement.Dtos
{
    public class EquEquipmentInfoInputDto : /* ILockable, IRecyclable,*/ IInputDto<Guid>, IAudited
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual EquipmentTypeInfo Equipmenttype { get; set; }

        /// <summary>
        /// 设备类别ID
        /// </summary>
        public Guid EquipmentType_Id { get; set; }

        /// <summary>
        /// 设备类别名称
        /// </summary>
        public string EquipmentType_Name { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public virtual EntDepartmentInfo DepartmentInfo { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DepartmentInfo_Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentInfo_Name { get; set; }

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
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// 设备厂家
        /// </summary>
        public virtual EquFactoryInfo EquFactoryInfo { get; set; }

        /// <summary>
        /// 设备厂家ID
        /// </summary>
        public Guid EquFactoryInfo_Id { get; set; }

        /// <summary>
        /// 设备厂家名称
        /// </summary>
        public string EquFactoryInfo_Name { get; set; }

        /// <summary>
        /// 出厂编号
        /// </summary>
        public string FactoryNumber { get; set; }

        /// <summary>
        /// 出厂日期
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
        public string Remark { get; set; }

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        public string LastUpdatorUserId { get; set; }

    }
}
