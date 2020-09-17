using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Dependency;
using OSharp.Core.Security.Models;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Utility.Filter;
using OSharp.Utility.Logging;
using OSharp.Web.Http;
using Solution.Core.Contracts;
using Solution.Core.Dtos.Identity;
using Solution.Core.Dtos.OAuth;
using Solution.Core.Models.Identity;
using Solution.Core.Models.OAuth;
using Solution.Core.Models.Security;
using Solution.Core.OAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 身份认证 webapi
    /// </summary>

    [Description("服务-身份认证")]
    public class IdentityController : BaseApiController
    {
        /// <summary>
        /// 日志
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(IdentityController));


        public IIdentityContract IdentityContract { get; set; }

        /// <summary>
        /// 获取或设置 客户端密钥仓储对象
        /// </summary>
        public IRepository<OAuthClientSecret, int> ClientSecretRepository { get; set; }

        /// <summary>
        /// 获取或设置 用户管理器
        /// </summary>
        public UserManager<User, int> UserManager { get; set; }

        /// <summary>
        /// 登陆成功后返回该用户的功能列表（数据权限类表后续在增加）
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        [Description("服务-身份认证-用户登录")]
        public async Task<IHttpActionResult> Login(LoginInfo loginInfo)
        {
            Logger.Info("{0}开始登陆", loginInfo.UserName);
            var result = await IdentityContract.Login(loginInfo, false);
            OperationResult createClientResult = new OperationResult();
            OperationResult createClientSecretResult = new OperationResult();
            if (result.Successed)
            {
                OAuthClientStore oAuthClientStore = ServiceProvider.GetService<OAuthClientStore>();
                OAuthClientRefreshTokenStore oAuthClientRefreshTokenStore = ServiceProvider.GetService<OAuthClientRefreshTokenStore>();
                OAuthClient oAuthClient = new OAuthClient();
                OAuthClientInputDto clientDto = new OAuthClientInputDto()
                {
                    Name = loginInfo.UserName,
                    OAuthClientType = OAuthClientType.Application,
                    Url = "http://localhost:13800/",
                    LogoUrl = "http://localhost:13800/",
                    RedirectUrl = "http://localhost:13800/"
                };
                try
                {
                    createClientResult = await oAuthClientStore.CreateClient(clientDto);
                }
                catch (Exception ex)
                {
                    Logger.Error("CreateClient错误:" + ex.ToString());
                }
                if (createClientResult.Successed)
                {
                    oAuthClient = oAuthClientStore.GetOAuthClient(clientDto);
                    OAuthClientSecretInputDto secretDto = new OAuthClientSecretInputDto()
                    {
                        Type = "Test Type",
                        Remark = "Remark",
                        ClientId = oAuthClient.Id,
                    };
                    try
                    {
                        createClientSecretResult = await oAuthClientStore.CreateClientSecret(secretDto);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("CreateClientSecret错误:" + ex.ToString());
                    }

                    if (createClientSecretResult.Successed)
                    {
                        OAuthClientSecret oAuthClientSecret = ClientSecretRepository.Entities.Where(x => x.Client.Id == oAuthClient.Id).FirstOrDefault();
                        ClientAndSecretData data = new ClientAndSecretData();
                        data.ClientId = oAuthClient.ClientId;
                        data.ClientSecret = oAuthClientSecret.Value;
                        result.Message = JsonHelper.ToJson(data);

                        return Json(new OperationResult(OperationResultType.Success, JsonHelper.ToJson(data), result.Data));
                    }
                    else
                    {
                        return Json(new OperationResult(OperationResultType.Error, "创建Client Secret失败！"));
                    }
                }
                else
                {
                    return Json(new OperationResult(OperationResultType.Error, "创建Client失败！"));
                }
            }
            else
            {
                return Json(result);
            }
        }

        [Description("服务-身份认证-查询菜单模块信息")]
        public IHttpActionResult ModuleData(LoginInfo loginInfo)
        {

            User user = UserManager.FindByName<User, int>(loginInfo.UserName);
#if DEBUG

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            var userModules = IdentityContract.GetUserRoleModules(user);
            //var result = (from userModule in userModules
            //              select new
            //              {
            //                  Id = userModule.Id,
            //                  Name = userModule.Name,
            //                  Remark = userModule.Remark,
            //                  OrderCode = userModule.OrderCode,
            //                  //TreePathString = userModule.TreePathString,
            //                  Parent_Id = userModule.Parent?.Id,
            //                  Functions = userModule.Functions,
            //              });
            //var result = userModules.AsQueryable().ToList().Select(x=>new { x.Id,x.Name,x.Remark,x.OrderCode, Parent_Id = x.Parent.Id, Functions = x.Functions});

            //List<Object> dataList = new List<object>();
            //using (IEnumerator<object> iterator = result.GetEnumerator())
            //{
            //    int count = 1;
            //    object current = iterator.Current;
            //    while (iterator.MoveNext())
            //    {
            //        count++;
            //        current = iterator.Current;
            //        dataList.Add(current);
            //    }
            //}
#if DEBUG
            stopwatch.Stop();
            System.Diagnostics.Debug.Print("登陆查询菜单用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
#endif
            if (userModules.Any())
            {

                PageResult<object> pageResult = new PageResult<object>();


                pageResult.Data = userModules.ToArray();

                pageResult.Total = userModules.Count();

                return Json(new OperationResult(OperationResultType.Success, "查询用户模块数据成功！", pageResult));
            }
            else
            {
                return Json(new OperationResult(OperationResultType.Error, "查询用户模块数据失败！"));
            }
        }



        [Description("系统管理-身份认证-修改密码")]
        public IHttpActionResult ModifyPassword(params ModifyPasswordDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            var r = Task.Run<OperationResult>(() =>
              {
                  try
                  {
                      var userDto = dtos.FirstOrDefault();
                      var user = UserManager.FindByName<User, int>(userDto?.UserName);
                      OperationResult result;
                      IdentityResult identityResult = UserManager.ChangePassword<User, int>(user.Id, userDto.Password, userDto.NewPassword);
                      if (identityResult.Succeeded)
                      {
                          result = new OperationResult(OperationResultType.Success, "密码修改成功，请牢记！");
                      }
                      else
                      {
                          result = new OperationResult(OperationResultType.Error, identityResult.Errors.ExpandAndToString(System.Environment.NewLine));
                      }
                      return result;
                  }
                  catch (Exception ex)
                  {
                      return new OperationResult(OperationResultType.Error, "修改用户密码错误！" + ex.ToString());
                  }
              });
            return Json(r.Result);
        }

    }
}
