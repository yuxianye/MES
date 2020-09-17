using OSharp.Utility;
using OSharp.Utility.Data;
using Solution.Core.Contracts;
using Solution.Core.Dtos.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("系统管理-功能管理")]
    public class FunctionManagerController : ServiceBaseApiController
    {
        /// <summary>
        /// 获取或设置 安全业务对象
        /// </summary>
        public ISecurityContract SecurityContract { get; set; }

        [Description("系统管理-功能管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(SecurityContract.Functions, m => new
                {
                    m.Id,
                    m.Name,
                    m.Url,
                    m.FunctionType,
                    m.OperateLogEnabled,
                    m.DataLogEnabled,
                    m.CacheExpirationSeconds,
                    m.IsCacheSliding,
                    m.Area,
                    m.Controller,
                    m.Action,
                    m.IsController,
                    m.IsAjax,
                    m.IsChild,
                    m.IsLocked,
                    m.IsTypeChanged,
                    m.IsCustom
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询功能数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询功能数据失败！" + ex.ToString()));
            }
        }

        //[Description("系统管理-功能管理-添加")]
        //public IHttpActionResult Add(FunctionInputDto[] dtos)
        //{
        //    dtos.CheckNotNull("dtos");
        //    OperationResult result = SecurityContract.AddFunctions(dtos);
        //    return Json(result);
        //}

        //[Description("系统管理-功能管理-编辑")]
        //public async Task<IHttpActionResult> Edit(FunctionInputDto[] dtos)
        //{
        //    dtos.CheckNotNull("dtos");
        //    OperationResult result = await SecurityContract.EditFunctions(dtos);
        //    return Json(result);
        //}

        //[Description("系统管理-功能管理-删除")]
        //public IHttpActionResult Delete(Guid[] ids)
        //{
        //    ids.CheckNotNull("ids");
        //    OperationResult result = SecurityContract.DeleteFunctions(ids);
        //    return Json(result);
        //}

    }
}