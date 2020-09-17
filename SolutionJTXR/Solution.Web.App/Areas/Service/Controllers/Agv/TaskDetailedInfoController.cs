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
    [Description("服务-Agv任务详细管理")]
    public class TaskDetailedInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// Agv信息契约
        /// </summary>
        public ITaskDetailedInfoContract TaskDetailedInfoContract { get; set; }

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
        [Description("服务-Agv任务详细管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {

                GridRequest request = new GridRequest(requestParams);

                var page = GetPageResult(TaskDetailedInfoContract.TaskDetailedInfos, m => new
                {
                    m.Id,
                    m.Quantity,
                    TaskInfo_Id = m.TaskInfo.Id,
                    m.TaskInfo.TaskName,
                    Material_Id = m.Material.Id,
                    m.Material.MaterialName,

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

        [Description("服务-Agv任务详细管理-增加Agv任务详细信息")]
        public async Task<IHttpActionResult> Add(params Agv.Dtos.TaskDetailedInfoInputDto[] dto)
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
            var result = await TaskDetailedInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-Agv任务详细管理-修改Agv任务详细信息")]
        public async Task<IHttpActionResult> Update(params Agv.Dtos.TaskDetailedInfoInputDto[] dto)
        {
            var result = await TaskDetailedInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-Agv任务详细管理-物理删除任务详细信息")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await TaskDetailedInfoContract.Delete(ids);
            return Json(result);
        }

    }
}
