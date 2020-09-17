using OSharp.Utility.Data;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers.EquipmentManagement
{
    public class EquMaintenancePlanInfoController : ServiceBaseApiController
    {
        //设备维护计划信息契约
        public IEquMaintenancePlanInfoContract EquMaintenancePlanInfoContract { get; set; }

        [Description("设备管理-维护计划信息-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EquMaintenancePlanInfoContract.EquMaintenancePlanInfos, m => new
                {
                    m.Id,
                    EquipmentName = m.EquipmentInfo.EquipmentName,
                    EquipmentInfo_Id = m.EquipmentInfo.Id,
                    m.MaintenanceCode,
                    m.MaintenanceTypeInfo,
                    m.Maker,
                    m.PlanStopDuration,
                    m.PlanStartTime,
                    m.PlanFinishTime,
                    m.ActualStartTime,
                    m.ActualFinishTime,
                    m.MaintenanceContent,
                    m.UndertakeTeamID,
                    m.Result,
                    CheckDepartmentID = m.CheckDepartment.Id,
                    DepartmentInfo_Name = m.CheckDepartment.DepartmentName,
                    m.MaintenancePerson,
                    m.MaintenancePlanStateInfo,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取设备维护计划信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取设备维护计划信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("设备管理-维护计划信息-增加")]
        public async Task<IHttpActionResult> Add(params EquMaintenancePlanInfoInputDto[] inputDtos)
        {
            inputDtos?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EquMaintenancePlanInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("设备管理-维护计划信息-修改")]
        public async Task<IHttpActionResult> Update(params EquMaintenancePlanInfoInputDto[] dto)
        {
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EquMaintenancePlanInfoContract.Update(dto);
            return Json(result);
        }

        [Description("设备管理-维护计划信息-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await EquMaintenancePlanInfoContract.Delete(ids);
            return Json(result);
        }
    }
}