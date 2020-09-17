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
    /// 仓库类型信息API控制器
    /// </summary>

    [Description("服务-仓库类型")]
    public class MatWareHouseTypeInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 仓库类型信息契约
        /// </summary>
        public IMatWareHouseTypeInfoContract MatWareHouseTypeInfoContract { get; set; }

        [Description("服务-仓库类型-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MatWareHouseTypeInfo MatWareHouseTypeInfoList = MatWareHouseTypeInfoContract.MatWareHouseTypeInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MatWareHouseTypeInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取仓库类型信息数据失败！", MatWareHouseTypeInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取仓库类型信息数据成功！", MatWareHouseTypeInfoList));
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
        [Description("服务-仓库类型-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MatWareHouseTypeInfoContract.MatWareHouseTypeInfos, m => new
                {
                    m.Id,
                    m.WareHouseTypeCode,
                    m.WareHouseTypeName,                     
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取仓库类型信息列表数据成功！", page));                
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取仓库类型信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-仓库类型-增加")]
        public async Task<IHttpActionResult> Add(params MatWareHouseTypeInfoInputDto[] inputDtos)
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
            //var result = await MatWareHouseTypeInfoContract.AddMatWareHouseTypes(inputDtos);
            var result = await MatWareHouseTypeInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-仓库类型-修改")]
        public async Task<IHttpActionResult> Update(params MatWareHouseTypeInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MatWareHouseTypeInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-仓库类型-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MatWareHouseTypeInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
