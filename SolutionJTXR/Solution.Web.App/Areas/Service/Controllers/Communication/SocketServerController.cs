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
    [Description("实时通讯服务器管理")]
    public class SocketServerController : ServiceBaseApiController
    {
        public ISocketServerContract SocketServerContract { get; set; }

        public UserManager<User, int> userManager { get; set; }
        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }

        [Description("通讯管理-实时通讯服务器管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(SocketServerContract.SocketServerInfos, m => new
                {
                    m.Id,
                    m.ServerName,
                    m.ServerIp,
                    m.ServerPort,
                    m.MaxConnectionNumber,
                    m.Description,
                    m.CreatedTime
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询实时通讯服务器数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询实时通讯服务器数据失败！" + ex.ToString()));
            }
        }

        [Description("通讯管理-实时通讯服务器管理-添加")]
        public async Task<IHttpActionResult> Add(params SocketServerInputDto[] inpuDto)
        {
            //创建和修改的人员、时间
            inpuDto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await SocketServerContract.AddSocketServers(inpuDto);
            return Json(result);
        }

        [Description("通讯管理-实时通讯服务器管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await SocketServerContract.DeleteSocketServers(ids);
            return Json(result);
        }

        [Description("通讯管理-实时通讯服务器管理-修改")]
        public async Task<IHttpActionResult> Update(params SocketServerInputDto[] inputDto)
        {
            //修改人员、时间
            inputDto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });

            var result = await SocketServerContract.EditSocketServers(inputDto);
            return Json(result);
        }

    }
}