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
    /// 库存调整信息API控制器
    /// </summary>

    [Description("服务-库存调整")]
    public class MatStorageModifyInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 库存调整信息契约
        /// </summary>
        public IMatStorageModifyInfoContract MatStorageModifyInfoContract { get; set; }

        [Description("服务-库存调整-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatStorageModifyInfo MatStorageModifyInfoList = MatStorageModifyInfoContract.MatStorageModifyInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatStorageModifyInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取库存调整信息数据失败！", MatStorageModifyInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取库存调整信息数据成功！", MatStorageModifyInfoList));
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
        [Description("服务-库存调整-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatStorageModifyInfoContract.MatStorageModifyInfos, m => new
                {
                    m.Id,

                    m.StorageModifyCode,

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

                    m.CurrentAmount,
                    m.OriginalAmount,
                    m.ChangedAmount,
                    m.StorageModifyState,
                    m.Operator,
                    m.FinishTime,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取库存调整信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取库存调整信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-库存调整-增加")]
        public async Task<IHttpActionResult> Add(params MatStorageModifyInfoInputDto[] inputDtos)
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
            var result = await MatStorageModifyInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-库存调整-修改")]
        public async Task<IHttpActionResult> Update(params MatStorageModifyInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MatStorageModifyInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-库存调整-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatStorageModifyInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-库存调整-库存调整作业")]
        public async Task<IHttpActionResult> Distribute(params MatStorageModifyInfoInputDto[] dtos)
        {
            try
            {
                //dtos.CheckNotNull("dtos");
                //修改人员、时间
                dtos?.ToList().ForEach((a) =>
                {
                    a.UserName = User.Identity.Name;
                });
                var result = await MatStorageModifyInfoContract.AddTask(dtos);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "增加库存调整任务！" + ex.ToString()));
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


        [Description("服务-库存调整-库存调整单号数据")]
        public IHttpActionResult GetStorageModifyCode()
        {
            try
            {
                List<string> codeInfoList = new List<string>();
                string sStorageModifyCode = "StorageModify" + string.Format($"{DateTime.Now:yyyyMMddHHmmss}", DateTime.Now);
                codeInfoList.Add(sStorageModifyCode);
                return Json(new OperationResult(OperationResultType.Success, "查询库存调整信息成功！", codeInfoList));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询库存调整信息失败！" + ex.ToString()));
            }
        }
    }
}
