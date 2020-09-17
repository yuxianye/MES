using OSharp.Core.Data.Extensions;
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
    /// 仓库区域信息API控制器
    /// </summary>

    [Description("服务-仓库区域")]
    public class MatWareHouseAreaInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 仓库区域信息契约
        /// </summary>
        public IMatWareHouseAreaInfoContract MatWareHouseAreaInfoContract { get; set; }

        [Description("服务-仓库区域-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatWareHouseAreaInfo MatWareHouseAreaInfoList = MatWareHouseAreaInfoContract.MatWareHouseAreaInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatWareHouseAreaInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取仓库区域信息数据失败！", MatWareHouseAreaInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取仓库区域信息数据成功！", MatWareHouseAreaInfoList));
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
        [Description("服务-仓库区域-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatWareHouseAreaInfoContract.MatWareHouseAreaInfos, m => new
                {
                    m.Id,
                    MatWareHouse_ID = m.MatWareHouse.Id,
                    WareHouseCode = m.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouse.WareHouseName,
                    m.WareHouseAreaCode,
                    m.WareHouseAreaName,
                    m.WareHouseLocationType,
                    WareHouseLocationCodeType = 0,
                    m.LayerNumber,
                    m.ColumnNumber,
                    m.LocationQuantity,
                    m.StorageRackSpecifications,
                    m.LocationSpecifications,
                    m.LocationLoadBearing,
                    m.Description,
                    m.IsGenerageLocation,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取仓库区域信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取仓库区域信息列表数据失败！" + ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-仓库区域-获取列表")]
        public IHttpActionResult GetWareHouseAreaListByID(MatWareHouseAreaInfo matwarehouseareaInfo)
        {
            try
            {
                var page = GetPageResult(MatWareHouseAreaInfoContract.MatWareHouseAreaInfos.Where(m => m.MatWareHouse.Id == matwarehouseareaInfo.Id), m => new
                {
                    m.Id,
                    MatWareHouse_ID = m.MatWareHouse.Id,
                    WareHouseCode = m.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouse.WareHouseName,
                    m.WareHouseAreaCode,
                    m.WareHouseAreaName,
                    m.WareHouseLocationType,
                    WareHouseLocationCodeType = 0,
                    m.LayerNumber,
                    m.ColumnNumber,
                    m.LocationQuantity,
                    m.StorageRackSpecifications,
                    m.LocationSpecifications,
                    m.LocationLoadBearing,
                    m.Description,
                    m.IsGenerageLocation,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                });
                return Json(new OperationResult(OperationResultType.Success, "读取仓库区域信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取仓库区域信息列表数据失败！" + ex.ToString()));
            }
        }


        [Description("服务-仓库区域-增加")]
        public async Task<IHttpActionResult> Add(params MatWareHouseAreaInfoInputDto[] inputDtos)
        {
            //创建和修改的人员、时间
            inputDtos?.ToList().ForEach((a) =>
            {
                //a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            //
            var result = await MatWareHouseAreaInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-仓库区域-修改")]
        public async Task<IHttpActionResult> Update(params MatWareHouseAreaInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MatWareHouseAreaInfoContract.UpdateMatWareHouseAreas(dto);
            return Json(result);
        }

        [Description("服务-仓库区域-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatWareHouseAreaInfoContract.DeleteMatWareHouseAreas(ids);
            return Json(result);
        }
    }
}
