using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.EnterpriseInformation.Dtos
{
    public class EnterpriseInfoOutputDto:IOutputDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        [StringLength(50)]
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 企业编号
        /// </summary>
        [StringLength(50)]
        public string EnterpriseCode { get; set; }
        /// <summary>
        /// 企业地址
        /// </summary>
        [StringLength(100)]
        public string EnterpriseAddress { get; set; }
        /// <summary>
        /// 企业电话
        /// </summary>
        [StringLength(50)]
        public string EnterprisePhone { get; set; }
        /// <summary>
        /// 企业描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }


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
