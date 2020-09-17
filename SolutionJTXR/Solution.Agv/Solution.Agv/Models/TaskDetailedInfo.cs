using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Solution.MatWarehouseStorageManagement.Models;

namespace Solution.Agv.Models
{
    [Description("任务信息")]
    public class TaskDetailedInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public virtual TaskInfo TaskInfo { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public virtual MaterialInfo Material { get; set; }

        /// <summary>
        /// 物料数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

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
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion

    }
}
