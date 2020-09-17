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
    [Description("服务-订单明细")]
    public class PlanOrderItemInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 订单明细信息契约
        /// </summary>
        public IPlanOrderItemInfoContract PlanOrderItemInfoContract { get; set; }
        /// <summary>
        /// 生产计划信息契约
        /// </summary>
        public IPlanProductionScheduleInfoContract PlanProductionScheduleInfoContract { get; set; }
        /// <summary>
        /// 工序需求信息契约
        /// </summary>
        public IPlanProductionProcessRequirementInfoContract PlanProductionProcessRequirementInfoContract { get; set; }
        /// <summary>
        /// 物料需求信息契约
        /// </summary>
        public IPlanMaterialRequirementInfoContract PlanMaterialRequirementInfoContract { get; set; }
        /// <summary>
        /// 设备需求信息契约
        /// </summary>
        public IPlanEquipmentRequirementInfoContract PlanEquipmentRequirementInfoContract { get; set; }

        [HttpPost]
        [Description("服务-订单明细-根据ID查询")]
        public IHttpActionResult GetPlanOrderItemInfo(PlanOrderItemInfo info)
        {
            PlanOrderItemInfo Info = PlanOrderItemInfoContract.PlanOrderItemInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取订单明细数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-订单明细-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(PlanOrderItemInfoContract.PlanOrderItemInfos, m => new
                {
                    m.Id,
                    Order_Id = m.Order.Id,
                    m.Order.OrderName,
                    m.Order.OrderCode,
                    Product_Id = m.Product.Id,
                    m.Product.ProductName,
                    m.Product.ProductCode,
                    m.OrderItemName,
                    m.OrderItemCode,
                    m.OrderQuantity,
                    m.RemainQuantity,
                    m.ProductUnit,
                    m.OrderState,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取订单明细信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取订单明细信息列表数据失败！" + ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-订单明细-获取某一子订单下的生产计划列表")]
        public IHttpActionResult GetScheduleInfoListByOrderItemID(PlanOrderItemInfo info)
        {
            try
            {

                var page = GetPageResult(PlanProductionScheduleInfoContract.PlanProductionScheduleInfos.Where(m => m.OrderItem.Id == info.Id), m => new
                {
                    m.Id,
                    Product_Id = m.OrderItem.Product.Id,
                    m.OrderItem.Product.ProductName,
                    m.OrderItem.Product.ProductCode,
                    Order_Id = m.OrderItem.Order.Id,
                    m.OrderItem.Order.OrderName,
                    m.OrderItem.Order.OrderCode,
                    OrderItem_Id = m.OrderItem.Id,
                    m.OrderItem.OrderItemName,
                    m.OrderItem.OrderItemCode,
                    ProductionRule_Id = m.ProductionRule.Id,
                    m.ProductionRule.ProductionRuleName,
                    m.ProductionRule.ProductionRuleVersion,
                    m.ProductionRule.ProductionRuleStatus.ProductionRuleStatusName,
                    m.ScheduleStatus,
                    m.ScheduleName,
                    m.ScheduleCode,
                    m.Quantity,
                    m.StartTime,
                    m.EndTime,
                    m.Priority,
                    m.IsPublish,
                    m.PublishedDate,
                    m.ActualStartTime,
                    m.ActualFinishTime,
                    m.FinishQuantity,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一子订单下的生产计划信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一子订单下的生产计划列表数据失败！", ex.ToString()));
            }
        }


        [Description("服务-订单明细-增加")]

        public async Task<IHttpActionResult> Add(params PlanOrderItemInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await PlanOrderItemInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-订单明细-修改")]
        public async Task<IHttpActionResult> Update(params PlanOrderItemInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await PlanOrderItemInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-订单明细-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await PlanOrderItemInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-订单明细-增加生产计划")]

        public async Task<IHttpActionResult> AddScheduleInfo(params PlanProductionScheduleInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await PlanProductionScheduleInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-订单明细-修改生产计划")]
        public async Task<IHttpActionResult> UpdateScheduleInfo(params PlanProductionScheduleInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await PlanProductionScheduleInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-订单明细-获取某一生产计划下的工序需求数据")]
        public IHttpActionResult GetProcessRequireInfoListByScheduleID(PlanProductionScheduleInfo info)
        {
            try
            {

                var page = GetPageResult(PlanProductionProcessRequirementInfoContract.PlanProductionProcessRequirementInfos.Where(m => m.ProductionSchedule.Id == info.Id), m => new
                {
                    m.Id,
                    ProductionSchedule_Id = m.ProductionSchedule.Id,
                    m.ProductionSchedule.ScheduleName,
                    m.ProductionSchedule.ScheduleCode,
                    m.ProductionSchedule.ScheduleStatus,
                    ProductionProcess_Id = m.ProductionProcess.Id,
                    m.ProductionProcess.ProductionProcessName,
                    m.ProductionProcess.ProductionProcessCode,
                    ProductionRule_Id = m.ProductionSchedule.ProductionRule.Id,
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
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一生产计划下的工序需求列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一生产计划下的工序需求列表数据失败！", ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-订单明细-获取某一生产计划下的物料需求数据")]
        public IHttpActionResult GetMaterialRequireInfoListByScheduleID(PlanProductionScheduleInfo info)
        {
            try
            {

                var page = GetPageResult(PlanMaterialRequirementInfoContract.PlanMaterialRequirementInfos.Where(m => m.ProductionProcessRequirement.ProductionSchedule.Id == info.Id).OrderBy(m => m.ProductionProcessRequirement.ProductionProcessOrder), m => new
                {
                    m.Id,
                    ProcessRequirement_Id = m.ProductionProcessRequirement.Id,
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
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一生产计划下的物料需求信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一生产计划下的物料需求列表数据失败！", ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-订单明细-获取某一生产计划下的设备需求数据")]
        public IHttpActionResult GetEquipmentRequireInfoListByScheduleID(PlanProductionScheduleInfo info)
        {
            try
            {

                var page = GetPageResult(PlanEquipmentRequirementInfoContract.PlanEquipmentRequirementInfos.Where(m => m.ProductionProcessRequirement.ProductionSchedule.Id == info.Id).OrderByDescending(m => m.ProductionProcessRequirement.ProductionProcessOrder), m => new
                {
                    m.Id,
                    ProcessRequirement_Id = m.ProductionProcessRequirement.Id,
                    ProductionProcess_Id = m.ProductionProcessRequirement.ProductionProcess.Id,
                    m.ProductionProcessRequirement.ProductionProcess.ProductionProcessName,
                    m.ProductionProcessRequirement.ProductionProcess.ProductionProcessCode,
                    Equipment_Id = m.Equipment.Id,
                    m.Equipment.EquipmentName,
                    m.Equipment.EquipmentCode,
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
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一生产计划下的设备需求列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一生产计划下的设备需求列表数据失败！", ex.ToString()));
            }
        }
    }
}
