using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Dtos;
using Solution.Core.Models.Identity;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("OpcUa服务器与通讯点关联管理")]
    public class CommOpcUaServerNodeMapController : ServiceBaseApiController
    {
        public ICommOpcUaServerNodeMapContract CommOpcUaServerNodeMapContract { get; set; }

        public UserManager<User, int> userManager { get; set; }
        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }


        [Description("通讯管理-OpcUa服务器与通讯点关联管理-分页数据")]
        public IHttpActionResult GridData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(CommOpcUaServerNodeMapContract.CommOpcUaServerNodeMapInfos, m => new
                {
                    m.Id,
                    ServerId = m.OpcUaServer.Id,
                    m.OpcUaServer.ServerName,
                    m.OpcUaNode.NodeName,
                    m.OpcUaNode.NodeUrl,
                    m.OpcUaNode.Interval,
                    m.OpcUaNode.Description,
                    m.OpcUaNode.CreatedTime,
                    m.OpcUaNode.CreatorUserId,
                    m.OpcUaNode.LastUpdatedTime,
                    m.OpcUaNode.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询OpcUa服务器与通讯点关联数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询OpcUa服务器与通讯点关联数据失败！" + ex.ToString()));
            }
        }

        [Description("通讯管理-OpcUa服务器与通讯点关联管理-添加")]
        public async Task<IHttpActionResult> Add(params CommOpcUaServerNodeMapInputDto[] inpuDto)
        {
            var result = await CommOpcUaServerNodeMapContract.AddCommOpcUaServerNodeMaps(inpuDto);
            return Json(result);
        }

        [Description("通讯管理-OpcUa服务器与通讯点关联管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await CommOpcUaServerNodeMapContract.DeleteCommOpcUaServerNodeMaps(ids);
            return Ok(result);
        }

        [Description("通讯管理-OpcUa服务器与通讯点关联管理-修改")]
        public async Task<IHttpActionResult> Update(params CommOpcUaServerNodeMapInputDto[] inputDto)
        {
            var result = await CommOpcUaServerNodeMapContract.EditCommOpcUaServerNodeMaps(inputDto);
            return Ok(result);
        }
    }
}