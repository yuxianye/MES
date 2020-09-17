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
    [Description("服务-报警管理")]
    public class AlarmInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 报警信息契约
        /// </summary>
        public IAlarmInfoContract AlarmInfoContract { get; set; }

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
        [Description("服务-报警管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);

                var page = GetPageResult(AlarmInfoContract.AlarmInfos, m => new
                {
                    m.Id,
                    m.AlarmCode,
                    AgvName = m.AgvInfo.CarName,
                    CarPosition_Name = m.CarPosition.MarkPointName,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取报警信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取报警信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-报警管理-增加报警信息")]
        public async Task<IHttpActionResult> Add(params Agv.Dtos.AlarmInfoInputDto[] dto)
        {
            var result = await AlarmInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-报警管理-修改报警信息")]
        public async Task<IHttpActionResult> Update(params Agv.Dtos.AlarmInfoInputDto[] dto)
        {
            var result = await AlarmInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-报警管理-物理删除报警信息")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await AlarmInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
