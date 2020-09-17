using OSharp.Utility.Data;
using Solution.StereoscopicWarehouseManagement.Contracts;
using Solution.StereoscopicWarehouseManagement.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 仓位图信息API控制器
    /// </summary>

    [Description("服务-仓位图查看")]
    public class MatWareHousAreaLocationInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 仓位图信息契约
        /// </summary>
        public IMatWareHousAreaLocationInfoContract MatWareHousAreaLocationInfoContract { get; set; }
        

        [Description("服务-仓位图查看-数据")]
        public IHttpActionResult GetPageData1(string id)
        {
            try
            {
                Guid guid = new Guid();
                if (!string.IsNullOrEmpty(id))
                {
                    guid = Guid.Parse(id);
                }
                List<MatWareHousAreaLocationInfoOutputDto> matwarehousarealocationInfo = MatWareHousAreaLocationInfoContract.Ini1(guid);
                return Json(new OperationResult(OperationResultType.Success, "读取仓位图信息列表数据成功！", matwarehousarealocationInfo));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取仓位图信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-仓位图查看-数据")]
        public IHttpActionResult GetPageData2(string id)
        {
            try
            {
                List<MatWareHousAreaLocationItemInfoOutputDto> matwarehousarealocationitemInfo = MatWareHousAreaLocationInfoContract.Ini2(id);
                return Json(new OperationResult(OperationResultType.Success, "读取仓位图信息列表数据成功！", matwarehousarealocationitemInfo));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取仓位图信息列表数据失败！" + ex.ToString()));
            }
        }
    }
}
