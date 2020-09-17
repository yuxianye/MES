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
using Solution.TakeOutWarehouseManagement.Dtos;
using Solution.TakeOutWarehouseManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 物料出库信息API控制器
    /// </summary>

    [Description("服务-出库单据")]
    public class MaterialOutStorageInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 物料出库信息契约
        /// </summary>
        public IMaterialOutStorageInfoContract MaterialOutStorageInfoContract { get; set; }
        public IMatPalletInfoContract MatPalletInfoContract { get; set; }
        public IMaterialInfoContract MaterialInfoContract { get; set; }
        public IMatWareHouseLocationInfoContract MatWareHouseLocationInfoContract { get; set; }

        [Description("服务-出库单据-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MaterialOutStorageInfo MaterialOutStorageInfoList = MaterialOutStorageInfoContract.MaterialOutStorageInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MaterialOutStorageInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取物料出库信息数据失败！", MaterialOutStorageInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取物料出库信息数据成功！", MaterialOutStorageInfoList));
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
        [Description("服务-出库单据-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MaterialOutStorageInfoContract.MaterialOutStorageInfos, m => new
                {
                    m.Id,
                    m.OutStorageBillCode,
                    m.OutStorageType,
                    m.PlanScheduleID,

                    MaterialID = m.MaterialID,
                    MaterialCode = MaterialInfoContract.MaterialInfos.Where(x => x.Id.ToString().Contains(m.MaterialID.ToString())).Select(x => x.MaterialCode).FirstOrDefault(),
                    MaterialName = MaterialInfoContract.MaterialInfos.Where(x => x.Id.ToString().Contains(m.MaterialID.ToString())).Select(x => x.MaterialName).FirstOrDefault(),
                    FullPalletQuantity = MaterialInfoContract.MaterialInfos.Where(x => x.Id.ToString().Contains(m.MaterialID.ToString())).Select(x => x.FullPalletQuantity).FirstOrDefault(),

                    m.Quantity,
                    m.PalletQuantity,

                    WareHouseLocationID = MatWareHouseLocationInfoContract.MatWareHouseLocationInfos.Where(x => x.PalletID.ToString().Contains(m.PalletID.ToString())).Select(x => x.Id).FirstOrDefault(),

                    m.PalletID,
                    PalletCode = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletCode).FirstOrDefault(),
                    PalletName = MatPalletInfoContract.MatPalletInfos.Where(x => x.Id.ToString().Contains(m.PalletID.ToString())).Select(x => x.PalletName).FirstOrDefault(),

                    m.OutStorageTime,
                    m.FinishTime,
                    m.OutStorageStatus,
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
                return Json(new OperationResult(OperationResultType.Success, "读取物料出库信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取物料出库信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-出库单据-增加")]
        public async Task<IHttpActionResult> Add(params MaterialOutStorageInfoInputDto[] inputDtos)
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
            var result = await MaterialOutStorageInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-出库单据-修改")]
        public async Task<IHttpActionResult> Update(params MaterialOutStorageInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MaterialOutStorageInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-出库单据-审核")]
        public async Task<IHttpActionResult> Audit(params MaterialOutStorageInfoInputDto[] dto)
        {
            //审核人和审核时间
            dto?.ToList().ForEach((a) =>
            {
                a.AuditPerson = User.Identity.Name;
            });
            var result = await MaterialOutStorageInfoContract.Audit(dto);
            return Json(result);
        }


        [Description("服务-出库单据-取消审核")]
        public async Task<IHttpActionResult> UnAudit(params MaterialOutStorageInfoInputDto[] dto)
        {
            //审核人和审核时间
            dto?.ToList().ForEach((a) =>
            {
                a.AuditPerson = "";
            });
            var result = await MaterialOutStorageInfoContract.Audit(dto);
            return Json(result);
        }


        [Description("服务-出库单据-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MaterialOutStorageInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-出库单据-出库作业")]
        public async Task<IHttpActionResult> Distribute(params MaterialOutStorageInfoInputDto[] dtos)
        {
            try
            {
                //dtos.CheckNotNull("dtos");
                //修改人员、时间
                dtos?.ToList().ForEach((a) =>
                {
                    a.UserName = User.Identity.Name;
                });
                var result = await MaterialOutStorageInfoContract.AddTask(dtos);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "增加物料出库任务！" + ex.ToString()));
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

        [Description("服务-出库单据-出库单号数据")]
        public IHttpActionResult GetOutStorageBillCode()
        {
            try
            {
                List<string> codeInfoList = new List<string>();
                string sOutStorageBillCode = "OutStorage" + string.Format($"{DateTime.Now:yyyyMMddHHmmss}", DateTime.Now);
                codeInfoList.Add(sOutStorageBillCode);
                return Json(new OperationResult(OperationResultType.Success, "查询出库单信息成功！", codeInfoList));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询出库单信息失败！" + ex.ToString()));
            }
        }

        [Description("服务-出库单据-原料自动出库演示操作")]
        public async Task<IHttpActionResult> MaterialOutStorageShowTask(params MaterialOutStorageInfoInputDto[] dtos)
        {
            //修改人员、时间
            dtos?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await MaterialOutStorageInfoContract.MaterialOutStorageShowTask(dtos);
            return Json(result);
        }
    }
}
