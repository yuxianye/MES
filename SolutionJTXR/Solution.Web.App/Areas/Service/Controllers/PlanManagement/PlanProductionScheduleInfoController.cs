using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using OSharp.Utility.Filter;
using Solution.Core.Identity;
using Solution.Core.Models.Identity;
using Solution.PlanManagement.Contracts;
using Solution.PlanManagement.Dtos;
using Solution.PlanManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-生产计划")]
    public class PlanProductionScheduleInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 生产计划信息契约
        /// </summary>
        public IPlanProductionScheduleInfoContract PlanProductionScheduleInfoContract { get; set; }

        [HttpPost]
        [Description("服务-生产计划-根据ID查询")]
        public IHttpActionResult GetPlanProductionScheduleInfo(PlanProductionScheduleInfo info)
        {
            PlanProductionScheduleInfo Info = PlanProductionScheduleInfoContract.PlanProductionScheduleInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取生产计划数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-生产计划-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(PlanProductionScheduleInfoContract.PlanProductionScheduleInfos, m => new
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
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取生产计划信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取生产计划信息列表数据失败！" + ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-生产计划-已下达计划分页数据")]
        public IHttpActionResult PageData_Published(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(PlanProductionScheduleInfoContract.PlanProductionScheduleInfos.Where(m => m.ScheduleStatus != 1 && m.ScheduleStatus != 2), m => new
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
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取生产计划信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取生产计划信息列表数据失败！" + ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-生产计划-获取某一子订单下的生产计划列表")]
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


        [Description("服务-生产计划-增加")]

        public async Task<IHttpActionResult> Add(params PlanProductionScheduleInfoInputDto[] dto)
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

        [Description("服务-生产计划-修改")]
        public async Task<IHttpActionResult> Update(params PlanProductionScheduleInfoInputDto[] dto)
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
        [Description("服务-生产计划-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await PlanProductionScheduleInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-生产计划-增加工单")]

        public async Task<IHttpActionResult> AddWorkList(params PlanProductionScheduleInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await PlanProductionScheduleInfoContract.AddWorkList(dto);
            return Json(result);
        }

        [Description("服务-生产计划-下达工单")]

        public async Task<IHttpActionResult> DistributeWorkOrder(params PlanProductionScheduleInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
                a.PublishedDate = DateTime.Now;
                a.IsPublish = true;
            });
            var result = await PlanProductionScheduleInfoContract.DistributeWorkOrder(dto);
            return Json(result);
        }
    }
}
