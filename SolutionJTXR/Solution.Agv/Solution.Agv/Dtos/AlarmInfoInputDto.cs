using OSharp.Core.Data;
using Solution.Agv.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Agv.Dtos
{
    public class AlarmInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// agv小车
        /// </summary>
        public Guid AgvInfoId { get; set; }

        /// <summary>
        /// 报警位置 地标编号，地标位置
        /// </summary>
        public Guid MarkPointInfoId { get; set; }

        /// <summary>
        /// 报警码1：急停、2：防撞、3：障碍物、4：左驱动、5：右驱动、6脱轨、7低电压、8低电压停止、9：过流）
        /// </summary>
        public int AlarmCode { get; set; }

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
