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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 物料批次信息API控制器
    /// </summary>

    [Description("服务-库存查看")]
    public class MaterialBatchInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 物料批次信息契约
        /// </summary>
        public IMaterialBatchInfoContract MaterialBatchInfoContract { get; set; }
        public IMatPalletInfoContract MatPalletInfoContract { get; set; }
        public IMatWareHouseLocationInfoContract MatWareHouseLocationInfoContract { get; set; }

        [Description("服务-库存查看-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MaterialBatchInfo MaterialBatchInfoList = MaterialBatchInfoContract.MaterialBatchInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MaterialBatchInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取物料批次信息数据失败！", MaterialBatchInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取物料批次信息数据成功！", MaterialBatchInfoList));
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

        [Description("服务-库存查看-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                string sCode = "";
                if (requestParams.FilterGroup != null)
                {
                    if (requestParams.FilterGroup.Rules.Count > 0)
                    {
                        foreach (FilterRule filterRule in requestParams.FilterGroup.Rules)
                        {
                            //filterRule.Field.Equals("MaterialCode")
                            sCode = filterRule.Value.ToString();
                        }
                    }
                    //
                    requestParams.FilterGroup = null;
                }
                //
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MaterialBatchInfoContract.MaterialBatchInfos.Where(m => m.Quantity != null && m.Quantity > 0 &&
                                                                                            ((!sCode.Equals("") ? m.Material.MaterialCode.Contains(sCode) : true) ||
                                                                                              (!sCode.Equals("") ? m.Material.MaterialName.Contains(sCode) : true) ||
                                                                                              (!sCode.Equals("") ? m.BatchCode.Contains(sCode) : true))
                                                                                            ), m => new
                                                                                            {
                                                                                                m.Id,

                                                                                                Material_ID = m.Material.Id,
                                                                                                MaterialCode = m.Material.MaterialCode,
                                                                                                MaterialName = m.Material.MaterialName,
                                                                                                MaterialType = m.Material.MaterialType,
                                                                                                MaterialUnit = m.Material.MaterialUnit,

                                                                                                MatWareHouse_Id = m.MatWareHouseLocation.MatWareHouse.Id,
                                                                                                WareHouseCode = m.MatWareHouseLocation.MatWareHouse.WareHouseCode,
                                                                                                WareHouseName = m.MatWareHouseLocation.MatWareHouse.WareHouseName,

                                                                                                MatWareHouseArea_Id = m.MatWareHouseLocation.MatWareHouseArea.Id,
                                                                                                WareHouseAreaCode = m.MatWareHouseLocation.MatWareHouseArea.WareHouseAreaCode,
                                                                                                WareHouseAreaName = m.MatWareHouseLocation.MatWareHouseArea.WareHouseAreaName,

                                                                                                MatWareHouseLocation_Id = m.MatWareHouseLocation.Id,
                                                                                                WareHouseLocationCode = m.MatWareHouseLocation.WareHouseLocationCode,
                                                                                                WareHouseLocationName = m.MatWareHouseLocation.WareHouseLocationName,
                                                                                                WareHouseLocationType = m.MatWareHouseLocation.WareHouseLocationType,

                                                                                                m.MaterialInStorage,
                                                                                                MaterialInStorage_ID = m.MaterialInStorage.Id,
                                                                                                m.BatchCode,
                                                                                                m.Description,
                                                                                                m.Quantity,
                                                                                                m.Remark,
                                                                                                m.CreatedTime,
                                                                                                m.CreatorUserId,
                                                                                                m.LastUpdatedTime,
                                                                                                m.LastUpdatorUserId,
                                                                                            }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取物料批次信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取物料批次信息列表数据失败！" + ex.ToString()));
            }
        }


        [Description("服务-库存查看-批次号分页数据")]
        public IHttpActionResult PageDataBatchCode(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MaterialBatchInfoContract.MaterialBatchInfos.Where(m => m.Quantity != null && m.Quantity > 0), m => new
                {
                    m.Id,

                    Material_ID = m.Material.Id,
                    MaterialCode = m.Material.MaterialCode,
                    MaterialName = m.Material.MaterialName,
                    MaterialType = m.Material.MaterialType,
                    MaterialUnit = m.Material.MaterialUnit,
                    FullPalletQuantity = m.Material.FullPalletQuantity,

                    MatWareHouse_Id = m.MatWareHouseLocation.MatWareHouse.Id,
                    WareHouseCode = m.MatWareHouseLocation.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouseLocation.MatWareHouse.WareHouseName,

                    MatWareHouseArea_Id = m.MatWareHouseLocation.MatWareHouseArea.Id,
                    WareHouseAreaCode = m.MatWareHouseLocation.MatWareHouseArea.WareHouseAreaCode,
                    WareHouseAreaName = m.MatWareHouseLocation.MatWareHouseArea.WareHouseAreaName,

                    MatWareHouseLocation_Id = m.MatWareHouseLocation.Id,
                    WareHouseLocationCode = m.MatWareHouseLocation.WareHouseLocationCode,
                    WareHouseLocationName = m.MatWareHouseLocation.WareHouseLocationName,
                    WareHouseLocationType = m.MatWareHouseLocation.WareHouseLocationType,

                    m.MaterialInStorage,
                    MaterialInStorage_ID = m.MaterialInStorage.Id,
                    BatchCode = m.BatchCode + "(" + m.MatWareHouseLocation.WareHouseLocationCode + ")",
                    m.Description,
                    m.Quantity,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取物料批次信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取物料批次信息列表数据失败！" + ex.ToString()));
            }
        }

        //成品手动出库
        //库位状态：满库位 3 /部分库位 4
        [Description("服务-库存查看-分页数据")]
        public IHttpActionResult PageDataMaterialBatchInfo1(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MaterialBatchInfoContract.MaterialBatchInfos.Where(m => m.Quantity > 0), m => new
                {
                    m.Id,

                    Material_ID = m.Material.Id,
                    MaterialCode = m.Material.MaterialCode,
                    MaterialName = m.Material.MaterialName,
                    MaterialType = m.Material.MaterialType,
                    MaterialUnit = m.Material.MaterialUnit,
                    FullPalletQuantity = m.Material.FullPalletQuantity,
                    //m.MatSupplier,

                    MatWareHouse_Id = m.MatWareHouseLocation.MatWareHouse.Id,
                    WareHouseCode = m.MatWareHouseLocation.MatWareHouse.WareHouseCode,
                    WareHouseName = m.MatWareHouseLocation.MatWareHouse.WareHouseName,

                    MatWareHouseArea_Id = m.MatWareHouseLocation.MatWareHouseArea.Id,
                    WareHouseAreaCode = m.MatWareHouseLocation.MatWareHouseArea.WareHouseAreaCode,
                    WareHouseAreaName = m.MatWareHouseLocation.MatWareHouseArea.WareHouseAreaName,

                    MatWareHouseLocation_Id = m.MatWareHouseLocation.Id,
                    WareHouseLocationCode = m.MatWareHouseLocation.WareHouseLocationCode,
                    WareHouseLocationName = m.MatWareHouseLocation.WareHouseLocationName,
                    WareHouseLocationType = m.MatWareHouseLocation.WareHouseLocationType,

                    WareHouseLocationStatus = m.Quantity == m.Material.FullPalletQuantity ? 3 : 4,
                    Pallet_Id = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id == m.MatWareHouseLocation.PalletID).Select(x => x.Id).FirstOrDefault(),
                    PalletCode = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.MatWareHouseLocation.PalletID.ToString())).Select(x => x.PalletCode).FirstOrDefault(),
                    PalletName = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.MatWareHouseLocation.PalletID.ToString())).Select(x => x.PalletName).FirstOrDefault(),

                    m.MaterialInStorage,
                    MaterialInStorage_ID = m.MaterialInStorage.Id,
                    m.BatchCode,
                    m.Description,
                    m.Quantity,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取物料批次信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取物料批次信息列表数据失败！" + ex.ToString()));
            }
        }

        //空托盘出库
        //库位状态：空托盘 2
        [Description("服务-库存查看-分页数据")]
        public IHttpActionResult PageDataMaterialBatchInfo2(PageRepuestParams requestParams)
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

                    Quantity = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id == m.Id && m2.Quantity > 0).Sum(m2 => m2.Quantity),

                    m.IsUse

                }, request);
                //
                return Json(new OperationResult(OperationResultType.Success, "读取库位信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取库位信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-库存查看-增加")]
        public async Task<IHttpActionResult> Add(params MaterialBatchInfoInputDto[] inputDtos)
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
            var result = await MaterialBatchInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-库存查看-修改")]
        public async Task<IHttpActionResult> Update(params MaterialBatchInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MaterialBatchInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-库存查看-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MaterialBatchInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
