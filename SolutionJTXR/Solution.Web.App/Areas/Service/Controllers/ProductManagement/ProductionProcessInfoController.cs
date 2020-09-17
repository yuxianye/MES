using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using Solution.ProductManagement.Contracts;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-工序管理")]
    public class ProductionProcessInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 工序信息契约
        /// </summary>
        public IProductionProcessInfoContract ProductionProcessInfoContract { get; set; }


        [HttpPost]
        [Description("服务-工序管理-根据ID查询")]
        public IHttpActionResult GetProductionProcessInfo(ProductionProcessInfo info)
        {
            ProductionProcessInfo Info = ProductionProcessInfoContract.ProductionProcessInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取工序数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-工序管理-根据ID查询列表")]
        public IHttpActionResult GetProductionProcessInfo1(ProductionProcessInfo info)
        {
            try
            {

                var page = GetPageResult(ProductionProcessInfoContract.ProductionProcessInfos.Where(m => m.Id == info.Id), m => new
                {
                    m.Id,
                    m.ProductionProcessName,
                    m.ProductionProcessCode,
                    EntProductionLine_Id = m.EntProductionLine.Id,
                    m.EntProductionLine.ProductionLineName,
                    m.EntProductionLine.ProductionLineCode,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                });
                return Json(new OperationResult(OperationResultType.Success, "读取工序信息数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取工序信息数据失败！", ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-工序管理-分页数据")]
        public IHttpActionResult GetProductionProcessInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ProductionProcessInfoContract.ProductionProcessInfos, m => new
                {
                    m.Id,
                    m.ProductionProcessName,
                    m.ProductionProcessCode,
                    EntProductionLine_Id = m.EntProductionLine.Id,
                    Enterprise_Id = m.EntProductionLine.EntArea.EntSite.Enterprise.Id,
                    m.EntProductionLine.EntArea.EntSite.Enterprise.EnterpriseName,
                    m.EntProductionLine.EntArea.EntSite.Enterprise.EnterpriseCode,
                    EntSite_Id = m.EntProductionLine.EntArea.EntSite.Id,
                    m.EntProductionLine.EntArea.EntSite.SiteName,
                    m.EntProductionLine.EntArea.EntSite.SiteCode,
                    EntArea_Id = m.EntProductionLine.EntArea.Id,
                    m.EntProductionLine.EntArea.AreaName,
                    m.EntProductionLine.EntArea.AreaCode,
                    m.EntProductionLine.ProductionLineName,
                    m.EntProductionLine.ProductionLineCode,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取工序信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取工序信息列表数据失败！" + ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-工序管理-获取某一产线下的工序列表")]
        public IHttpActionResult GetProductionProcessInfoListByLineID(EntProductionLineInfo lineInfo)
        {
            try
            {

                var page = GetPageResult(ProductionProcessInfoContract.ProductionProcessInfos.Where(m => m.EntProductionLine.Id == lineInfo.Id), m => new
                {
                    m.Id,
                    m.ProductionProcessName,
                    m.ProductionProcessCode,
                    EntProductionLine_Id = m.EntProductionLine.Id,
                    m.EntProductionLine.ProductionLineName,
                    m.EntProductionLine.ProductionLineCode,
                    m.Description,
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


        [Description("服务-工序管理-增加")]

        public async Task<IHttpActionResult> Add(params ProductionProcessInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await ProductionProcessInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-工序管理-修改")]
        public async Task<IHttpActionResult> Update(params ProductionProcessInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await ProductionProcessInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-工序管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await ProductionProcessInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
