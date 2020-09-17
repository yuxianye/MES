using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Agv.Models
{
    [Description("地标点信息")]
    public class MarkPointInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 地标编号
        /// </summary>
        [StringLength(50)]
        public string MarkPointNo { get; set; }

        /// <summary>
        /// 地标名称
        /// </summary>
        [StringLength(50)]
        public string MarkPointName { get; set; }

        /// <summary>
        /// 所属区域
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 所属产线
        /// </summary>
        public int ProductLineId { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// 是否虚拟地标
        /// </summary>
        public bool IsVirtualMarkPoint { get; set; }

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
