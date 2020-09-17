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
    [Description("服务-订单管理")]
    public class PlanOrderInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 订单契约
        /// </summary>
        public IPlanOrderInfoContract PlanOrderInfoContract { get; set; }
        /// <summary>
        /// 订单明细契约
        /// </summary>
        public IPlanOrderItemInfoContract PlanOrderItemInfoContract { get; set; }

        [HttpPost]
        [Description("服务-订单管理-根据ID查询")]
        public IHttpActionResult GetPlanOrderInfo(PlanOrderInfo info)
        {
            PlanOrderInfo Info = PlanOrderInfoContract.PlanOrderInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取订单数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-订单管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(PlanOrderInfoContract.PlanOrderInfos, m => new
                {
                    m.Id,
                    m.OrderName,
                    m.OrderCode,
                    m.Procurement,
                    m.ProcurePhone,
                    m.DeliveryTime,
                    m.ExpectedFinishTime,
                    m.ActualStartTime,
                    m.ActualFinishTime,
                    m.DeliverAddress,
                    m.OrderState,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取订单列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取订单列表数据失败！" + ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-订单管理-获取某一订单下的订单明细列表")]
        public IHttpActionResult GetPlanOrderItemInfoListByOrderID(PlanOrderInfo info)
        {
            try
            {

                var page = GetPageResult(PlanOrderItemInfoContract.PlanOrderItemInfos.Where(m => m.Order.Id == info.Id), m => new
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
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一订单下的订单明细列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一配方下的配方明细列表数据失败！", ex.ToString()));
            }
        }

        [Description("服务-订单管理-增加")]

        public async Task<IHttpActionResult> Add(params PlanOrderInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await PlanOrderInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-订单管理-修改")]
        public async Task<IHttpActionResult> Update(params PlanOrderInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await PlanOrderInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-订单管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await PlanOrderInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-订单管理-增加订单明细")]

        public async Task<IHttpActionResult> AddOrderItem(params PlanOrderItemInfoInputDto[] dto)
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

        [Description("服务-订单管理-修改订单明细")]
        public async Task<IHttpActionResult> UpdateOrderItem(params PlanOrderItemInfoInputDto[] dto)
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
    }
}
