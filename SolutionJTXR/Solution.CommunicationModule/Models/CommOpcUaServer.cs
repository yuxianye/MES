using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Models
{
    /// <summary>
    /// 实体类——OpcUa服务器配置
    /// </summary>
    [Description("OpcUa服务器配置")]
    public class CommOpcUaServer : EntityBase<Guid>, IAudited/*, IRecyclable, ILockable*/
    {
        public CommOpcUaServer() => Id = CombHelper.NewComb();

        [Display(Description = "服务器名称")]
        [Required, StringLength(20)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string ServerName { get; set; }

        [Display(Description = "服务器链接URL")]
        [Required, StringLength(200)]
        public string Url { get; set; }

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
