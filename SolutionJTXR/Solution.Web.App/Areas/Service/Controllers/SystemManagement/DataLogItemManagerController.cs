using OSharp.Utility.Data;
using Solution.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("系统管理-操作日志明细管理")]
    public class DataLogItemManagerController : ServiceBaseApiController
    {
        public ILoggingContract LoggingContract { get; set; }

        [Description("系统管理-操作日志明细管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(LoggingContract.DataLogItems, m => new
                {
                    m.Id,
                    m.Field,
                    m.FieldName,
                    m.OriginalValue,
                    m.NewValue,
                    m.DataType,
                    m.DataLog.EntityName,
                    m.DataLog.Name,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询操作日志明细数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询操作日志明细数据失败！" + ex.ToString()));
            }
        }
    }
}