using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.Core.Models.OAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("系统管理-客户端令牌管理")]
    public class OAuthClientRefreshTokenManagerController : ServiceBaseApiController
    {
        public IRepository<OAuthClientRefreshToken, Guid> ClientRefreshTokenRepository { get; set; }

        [Description("系统管理-客户端令牌管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ClientRefreshTokenRepository.Entities, m => new
                {
                    m.Id,
                    m.Value,
                    m.ProtectedTicket,
                    m.IssuedUtc,
                    m.ExpiresUtc,
                    Client_Id = m.Client.Id.ToString(),
                    Client_Name = m.Client.Name,
                    User_Id = m.User.Id.ToString(),
                    User_Name = m.User.UserName
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询客户端Token数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询客户端Token数据失败！" + ex.ToString()));
            }
        }

    }
}