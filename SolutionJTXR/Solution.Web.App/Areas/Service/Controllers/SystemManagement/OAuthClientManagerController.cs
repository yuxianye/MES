using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.Core.Models.OAuth;
using Solution.Core.OAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("系统管理-客户端管理")]
    public class OAuthClientManagerController : ServiceBaseApiController
    {

        public IRepository<OAuthClient, int> ClientRepository { private get; set; }

        [Description("系统管理-客户端管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ClientRepository.Entities, m => new
                {
                    m.Id,
                    m.Name,
                    m.OAuthClientType,
                    m.ClientId,
                    m.Url,
                    m.LogoUrl,
                    m.RedirectUrl,
                    m.RequireConsent,
                    m.AllowRememberConsent,
                    m.AllowClientCredentialsOnly,
                    m.IsLocked,
                    m.CreatedTime
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询客户端数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询客户端数据失败！" + ex.ToString()));
            }
        }
    }
}