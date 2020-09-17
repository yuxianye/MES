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
    [Description("服务-配方状态")]
    public class ProductionRuleStatusInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 配方状态契约
        /// </summary>
        public IProductionRuleStatusInfoContract ProductionRuleStatusInfoContract { get; set; }

        [HttpPost]
        [Description("服务-配方状态-根据ID查询")]
        public IHttpActionResult GetProductionRuleStatusInfo(ProductionRuleStatusInfo info)
        {
            ProductionRuleStatusInfo Info = ProductionRuleStatusInfoContract.ProductionRuleStatusInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取配方状态数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-配方状态-分页数据")]
        public IHttpActionResult GetProductionRuleStatusInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ProductionRuleStatusInfoContract.ProductionRuleStatusInfos, m => new
                {
                    m.Id,
                    m.ProductionRuleStatusName,
                    m.ProductionRuleStatusCode,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取配方状态列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取配方状态列表数据失败！", ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-配方状态-获取配方审核状态列表")]
        public IHttpActionResult GetProductionRuleStatusAuditInfoList()
        {
            try
            {
             
                var page = GetPageResult(ProductionRuleStatusInfoContract.ProductionRuleStatusInfos.Where(m=>m.ProductionRuleStatusCode!="UnCommit"), m => new
                {
                    m.Id,
                    m.ProductionRuleStatusName,
                    m.ProductionRuleStatusCode,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                });
                return Json(new OperationResult(OperationResultType.Success, "读取配方审核状态列表数据成功！", page));
        
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取配方审核状态列表数据失败！", ex.ToString()));
            }
        }

        [Description("服务-配方状态-增加")]

        public async Task<IHttpActionResult> Add(params ProductionRuleStatusInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await ProductionRuleStatusInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-配方状态-修改")]
        public async Task<IHttpActionResult> Update(params ProductionRuleStatusInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await ProductionRuleStatusInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-配方状态-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await ProductionRuleStatusInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
