using OSharp.Utility.Data;
using Solution.PlanManagement.Contracts;
using Solution.PlanManagement.Dtos;
using Solution.PlanManagement.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-工序需求")]
    public class PlanProductionProcessRequirementInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 工序需求信息契约
        /// </summary>
        public IPlanProductionProcessRequirementInfoContract PlanProductionProcessRequirementInfoContract { get; set; }

        [HttpPost]
        [Description("服务-工序需求-根据ID查询")]
        public IHttpActionResult GetPlanProductionProcessRequirementInfo(PlanProductionProcessRequirementInfo info)
        {
            PlanProductionProcessRequirementInfo Info = PlanProductionProcessRequirementInfoContract.PlanProductionProcessRequirementInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取工序需求数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-工序需求-分页数据")]
        public IHttpActionResult GetPlanProductionProcessRequirementInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(PlanProductionProcessRequirementInfoContract.PlanProductionProcessRequirementInfos, m => new
                {
                    m.Id,
                    ProductionSchedule_Id=m.ProductionSchedule.Id,
                    m.ProductionSchedule.ScheduleName,
                    m.ProductionSchedule.ScheduleCode,
                    m.ProductionSchedule.ScheduleStatus,
                    ProductionProcess_Id=m.ProductionProcess.Id,
                    m.ProductionProcess.ProductionProcessName,
                    m.ProductionProcess.ProductionProcessCode,
                    ProductionRule_Id=m.ProductionSchedule.ProductionRule.Id,
                    m.ProductionSchedule.ProductionRule.ProductionRuleName,
                    m.ProductionSchedule.ProductionRule.ProductionRuleVersion,
                    m.ProductionSchedule.ProductionRule.ProductionRuleStatus.ProductionRuleStatusName,
                    m.StartTime,
                    m.EndTime,
                    m.Duration,
                    m.DurationUnit,
                    m.ProductionProcessOrder,
                    m.ActualStartTime,
                    m.ActualFinishTime,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取工序需求信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取工序需求信息列表数据失败！" + ex.ToString()));
            }
        }



        [Description("服务-工序需求-增加")]

        public async Task<IHttpActionResult> Add(params PlanProductionProcessRequirementInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await PlanProductionProcessRequirementInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-工序需求-修改")]
        public async Task<IHttpActionResult> Update(params PlanProductionProcessRequirementInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await PlanProductionProcessRequirementInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-工序需求-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await PlanProductionProcessRequirementInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
