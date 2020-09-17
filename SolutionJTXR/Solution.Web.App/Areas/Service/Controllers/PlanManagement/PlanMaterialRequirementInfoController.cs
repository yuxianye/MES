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
    [Description("服务-工序材料需求")]
    public class PlanMaterialRequirementInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 工序材料需求信息契约
        /// </summary>
        public IPlanMaterialRequirementInfoContract PlanMaterialRequirementInfoContract { get; set; }

        [HttpPost]
        [Description("服务-工序材料需求-根据ID查询")]
        public IHttpActionResult GetPlanMaterialRequirementInfo(PlanMaterialRequirementInfo info)
        {
            PlanMaterialRequirementInfo Info = PlanMaterialRequirementInfoContract.PlanMaterialRequirementInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取工序材料需求数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-工序材料需求-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(PlanMaterialRequirementInfoContract.PlanMaterialRequirementInfos, m => new
                {
                    m.Id,
                    ProcessRequirementID = m.ProductionProcessRequirement.Id,
                    ProductionProcess_Id = m.ProductionProcessRequirement.ProductionProcess.Id,
                    m.ProductionProcessRequirement.ProductionProcess.ProductionProcessName,
                    m.ProductionProcessRequirement.ProductionProcess.ProductionProcessCode,
                    Material_Id = m.Material.Id,
                    m.Material.MaterialName,
                    m.Material.MaterialCode,
                    m.Material.MaterialUnit,
                    ProductionRule_Id = m.ProductionProcessRequirement.ProductionSchedule.ProductionRule.Id,
                    m.ProductionProcessRequirement.ProductionSchedule.ProductionRule.ProductionRuleName,
                    m.ProductionProcessRequirement.ProductionSchedule.ProductionRule.ProductionRuleVersion,
                    m.ProductionProcessRequirement.ProductionSchedule.ProductionRule.ProductionRuleStatus.ProductionRuleStatusName,
                    m.RequireQuantity,
                    m.ActualQuantity,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取工序材料需求信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取工序材料需求信息列表数据失败！" + ex.ToString()));
            }
        }



        [Description("服务-工序材料需求-增加")]

        public async Task<IHttpActionResult> Add(params PlanMaterialRequirementInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await PlanMaterialRequirementInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-工序材料需求-修改")]
        public async Task<IHttpActionResult> Update(params PlanMaterialRequirementInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await PlanMaterialRequirementInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-工序材料需求-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await PlanMaterialRequirementInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
