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
    [Description("服务-Agv任务管理")]
    public class TaskInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// Agv任务信息契约
        /// </summary>
        public ITaskInfoContract TaskInfoContract { get; set; }

        /// <summary>
        /// Agv任务详细信息契约
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
        [Description("服务-Agv任务管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {

                GridRequest request = new GridRequest(requestParams);

                var page = GetPageResult(TaskInfoContract.TaskInfos, m => new
                {
                    m.Id,
                    m.TaskNo,
                    m.TaskName,
                    m.TaskStatus,
                    m.TaskType,
                    TargetPointInfo_Id = m.TargetPointInfo.Id,
                    TaskCar_Id = m.TaskCar.Id,
                    RouteInfo_Id = m.RouteInfo.Id,
                    TargetPointInfoName = m.TargetPointInfo.MarkPointName,
                    TaskCarName = m.TaskCar.CarName,
                    RouteInfoName = m.RouteInfo.MarkPoints,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取AGV任务信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取AGV任务信息列表数据失败！" + ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-Agv任务管理-获取某一任务的任务详细")]
        public IHttpActionResult GetTaskDetailedInfoListByInventoryID(TaskDetailedInfo info)
        {
            try
            {

                var page = GetPageResult(TaskDetailedInfoContract.TaskDetailedInfos.Where(m => m.TaskInfo.Id == info.Id), m => new
                {
                    m.Id,
                    TaskInfo_Id = m.TaskInfo.Id,
                    m.TaskInfo.TaskName,
                    Material_Id = m.Material.Id,
                    m.Material.MaterialName,

                    m.Quantity,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一分步教学任务调度主信息下的调度明细信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一分步教学任务调度主信息下的调度明细信息列表数据失败！", ex.ToString()));
            }
        }

        [Description("服务-Agv任务管理-增加Agv任务信息")]
        public async Task<IHttpActionResult> Add(params Agv.Dtos.TaskInfoInputDto[] dto)
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
            var result = await TaskInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-Agv任务管理-修改Agv任务信息")]
        public async Task<IHttpActionResult> Update(params Agv.Dtos.TaskInfoInputDto[] dto)
        {
            var result = await TaskInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-Agv任务管理-物理删除任务信息")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await TaskInfoContract.Delete(ids);
            return Json(result);
        }

    }
}
