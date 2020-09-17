using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Contracts;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 库位信息API控制器
    /// </summary>

    [Description("服务-库位管理")]
    public class MatWareHouseLocationInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 库位信息契约
        /// </summary>
        public IMatWareHouseLocationInfoContract MatWareHouseLocationInfoContract { get; set; }
        //
        public IMatPalletInfoContract MatPalletInfoContract { get; set; }
        //
        public IMaterialBatchInfoContract MaterialBatchInfoContract { get; set; }

        [Description("服务-库位管理-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatWareHouseLocationInfo MatWareHouseLocationInfoList = MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatWareHouseLocationInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取库位信息数据失败！", MatWareHouseLocationInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取库位信息数据成功！", MatWareHouseLocationInfoList));
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
        [Description("服务-库位管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatWareHouseLocationInfoContract.MatWareHouseLocationInfos, m => new
                {
                    m.Id,

                    MatWareHouse_Id = m.MatWareHouse.Id,
                    WareHouseCode = m.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouse.WareHouseName,

                    MatWareHouseArea_Id = m.MatWareHouseArea.Id,
                    WareHouseAreaCode = m.MatWareHouseArea.WareHouseAreaCode,
                    WareHouseAreaName = m.MatWareHouseArea.WareHouseAreaName,

                    m.WareHouseLocationCode,
                    m.WareHouseLocationName,
                    m.WareHouseLocationType,
                    //m.WareHouseLocationStatus,

                    m.PalletID,
                    PalletCode = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletCode).FirstOrDefault(),
                    PalletName = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletName).FirstOrDefault(),

                    StorageQuantity = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Sum(m2 => m2.Quantity),

                    m.IsUse,
                    m.Remark,

                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取库位信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取库位信息列表数据失败！" + ex.ToString()));
            }
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
        //空托盘 = 2
        [Description("服务-库位管理-分页数据(有托盘)")]
        public IHttpActionResult PageDataPallet1(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(m => m.PalletID != null), m => new
                {
                    m.Id,

                    MatWareHouse_Id = m.MatWareHouse.Id,
                    WareHouseCode = m.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouse.WareHouseName,

                    MatWareHouseArea_Id = m.MatWareHouseArea.Id,
                    WareHouseAreaCode = m.MatWareHouseArea.WareHouseAreaCode,
                    WareHouseAreaName = m.MatWareHouseArea.WareHouseAreaName,

                    m.WareHouseLocationCode,
                    m.WareHouseLocationName,
                    m.WareHouseLocationType,
                    WareHouseLocationStatus = 2,

                    m.PalletID,
                    PalletCode = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletCode).FirstOrDefault(),
                    PalletName = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletName).FirstOrDefault(),

                    StorageQuantity = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Sum(m2 => m2.Quantity),

                    m.IsUse,
                    m.Remark,

                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                //
                return Json(new OperationResult(OperationResultType.Success, "读取库位信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取库位信息列表数据失败！" + ex.ToString()));
            }
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
        //空库位 = 1
        [Description("服务-库位管理-分页数据(无托盘)")]
        public IHttpActionResult PageDataPallet2(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(m => m.PalletID == null), m => new
                {
                    m.Id,

                    MatWareHouse_Id = m.MatWareHouse.Id,
                    WareHouseCode = m.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouse.WareHouseName,

                    MatWareHouseArea_Id = m.MatWareHouseArea.Id,
                    WareHouseAreaCode = m.MatWareHouseArea.WareHouseAreaCode,
                    WareHouseAreaName = m.MatWareHouseArea.WareHouseAreaName,

                    m.WareHouseLocationCode,
                    m.WareHouseLocationName,
                    m.WareHouseLocationType,
                    WareHouseLocationStatus = 1,

                    m.PalletID,
                    PalletCode = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletCode).FirstOrDefault(),
                    PalletName = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletName).FirstOrDefault(),

                    m.StorageQuantity,
                    m.IsUse,
                    m.Remark,

                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取库位信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取库位信息列表数据失败！" + ex.ToString()));
            }
        }
        [Description("服务-库位管理-成品空库位列表")]
        public IHttpActionResult PageDataProductEmptyLocation(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(m => (m.PalletID == null || m.PalletID == Guid.Empty) && m.WareHouseLocationType == 2 && m.IsUse), m => new
                {
                    m.Id,

                    MatWareHouse_Id = m.MatWareHouse.Id,
                    WareHouseCode = m.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouse.WareHouseName,

                    MatWareHouseArea_Id = m.MatWareHouseArea.Id,
                    WareHouseAreaCode = m.MatWareHouseArea.WareHouseAreaCode,
                    WareHouseAreaName = m.MatWareHouseArea.WareHouseAreaName,

                    m.WareHouseLocationCode,
                    m.WareHouseLocationName,
                    m.WareHouseLocationType,
                    WareHouseLocationStatus = 1,

                    m.PalletID,
                    PalletCode = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletCode).FirstOrDefault(),
                    PalletName = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletName).FirstOrDefault(),

                    m.StorageQuantity,
                    m.IsUse,
                    m.Remark,

                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取库位信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取库位信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-库位管理-增加")]
        public async Task<IHttpActionResult> Add(params MatWareHouseLocationInfoInputDto[] inputDtos)
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
            var result = await MatWareHouseLocationInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-库位管理-修改")]
        public async Task<IHttpActionResult> Update(params MatWareHouseLocationInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MatWareHouseLocationInfoContract.UpdateMatWareHouseLocations(dto);
            return Json(result);
        }

        [Description("服务-库位管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatWareHouseLocationInfoContract.DeleteMatWareHouseLocations(ids);
            return Json(result);
        }
    }
}
