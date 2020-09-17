using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 仓库信息API控制器
    /// </summary>

    [Description("服务-仓库管理")]
    public class MatWareHouseInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 仓库信息契约
        /// </summary>
        public IMatWareHouseInfoContract MatWareHouseInfoContract { get; set; }

        [Description("服务-仓库管理-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatWareHouseInfo MatWareHouseInfoList = MatWareHouseInfoContract.MatWareHouseInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatWareHouseInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取仓库信息数据失败！", MatWareHouseInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取仓库信息数据成功！", MatWareHouseInfoList));
        }

        /// <summary>
        /// 分页数据
        /// PageRepuestParams举例：
        /// {
	    ///     "filterGroup":"",
	    ///     "pageIndex":1,
	    ///     "pageSize":5,
	    ///     "sortField":"Id,NodeId,NodeName,NodeUrl,Interval,Description,IsLocked,CreatedTime",
	    ///     "sortOrder":",asc,,,,,,"
        ///}
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Description("服务-仓库管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatWareHouseInfoContract.MatWareHouseInfos, m => new
                {
                    m.Id,

                    Enterprise_ID = m.EntArea.EntSite.Enterprise.Id,
                    EnterpriseCode = m.EntArea.EntSite.Enterprise.EnterpriseCode,
                    EnterpriseName = m.EntArea.EntSite.Enterprise.EnterpriseName,

                    EntSite_ID = m.EntArea.EntSite.Id,
                    EntSiteCode = m.EntArea.EntSite.SiteCode,
                    EntSiteName = m.EntArea.EntSite.SiteName,

                    EntArea_ID = m.EntArea.Id,
                    EntAreaCode = m.EntArea.AreaCode,
                    EntAreaName = m.EntArea.AreaName,

                    MatWareHouseType_ID = m.MatWareHouseType.Id,
                    WareHouseTypeName = m.MatWareHouseType.WareHouseTypeName,

                    m.WareHouseCode,
                    m.WareHouseName,
                    m.Description,
                    m.Manager,
                    m.WareHousePhone,                        
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取仓库信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取仓库信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-仓库管理-增加")]
        public async Task<IHttpActionResult> Add(params MatWareHouseInfoInputDto[] inputDtos)
        {
            //创建和修改的人员、时间
            inputDtos?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            //
            var result = await MatWareHouseInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-仓库管理-修改")]
        public async Task<IHttpActionResult> Update(params MatWareHouseInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MatWareHouseInfoContract.UpdateMatWareHouses(dto);
            return Json(result);
        }

        [Description("服务-仓库管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatWareHouseInfoContract.DeleteMatWareHouses(ids);
            return Json(result);
        }
    }
}
