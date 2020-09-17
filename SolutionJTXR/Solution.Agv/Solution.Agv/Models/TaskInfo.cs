using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Agv.Models
{
    [Description("任务信息")]
    public class TaskInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public int TaskNo { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public int TaskType { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [StringLength(50)]
        public string TaskName { get; set; }

        #region 目的地标点
        /// <summary>
        /// 目的地标点
        /// </summary>
        public virtual MarkPointInfo TargetPointInfo { get; set; }

        #endregion
        /// <summary>
        /// 任务状态
        /// </summary>
        public int TaskStatus { get; set; }

        #region 任务车辆
        /// <summary>
        /// 任务车辆
        /// </summary>
        public virtual AgvInfo TaskCar { get; set; }
        #endregion

        #region 路径库
        /// <summary>
        /// 路径库
        /// </summary>
        public virtual RouteInfo RouteInfo { get; set; }
        #endregion

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
