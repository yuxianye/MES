// -----------------------------------------------------------------------
//  <copyright file="ClientRefreshTokenStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-11-10 5:02</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OSharp.Core.Security;
using OSharp.Utility.Logging;
using Solution.Core.Identity;
using Solution.Core.Models.Identity;
using Solution.Core.Models.OAuth;


namespace Solution.Core.OAuth
{
    /// <summary>
    /// 客户端刷新Token存储
    /// </summary>
    public class OAuthClientRefreshTokenStore : OAuthClientRefreshTokenStoreBase<OAuthClientRefreshToken, Guid, OAuthClient, int, User, int>
    {
        private readonly ILogger Logger = LogManager.GetLogger(typeof(OAuthClientRefreshTokenStore));

        /// <summary>
        /// 获取或设置 用户管理器
        /// </summary>
        public UserManager UserManager { get; set; }
        private UserStore userStore = new UserStore();
        public override async Task<bool> SaveToken(RefreshTokenInfo tokenInfo)
        {
            int result = 0;
            try
            {
                OAuthClientRefreshToken token = new OAuthClientRefreshToken()
                {
                    Value = tokenInfo.Value,
                    ProtectedTicket = tokenInfo.ProtectedTicket,
                    IssuedUtc = tokenInfo.IssuedUtc,
                    ExpiresUtc = tokenInfo.ExpiresUtc
                };
                try
                {
                    ClientRepository.UnitOfWork.BeginTransaction();
                    var client = ClientRepository.TrackEntities.Where(m => m.ClientId == tokenInfo.ClientId).FirstOrDefault();
                    ClientRepository.UnitOfWork.Commit();
                    if (client == null)
                    {
                        return false;
                    }
                    token.Client = client;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                UserRepository.UnitOfWork.BeginTransaction();
                User user = UserRepository.TrackEntities.Where(m => m.UserName == tokenInfo.UserName).FirstOrDefault();
                UserRepository.UnitOfWork.Commit();
                //User user = await UserManager.FindByNameAsync(tokenInfo.UserName);
                //User user = await userStore.FindByNameAsync(tokenInfo.UserName);
                if (user == null)
                {
                    return false;
                }
                token.User = user;
                ClientRefreshTokenRepository.UnitOfWork.BeginTransaction();
                result = await ClientRefreshTokenRepository.InsertAsync(token);
                ClientRefreshTokenRepository.UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error("OAuthClientRefreshTokenStore.SaveToken错误：" + ex.ToString());
            }


            return result > 0;
        }

        /// <summary>
        /// 获取刷新Token
        /// </summary>
        /// <param name="value">token值</param>
        /// <returns></returns>
        public override Task<RefreshTokenInfo> GetTokenInfo(string value)
        {
            RefreshTokenInfo tokenInfo = new RefreshTokenInfo();
            try
            {
                tokenInfo = ClientRefreshTokenRepository.Entities.Where(m => m.Value == value).Select(m => new RefreshTokenInfo()
                {
                    Value = m.Value,
                    IssuedUtc = m.IssuedUtc,
                    ExpiresUtc = m.ExpiresUtc,
                    ProtectedTicket = m.ProtectedTicket,
                    ClientId = m.Client.ClientId,
                    UserName = m.User.UserName
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Error("OAuthClientRefreshTokenStore.GetTokenInfo错误：" + ex.ToString());
            }

            return Task.FromResult(tokenInfo);
        }

        /// <summary>
        /// 移除刷新Token
        /// </summary>
        /// <param name="value">Token值</param>
        /// <returns></returns>
        public async override Task<bool> Remove(string value)
        {
            int result = 0;
            try
            {
                var token = ClientRefreshTokenRepository.Entities.Where(m => m.Value == value)
               .Select(m => new { UserId = m.User.Id }).FirstOrDefault();
                if (token == null)
                {
                    return false;
                }
                int userId = token.UserId;
                ClientRefreshTokenRepository.UnitOfWork.BeginTransaction();
                result = await ClientRefreshTokenRepository.DeleteDirectAsync(m => m.User.Id.Equals(userId));
                ClientRefreshTokenRepository.UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error("OAuthClientRefreshTokenStore.Remove错误：" + ex.ToString());
            }

            return result > 0;
        }

    }
}