using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using Solution.Agv.Contracts;
using Solution.Agv.Models;
using Solution.Core.Models.Identity;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-地标管理")]
    public class MarkPointInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 地标信息契约
        /// </summary>
        public IMarkPointInfoContract MarkPointInfoContract { get; set; }

        /// <summary>
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
        [Description("服务-地标管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {

                GridRequest request = new GridRequest(requestParams);

                var page = GetPageResult(MarkPointInfoContract.MarkPointInfos, m => new
                {
                    m.Id,
                    m.MarkPointNo,
                    m.MarkPointName,
                    m.AreaId,
                    m.ProductLineId,
                    m.X,
                    m.Y,
                    m.IsVirtualMarkPoint,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取地标信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取地标信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-地标管理-增加地标信息")]
        public async Task<IHttpActionResult> Add(params Agv.Dtos.MarkPointInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await MarkPointInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-地标管理-修改地标信息")]
        public async Task<IHttpActionResult> Update(params Agv.Dtos.MarkPointInfoInputDto[] dto)
        {
            var result = await MarkPointInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-地标管理-物理删除地标信息")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await MarkPointInfoContract.Delete(ids);
            return Json(result);
        }

    }
}
