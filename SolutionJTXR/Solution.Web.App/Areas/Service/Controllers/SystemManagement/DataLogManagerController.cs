using OSharp.Utility.Data;
using Solution.Core.Contracts;
using System;
using System.ComponentModel;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("系统管理-数据日志管理")]
    public class DataLogManagerController : ServiceBaseApiController
    {
        public ILoggingContract LoggingContract { get; set; }

        [Description("系统管理-数据日志管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(LoggingContract.DataLogs, m => new
                {
                    m.Id,
                    m.EntityName,
                    m.Name,
                    m.EntityKey,
                    m.OperateType,
                    OperateLog_Id = m.OperateLog.Id
                }, request);

                return Json(new OperationResult(OperationResultType.Success, "查询数据日志数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询数据日志数据失败！" + ex.ToString()));
            }
        }
    }
}