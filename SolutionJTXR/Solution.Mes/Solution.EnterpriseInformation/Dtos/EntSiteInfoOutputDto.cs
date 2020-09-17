using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.EnterpriseInformation.Dtos
{
    public class EntSiteInfoOutputDto : IOutputDto
    {

        public Guid Id { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        public Guid Enterprise_Id { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 厂区名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 厂区编号
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 厂区管理员
        /// </summary>
        public string SiteManager { get; set; }

        /// <summary>
        /// 厂区电话
        /// </summary>
        public string SitePhone { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
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
