﻿using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.MatWarehouseStorageManagement.Models;
using System.Collections.Generic;
using Solution.StoredInWarehouseManagement.Models;

namespace Solution.TakeOutWarehouseManagement.Dtos
{
    /// <summary>
    /// 物料出库信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class MaterialOutStorageInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 出库单编号
        /// </summary>
        [StringLength(50)]
        public string OutStorageBillCode { get; set; }

        /// <summary>
        /// 出库类型
        /// </summary>
        public int OutStorageType { get; set; }

        /// <summary>
        /// 生产排产ID
        /// </summary>
        public Guid? PlanScheduleID { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        //public Guid Material_Id { get; set; }

        //public MaterialInfo Material { get; set; }

        public Guid? MaterialID { get; set; }

        ///// <summary>
        ///// 物料单位
        ///// </summary>
        //public int? MaterialUnit { get; set; }

        /// <summary>
        /// 出库数量
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 配盘数量
        /// </summary>
        public decimal? PalletQuantity { get; set; }

        /// <summary>
        /// 托盘ID
        /// </summary>
        public Guid? PalletID { get; set; }

        ///// <summary>
        ///// 完成数
        ///// </summary>
        //public decimal? PalletFinishQuantity { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime? OutStorageTime { get; set; }

        /// <summary>
        /// 出库完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// 出库状态
        /// </summary>
        public int? OutStorageStatus { get; set; }

        ///// <summary>
        ///// 制单人
        ///// </summary>
        //[StringLength(50)]
        //public string CreateUser { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [StringLength(50)]
        public string AuditPerson { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int? AuditStatus { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 库位批次信息
        /// </summary>
        public List<MaterialBatchInfo> MaterialBatchs { get; set; }

        /// <summary>
        /// 当前系统登录用户
        /// </summary>
        public string UserName { get; set; }

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

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
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
