using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Dtos;
using Solution.Core.Models.Identity;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("历史数据管理")]
    public class HistoryDataController : ServiceBaseApiController
    {
        public IHistoryDataContract HistoryDataContract { get; set; }
        public UserManager<User, int> userManager { get; set; }
        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }


        [Description("通讯管理-历史数据管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            GridRequest request = new GridRequest(requestParams);
            try
            {
                var page = GetPageResult(HistoryDataContract.HistoryDatas, m => new
                {
                    m.Id,
                    m.DeviceServer_Id,
                    m.DeviceServerName,
                    m.DeviceNode_Id,
                    m.NodeName,
                    m.DataValue,
                    m.Quality,
                    m.CreatedTime
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询通讯历史数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询通讯历史数据失败！" + ex.ToString()));
            }
        }

        [Description("通讯管理-历史数据管理-添加")]
        public IHttpActionResult Add(params HistoryDataInputDto[] inpuDto)
        {
            var result = HistoryDataContract.Add(inpuDto);
            return Json(result);
        }

        [Description("通讯管理-历史数据管理-删除")]
        public async Task<IHttpActionResult> Remove(params long[] ids)
        {
            var result = await HistoryDataContract.Delete(ids);
            return Json(result);
        }

        [Description("通讯管理-历史数据管理-修改")]
        public async Task<IHttpActionResult> Update(params HistoryDataInputDto[] inputDto)
        {
            var result = await HistoryDataContract.Edit(inputDto);
            return Json(result);
        }

        //[Description("通讯管理-通讯历史数据管理-逻辑删除")]

        //public async Task<IHttpActionResult> Recycle(params CommOpcUaHistory[] enterinfo)
        //{
        //    var result = await CommOpcUaHistoryContract.RecycleCommOpcUaHistory(enterinfo);
        //    return Json(result);
        //}

        //[Description("通讯管理-通讯历史数据管理-逻辑还原")]

        //public async Task<IHttpActionResult> Restore(params CommOpcUaHistory[] enterinfo)
        //{
        //    var result = await CommOpcUaHistoryContract.RestoreCommOpcUaHistory(enterinfo);
        //    return Json(result);
        //}
    }
}