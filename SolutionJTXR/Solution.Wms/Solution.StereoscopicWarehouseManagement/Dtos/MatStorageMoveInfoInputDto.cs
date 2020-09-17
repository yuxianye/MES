using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.MatWarehouseStorageManagement.Models;

namespace Solution.StereoscopicWarehouseManagement.Dtos
{
    /// <summary>
    /// 移库信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class MatStorageMoveInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 原库位ID
        /// </summary>
        public Guid? FromLocationID { get; set; }

        /// <summary>
        /// 移动到库位ID
        /// </summary>
        public Guid? ToLocationID { get; set; }

        /// <summary>
        /// 移库编号
        /// </summary>
        [StringLength(50)]
        public string StorageMoveCode { get; set; }

        /// <summary>
        /// 移库原因
        /// </summary>
        [StringLength(100)]
        public string StorageMoveReason { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        [StringLength(50)]
        public string Operator { get; set; }

        /// <summary>
        /// 移库状态
        /// </summary>
        public int StorageMoveState { get; set; }

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
