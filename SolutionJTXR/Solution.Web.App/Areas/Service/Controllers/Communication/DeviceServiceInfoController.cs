using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using Solution.Core.Models.Identity;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("设备通讯服务器管理")]
    public class DeviceServerInfoController : ServiceBaseApiController
    {
        public IDeviceServerInfoConract DeviceServerInfoConract { get; set; } //必须是PUBLIC

        public ICommOpcUaServerNodeMapContract CommOpcUaServerNodeMapContract { get; set; }

        public UserManager<User, int> userManager { get; set; }
        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }
        //[AjaxOnly]
        [Description("通讯管理-设备通讯服务器管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(DeviceServerInfoConract.DeviceServerInfos, m => new
                {
                    m.Id,
                    m.DeviceServerName,
                    m.DeviceUrl,
                    m.DeviceDriveAssemblyName,
                    m.DeviceDriveClassName,
                    m.IsLocked,
                    m.Remark,
                    m.CreatorUserId,
                    m.CreatedTime,
                    m.LastUpdatorUserId,
                    m.LastUpdatedTime,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "查询设备通讯服务器管理-列表数据成功！", page));
            }
            catch (Exception ex)
            {

                return Json(new OperationResult(OperationResultType.Error, "查询设备通讯服务器管理-列表数失败！" + ex.ToString()));
            }
        }

        [Description("通讯管理-设备通讯服务器管理-添加")]
        public async Task<IHttpActionResult> Add(params DeviceServerInfoInputDto[] dtos)
        {
            //创建和修改的人员、时间
            dtos?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await DeviceServerInfoConract.AddDeviceServerInfos(dtos);
            return Json(result);
        }

        [Description("通讯管理-设备通讯服务器管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {

            var result = await DeviceServerInfoConract.DeleteDeviceServerInfos(ids);
            return Json(result);
        }

        [Description("通讯管理-设备通讯服务器管理-修改")]
        public async Task<IHttpActionResult> Update(params DeviceServerInfoInputDto[] inputDto)
        {
            //修改人员、时间
            inputDto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await DeviceServerInfoConract.EditDeviceServerInfos(inputDto);
            return Json(result);
        }
    }
}
