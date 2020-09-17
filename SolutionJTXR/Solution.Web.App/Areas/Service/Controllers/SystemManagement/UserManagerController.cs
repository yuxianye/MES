using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using Solution.Core.Contracts;
using Solution.Core.Dtos.Identity;
using Solution.Core.Models.Identity;
using Solution.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    //[Description("系统管理-用户管理")]
    [Description("服务-系统管理-用户管理")]
    public class UserManagerController : ServiceBaseApiController
    {
        /// <summary>
        /// 获取或设置 身份认证业务对象
        /// </summary>
        public IIdentityContract IdentityContract { get; set; }
        /// <summary>
        /// 安全服务
        /// </summary>
        public SecurityService SecurityService { get; set; }

        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }

        /// <summary>
        /// 获取或设置 用户管理器
        /// </summary>
        public UserManager<User, int> UserManager { get; set; }

        [Description("系统管理-用户管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(UserManager.Users, m => new
                {
                    m.Id,
                    m.UserName,
                    m.NickName,
                    Password = m.PasswordHash,
                    m.Email,
                    m.EmailConfirmed,
                    m.PhoneNumber,
                    m.PhoneNumberConfirmed,
                    m.LockoutEndDateUtc,
                    m.AccessFailedCount,
                    m.IsLocked,
                    m.CreatedTime
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询用户数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询用户数据失败！" + ex.ToString()));
            }
        }

        public IHttpActionResult GetRolesByUserId(int id)
        {
            try
            {
                List<int> roleIds = UserRoleMapRepository.Entities.Where(x => x.User.Id == id).Select(x => x.Role.Id).ToList();
                return Json(new OperationResult(OperationResultType.Success, "查询用户对应角色数据成功！", roleIds));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询用户对应角色数据失败！" + ex.ToString()));
            }
        }

        //[Description("系统管理-用户管理-查询一个")]
        //public IHttpActionResult Get(int id)
        //{
        //    User user = IdentityContract.Users.ToList().Find(delegate (User data)
        //    {
        //        return data.Id == id;
        //    });
        //    if (user == null)
        //    {
        //        return Json(new OperationResult(OperationResultType.Error, "查询用户数据失败！"));
        //    }
        //    return Json(new OperationResult(OperationResultType.Success, "查询用户数据成功！", user));
        //}

        [Description("系统管理-用户管理-添加")]
        public async Task<IHttpActionResult> Add(params UserInputDto[] dtos)
        {
            try
            {
                dtos.CheckNotNull("dtos");
                OperationResult result = await IdentityContract.AddUsers(dtos);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "添加用户！" + ex.ToString()));
            }
        }

        [Description("系统管理-用户管理-修改")]
        public async Task<IHttpActionResult> Update(params UserInputDto[] dtos)
        {
            try
            {
                dtos.CheckNotNull("dtos");
                OperationResult result = await IdentityContract.EditUsers(dtos);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "修改用户！" + ex.ToString()));
            }
        }

        [Description("系统管理-用户管理-删除")]
        public async Task<IHttpActionResult> Remove(int[] ids)
        {
            try
            {
                ids.CheckNotNull("ids");
                OperationResult result = await IdentityContract.DeleteUsers(ids);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "删除用户！" + ex.ToString()));
            }
        }

    }
}