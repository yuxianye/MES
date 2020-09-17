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
    [Description("设备点表管理")]
    public class DeviceNodeController : ServiceBaseApiController
    {
        public IDeviceNodeContract DeviceNodeContract { get; set; }
        public UserManager<User, int> userManager { get; set; }
        public IRepository<DeviceNode, Guid> DeviceNodeRepository { get; set; }
        public IRepository<DeviceServerInfo, Guid> DeviceServerInfoRepository { get; set; }


        [Description("通讯管理-设备点表管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {


                GridRequest request = new GridRequest(requestParams);
                var querySource = DeviceNodeRepository.Entities;
                var deviceServerNameFilter = request.FilterGroup.Rules.FirstOrDefault(a => a.Field == "DeviceServerInfoName");
                if (requestParams.FilterGroup != null && requestParams.FilterGroup.Rules.Any() && deviceServerNameFilter != null)
                {
                    querySource = DeviceNodeRepository.Entities.Where(a => a.DeviceServerInfo.DeviceServerName.Contains(deviceServerNameFilter.Value.ToString())
                     || a.NodeName.Contains(deviceServerNameFilter.Value.ToString())
                     || a.NodeUrl.Contains(deviceServerNameFilter.Value.ToString()));
                    request.FilterGroup.Rules.Clear();
                }
                var page = GetPageResult(querySource, data => new
                {
                    data.Id,
                    DeviceServerInfoId = data.DeviceServerInfo.Id,
                    DeviceServerInfoName = data.DeviceServerInfo.DeviceServerName,
                    data.NodeName,
                    data.NodeUrl,
                    data.Interval,
                    data.DataType,
                    data.Description,
                    data.CreatedTime,
                    data.CreatorUserId,
                    data.LastUpdatedTime,
                    data.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询设备点表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, $"查询设备点表数据失败！{ex.ToString() }"));
            }
        }

        [Description("通讯管理-设备点表管理-添加")]
        public async Task<IHttpActionResult> Add(params DeviceNodeInputDto[] deviceNodeInputDto)
        {
            //创建和修改的人员、时间
            deviceNodeInputDto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });

            var result = await DeviceNodeContract.AddDeviceNodes(deviceNodeInputDto);

            return Json(result);
        }

        [Description("通讯管理-设备点表管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await DeviceNodeContract.DeleteDeviceNodes(ids);
            return Json(result);
        }


        [Description("通讯管理-设备点表管理-修改")]
        public async Task<IHttpActionResult> Update(params DeviceNodeInputDto[] deviceNodeInputDto)
        {
            //修改人员、时间
            deviceNodeInputDto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await DeviceNodeContract.EditDeviceNodes(deviceNodeInputDto);
            return Json(result);
        }
    }
}