using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using Solution.StereoscopicWarehouseManagement.Contracts;
using Solution.StereoscopicWarehouseManagement.Dtos;
using Solution.StereoscopicWarehouseManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 盘点信息API控制器
    /// </summary>

    [Description("服务-库存盘点")]
    public class MatInventoryInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 盘点信息契约
        /// </summary>
        public IMatInventoryInfoContract MatInventoryInfoContract { get; set; }
        /// <summary>
        /// 盘点明细信息契约
        /// </summary>
        public IMatInventoryItemInfoContract MatInventoryItemInfoContract { get; set; }


        [Description("服务-库存盘点-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatInventoryInfo MatInventoryInfoList = MatInventoryInfoContract.MatInventoryInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatInventoryInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取盘点信息数据失败！", MatInventoryInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取盘点信息数据成功！", MatInventoryInfoList));
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
        [Description("服务-库存盘点-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatInventoryInfoContract.MatInventoryInfos, m => new
                {
                    m.Id,

                    MatWareHouse_Id = m.MatWareHouse.Id,
                    WareHouseCode = m.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouse.WareHouseName,

                    m.InventoryType,
                    m.InventoryCode,

                    ItemCount = MatInventoryItemInfoContract.MatInventoryItemInfos.Count(m2 => m2.MatInventory.Id == m.Id ),

                    m.Operator,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取盘点信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取盘点信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-库存盘点-增加")]
        public async Task<IHttpActionResult> Add(params MatInventoryInfoInputDto[] inputDtos)
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
            var result = await MatInventoryInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-库存盘点-修改")]
        public async Task<IHttpActionResult> Update(params MatInventoryInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MatInventoryInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-库存盘点-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatInventoryInfoContract.Delete(ids);
            return Json(result);
        }       

        /// ////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        [Description("服务-库存盘点-获取某一盘点单下的盘点明细列表数据")]
        public IHttpActionResult GetMatInventoryItemInfoListByInventoryID(MatInventoryInfo info)
        {
            try
            {

                var page = GetPageResult(MatInventoryItemInfoContract.MatInventoryItemInfos.Where(m => m.MatInventory.Id == info.Id), m => new
                {
                    m.Id,

                    MatInventory_Id = m.MatInventory.Id,

                    MaterialBatch_Id = m.MaterialBatch.Id,
                    BatchCode = m.MaterialBatch.BatchCode,

                    Material_ID = m.MaterialBatch.Material.Id,
                    MaterialCode = m.MaterialBatch.Material.MaterialCode,
                    MaterialName = m.MaterialBatch.Material.MaterialName,
                    MaterialType = m.MaterialBatch.Material.MaterialType,
                    MaterialUnit = m.MaterialBatch.Material.MaterialUnit,
                    FullPalletQuantity = m.MaterialBatch.Material.FullPalletQuantity,

                    MatWareHouseLocation_Id = m.MaterialBatch.MatWareHouseLocation.Id,
                    WareHouseLocationCode = m.MaterialBatch.MatWareHouseLocation.WareHouseLocationCode,
                    WareHouseLocationName = m.MaterialBatch.MatWareHouseLocation.WareHouseLocationName,
                    WareHouseLocationType = m.MaterialBatch.MatWareHouseLocation.WareHouseLocationType,

                    m.AccuntAmount,
                    m.ActualAmount,
                    m.DifferenceAmount,
                    m.InventoryTime,
                    m.InventoryState,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一盘点单下的盘点明细信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一盘点单下的盘点明细信息列表数据失败！", ex.ToString()));
            }
        }


        [Description("服务-库存盘点-增加明细")]
        public async Task<IHttpActionResult> AddItem(params MatInventoryItemInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await MatInventoryItemInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-库存盘点-修改明细")]
        public async Task<IHttpActionResult> UpdateItem(params MatInventoryItemInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await MatInventoryItemInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-库存盘点-盘点单号数据")]
        public IHttpActionResult GetInventoryCode()
        {
            try
            {
                List<string> codeInfoList = new List<string>();
                string sInventoryCode = "Inventory" + string.Format($"{DateTime.Now:yyyyMMddHHmmss}", DateTime.Now);
                codeInfoList.Add(sInventoryCode);
                return Json(new OperationResult(OperationResultType.Success, "查询盘点单信息成功！", codeInfoList));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询盘点单信息失败！" + ex.ToString()));
            }
        }
    }
}
