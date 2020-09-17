using OSharp.Utility.Data;
using Solution.ProductManagement.Contracts;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-BOM明细")]
    public class ProManufacturingBOMBillItemInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// BOM明细契约
        /// </summary>
        public IProManufacturingBOMBillItemInfoContract ProManufacturingBOMBillItemInfoContract { get; set; }

        [HttpPost]
        [Description("服务-BOM明细-根据ID查询")]
        public IHttpActionResult GetProManufacturingBillItemInfo(ProManufacturingBOMBillItemInfo info)
        {
            ProManufacturingBOMBillItemInfo Info = ProManufacturingBOMBillItemInfoContract.ProManufacturingBOMBillItemInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取BOM明细数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-BOM明细-分页数据")]
        public IHttpActionResult GetProManufacturingBillItemInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ProManufacturingBOMBillItemInfoContract.ProManufacturingBOMBillItemInfos, m => new
                {
                    m.Id,
                    ProManufacturingBill_Id = m.ProManufacturingBill.Id,
                    m.ProManufacturingBill.BillName,
                    m.ProManufacturingBill.BillCode,
                    ProductionProcess_Id = m.ProductionProcess.Id,
                    m.ProductionProcess.ProductionProcessName,
                    m.ProductionProcess.ProductionProcessCode,
                    ProductionRule_Id = m.ProManufacturingBill.ProductionRule.Id,
                    m.ProManufacturingBill.ProductionRule.ProductionRuleName,
                    m.ProManufacturingBill.ProductionRule.ProductionRuleVersion,
                    Material_Id = m.Material.Id,
                    m.Material.MaterialName,
                    m.Material.MaterialCode,
                    ProductionRuleStatus_Id = m.ProManufacturingBill.ProductionRule.ProductionRuleStatus.Id,
                    m.ProManufacturingBill.ProductionRule.ProductionRuleStatus.ProductionRuleStatusCode,
                    m.ProManufacturingBill.ProductionRule.ProductionRuleStatus.ProductionRuleStatusName,
                    m.Quantity,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取BOM明细列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取BOM明细列表数据失败！" + ex.ToString()));
            }
        }



        [Description("服务-BOM明细-增加")]

        public async Task<IHttpActionResult> Add(params ProManufacturingBOMBillItemInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await ProManufacturingBOMBillItemInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-BOM明细-修改")]
        public async Task<IHttpActionResult> Update(params ProManufacturingBOMBillItemInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await ProManufacturingBOMBillItemInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-BOM明细-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await ProManufacturingBOMBillItemInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
