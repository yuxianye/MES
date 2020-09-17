using OSharp.Utility;
using OSharp.Utility.Data;
using Solution.Core.Contracts;
using Solution.Core.Dtos.Identity;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-系统管理-角色管理")]
    public class RoleManagerController : ServiceBaseApiController
    {
        /// <summary>
        /// 获取或设置 身份认证业务对象
        /// </summary>
        public IIdentityContract IdentityContract { get; set; }

        [Description("系统管理-角色管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(IdentityContract.Roles, m => new
                {
                    m.Id,
                    m.Name,
                    m.Remark,
                    m.IsAdmin,
                    m.IsSystem,
                    m.IsDefault,
                    m.IsLocked,
                    m.CreatedTime,
                    OrganizationId = m.Organization == null ? 0 : m.Organization.Id,
                    OrganizationName = m.Organization.Name,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询角色数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询角色数据失败！" + ex.ToString()));
            }
        }


        [Description("系统管理-角色管理-增加")]
        public async Task<IHttpActionResult> Add(RoleInputDto[] dtos)
        {
            try
            {
                dtos.CheckNotNull("dtos");
                OperationResult result = await IdentityContract.AddRoles(dtos);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "添加角色！" + ex.ToString()));
            }
        }

        [Description("系统管理-角色管理-修改")]
        public async Task<IHttpActionResult> Update(RoleInputDto[] dtos)
        {
            try
            {
                dtos.CheckNotNull("dtos");
                OperationResult result = await IdentityContract.EditRoles(dtos);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "修改角色！" + ex.ToString()));
            }
        }

        [Description("系统管理-角色管理-删除")]
        public IHttpActionResult Remove(int[] ids)
        {
            try
            {
                ids.CheckNotNull("ids");
                OperationResult result = IdentityContract.DeleteRoles(ids);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "删除角色！" + ex.ToString()));
            }
        }
    }
}