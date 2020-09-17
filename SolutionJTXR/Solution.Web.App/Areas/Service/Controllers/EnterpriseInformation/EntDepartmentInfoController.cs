using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Contracts;
using Solution.EnterpriseInformation.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-部门管理")]
    public class EntDepartmentInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 部门信息服务契约接口
        /// </summary>
        public IEntDepartmentInfoContract EntDepartmentInfoContract { get; set; }

        [Description("服务-部门管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                string username = User.Identity.Name;
                //User user = userManager.Users.Where(x => x.UserName.Equals(username)).FirstOrDefault();
                //UserRoleMap userRoleMap = UserRoleMapRepository.Entities.Where(x => x.User.Id == user.Id).First();
                //Role role = userRoleMap.Role;

                GridRequest request = new GridRequest(requestParams);

                var page = GetPageResult(EntDepartmentInfoContract.EntDepartmentInfos, m => new
                {
                    m.Id,
                    m.DepartmentName,
                    m.DepartmentCode,
                    m.DepartmentFunction,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取部门信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取部门信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-部门管理-增加")]
        public async Task<IHttpActionResult> Add(params EntDepartmentInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.DepartmentName = a.DepartmentName;
                a.DepartmentCode = a.DepartmentCode;
                a.DepartmentFunction = a.DepartmentFunction;
                a.Description = a.Description;
                a.Remark = a.Remark;
                a.CreatedTime = DateTime.Now;
                a.CreatorUserId = User.Identity.Name;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EntDepartmentInfoContract.AddEntDepartmentInfos(dto);
            return Json(result);
        }

        [Description("服务-部门管理-修改")]
        public async Task<IHttpActionResult> Update(params EntDepartmentInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });

            var result = await EntDepartmentInfoContract.UpdateEntDepartmentInfos(dto);
            return Json(result);
        }

        [Description("服务-部门管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await EntDepartmentInfoContract.DeleteEntDepartmentInfos(ids);
            return Json(result);
        }
    }
}