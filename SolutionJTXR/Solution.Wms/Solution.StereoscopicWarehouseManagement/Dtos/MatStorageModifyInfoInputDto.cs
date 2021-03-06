﻿using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Models;

namespace Solution.StereoscopicWarehouseManagement.Dtos
{
    /// <summary>
    /// 库存调整信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class MatStorageModifyInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public Guid MatWareHouseLocation_Id { get; set; }
        /// 
        public MatWareHouseLocationInfo MatWareHouseLocation { get; set; }

        /// <summary>
        /// 库存调整编号
        /// </summary>
        [StringLength(50)]
        public string StorageModifyCode { get; set; }


        /// <summary>
        /// 物料批次ID
        /// </summary>
        public Guid MaterialBatch_Id { get; set; }

        public MaterialBatchInfo MaterialBatch { get; set; }


        /// <summary>
        /// 满盘数量
        /// </summary>
        public int FullPalletQuantity { get; set; }


        /// <summary>
        /// 当前库存数量
        /// </summary>
        public decimal? CurrentAmount { get; set; }

        /// <summary>
        /// 原库存数量
        /// </summary>
        public decimal? OriginalAmount { get; set; }

        /// <summary>
        /// 调整数量（当前数量—原来数量）
        /// </summary>
        public decimal? ChangedAmount { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        [StringLength(50)]
        public string Operator { get; set; }

        ///// <summary>
        ///// 调整时间
        ///// </summary>
        //public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 调整结束时间
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// 修改库存状态
        /// </summary>
        public int StorageModifyState { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

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
