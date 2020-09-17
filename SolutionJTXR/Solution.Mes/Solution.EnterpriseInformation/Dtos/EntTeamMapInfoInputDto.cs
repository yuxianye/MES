﻿using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;

namespace Solution.EnterpriseInformation.Dtos
{
    public class EntTeamMapInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 班组
        /// </summary>
        public EntTeamInfo EntTeamInfo { get; set; }
        /// <summary>
        /// 班组Id
        /// </summary>
        public Guid EntTeamInfo_Id { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public EntEmployeeInfo EntEmployeeInfo { get; set; }
        /// <summary>
        /// 人员Id
        /// </summary>
        public Guid EntEmployeeInfo_Id { get; set; }
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
