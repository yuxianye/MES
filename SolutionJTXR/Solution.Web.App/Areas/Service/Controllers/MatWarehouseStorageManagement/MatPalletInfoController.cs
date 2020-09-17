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
    /// 托盘信息API控制器
    /// </summary>

    [Description("服务-托盘管理")]
    public class MatPalletInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 托盘信息契约
        /// </summary>
        public IMatPalletInfoContract MatPalletInfoContract { get; set; }
        public IMatWareHouseLocationInfoContract MatWareHouseLocationInfoContract { get; set; }
        public IMaterialBatchInfoContract MaterialBatchInfoContract { get; set; }
        public IMaterialInStorageInfoContract MaterialInStorageInfoContract { get; set; }

        [Description("服务-托盘管理-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatPalletInfo MatPalletInfoList = MatPalletInfoContract.MatPalletInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatPalletInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取托盘信息数据失败！", MatPalletInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取托盘信息数据成功！", MatPalletInfoList));
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
        /// 待组盘 = 1,
        [Description("服务-托盘管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatPalletInfoContract.MatPalletInfos , m => new
                {
                    m.Id,
                    m.PalletCode,
                    m.PalletName,
                    m.PalletMaxWeight,
                    m.PalletSpecifications,

                    WareHouseLocationCode = MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(x => x.PalletID.ToString().Contains(m.Id.ToString())).Select(x => x.WareHouseLocationCode).FirstOrDefault(),
                    WareHouseLocationName = MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(x => x.PalletID.ToString().Contains(m.Id.ToString())).Select(x => x.WareHouseLocationName).FirstOrDefault(),

                    StorageQuantity = MaterialBatchInfoContract.MaterialBatchInfos.Where(m2 => m2.MatWareHouseLocation.Id.ToString().Contains(
                                                                                                                                               MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(x => x.PalletID.ToString().Contains(m.Id.ToString())).Select(x => x.Id.ToString()).FirstOrDefault()
                                                                                                                                              ) 
                                                                                                      && m2.Quantity > 0).Sum(m2 => m2.Quantity),

                    MaterialInStorageQuantity = MaterialInStorageInfoContract.MaterialInStorageInfos.Where(m2 => m2.PalletID.ToString().Contains(m.Id.ToString()) && m2.InStorageStatus == 1 ).Count(),

                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取托盘信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取托盘信息列表数据失败！" + ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-托盘管理-获取可用的托盘信息列表")]
        public IHttpActionResult GetMatPalletInfoList_IsUse()
        {
            try
            {
                var page = GetPageResult(MatPalletInfoContract.MatPalletInfos, m => new
                {
                    m.Id,
                    m.PalletCode,
                    m.PalletName,
                    m.PalletMaxWeight,
                    m.PalletSpecifications,
                    m.Remark,
                    m.CreatedTime
                });
                return Json(new OperationResult(OperationResultType.Success, "读取可用的托盘信息列表成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取可用的托盘信息列表失败！", ex.ToString()));
            }
        }

        [Description("服务-托盘管理-增加")]
        public async Task<IHttpActionResult> Add(params MatPalletInfoInputDto[] inputDtos)
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
            var result = await MatPalletInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-托盘管理-修改")]
        public async Task<IHttpActionResult> Update(params MatPalletInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MatPalletInfoContract.UpdateMatPallets(dto);
            return Json(result);
        }

        [Description("服务-托盘管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatPalletInfoContract.DeleteMatPallets(ids);
            return Json(result);
        }
    }
}
