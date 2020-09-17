using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using OSharp.Utility.Filter;
using Solution.Core.Identity;
using Solution.Core.Models.Identity;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
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
    /// 分步操作信息API控制器
    /// </summary>

    [Description("服务-分步操作信息表")]
    public class DisStepActionInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 分步操作信息契约
        /// </summary>
        public IDisStepActionInfoContract DisStepActionInfoContract { get; set; }
        public IDisStepActionProcessMapInfoContract DisStepActionProcessMapInfoContract { get; set; }

        [Description("服务-分步操作信息-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            DisStepActionInfo DisStepActionInfoList = DisStepActionInfoContract.DisStepActionInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (DisStepActionInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取分步操作信息数据失败！", DisStepActionInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取分步操作信息数据成功！", DisStepActionInfoList));
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
        [Description("服务-分步操作信息-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(DisStepActionInfoContract.DisStepActionInfos, m => new
                {
                    m.Id,
                    m.StepActionCode,
                    m.StepActionName,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取分步操作信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取分步操作信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-分步操作信息-增加")]
        public async Task<IHttpActionResult> Add(params DisStepActionInfoInputDto[] inputDtos)
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
            var result = await DisStepActionInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-分步操作信息-修改")]
        public async Task<IHttpActionResult> Update(params DisStepActionInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await DisStepActionInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-分步操作信息-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await DisStepActionInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-分步操作信息-配置工序")]
        public async Task<IHttpActionResult> Setting(params DisStepActionProcessMapManageInputDto[] inputDtos)
        {
            inputDtos?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await DisStepActionProcessMapInfoContract.Setting(inputDtos);
            return Json(result);
        }
    }
}
