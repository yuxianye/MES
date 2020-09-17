using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.CommunicationModule.Models
{
    /// <summary>
    /// 实体类——OpcUa数据点配置
    /// </summary>
    [Description("OpcUa数据点配置")]
    public class CommOpcUaNode : EntityBase<Guid>, IAudited/*, IRecyclable, ILockable*/
    {
        public CommOpcUaNode() => Id = CombHelper.NewComb();

        [Display(Description = "数据点名称")]
        [Required, StringLength(20)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string NodeName { get; set; }

        [Display(Description = "数据点URL")]
        [Required]
        public string NodeUrl { get; set; }

        [Display(Description = "订阅间隔")]
        [Required]
        public int Interval { get; set; }

        [Display(Description = "描述")]
        [StringLength(100)]
        public string Description { get; set; }

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

        //#region Implementation of IRecyclable
        //public bool IsDeleted { get; set; }

        //#endregion

        //#region Implementation of ILockable
        //public bool IsLocked { get; set; }

        //#endregion
    }
}
