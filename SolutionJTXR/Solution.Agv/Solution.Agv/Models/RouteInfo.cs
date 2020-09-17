using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Agv.Models
{
    [Description("路径信息")]
    public class RouteInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 路径编号
        /// </summary>
        [StringLength(50)]
        public string RouteNo { get; set; }

        /// <summary>
        /// 路径名称
        /// </summary>
        [StringLength(50)]
        public string RouteName { get; set; }

        /// <summary>
        /// 开始地标点
        /// </summary>
        public virtual MarkPointInfo StartMarkPointInfo { get; set; }

        /// <summary>
        /// 结束地标点
        /// </summary>
        public virtual MarkPointInfo EndMarkPointInfo { get; set; }

        /// <summary>
        /// 经过地标点的Id
        /// </summary>
        public string MarkPoints { get; set; }

        /// <summary>
        /// 总距离
        /// </summary>
        public float TotalDistance { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int RouteStatus { get; set; }

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
