using OSharp.Utility.Data;
using Solution.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("系统管理-实体管理")]
    public class EntityInfoManagerController : ServiceBaseApiController
    {
        /// <summary>
        /// 获取或设置 安全业务对象
        /// </summary>
        public ISecurityContract SecurityContract { get; set; }

        [Description("系统管理-实体管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(SecurityContract.EntityInfos, m => new
                {
                    m.Id,
                    m.Name,
                    m.ClassName,
                    m.DataLogEnabled
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询实体数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询实体数据失败！" + ex.ToString()));
            }
        }
    }
}