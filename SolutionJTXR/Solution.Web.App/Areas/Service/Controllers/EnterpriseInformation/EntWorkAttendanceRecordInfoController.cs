using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Contracts;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers.EnterpriseInformation
{
    [Description("服务-考勤管理")]
    public class EntWorkAttendanceRecordInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 车间信息契约
        /// </summary>
        public IEntWorkAttendanceRecordInfoContract EntWorkAttendanceRecordInfoContract { get; set; }

        [HttpPost]
        [Description("服务-员工管理-根据ID查询")]
        public IHttpActionResult GetEntEmployeeInfo(EntWorkAttendanceRecordInfo entareainfo)
        {
            EntWorkAttendanceRecordInfo entEmployeeInfo = EntWorkAttendanceRecordInfoContract.EntWorkAttendanceRecordInfos.ToList().Find(s =>
            {
                return s.Id == entareainfo.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取员工数据成功！", entEmployeeInfo));
        }

        [HttpPost]
        [Description("服务-员工管理-分页数据")]
        public IHttpActionResult GetEntWorkAttendanceRecordList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EntWorkAttendanceRecordInfoContract.EntWorkAttendanceRecordInfos, m => new
                {
                    m.Id,
                    m.EntEmployeeInfo.EntEmployeeCode,
                    m.EntEmployeeInfo.EntEmployeeName,
                    EntEmployeeID = m.EntEmployeeInfo.Id,
                    m.AttendanceDate,
                    m.StartTime,
                    m.EndTime,
                    m.Shifts,
                    m.LeaveType,
                    m.LeaveDuration,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取员工信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取员工信息列表数据失败！", ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-员工管理-获取某一部门列表")]
        public IHttpActionResult GetEntAreaListByEntSiteID(EntWorkAttendanceRecordInfo entsiteInfo)
        {
            try
            {

                var page = GetPageResult(EntWorkAttendanceRecordInfoContract.EntWorkAttendanceRecordInfos.Where(m => m.EntEmployeeInfo.Id == entsiteInfo.Id), m => new
                {
                    m.Id,
                    //m.EntEmployeeName,
                    //m.EntEmployeeCode,
                    //m.EntEmployeeSex,
                    //m.BirthDate,
                    //m.DepartmentInfo.DepartmentName,
                    //m.WorkPost,
                    //m.WorkBranch,
                    //m.Skills,
                    //m.EntryDate,
                    //m.Education,
                    //m.ProfessionalTitles,
                    //m.WorkExperience,
                    //m.AwardRecord,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                });
                return Json(new OperationResult(OperationResultType.Success, "读取部门信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取部门信息列表数据失败！", ex.ToString()));
            }
        }

        [Description("服务-员工管理-增加")]

        public async Task<IHttpActionResult> Add(params EntWorkAttendanceRecordInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EntWorkAttendanceRecordInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-员工管理-修改")]
        public async Task<IHttpActionResult> Update(params EntWorkAttendanceRecordInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EntWorkAttendanceRecordInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-员工管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await EntWorkAttendanceRecordInfoContract.Delete(ids);
            return Json(result);
        }
    }
}