using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using Solution.StepTeachingDispatchManagement.Contracts;
using Solution.StepTeachingDispatchManagement.Dtos;
using Solution.StepTeachingDispatchManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 分步教学任务调度信息API控制器
    /// </summary>

    [Description("服务-分步教学任务调度主表")]
    public class DisTaskDispatchInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 分步教学任务调度信息契约
        /// </summary>
        public IDisTaskDispatchInfoContract DisTaskDispatchInfoContract { get; set; }
        /// <summary>
        /// 分步教学任务调度明细信息契约
        /// </summary>
        public IDisTaskDispatchItemInfoContract DisTaskDispatchItemInfoContract { get; set; }



        [Description("服务-分步教学任务调度主表-根据ID查询")]
        public IHttpActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            DisTaskDispatchInfo DisTaskDispatchInfoList = DisTaskDispatchInfoContract.DisTaskDispatchInfos.ToList().Find(s =>
               {
                   return s.Id == guid;
               });
            if (DisTaskDispatchInfoList == null)
            {
                return Json(new OperationResult(OperationResultType.Success, "读取分步教学任务调度主表信息数据失败！", DisTaskDispatchInfoList));
            }
            return Json(new OperationResult(OperationResultType.Success, "读取分步教学任务调度主表信息数据成功！", DisTaskDispatchInfoList));
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
        [Description("服务-分步教学任务调度主表-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(DisTaskDispatchInfoContract.DisTaskDispatchInfos, m => new
                {
                    m.Id,
                    DisStepAction_Id = m.DisStepAction.Id,
                    DisStepActionName = m.DisStepAction.StepActionName,
                    DisStepActionCode = m.DisStepAction.StepActionCode,
                    m.TaskCode,
                    m.TaskResult,
                    m.FinishTime,
                    m.Description,
                    //ItemCount = DisTaskDispatchItemInfoContract.DisTaskDispatchItemInfos.Count(m2 => m2.DisStepTeachingTaskDispatch.Id == m.Id),
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取分步教学任务调度信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取分步教学任务调度信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-分步教学任务调度主表-增加")]
        public async Task<IHttpActionResult> Add(params DisTaskDispatchInfoInputDto[] inputDtos)
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
            var result = await DisTaskDispatchInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("服务-分步教学任务调度主表-修改")]
        public async Task<IHttpActionResult> Update(params DisTaskDispatchInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            //
            var result = await DisTaskDispatchInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-分步教学任务调度主表-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await DisTaskDispatchInfoContract.Delete(ids);
            return Json(result);
        }


        [HttpPost]
        [Description("服务-分步教学任务调度主表-获取某一分步教学任务调度主信息下的调度明细列表数据")]
        public IHttpActionResult GetDisTaskDispatchItemInfoListByInventoryID(DisTaskDispatchItemInfo info)
        {
            try
            {

                var page = GetPageResult(DisTaskDispatchItemInfoContract.DisTaskDispatchItemInfos.Where(m => m.DisStepTeachingTaskDispatch.Id == info.Id), m => new
                {
                    m.Id,

                    DisStepTeachingTaskDispatch_Id = m.DisStepTeachingTaskDispatch.Id,
                    m.DisStepTeachingTaskDispatch.TaskCode,
                    Equipment_Id = m.Equipment.Id,
                    m.Equipment.EquipmentName,
                    m.Equipment.EquipmentCode,
                    EquipmentAction_Id = m.EquipmentAction.Id,
                    //m.EquipmentAction.EquipmentActionName,
                    //m.EquipmentAction.EquipmentActionCode,
                    //m.EquipmentAction,
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
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某一分步教学任务调度主信息下的调度明细信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某一分步教学任务调度主信息下的调度明细信息列表数据失败！", ex.ToString()));
            }
        }


        [Description("服务-分步教学任务调度主表-增加明细")]
        public async Task<IHttpActionResult> AddItem(params DisTaskDispatchItemInfoInputDto[] dto)
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
            var result = await DisTaskDispatchItemInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-分步教学任务调度主表-修改明细")]
        public async Task<IHttpActionResult> UpdateItem(params DisTaskDispatchItemInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await DisTaskDispatchItemInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-分步教学任务调度主表-调度单号数据")]
        public IHttpActionResult GetInventoryCode()
        {
            try
            {
                List<string> codeInfoList = new List<string>();
                string sInventoryCode = "Inventory" + string.Format($"{DateTime.Now:yyyyMMddHHmmss}", DateTime.Now);
                codeInfoList.Add(sInventoryCode);
                return Json(new OperationResult(OperationResultType.Success, "查询调度单信息成功！", codeInfoList));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询调度单信息失败！" + ex.ToString()));
            }
        }
    }
}
