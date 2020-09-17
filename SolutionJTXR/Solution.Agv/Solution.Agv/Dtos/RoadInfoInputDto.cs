using OSharp.Core.Data;
using Solution.Agv.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Agv.Dtos
{
    [Description("路段信息")]
    public class RoadInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 路段编号
        /// </summary>
        [StringLength(50)]
        public string RoadNo { get; set; }

        /// <summary>
        /// 车辆名称
        /// </summary>
        [StringLength(50)]
        public string RoadName { get; set; }

        /// <summary>
        /// 开始地标点
        /// </summary>
        public Guid StartMarkPointInfo_Id { get; set; }

        /// <summary>
        /// 结束地标点
        /// </summary>
        public Guid EndMarkPointInfo_Id { get; set; }

        /// <summary>
        /// 开始地标点
        /// </summary>
        public virtual MarkPointInfo StartMarkPointInfo { get; set; }

        /// <summary>
        /// 结束地标点
        /// </summary>
        public virtual MarkPointInfo EndMarkPointInfo { get; set; }

        /// <summary>
        /// 距离
        /// </summary>
        public float Distance { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public int RoadStatus { get; set; }

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
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion

    }
}
