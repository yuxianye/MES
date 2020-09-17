using OSharp.Core.Data.Extensions;
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
    [Description("服务-制造清单")]
    public class ProManufacturingBillInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 制造清单契约
        /// </summary>
        public IProManufacturingBillInfoContract ProManufacturingBillInfoContract { get; set; }

        public IProManufacturingBOMBillItemInfoContract ProManufacturingBOMBillItemInfoContract { get; set; }

        public IProManufacturingBORBillItemInfoContract ProManufacturingBORBillItemInfoContract { get; set; }

        [HttpPost]
        [Description("服务-制造清单-根据ID查询")]
        public IHttpActionResult GetProManufacturingBillInfo(ProManufacturingBillInfo info)
        {
            ProManufacturingBillInfo Info = ProManufacturingBillInfoContract.ProManufacturingBillInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取制造清单数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-制造清单-分页数据")]
        public IHttpActionResult GetProManufacturingBillInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                    var page = GetPageResult(ProManufacturingBillInfoContract.ProManufacturingBillInfos, m => new
                {
                        m.Id,
                        m.BillName,
                        m.BillCode,
                        m.BillType,
                        BillTypeName = m.BillType == 1 ? "BOM" : "BOR",
                        Product_Id = m.Product.Id,
                        m.Product.ProductName,
                        m.Product.ProductCode,
                        ProductionRule_Id = m.ProductionRule.Id,
                        m.ProductionRule.ProductionRuleName,
                        m.ProductionRule.ProductionRuleVersion,
                        ProductionRuleStatus_Id = m.ProductionRule.ProductionRuleStatus.Id,
                        m.ProductionRule.ProductionRuleStatus.ProductionRuleStatusCode,
                        m.ProductionRule.ProductionRuleStatus.ProductionRuleStatusName,
                        m.Description,
                        m.Remark,
                        m.CreatedTime,
                        m.CreatorUserId,
                        m.LastUpdatedTime,
                        m.LastUpdatorUserId
                    }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取制造清单列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取制造清单列表数据失败！", ex.ToString()));
            }
        }


        [HttpPost]
        [Description("服务-制造清单-获取某一清单下的BOM明细列表")]
        public IHttpActionResult GetProManufacturingBOMBillItemInfoListByBillID(ProManufacturingBillInfoInputDto info)
        {
            try
            {

                var page = GetPageResult(ProManufacturingBOMBillItemInfoContract.ProManufacturingBOMBillItemInfos.Where(m => m.ProManufacturingBill.Id == info.Id), m => new
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
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一清单下的可用BOM明细信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一清单下的可用BOM明细信息列表数据失败！", ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-制造清单-获取某一清单下的BOR明细列表")]
        public IHttpActionResult GetProManufacturingBORBillItemInfoListByBillID(ProManufacturingBillInfoInputDto info)
        {
            try
            {

                var page = GetPageResult(ProManufacturingBORBillItemInfoContract.ProManufacturingBORBillItemInfos.Where(m => m.ProManufacturingBill.Id == info.Id), m => new
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
                    ProductionRuleStatus_Id = m.ProManufacturingBill.ProductionRule.ProductionRuleStatus.Id,
                    m.ProManufacturingBill.ProductionRule.ProductionRuleStatus.ProductionRuleStatusCode,
                    m.ProManufacturingBill.ProductionRule.ProductionRuleStatus.ProductionRuleStatusName,
                    Equipment_Id = m.Equipment.Id,
                    m.Equipment.EquipmentName,
                    m.Equipment.EquipmentCode,
                    m.Quantity,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一清单下的可用BOR明细信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一清单下的可用BOR明细信息列表数据失败！", ex.ToString()));
            }
        }

        [Description("服务-制造清单-增加")]

        public async Task<IHttpActionResult> Add(params ProManufacturingBillInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await ProManufacturingBillInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-制造清单-修改")]
        public async Task<IHttpActionResult> Update(params ProManufacturingBillInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await ProManufacturingBillInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-制造清单-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await ProManufacturingBillInfoContract.Delete(ids);
            return Json(result);
        }
        [Description("服务-制造清单-增加BOM明细")]

        public async Task<IHttpActionResult> AddBOMItem(params ProManufacturingBOMBillItemInfoInputDto[] dto)
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

        [Description("服务-制造清单-修改BOM明细")]
        public async Task<IHttpActionResult> UpdateBOMItem(params ProManufacturingBOMBillItemInfoInputDto[] dto)
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

        [Description("服务-制造清单-增加BOR明细")]

        public async Task<IHttpActionResult> AddBORItem(params ProManufacturingBORBillItemInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await ProManufacturingBORBillItemInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-制造清单-修改BOR明细")]
        public async Task<IHttpActionResult> UpdateBORItem(params ProManufacturingBORBillItemInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await ProManufacturingBORBillItemInfoContract.Update(dto);
            return Json(result);
        }
    }
}
