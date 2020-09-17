using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using OSharp.Utility.Filter;
using Solution.Core.Identity;
using Solution.Core.Models.Identity;
using Solution.StepTeachingDispatchManagement.Contracts;
using Solution.StepTeachingDispatchManagement.Dtos;
using Solution.StepTeachingDispatchManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 分步操作与工序关联信息API控制器
    /// </summary>

    [Description("服务-分步操作与工序关联")]
    public class DisStepActionProcessMapInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 分步操作与工序关联信息契约
        /// </summary>
        public IDisStepActionProcessMapInfoContract DisStepActionProcessMapInfoContract { get; set; }

        [Description("服务-分步操作与工序关联-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            DisStepActionProcessMapInfo DisStepActionProcessMapInfoList = DisStepActionProcessMapInfoContract.DisStepActionProcessMapInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (DisStepActionProcessMapInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取分步操作与工序关联信息数据失败！", DisStepActionProcessMapInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取分步操作与工序关联信息数据成功！", DisStepActionProcessMapInfoList));
        }

        /// <summary>
        /// 分页数据
        /// PageRepuestParams举例：
        /// {
	    ///     "filterGroup":"",
	    ///     "pageIndex":1,
	    ///     "pageSize":5,
	    ///     "sortField":"Id,NodeId,NodeName,NodeUrl,Interval,Description,IsLocked,CreatedTime",
	    ///     "sortOrder":",asc,,,,,,"
        ///}
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Description("服务-分步操作与工序关联-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(DisStepActionProcessMapInfoContract.DisStepActionProcessMapInfos, m => new
                {
                    m.Id,
                    // m.DisStepActionInfo_ID,
                    StepActionName = m.DisStepActionInfo.StepActionName,
                    StepActionCode = m.DisStepActionInfo.StepActionCode,
                    m.DisStepActionInfo,
                    ProductionProcessInfo_ID = m.ProductionProcessInfo.Id,
                    ProductionProcessName = m.ProductionProcessInfo.ProductionProcessName,
                    ProductionProcessCode = m.ProductionProcessInfo.ProductionProcessCode,
                    m.ProductionProcessInfo,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取分步操作与工序关联信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取分步操作与工序关联信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-分步操作与工序关联-增加")]
        public async Task<IHttpActionResult> Add(params DisStepActionProcessMapInfoInputDto[] inputDtos)
        {
            //创建和修改的人员、时间
            inputDtos?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            //
            //var result = await MatWareHouseTypeInfoContract.AddMatWareHouseTypes(inputDtos);
            var result = await DisStepActionProcessMapInfoContract.AddDisStepActionProcessMaps(inputDtos);
            return Json(result);
        }

        [Description("服务-分步操作与工序关联-修改")]
        public async Task<IHttpActionResult> Update(params DisStepActionProcessMapInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await DisStepActionProcessMapInfoContract.EditDisStepActionProcessMaps(dto);
            return Json(result);
        }

        [Description("服务-分步操作与工序关联-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await DisStepActionProcessMapInfoContract.DeleteDisStepActionProcessMaps(ids);
            return Json(result);
        }
    }
}
