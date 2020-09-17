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
    /// 盘点明细信息API控制器
    /// </summary>

    [Description("服务-库存盘点")]
    public class MatInventoryItemInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 盘点明细信息契约
        /// </summary>
        public IMatInventoryItemInfoContract MatInventoryItemInfoContract { get; set; }

        [Description("服务-库存盘点-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatInventoryItemInfo MatInventoryItemInfoList = MatInventoryItemInfoContract.MatInventoryItemInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatInventoryItemInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取盘点明细信息数据失败！", MatInventoryItemInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取盘点明细信息数据成功！", MatInventoryItemInfoList));
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
                var page = GetPageResult(MatInventoryItemInfoContract.MatInventoryItemInfos, m => new
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
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取盘点明细信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取盘点明细信息列表数据失败！" + ex.ToString()));
            }
        }


        [Description("服务-库存盘点-删除明细")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatInventoryItemInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-库存盘点-盘点作业")]
        public async Task<IHttpActionResult> Distribute(params MatInventoryItemInfoInputDto[] dtos)
        {
            try
            {
                //dtos.CheckNotNull("dtos");
                //修改人员、时间
                dtos?.ToList().ForEach((a) =>
                {
                    a.UserName = User.Identity.Name;
                });
                var result = await MatInventoryItemInfoContract.AddTask(dtos);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "增加盘点作业任务！" + ex.ToString()));
            }

            ////修改人员、时间
            //dto?.ToList().ForEach((a) =>
            //{
            //    a.LastUpdatedTime = DateTime.Now;
            //    a.LastUpdatorUserId = User.Identity.Name;
            //});
            ////
            //var result = await MaterialInStorageInfoContract.Update(dto);
            //return Json(result);
        }

        [Description("服务-库存盘点-物料批次数据")]
        public IHttpActionResult GetMaterialBatchID()
        {
            try
            {
                List<string> codeInfoList = new List<string>();
                //
                List<Guid> ItemInfo = MatInventoryItemInfoContract.MatInventoryItemInfos.Where(m => m.InventoryState == 1)
                                                                                                 .Select(m => m.MaterialBatch.Id).ToList();
                foreach(Guid temp in ItemInfo)
                {
                    codeInfoList.Add(temp.ToString());
                }
                //
                return Json(new OperationResult(OperationResultType.Success, "查询物料批次信息成功！", codeInfoList));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询物料批次信息失败！" + ex.ToString()));
            }
        }
    }
}
