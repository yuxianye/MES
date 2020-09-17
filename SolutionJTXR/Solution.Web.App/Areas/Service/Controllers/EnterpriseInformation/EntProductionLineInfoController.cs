using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using OSharp.Utility.Filter;
using Solution.EnterpriseInformation.Contracts;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using Solution.ProductManagement.Contracts;
using Solution.EquipmentManagement.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-产线管理")]
    public class EntProductionLineInfoController : ServiceBaseApiController
    {


        /// <summary>
        /// 生产线契约
        /// </summary>
        public IEntProductionLineInfoContract EntProductionLineInfoContract { get; set; }

        public IProductionProcessInfoContract ProductionProcessInfoContract { get; set; }
        public IEquEquipmentInfoContract EquipmentInfoContract { get; set; }

        [HttpPost]
        [Description("服务-产线管理-根据ID查询")]
        public IHttpActionResult GetEntProductionLineInfo(EntProductionLineInfo info)
        {
            EntProductionLineInfo entLineInfo = EntProductionLineInfoContract.EntProductionLineInfo.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取生产线数据成功！", entLineInfo));
        }

        [HttpPost]
        [Description("服务-产线管理-根据ID查询列表")]
        public IHttpActionResult GetEntProductionLineInfo1(EntProductionLineInfo info)
        {
            try
            {

                var page = GetPageResult(EntProductionLineInfoContract.EntProductionLineInfo.Where(m => m.Id == info.Id), m => new
                {
                    m.Id,
                    m.ProductionLineName,
                    m.ProductionLineCode,
                    m.Duration,
                    m.DurationUnit,
                    DurationUnitName = m.DurationUnit == 3 ? "秒" : "分钟",
                    EntArea_Id = m.EntArea.Id,
                    m.EntArea.AreaName,
                    Enterprise_Id = m.EntArea.EntSite.Enterprise.Id,
                    m.EntArea.EntSite.Enterprise.EnterpriseName,
                    EntSite_Id = m.EntArea.EntSite.Id,
                    m.EntArea.EntSite.SiteName,
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
        [Description("服务-产线管理-分页数据")]
        public IHttpActionResult GetEntProductionLineInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);

                var page = GetPageResult(EntProductionLineInfoContract.EntProductionLineInfo, m => new
                {
                    m.Id,
                    m.ProductionLineName,
                    m.ProductionLineCode,
                    m.Duration,
                    m.DurationUnit,
                    DurationUnitName = m.DurationUnit == 3 ? "秒" : "分钟",
                    EntArea_Id = m.EntArea.Id,
                    m.EntArea.AreaName,
                    Enterprise_Id = m.EntArea.EntSite.Enterprise.Id,
                    m.EntArea.EntSite.Enterprise.EnterpriseName,
                    EntSite_Id = m.EntArea.EntSite.Id,
                    m.EntArea.EntSite.SiteName,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取生产线信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取生产线列表数据失败！", ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-产线管理-获取某一车间下的产线列表")]
        public IHttpActionResult GetEntProductionLineListByEntAreaID(EntAreaInfo entareaInfo)
        {
            try
            {

                var page = GetPageResult(EntProductionLineInfoContract.EntProductionLineInfo.Where(m => m.EntArea.Id == entareaInfo.Id), m => new
                {
                    m.Id,
                    m.ProductionLineName,
                    m.ProductionLineCode,
                    m.Duration,
                    m.DurationUnit,
                    DurationUnitName = m.DurationUnit == 3 ? "秒" : "分钟",
                    EntArea_Id = m.EntArea.Id,
                    m.EntArea.AreaName,
                    Enterprise_Id = m.EntArea.EntSite.Enterprise.Id,
                    m.EntArea.EntSite.Enterprise.EnterpriseName,
                    EntSite_Id = m.EntArea.EntSite.Id,
                    m.EntArea.EntSite.SiteName,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某车间下生产线信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某车间下生产线信息列表数据失败！", ex.ToString()));
            }
        }

        [Description("服务-产线管理-增加")]

        public async Task<IHttpActionResult> Add(params EntProductionLineInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EntProductionLineInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-产线管理-修改")]
        public async Task<IHttpActionResult> Update(params EntProductionLineInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EntProductionLineInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-产线管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            foreach (var id in ids)
            {
                int count1 = ProductionProcessInfoContract.ProductionProcessInfos.Where(m => m.EntProductionLine.Id == id).Count();
                if (count1 > 0)
                {
                    return Json(new OperationResult(OperationResultType.Error, "生产线数据关联工序信息，不能被删除。"));
                }
                //int count2 = EquipmentInfoContract.EquipmentInfos.Where(m => m.Entproductionline.Id == id).Count();
                //if (count2 > 0)
                //{
                //    return Json(new OperationResult(OperationResultType.Error, "生产线数据关联设备信息，不能被删除。"));
                //}
            }
            
            var result = await EntProductionLineInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
