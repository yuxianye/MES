using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.StereoscopicWarehouseManagement.Contracts;
using Solution.StereoscopicWarehouseManagement.Dtos;
using Solution.StereoscopicWarehouseManagement.Models;
using Solution.StoredInWarehouseManagement.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 移库信息API控制器
    /// </summary>

    [Description("服务-移库管理")]
    public class MatStorageMoveInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 移库信息契约
        /// </summary>
        public IMatStorageMoveInfoContract MatStorageMoveInfoContract { get; set; }
        public IMatWareHouseLocationInfoContract MatWareHouseLocationInfoContract { get; set; }
        public IMaterialBatchInfoContract MaterialBatchInfoContract { get; set; }

        [Description("服务-移库管理-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatStorageMoveInfo MatStorageMoveInfoList = MatStorageMoveInfoContract.MatStorageMoveInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatStorageMoveInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取移库信息数据失败！", MatStorageMoveInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取移库信息数据成功！", MatStorageMoveInfoList));
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
        [Description("服务-移库管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatStorageMoveInfoContract.MatStorageMoveInfos, m => new
                {
                    m.Id,

                    m.FromLocationID,
                    FromWareHouseLocationCode = MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(x => x.Id.ToString().Contains(m.FromLocationID.ToString())).Select(x => x.WareHouseLocationCode).FirstOrDefault(),
                    FromWareHouseLocationName = MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(x => x.Id.ToString().Contains(m.FromLocationID.ToString())).Select(x => x.WareHouseLocationName).FirstOrDefault(),

                    m.ToLocationID,
                    ToWareHouseLocationCode = MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(x => x.Id.ToString().Contains(m.ToLocationID.ToString())).Select(x => x.WareHouseLocationCode).FirstOrDefault(),
                    ToWareHouseLocationName = MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(x => x.Id.ToString().Contains(m.ToLocationID.ToString())).Select(x => x.WareHouseLocationName).FirstOrDefault(),

                    m.StorageMoveCode,
                    m.StorageMoveReason,
                    m.Operator,
                    m.StorageMoveState,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取移库信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取移库信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-移库管理-增加")]
        public async Task<IHttpActionResult> Add(params MatStorageMoveInfoInputDto[] inputDtos)
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
            var result = await MatStorageMoveInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-移库管理-修改")]
        public async Task<IHttpActionResult> Update(params MatStorageMoveInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MatStorageMoveInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-移库管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatStorageMoveInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-移库管理-移库作业")]
        public async Task<IHttpActionResult> Distribute(params MatStorageMoveInfoInputDto[] dtos)
        {
            try
            {
                //dtos.CheckNotNull("dtos");
                //修改人员、时间
                dtos?.ToList().ForEach((a) =>
                {
                    a.UserName = User.Identity.Name;
                });
                var result = await MatStorageMoveInfoContract.AddTask(dtos);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "增加移库任务！" + ex.ToString()));
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

        [Description("服务-移库管理-移库单号数据")]
        public IHttpActionResult GetStorageMoveCode()
        {
            try
            {
                List<string> codeInfoList = new List<string>();
                string sStorageMoveCode = "StorageMove" + string.Format($"{DateTime.Now:yyyyMMddHHmmss}", DateTime.Now);
                codeInfoList.Add(sStorageMoveCode);
                return Json(new OperationResult(OperationResultType.Success, "查询移库信息成功！", codeInfoList));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询移库信息失败！" + ex.ToString()));
            }
        }

        [Description("服务-移库管理-已使用的原库位列表")]
        public IHttpActionResult GetFromWareHouseLocationID()
        {
            try
            {
                List<string> codeInfoList = new List<string>();
                //
                List<Guid> ItemInfo = MatStorageMoveInfoContract.MatStorageMoveInfos.Where(m => m.StorageMoveState == 1)
                                                                                                 .Select(m => m.FromLocationID.Value).ToList();
                foreach (Guid temp in ItemInfo)
                {
                    codeInfoList.Add(temp.ToString());
                }
                //
                return Json(new OperationResult(OperationResultType.Success, "查询已使用的原库位信息成功！", codeInfoList));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询已使用的原库位信息失败！" + ex.ToString()));
            }
        }

        [Description("服务-移库管理-已使用的现库位列表")]
        public IHttpActionResult GetToWareHouseLocationID()
        {
            try
            {
                List<string> codeInfoList = new List<string>();
                //
                List<Guid> ItemInfo = MatStorageMoveInfoContract.MatStorageMoveInfos.Where(m => m.StorageMoveState == 1)
                                                                                                 .Select(m => m.ToLocationID.Value).ToList();
                foreach (Guid temp in ItemInfo)
                {
                    codeInfoList.Add(temp.ToString());
                }
                //
                return Json(new OperationResult(OperationResultType.Success, "查询已使用的现库位信息成功！", codeInfoList));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询已使用的现库位信息失败！" + ex.ToString()));
            }
        }
    }
}
