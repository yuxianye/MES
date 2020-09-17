using OSharp.Utility.Data;
using Solution.Core.Contracts;
using System;
using System.ComponentModel;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("系统管理-操作日志管理")]
    public class OperateLogManagerController : ServiceBaseApiController
    {
        public ILoggingContract LoggingContract { get; set; }

        [Description("系统管理-操作日志管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(LoggingContract.OperateLogs, m => new
                {
                    m.Id,
                    m.FunctionName,
                    m.Operator.UserId,
                    m.Operator.Name,
                    m.Operator.NickName,
                    m.Operator.Ip,
                    m.CreatedTime
                }, request);

                return Json(new OperationResult(OperationResultType.Success, "查询操作日志数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询操作日志数据失败！" + ex.ToString()));
            }
        }
    }
}