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
    [Description("服务-配方明细")]
    public class ProductionRuleItemInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 配方明细契约
        /// </summary>
        public IProductionRuleItemInfoContract ProductionRuleItemInfoContract { get; set; }

        [HttpPost]
        [Description("服务-配方明细-根据ID查询")]
        public IHttpActionResult GetProductionRuleItemInfo(ProductionRuleItemInfo info)
        {
            ProductionRuleItemInfo Info = ProductionRuleItemInfoContract.ProductionRuleItemInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取配方明细数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-配方明细-分页数据")]
        public IHttpActionResult GetProductionRuleItemInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ProductionRuleItemInfoContract.ProductionRuleItemInfos, m => new
                {
                    m.Id,
                    ProductionRule_Id = m.ProductionRule.Id,
                    m.ProductionRule.ProductionRuleName,
                    m.ProductionRule.ProductionRuleVersion,
                    ProductionProcess_Id = m.ProductionProcess.Id,
                    m.ProductionProcess.ProductionProcessName,
                    m.ProductionProcess.ProductionProcessCode,
                    m.ProductionProcessOrder,
                    m.Duration,
                    m.DurationUnit,
                    DurationUnitName = m.DurationUnit == 3 ? "秒" : "分钟",
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取配方明细列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取配方明细列表数据失败！" + ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-配方明细-获取某一配方下的配方明细列表")]
        public IHttpActionResult GetProductionRuleItemInfoListByRuleID(ProductionRuleInfo ruleInfo)
        {
            try
            {

                var page = GetPageResult(ProductionRuleItemInfoContract.ProductionRuleItemInfos.Where(m => m.ProductionRule.Id == ruleInfo.Id), m => new
                {
                    m.Id,
                    ProductionRule_Id = m.ProductionRule.Id,
                    m.ProductionRule.ProductionRuleName,
                    m.ProductionRule.ProductionRuleVersion,
                    ProductionProcess_Id = m.ProductionProcess.Id,
                    m.ProductionProcess.ProductionProcessName,
                    m.ProductionProcess.ProductionProcessCode,
                    m.ProductionProcessOrder,
                    m.Duration,
                    m.DurationUnit,
                    DurationUnitName = m.DurationUnit == 3 ? "秒" : "分钟",
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某生产线下工序信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某生产线下工序信息列表数据失败！", ex.ToString()));
            }
        }


        [Description("服务-配方明细-增加")]

        public async Task<IHttpActionResult> Add(params ProductionRuleItemInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await ProductionRuleItemInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-配方明细-修改")]
        public async Task<IHttpActionResult> Update(params ProductionRuleItemInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await ProductionRuleItemInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-配方明细-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await ProductionRuleItemInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
