using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using OSharp.Utility.Filter;
using Solution.Core.Identity;
using Solution.Core.Models.Identity;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Contracts;
using Solution.StoredInWarehouseManagement.Dtos;
using Solution.StoredInWarehouseManagement.Models;
using Solution.TakeOutWarehouseManagement.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 物料库存日志信息API控制器
    /// </summary>

    [Description("服务-库存流水")]
    public class MaterialStorageLogInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 物料库存日志信息契约
        /// </summary>
        public IMaterialStorageLogInfoContract MaterialStorageLogInfoContract { get; set; }
        public IMaterialInStorageInfoContract MaterialInStorageInfoContract { get; set; }
        public IMaterialOutStorageInfoContract MaterialOutStorageInfoContract { get; set; }

        [Description("服务-库存流水-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MaterialStorageLogInfo MaterialStorageLogInfoList = MaterialStorageLogInfoContract.MaterialStorageLogInfos.ToList().Find(s =>
            {
                return s.Id == guid;
            });
            if (MaterialStorageLogInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取物料库存日志信息数据失败！", MaterialStorageLogInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取物料库存日志信息数据成功！", MaterialStorageLogInfoList));
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
        [Description("服务-库存流水-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MaterialStorageLogInfoContract.MaterialStorageLogInfos, m => new
                {
                    m.Id,

                    MatWareHouse_Id = m.MaterialBatch.MatWareHouseLocation.MatWareHouse.Id,
                    WareHouseCode = m.MaterialBatch.MatWareHouseLocation.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MaterialBatch.MatWareHouseLocation.MatWareHouse.WareHouseName,

                    MatWareHouseArea_Id = m.MaterialBatch.MatWareHouseLocation.MatWareHouseArea.Id,
                    WareHouseAreaCode = m.MaterialBatch.MatWareHouseLocation.MatWareHouseArea.WareHouseAreaCode,
                    WareHouseAreaName = m.MaterialBatch.MatWareHouseLocation.MatWareHouseArea.WareHouseAreaName,

                    MatWareHouseLocation_Id = m.MaterialBatch.MatWareHouseLocation.Id,
                    WareHouseLocationCode = m.MaterialBatch.MatWareHouseLocation.WareHouseLocationCode,
                    WareHouseLocationName = m.MaterialBatch.MatWareHouseLocation.WareHouseLocationName,
                    WareHouseLocationType = m.MaterialBatch.MatWareHouseLocation.WareHouseLocationType,

                    Material_ID = m.Material.Id,
                    MaterialCode = m.Material.MaterialCode,
                    MaterialName = m.Material.MaterialName,
                    MaterialType = m.Material.MaterialType,
                    MaterialUnit = m.Material.MaterialUnit,

                    BatchCode = m.MaterialBatch.BatchCode,
                    InStorageBillCode = MaterialInStorageInfoContract.MaterialInStorageInfos.Where(x => x.Id.ToString().Contains(m.InStorageID.ToString())).Select(x => x.InStorageBillCode).FirstOrDefault(),
                    OutStorageBillCode = MaterialOutStorageInfoContract.MaterialOutStorageInfos.Where(x => x.Id.ToString().Contains(m.OutStorageID.ToString())).Select(x => x.OutStorageBillCode).FirstOrDefault(),

                    m.StorageChangeType,
                    m.InStorageID,
                    m.OutStorageID,
                    m.ChangedAmount,
                    m.OriginalAmount,
                    m.CurrentAmount,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取物料库存日志信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取物料库存日志信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-库存流水-增加")]
        public async Task<IHttpActionResult> Add(params MaterialStorageLogInfoInputDto[] inputDtos)
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
            var result = await MaterialStorageLogInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-库存流水-修改")]
        public async Task<IHttpActionResult> Update(params MaterialStorageLogInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MaterialStorageLogInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-库存流水-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MaterialStorageLogInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
