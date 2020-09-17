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
    [Description("服务-员工管理")]
    public class EntEmployeeInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 车间信息契约
        /// </summary>
        public IEntEmployeeInfoContract EntEmployeeInfoContract { get; set; }

        [HttpPost]
        [Description("服务-员工管理-根据ID查询")]
        public IHttpActionResult GetEntEmployeeInfo(EntEmployeeInfo entareainfo)
        {
            EntEmployeeInfo entEmployeeInfo = EntEmployeeInfoContract.EntEmployeeInfo.ToList().Find(s =>
            {
                return s.Id == entareainfo.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取员工数据成功！", entEmployeeInfo));
        }

        [HttpPost]
        [Description("服务-员工管理-分页数据")]
        public IHttpActionResult GetEntEmployeeList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EntEmployeeInfoContract.EntEmployeeInfo, m => new
                {
                    m.Id,
                    m.EntEmployeeName,
                    m.EntEmployeeCode,
                    m.EntEmployeeSex,
                    m.BirthDate,
                    m.DepartmentInfo.DepartmentName,
                    m.WorkPost,
                    m.WorkBranch,
                    m.Skills,
                    m.EntryDate,
                    m.Education,
                    m.ProfessionalTitles,
                    m.WorkExperience,
                    m.AwardRecord,
                    DepartmentInfoName= m.DepartmentInfo.DepartmentName,
                    DepartmentCode = m.DepartmentInfo.Id,
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
        public IHttpActionResult GetEntAreaListByEntSiteID(EntDepartmentInfo entsiteInfo)
        {
            try
            {

                var page = GetPageResult(EntEmployeeInfoContract.EntEmployeeInfo.Where(m => m.DepartmentInfo.Id == entsiteInfo.Id), m => new
                {
                    m.Id,
                    m.EntEmployeeName,
                    m.EntEmployeeCode,
                    m.EntEmployeeSex,
                    m.BirthDate,
                    m.DepartmentInfo.DepartmentName,
                    m.WorkPost,
                    m.WorkBranch,
                    m.Skills,
                    m.EntryDate,
                    m.Education,
                    m.ProfessionalTitles,
                    m.WorkExperience,
                    m.AwardRecord,
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

        public async Task<IHttpActionResult> Add(params EntEmployeeInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EntEmployeeInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-员工管理-修改")]
        public async Task<IHttpActionResult> Update(params EntEmployeeInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EntEmployeeInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-员工管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await EntEmployeeInfoContract.Delete(ids);
            return Json(result);
        }
    }
}