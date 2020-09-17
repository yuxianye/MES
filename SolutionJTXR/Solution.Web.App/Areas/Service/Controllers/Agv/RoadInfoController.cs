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
    [Description("服务-路段管理")]
    public class RoadInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// Agv信息契约
        /// </summary>
        public IRoadInfoContract RoadInfoContract { get; set; }

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
        [Description("服务-路段管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {

                GridRequest request = new GridRequest(requestParams);

                var page = GetPageResult(RoadInfoContract.RoadInfos, m => new
                {
                    m.Id,
                    m.RoadNo,
                    m.RoadName,
                    m.Distance,
                    m.RoadStatus,
                    StartMarkPointInfo_Id = m.StartMarkPointInfo.Id,
                    EndMarkPointInfo_Id = m.EndMarkPointInfo.Id,
                    StartMarkPointInfoName = m.StartMarkPointInfo.MarkPointName,
                    EndMarkPointInfoName = m.EndMarkPointInfo.MarkPointName,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取路段信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取路段信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-路段管理-增加路段信息")]
        public async Task<IHttpActionResult> Add(params Agv.Dtos.RoadInfoInputDto[] dto)
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
            var result = await RoadInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-路段管理-修改路段信息")]
        public async Task<IHttpActionResult> Update(params Agv.Dtos.RoadInfoInputDto[] dto)
        {
            var result = await RoadInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-路段管理-物理删除路段信息")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await RoadInfoContract.Delete(ids);
            return Json(result);
        }

    }
}
