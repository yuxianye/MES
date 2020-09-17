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
    [Description("OpcUa服务器管理")]
    public class CommOpcUaServerController : ServiceBaseApiController
    {
        public ICommOpcUaServerConract CommOpcUaServerConract { get; set; } //必须是PUBLIC

        public ICommOpcUaServerNodeMapContract CommOpcUaServerNodeMapContract { get; set; }

        public UserManager<User, int> userManager { get; set; }
        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }
        //[AjaxOnly]
        [Description("通讯管理-OpcUa服务器管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(CommOpcUaServerConract.CommOpcUaServerInfos, m => new
                {
                    m.Id,
                    m.ServerName,
                    m.Url,
                    m.Description,
                    m.CreatedTime
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询OPC UA 服务器-列表数据成功！", page));
            }
            catch (Exception ex)
            {

                return Json(new OperationResult(OperationResultType.Error, "查询OpcUa服务器管理-列表数失败！" + ex.ToString()));
            }
        }

        [Description("通讯管理-OpcUa服务器管理-添加")]
        public async Task<IHttpActionResult> Add(params CommOpcUaServerInputDto[] commOpcUaServer)
        {
            //创建和修改的人员、时间
            commOpcUaServer?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await CommOpcUaServerConract.AddCommOpcUaServers(commOpcUaServer);
            return Json(result);
        }

        [Description("通讯管理-OpcUa服务器管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {

            var result = await CommOpcUaServerConract.DeleteCommOpcUaServers(ids);
            return Json(result);
        }

        [Description("通讯管理-OpcUa服务器管理-修改")]
        public async Task<IHttpActionResult> Update(params CommOpcUaServerInputDto[] inputDto)
        {
            //修改人员、时间
            inputDto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await CommOpcUaServerConract.EditCommOpcUaServers(inputDto);
            return Json(result);
        }
    }
}
