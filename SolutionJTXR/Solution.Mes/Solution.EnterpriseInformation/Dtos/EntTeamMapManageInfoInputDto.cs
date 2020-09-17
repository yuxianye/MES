using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using System.Collections.Generic;

namespace Solution.EnterpriseInformation.Dtos
{
    /// <summary>
    /// 班组与人员关联信息输入Dto
    /// </summary>
    public class EntTeamMapManageInfoInputDto : EntTeamMapInfoInputDto
    {
        /// <summary>
        /// 人员列表
        /// </summary>
        public List<EntEmployeeInfo> EntEmployeeInfoList { get; set; }
    }
}
