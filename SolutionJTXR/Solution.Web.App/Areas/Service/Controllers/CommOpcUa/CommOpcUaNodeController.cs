using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using OSharp.Utility.Filter;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using Solution.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    //[Description("通讯点表管理")]
    //public class CommOpcUaNodeController : ServiceBaseApiController
    //{
    //    public IDeviceNodeContract DeviceNodeContract { get; set; }
    //    public ICommOpcUaServerNodeMapContract CommOpcUaServerNodeContract { get; set; }
    //    public ICommOpcUaServerConract CommOpcUaServerConract { get; set; }
    //    public UserManager<User, int> userManager { get; set; }
    //    public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }

    //    public IRepository<CommOpcUaServerNodeMap, Guid> CommOpcUaServerNodeMapRepository { get; set; }
    //    public IRepository<CommOpcUaServer, Guid> CommOpcUaServerRepository { get; set; }

    //    public IRepository<DeviceNode, Guid> CommOpcUaNodeRepository { get; set; }

    //    [Description("通讯管理-通讯点表管理-分页数据")]
    //    public IHttpActionResult PageData(PageRepuestParams requestParams)
    //    {
    //        GridRequest request = new GridRequest(requestParams);
    //        PageRepuestParams localRequestParams = new PageRepuestParams();
    //        localRequestParams.PageIndex = requestParams.PageIndex;
    //        localRequestParams.PageSize = requestParams.PageSize;
    //        localRequestParams.SortField = requestParams.SortField;
    //        localRequestParams.SortOrder = requestParams.SortOrder;
    //        var sourceData = DeviceNodeContract.DeviceNodes;
    //        if (requestParams.FilterGroup != null && requestParams.FilterGroup.Rules.Any())
    //        {
    //            var data = requestParams.FilterGroup.Rules.ToList()[0].Value;

    //            sourceData = DeviceNodeContract.DeviceNodes.Where(
    //                            x =>
    //                            CommOpcUaServerNodeContract.CommOpcUaServerNodeMapInfos.Where(m => m.OpcUaServer.ServerName.Contains(data.ToString())).Select(a => a.OpcUaNode.Id).Contains(x.Id)
    //                            ||
    //                            x.NodeName.Contains(data.ToString()));

    //            request = new GridRequest(localRequestParams);
    //        }

    //        try
    //        {
    //            var page = GetPageResult(sourceData, data => new
    //            {
    //                data.Id,
    //                ServerId = CommOpcUaServerNodeContract.CommOpcUaServerNodeMapInfos.Where(x => x.OpcUaNode.Id.Equals(data.Id)).FirstOrDefault().OpcUaServer.Id,
    //                CommOpcUaServerNodeContract.CommOpcUaServerNodeMapInfos.Where(x => x.OpcUaNode.Id.Equals(data.Id)).FirstOrDefault().OpcUaServer.ServerName,
    //                data.NodeName,
    //                data.NodeUrl,
    //                data.Interval,
    //                data.Description,
    //                data.CreatedTime,
    //                data.CreatorUserId,
    //                data.LastUpdatedTime,
    //                data.LastUpdatorUserId,
    //            }, request);
    //            return Json(new OperationResult(OperationResultType.Success, "查询通讯点表数据成功！", page));
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new OperationResult(OperationResultType.Error, $"查询通讯点表数据失败！{ex.ToString() }"));
    //        }
    //    }

    //    [Description("通讯管理-通讯点表管理-添加")]
    //    public async Task<IHttpActionResult> Add(params DeviceNodeInputDto[] commOpcUaNodeInputDto)
    //    {
    //        //创建和修改的人员、时间
    //        commOpcUaNodeInputDto?.ToList().ForEach((a) =>
    //        {
    //            a.Id = CombHelper.NewComb();
    //            a.CreatorUserId = User.Identity.Name;
    //            a.CreatedTime = DateTime.Now;
    //            a.LastUpdatedTime = a.CreatedTime;
    //            a.LastUpdatorUserId = a.CreatorUserId;
    //        });

    //        var result = await DeviceNodeContract.AddDeviceNodes(commOpcUaNodeInputDto);

    //        return Json(result);
    //    }

    //    [Description("通讯管理-通讯点表管理-删除")]
    //    public async Task<IHttpActionResult> Remove(params Guid[] ids)
    //    {
    //        var result = await DeviceNodeContract.DeleteDeviceNodes(ids);
    //        return Json(result);
    //    }


    //    [Description("通讯管理-通讯点表管理-修改")]
    //    public async Task<IHttpActionResult> Update(params DeviceNodeInputDto[] commOpcUaNodeInputDto)
    //    {
    //        //修改人员、时间
    //        commOpcUaNodeInputDto?.ToList().ForEach((a) =>
    //        {
    //            a.LastUpdatedTime = DateTime.Now;
    //            a.LastUpdatorUserId = User.Identity.Name;
    //        });
    //        var result = await DeviceNodeContract.EditDeviceNodes(commOpcUaNodeInputDto);
    //        return Json(result);
    //    }
    //}
}