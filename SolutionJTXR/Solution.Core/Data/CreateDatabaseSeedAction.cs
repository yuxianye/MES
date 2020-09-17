using System;
using System.Data.Entity;

using OSharp.Data.Entity.Migrations;
using Solution.Core.Models.Identity;


namespace Solution.Core.Data
{
    public class CreateDatabaseSeedAction : ISeedAction
    {
        #region Implementation of ISeedAction

        /// <summary>
        /// 获取 操作排序，数值越小越先执行
        /// </summary>
        public int Order { get { return 1; } }

        /// <summary>
        /// 定义种子数据初始化过程
        /// </summary>
        /// <param name="context">数据上下文</param>
        public void Action(DbContext context)
        {
            //Delete By yuxianye 2018.04.26
            //在其他地方统一添加管理员角色和用户
            //context.Set<Role>().Add(new Role() { Name = "系统管理员", Remark = "系统管理员角色，拥有系统最高权限", IsAdmin = true, IsSystem = true, CreatedTime = DateTime.Now });

            //Add By yuxianye 2018.04.26 
            //增加初始化数据 角色：管理员、人资、库管，用户：admin、hr1、warehouse1,默认密码yuxianye1
            //管理员角色和用户
            context.Set<UserRoleMap>().Add(new UserRoleMap()
            {
                Id = 1,
                IsLocked = false,
                CreatedTime = DateTime.Now,
                BeginTime = DateTime.Now,
                EndTime = DateTime.MaxValue,
                Role = new Role() { Name = "系统管理员", Remark = "系统管理员角色，拥有系统最高权限", IsAdmin = true, IsSystem = true, CreatedTime = DateTime.Now },
                User = context.Set<User>().Add(new User()
                {
                    Id = 1,
                    AccessFailedCount = 0,
                    LockoutEndDateUtc = null,
                    TwoFactorEnabled = false,
                    SecurityStmp = "",
                    Extend = new UserExtend() { Id = 1, RegistedIp = "127.0.0.1" },
                    UserName = "admin",
                    NickName = "系统管理员",
                    Email = "yuxianye@163.com",
                    EmailConfirmed = true,
                    PhoneNumber = "15040076798",
                    PhoneNumberConfirmed = true,
                    IsLocked = false,
                    LockoutEnabled = false,
                    PasswordHash = "AG0fOEPzuHvgaVEA0IYnly+IZ/GPmAIigo4xwWhYlQb0qiketRSVfAm8Xn5bclySsQ==",
                    CreatedTime = DateTime.Now
                }),

            });

            //人资角色和用户
            context.Set<UserRoleMap>().Add(new UserRoleMap()
            {
                Id = 2,
                IsLocked = false,
                CreatedTime = DateTime.Now,
                BeginTime = DateTime.Now,
                EndTime = DateTime.MaxValue,
                Role = new Role() { Name = "人资", Remark = "人资角色", IsAdmin = false, IsSystem = true, CreatedTime = DateTime.Now },
                User = context.Set<User>().Add(new User()
                {
                    Id = 2,
                    AccessFailedCount = 0,
                    LockoutEndDateUtc = null,
                    TwoFactorEnabled = false,
                    SecurityStmp = "",
                    Extend = new UserExtend() { Id = 2, RegistedIp = "127.0.0.1" },
                    UserName = "hr1",
                    NickName = "人资1",
                    Email = "yuxianye@qq.com",
                    EmailConfirmed = true,
                    PhoneNumber = "15040076798",
                    PhoneNumberConfirmed = true,
                    IsLocked = false,
                    LockoutEnabled = false,
                    PasswordHash = "AG0fOEPzuHvgaVEA0IYnly+IZ/GPmAIigo4xwWhYlQb0qiketRSVfAm8Xn5bclySsQ==",
                    CreatedTime = DateTime.Now
                }),

            });

            //库管角色和用户
            context.Set<UserRoleMap>().Add(new UserRoleMap()
            {
                Id = 3,
                IsLocked = false,
                CreatedTime = DateTime.Now,
                BeginTime = DateTime.Now,
                EndTime = DateTime.MaxValue,
                Role = new Role() { Name = "库管", Remark = "库管角色", IsAdmin = false, IsSystem = true, CreatedTime = DateTime.Now },
                User = context.Set<User>().Add(new User()
                {
                    Id = 3,
                    AccessFailedCount = 0,
                    LockoutEndDateUtc = null,
                    TwoFactorEnabled = false,
                    SecurityStmp = "",
                    Extend = new UserExtend() { Id = 3, RegistedIp = "127.0.0.1" },
                    UserName = "warehouse1",
                    NickName = "库管1",
                    Email = "yuxianye@qq.com",
                    EmailConfirmed = true,
                    PhoneNumber = "15040076798",
                    PhoneNumberConfirmed = true,
                    IsLocked = false,
                    LockoutEnabled = false,
                    PasswordHash = "AG0fOEPzuHvgaVEA0IYnly+IZ/GPmAIigo4xwWhYlQb0qiketRSVfAm8Xn5bclySsQ==",
                    CreatedTime = DateTime.Now
                }),

            });

        }

        #endregion
    }
}
