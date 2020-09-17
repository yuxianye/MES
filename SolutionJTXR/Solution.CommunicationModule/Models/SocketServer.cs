using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.CommunicationModule.Models
{
    /// <summary>
    /// 实体类——实时通讯服务器
    /// </summary>
    [Description("实时通讯服务器")]
    public class SocketServer : EntityBase<Guid>, IAudited
    {

        [Display(Description = "实时通讯服务器名称")]
        [Required, StringLength(20)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string ServerName { get; set; }

        [Display(Description = "实时通讯服务器IP地址")]
        [Required, StringLength(19)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string ServerIp { get; set; }

        [Display(Description = "实时通讯服务器端口号")]
        [Required]
        public int ServerPort { get; set; }

        [Display(Description = "实时通讯服务器最大连接数")]
        [Required]
        //[Range(1, 2, ErrorMessage = "")]
        public int MaxConnectionNumber { get; set; }

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

    }
}
