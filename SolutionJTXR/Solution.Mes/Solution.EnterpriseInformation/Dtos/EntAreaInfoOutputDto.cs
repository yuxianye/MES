using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.EnterpriseInformation.Dtos
{
    class EntAreaInfoOutputDto : IOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// 厂区ID
        /// </summary>
        public Guid EntSite_Id { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 车间编号
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 车间管理员
        /// </summary>
        public string AreaManager { get; set; }

        /// <summary>
        /// 车间电话
        /// </summary>
        public string SitePhone { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
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
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
