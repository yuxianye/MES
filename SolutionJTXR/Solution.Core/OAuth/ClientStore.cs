// -----------------------------------------------------------------------
//  <copyright file="ClientStore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-11-05 19:04</last-date>
// -----------------------------------------------------------------------

using OSharp.Core.Data;
using OSharp.Core.Security;
using OSharp.Utility.Data;
using OSharp.Utility.Logging;
using Solution.Core.Dtos.OAuth;
using Solution.Core.Models.OAuth;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Core.OAuth
{
    /// <summary>
    /// 客户端存储实现
    /// </summary>
    public class OAuthClientStore : OAuthClientStoreBase<OAuthClient, int, OAuthClientSecret, int, OAuthClientInputDto, OAuthClientSecretInputDto>
    {
        /// <summary>
        /// 获取或设置 客户端仓储对象
        /// </summary>
        public new IRepository<OAuthClient, int> ClientRepository { get; set; }
        private readonly ILogger Logger = LogManager.GetLogger(typeof(OAuthClientStore));
        /// <summary>
        /// 获取或设置 客户端密钥仓储对象
        /// </summary>
        //public new IRepository<OAuthClientSecret, int> ClientSecretRepository { get; set; }

        public OAuthClient GetOAuthClient(OAuthClientInputDto dto)
        {
            OAuthClient oAuthClient = new OAuthClient();
            try
            {
                oAuthClient = ClientRepository.Entities.Where(x => x.Name.Equals(dto.Name)).OrderByDescending(x => x.CreatedTime).First();
            }
            catch (Exception ex)
            {
                Logger.Error("OAuthClientStore.GetOAuthClient错误：" + ex.ToString());
            }
            return oAuthClient;
        }
    }
}