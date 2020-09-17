using OSharp.Core.Data.Extensions;
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
    /// 物料信息API控制器
    /// </summary>

    [Description("服务-物料管理")]
    public class MaterialInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 物料信息契约
        /// </summary>
        public IMaterialInfoContract MaterialInfoContract { get; set; }

        [Description("服务-物料管理-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            MaterialInfo MaterialInfoList = MaterialInfoContract.MaterialInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (MaterialInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取物料信息数据失败！", MaterialInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取物料信息数据成功！", MaterialInfoList));
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
        [Description("服务-物料管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MaterialInfoContract.MaterialInfos, m => new
                {
                    m.Id,
                    m.MaterialCode,
                    m.MaterialName,
                    m.MaterialType,
                    m.MaterialUnit,
                    m.FullPalletQuantity,
                    m.Specification,
                    m.UnitWeight,
                    m.MaxStock,
                    m.MinStock,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取物料信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取物料信息列表数据失败！" + ex.ToString()));
            }
        }

        //原料 = 1,
        [Description("服务-物料管理-分页数据(原料)")]
        public IHttpActionResult PageData1(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MaterialInfoContract.MaterialInfos.Where(m=>m.MaterialType == 1), m => new
                {
                    m.Id,
                    m.MaterialCode,
                    m.MaterialName,
                    m.MaterialType,
                    m.MaterialUnit,
                    m.FullPalletQuantity,
                    m.Specification,
                    m.UnitWeight,
                    m.MaxStock,
                    m.MinStock,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取物料信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取物料信息列表数据失败！" + ex.ToString()));
            }
        }

        //成品 = 3
        [Description("服务-物料管理-分页数据(成品)")]
        public IHttpActionResult PageData2(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(MaterialInfoContract.MaterialInfos.Where(m => m.MaterialType == 3), m => new
                {
                    m.Id,
                    m.MaterialCode,
                    m.MaterialName,
                    m.MaterialType,
                    m.MaterialUnit,
                    m.FullPalletQuantity,
                    m.Specification,
                    m.UnitWeight,
                    m.MaxStock,
                    m.MinStock,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取物料信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取物料信息列表数据失败！" + ex.ToString()));
            }
        }
        [Description("服务-物料管理-增加")]
        public async Task<IHttpActionResult> Add(params MaterialInfoInputDto[] inputDtos)
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
            var result = await MaterialInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-物料管理-修改")]
        public async Task<IHttpActionResult> Update(params MaterialInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await MaterialInfoContract.UpdateMaterials(dto);
            return Json(result);
        }

        [Description("服务-物料管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MaterialInfoContract.DeleteMaterials(ids);
            return Json(result);
        }
    }
}
