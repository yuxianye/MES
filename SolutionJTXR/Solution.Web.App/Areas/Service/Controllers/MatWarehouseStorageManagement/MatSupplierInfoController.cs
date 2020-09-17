using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 供应商信息API控制器
    /// </summary>

    [Description("服务-供应商管理")]
    public class MatSupplierInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 供应商信息契约
        /// </summary>
        public IMatSupplierInfoContract MatSupplierInfoContract { get; set; }

        [Description("服务-供应商管理-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatSupplierInfo MatSupplierInfoList = MatSupplierInfoContract.MatSupplierInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatSupplierInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取供应商信息数据失败！", MatSupplierInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取供应商信息数据成功！", MatSupplierInfoList));
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
        /// 
        [Description("服务-供应商管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatSupplierInfoContract.MatSupplierInfos, m => new
                {
                    m.Id,
                    m.SupplierCode,
                    m.SupplierName,
                    m.SupplierPhone,
                    m.SupplierFax,
                    m.SupplierAddress,
                    m.SupplierEmail,
                    m.Contact,
                    m.Description,
                    m.IsUse,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取供应商信息列表数据成功！", page));                
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取供应商信息列表数据失败！" + ex.ToString()));
            }
        }


        [Description("服务-供应商管理-分页数据")]
        public IHttpActionResult PageData2(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatSupplierInfoContract.MatSupplierInfos.Where( m=> m.IsUse == true ), m => new
                {
                    m.Id,
                    m.SupplierCode,
                    m.SupplierName,
                    m.SupplierPhone,
                    m.SupplierFax,
                    m.SupplierAddress,
                    m.SupplierEmail,
                    m.Contact,
                    m.Description,
                    m.IsUse,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取供应商信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取供应商信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-供应商管理-增加")]
        public async Task<IHttpActionResult> Add(params MatSupplierInfoInputDto[] inputDtos)
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
            //var result = await MatSupplierInfoContract.AddMatWareHouseTypes(inputDtos);
            var result = await MatSupplierInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-供应商管理-修改")]
        public async Task<IHttpActionResult> Update(params MatSupplierInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MatSupplierInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-供应商管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatSupplierInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
