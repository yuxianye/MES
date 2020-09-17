using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
namespace Solution.EnterpriseInformation.Dtos
{
    public class EntTeamInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 班组编号，不能重复
        /// </summary>
        public string TeamCode { get; set; }

        /// <summary>
        /// 班组名称，不能重复
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 人员个数
        /// </summary>
        public int? TeamNumber { get; set; }

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
