using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;

namespace Solution.EnterpriseInformation.Dtos
{
    /// <summary>
    /// 企业信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class EnterpriseInfoInputDto :/* ILockable, IRecyclable,*/ IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 企业编号
        /// </summary>

        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 企业地址
        /// </summary>
        public string EnterpriseAddress { get; set; }

        /// <summary>
        /// 企业电话
        /// </summary>
        public string EnterprisePhone { get; set; }

        /// <summary>
        /// 企业描述
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
