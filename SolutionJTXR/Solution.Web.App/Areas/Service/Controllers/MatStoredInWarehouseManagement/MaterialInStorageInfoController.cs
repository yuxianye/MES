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
    /// 物料入库信息API控制器
    /// </summary>

    [Description("服务-入库单据")]
    public class MaterialInStorageInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 物料入库信息契约
        /// </summary>
        public IMaterialInStorageInfoContract MaterialInStorageInfoContract { get; set; }
        public IMatPalletInfoContract MatPalletInfoContract { get; set; }
        public IMaterialInfoContract MaterialInfoContract { get; set; }
        public IMatSupplierInfoContract MatSupplierInfoContract { get; set; }

        [Description("服务-入库单据-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MaterialInStorageInfo MaterialInStorageInfoList = MaterialInStorageInfoContract.MaterialInStorageInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MaterialInStorageInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取物料入库信息数据失败！", MaterialInStorageInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取物料入库信息数据成功！", MaterialInStorageInfoList));
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
        [Description("服务-入库单据-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MaterialInStorageInfoContract.MaterialInStorageInfos, m => new
                {
                    m.Id,
                    m.InStorageBillCode,
                    m.InStorageType,
                    m.ScheduleID,

                    MaterialID = m.MaterialID,
                    MaterialCode = MaterialInfoContract.MaterialInfos.Where(x => x.Id.ToString().Contains(m.MaterialID.ToString())).Select(x => x.MaterialCode).FirstOrDefault(),
                    MaterialName = MaterialInfoContract.MaterialInfos.Where(x => x.Id.ToString().Contains(m.MaterialID.ToString())).Select(x => x.MaterialName).FirstOrDefault(),
                    FullPalletQuantity = MaterialInfoContract.MaterialInfos.Where(x => x.Id.ToString().Contains(m.MaterialID.ToString())).Select(x => x.FullPalletQuantity).FirstOrDefault(),

                    m.Quantity,
                    m.PalletQuantity,

                    m.PalletID,
                    PalletCode = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletCode).FirstOrDefault(),
                    PalletName = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletName).FirstOrDefault(),

                    MatSupplierID = m.MatSupplierID,
                    SupplierCode = MatSupplierInfoContract.MatSupplierInfos.Where(x => x.Id.ToString().Contains(m.MatSupplierID.ToString())).Select(x => x.SupplierCode).FirstOrDefault(),
                    SupplierName = MatSupplierInfoContract.MatSupplierInfos.Where(x => x.Id.ToString().Contains(m.MatSupplierID.ToString())).Select(x => x.SupplierName).FirstOrDefault(),

                    //m.PalletFinishQuantity,
                    m.InStorageTime,
                    m.FinishTime,
                    m.InStorageStatus,
                    //m.CreateUser,
                    m.AuditPerson,
                    m.AuditStatus,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取物料入库信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取物料入库信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-入库单据-增加")]
        public async Task<IHttpActionResult> Add(params MaterialInStorageInfoInputDto[] inputDtos)
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
            var result = await MaterialInStorageInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-入库单据-修改")]
        public async Task<IHttpActionResult> Update(params MaterialInStorageInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MaterialInStorageInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-入库单据-审核")]
        public async Task<IHttpActionResult> Audit(params MaterialInStorageInfoInputDto[] dto)
        {
            //审核人和审核时间
            dto?.ToList().ForEach((a) =>
            {
                a.AuditPerson = User.Identity.Name;
            });
            var result = await MaterialInStorageInfoContract.Audit(dto);
            return Json(result);
        }

        [Description("服务-入库单据-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MaterialInStorageInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-入库单据-入库作业")]
        public async Task<IHttpActionResult> Distribute(params MaterialInStorageInfoInputDto[] dtos)
        {
            try
            {
                //dtos.CheckNotNull("dtos");
                //修改人员、时间
                dtos?.ToList().ForEach((a) =>
                {
                    a.UserName = User.Identity.Name;
                });
                var result = await MaterialInStorageInfoContract.AddTask(dtos);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "增加入库任务！" + ex.ToString()));
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

        [Description("服务-入库单据-入库单号数据")]
        public IHttpActionResult GetInStorageBillCode()
        {
            try
            {
                List<string> codeInfoList = new List<string>();
                string sInStorageBillCode = "InStorage" + string.Format($"{DateTime.Now:yyyyMMddHHmmss}", DateTime.Now);
                codeInfoList.Add(sInStorageBillCode);
                return Json(new OperationResult(OperationResultType.Success, "查询入库单信息成功！", codeInfoList));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询入库单信息失败！" + ex.ToString()));
            }
        }

        [Description("服务-入库单据-成品自动入库")]
        public async Task<IHttpActionResult> ProductInStorageShowTask(params MaterialInStorageInfoInputDto[] dtos)
        {
            //修改人员、时间
            dtos?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await MaterialInStorageInfoContract.ProductInStorageShowTask(dtos);
            return Json(result);
        }
    }
}
