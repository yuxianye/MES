using OSharp.Utility.Data;
using Solution.StepTeachingDispatchManagement.Contracts;
using Solution.StepTeachingDispatchManagement.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 分步教学任务调度明细信息API控制器
    /// </summary>

    [Description("服务-分步教学任务调度明细表")]
    public class DisTaskDispatchItemInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 分步教学任务调度信息契约
        /// </summary>
        public IDisTaskDispatchInfoContract DisTaskDispatchInfoContract { get; set; }
        /// <summary>
        /// 分步教学任务调度明细信息契约
        /// </summary>
        public IDisTaskDispatchItemInfoContract DisTaskDispatchItemInfoContract { get; set; }

        [Description("服务-分步教学任务调度明细表-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            DisTaskDispatchItemInfo DisTaskDispatchItemInfoList = DisTaskDispatchItemInfoContract.DisTaskDispatchItemInfos.ToList().Find(s =>
            {
                return s.Id == guid;
            });
            if (DisTaskDispatchItemInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取分步教学任务调度明细信息数据失败！", DisTaskDispatchItemInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取分步教学任务调度明细信息数据成功！", DisTaskDispatchItemInfoList));
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
        [Description("服务-分步教学任务调度明细表-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(DisTaskDispatchItemInfoContract.DisTaskDispatchItemInfos, m => new
                {
                    m.Id,

                    DisStepTeachingTaskDispatch_Id = m.DisStepTeachingTaskDispatch.Id,
                    Equipment_Id = m.Equipment.Id,
                    m.Equipment.EquipmentName,
                    m.Equipment.EquipmentCode,
                    EquipmentAction_Id = m.EquipmentAction.Id,
                    //m.EquipmentAction.EquipmentActionName,
                    //m.EquipmentAction.EquipmentActionCode,

                    m.ActionOrder,
                    m.TaskItemCode,
                    m.TaskItemResult,
                    m.FinishTime,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取分步教学任务调度明细信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取分步教学任务调度明细信息列表数据失败！" + ex.ToString()));
            }
        }


        //[Description("服务-分步教学任务调度明细表-删除明细")]
        //public async Task<IHttpActionResult> Remove(params Guid[] ids)
        //{
        //    var result = await DisTaskDispatchItemInfoContract.Delete(ids);
        //    return Json(result);
        //}
    }
}
