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
    [Description("系统管理-客户端密钥管理")]
    public class OAuthClientSecretManagerController : ServiceBaseApiController
    {
        /// <summary>
        /// 获取或设置 客户端密钥仓储对象
        /// </summary>
        public IRepository<OAuthClientSecret, int> ClientSecretRepository { get; set; }

        [Description("系统管理-客户端密钥管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ClientSecretRepository.Entities, m => new
                {
                    m.Id,
                    m.Value,
                    m.Type,
                    m.Remark,
                    m.IsLocked,
                    m.BeginTime,
                    m.EndTime,
                    Client_Id = m.Client.Id,
                    Client_Name = m.Client.Name
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询客户端密钥数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询客户端密钥数据失败！" + ex.ToString()));
            }
        }
    }
}