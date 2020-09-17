using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.StepTeachingDispatchManagement.Models;
using Solution.ProductManagement.Models;
using System.Collections.Generic;

namespace Solution.StepTeachingDispatchManagement.Dtos
{
    /// <summary>
    /// 分步操作与工序关联信息输入Dto
    /// </summary>
    public class DisStepActionProcessMapManageInputDto : DisStepActionProcessMapInfoInputDto
    {
        /// <summary>
        /// 工序列表
        /// </summary>
        public List<ProductionProcessInfo> ProductionProcessInfoList { get; set; }        
    }
}
