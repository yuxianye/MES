﻿using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Models;

namespace Solution.StoredInWarehouseManagement.Dtos
{
    /// <summary>
    /// 物料库存日志信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class MaterialStorageLogInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public MaterialInfo Material { get; set; }

        /// <summary>
        /// 物料批次ID
        /// </summary>
        public MaterialBatchInfo MaterialBatch { get; set; }

        /// <summary>
        /// 库位改变类型
        /// </summary>
        public int StorageChangeType { get; set; }

        /// <summary>
        /// 入库ID
        /// </summary>
        public Guid? InStorageID { get; set; }

        /// <summary>
        /// 出库ID
        /// </summary>
        public Guid? OutStorageID { get; set; }

        /// <summary>
        /// 改变数量
        /// </summary>
        public decimal? ChangedAmount { get; set; }

        /// <summary>
        /// 原数量
        /// </summary>
        public decimal? OriginalAmount { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal? CurrentAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

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
