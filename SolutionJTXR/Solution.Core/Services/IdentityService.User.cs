// -----------------------------------------------------------------------
//  <copyright file="IdentityService.User.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-03-24 17:25</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNet.Identity;
using OSharp.Core.Data.Extensions;
using OSharp.Core.Identity;
using OSharp.Core.Mapping;
using OSharp.Core.Security.Models;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using Solution.Core.Dtos.Identity;
using Solution.Core.Dtos.OAuth;
using Solution.Core.Models.Identity;
using Solution.Core.Models.Security;
using Solution.Core.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OSharp.Core.Dependency;
using Solution.Core.Contracts;
using OSharp.Core.Data;
using OSharp.Utility.Filter;
using OSharp.Utility.Logging;

namespace Solution.Core.Services
{
    public partial class IdentityService
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(IdentityService));

        #region Implementation of IIdentityContract

        /// <summary>
        /// 获取或设置 身份认证业务对象
        /// </summary>
        public IIdentityContract IdentityContract { get; set; }

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<User> Users
        {
            get { return UserRepository.Entities; }
        }

        /// <summary>
        /// 获取或设置 用户管理器
        /// </summary>
        public UserManager<User, int> UserManager { get; set; }

        /// <summary>
        /// 安全服务
        /// </summary>
        public SecurityService SecurityService { get; set; }


        /// <summary>
        /// 检查用户信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的用户信息编号</param>
        /// <returns>用户信息是否存在</returns>
        public bool CheckUserExists(Expression<Func<User, bool>> predicate, int id = 0)
        {
            return UserRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 添加用户信息信息
        /// </summary>
        /// <param name="dtos">要添加的用户信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddUsers(params UserInputDto[] dtos)
        {
            List<string> names = new List<string>();
            UserRepository.UnitOfWork.BeginTransaction();
            foreach (UserInputDto dto in dtos)
            {

                IdentityResult result;
                User user = dto.MapTo<User>();
                //密码单独处理
                if (!dto.Password.IsNullOrEmpty())
                {
                    result = await UserManager.PasswordValidator.ValidateAsync(dto.Password);
                    if (!result.Succeeded)
                    {
                        return result.ToOperationResult();
                    }
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(dto.Password);
                }
                user.Extend = new UserExtend() { RegistedIp = dto.RegistedIp };
                result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return new OperationResult(OperationResultType.Error, result.Errors.ExpandAndToString());
                }
                names.Add(user.UserName);

                User savedUser = UserManager.Users.Where(x => x.UserName.Equals(dto.UserName) &&
                                                            x.NickName.Equals(dto.NickName) &&
                                                            x.Email.Equals(dto.Email) &&
                                                            x.PhoneNumber.Equals(dto.PhoneNumber)).FirstOrDefault();
                if (savedUser != null)
                {
                    List<int> roleIds = new List<int>();
                    roleIds = dto.Roles.Select(x => x.Id).ToList();
                    var setRoleAndUserResult = await SetUserRoles(savedUser.Id, roleIds.ToArray());
                    if (setRoleAndUserResult.ResultType.Equals(OperationResultType.Error))
                    {
                        return setRoleAndUserResult;
                    }
                }
            }
            UserRepository.UnitOfWork.Commit();
            return new OperationResult(OperationResultType.Success, "用户“{0}”创建成功".FormatWith(names.ExpandAndToString()));
        }

        /// <summary>
        /// 更新用户信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的用户信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditUsers(params UserInputDto[] dtos)
        {
            List<string> names = new List<string>();
            UserRepository.UnitOfWork.BeginTransaction();
            foreach (UserInputDto dto in dtos)
            {
                IdentityResult result;
                User user = UserManager.FindById(dto.Id);
                if (user == null)
                {
                    return new OperationResult(OperationResultType.QueryNull);
                }

                user = dto.MapTo(user);
                //密码单独处理
                if (!user.PasswordHash.Equals(dto.Password) && !dto.Password.IsNullOrEmpty())
                {
                    result = await UserManager.PasswordValidator.ValidateAsync(dto.Password);
                    if (!result.Succeeded)
                    {
                        return result.ToOperationResult();
                    }
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(dto.Password);
                }
                result = await UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return new OperationResult(OperationResultType.Error, result.Errors.ExpandAndToString());
                }
                names.Add(dto.UserName);

                User savedUser = UserManager.Users.Where(x => x.UserName.Equals(dto.UserName) &&
                                                           x.NickName.Equals(dto.NickName) &&
                                                           x.Email.Equals(dto.Email) &&
                                                           x.PhoneNumber.Equals(dto.PhoneNumber)).FirstOrDefault();
                if (savedUser != null)
                {
                    List<int> roleIds = new List<int>();
                    roleIds = dto.Roles.Select(x => x.Id).ToList();
                    var setRoleAndUserResult = await SetUserRoles(savedUser.Id, roleIds.ToArray());
                    if (setRoleAndUserResult.ResultType.Equals(OperationResultType.Error))
                    {
                        return setRoleAndUserResult;
                    }
                }

            }
            UserRepository.UnitOfWork.Commit();
            return new OperationResult(OperationResultType.Success, "用户“{0}”更新成功".FormatWith(names.ExpandAndToString()));
        }

        /// <summary>
        /// 删除用户信息信息
        /// </summary>
        /// <param name="ids">要删除的用户信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteUsers(params int[] ids)
        {
            UserRepository.UnitOfWork.BeginTransaction();
            List<int> mapIds = new List<int>();
            foreach (var id in ids)
            {
                List<int> subIds = UserRoleMapRepository.TrackEntities.Where(x => x.User.Id == id).Select(x => x.Id).ToList();
                if (subIds.Any())
                {
                    mapIds.AddRange(subIds);
                }
            }
            var result = await UserRoleMapRepository.DeleteAsync(mapIds);
            if (result.ResultType == OperationResultType.Error)
            {
                return result;
            }

            foreach (var id in ids)
            {
                User user = UserManager.Users.Where(x => x.Id == id).FirstOrDefault();
                //var deleteResult = await UserManager.DeleteAsync(user);
                //if (deleteResult.ToOperationResult().ResultType == OperationResultType.Error)
                int deleteExtendCount = await UserExtendRepository.DeleteDirectAsync(user.Extend.Id);
                if (deleteExtendCount < 0)
                {
                    return new OperationResult(OperationResultType.Error, $"删除用户{user.UserName}扩展信息失败！");
                }

                int count = await UserRepository.DeleteDirectAsync(user.Id);
                if (count <= 0)
                {
                    return new OperationResult(OperationResultType.Error, $"删除用户{user.UserName}信息失败！");
                }
            }
            UserRepository.UnitOfWork.Commit();
            return new OperationResult(OperationResultType.Success, $"删除用户成功！"); ;
        }

        /// <summary>
        /// 设置用户的角色
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="roleIds">角色编号集合</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetUserRoles(int id, int[] roleIds)
        {
            User user = await UserRepository.GetByKeyAsync(id);
            if (user == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定编号的用户信息不存在");
            }
            int[] existIds = UserRoleMapRepository.Entities.Where(m => m.User.Id == id).Select(m => m.Role.Id).ToArray();
            int[] addIds = roleIds.Except(existIds).ToArray();
            int[] removeIds = existIds.Except(roleIds).ToArray();
            UserRoleMapRepository.UnitOfWork.BeginTransaction();
            int count = 0;
            foreach (int addId in addIds)
            {
                Role role = await RoleRepository.GetByKeyAsync(addId);
                if (role == null)
                {
                    return new OperationResult(OperationResultType.QueryNull, "指定编号的角色信息不存在");
                }
                UserRoleMap map = new UserRoleMap() { User = user, Role = role };
                count += await UserRoleMapRepository.InsertAsync(map);
            }
            count += await UserRoleMapRepository.DeleteAsync(m => m.User.Id == id && removeIds.Contains(m.Role.Id));
            UserRoleMapRepository.UnitOfWork.Commit();
            return count > 0
                ? new OperationResult(OperationResultType.Success, "用户“{0}”指派角色操作成功".FormatWith(user.UserName))
                : OperationResult.NoChanged;
        }

        #endregion

        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">用户登录信息</param>
        /// <param name="shouldLockout">是否启用登录锁定</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult<object>> Login(LoginInfo loginInfo, bool shouldLockout)
        {
#if DEBUG

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif

            User user = await UserManager.FindByNameAsync(loginInfo.UserName);
            if (user == null)
            {
                return new OperationResult<object>(OperationResultType.Error, "用户不存在");
            }
            if (user.IsLocked)
            {
                return new OperationResult<object>(OperationResultType.Error, "用户已被冻结，无法登录");
            }
            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return new OperationResult<object>(OperationResultType.Error,
                    $"用户因密码错误次数过多而被锁定 {UserManager.DefaultAccountLockoutTimeSpan.Minutes} 分钟，请稍后重试");
            }
            if (!await UserManager.CheckPasswordAsync(user, loginInfo.Password))
            {
                if (shouldLockout)
                {
                    await UserManager.AccessFailedAsync(user.Id);
                    if (await UserManager.IsLockedOutAsync(user.Id))
                    {
                        return new OperationResult<object>(OperationResultType.Error,
                            $"用户因密码错误次数过多而被锁定 {UserManager.DefaultAccountLockoutTimeSpan.Minutes} 分钟，请稍后重试");
                    }
                    return new OperationResult<object>(OperationResultType.Error,
                        $"用户名或密码错误，您还有 {UserManager.MaxFailedAccessAttemptsBeforeLockout - user.AccessFailedCount} 次机会");
                }
                return new OperationResult<object>(OperationResultType.Error, "用户名或密码错误");
            }
            PageResult<object> pageResult = new PageResult<object>();
#if DEBUG

            var userModules = GetUserRoleModules(user);
            //PageResult<object> pageResult = new PageResult<object>();
            pageResult.Data = userModules.ToArray();
            pageResult.Total = userModules.Count();

#else
            var userModules = GetUserRoleModules(user);
            pageResult.Data = userModules.ToArray();
            pageResult.Total = userModules.Count();
#endif

#if DEBUG
            stopwatch.Stop();
            System.Diagnostics.Debug.Print("登陆查询菜单用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
#endif

            return new OperationResult<object>(OperationResultType.Success, "用户登录成功", pageResult);
        }


        /// <summary>
        /// 获取用户角色的模块(用户-角色-模块)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IQueryable<Object> GetUserRoleModules(User user)
        {
            try
            {
                List<Object> result = new List<Object>();
                var userRoles = this.UserRoleMapRepository.Entities.Where(a => a.User.Id == user.Id).Unlocked().Unexpired().Select(a => a.Role).ToList().Distinct();
                foreach (var role in userRoles)
                {
                    var modules = this.SecurityService.SecurityManager.Modules.Where(a => a.Roles.Select(r => r.Id).Contains(role.Id)).
                       Select(x => new
                       {
                           x.Id,
                           x.Name,
                           x.Remark,
                           x.OrderCode,
                           Parent_Id = x.Parent.Id.ToString(),
                           Functions = x.Functions
                       });
                    if (!Equals(modules, null) && modules.Count() > 0)
                    {
                        result.AddRange(modules);
                    }
                }
                return result.AsQueryable();
            }
            catch (Exception ex)
            {
                Logger.Error("GetUserRoleModules错误！", ex);
                return null;
            }

        }

    }

}