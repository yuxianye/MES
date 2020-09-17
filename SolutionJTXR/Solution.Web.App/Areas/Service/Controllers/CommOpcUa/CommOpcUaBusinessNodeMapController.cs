using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Utility.Data;
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
    [Description("通讯业务点表与通讯点表关联管理")]
    public class CommOpcUaBusinessNodeMapController : ServiceBaseApiController
    {

        public ICommOpcUaBusinessNodeMapContract CommOpcUaBusinessNodeMapContract { get; set; }

        public UserManager<User, int> userManager { get; set; }
        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }
        public IRepository<CommOpcUaBusinessNodeMap, Guid> CommOpcUaBusinessNodeMapRepository { get; set; }

        [Description("通讯管理-通讯业务点表与通讯点表关联管理-分页数据")]
        public IHttpActionResult GridData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(CommOpcUaBusinessNodeMapContract.CommOpcUaBusinessNodeMapInfos, m => new
                {
                    m.Id,
                    m.OpcUaBusiness.BusinessName,
                    OpcUaBusiness_Id = m.OpcUaBusiness.Id,
                    m.OpcUaNode.NodeName,
                    OpcUaNode_Id = m.OpcUaNode.Id,
                    m.CreatedTime
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取通讯业务点表与通讯点表关联数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取通讯业务点表与通讯点表关联数据失败！" + ex.ToString()));
            }
        }


        [Description("通讯管理-通讯业务点表与通讯点表关联管理-添加")]
        public async Task<IHttpActionResult> Add(CommOpcUaBusinessNodeMapInputDto inpuDto)
        {
            var result = await CommOpcUaBusinessNodeMapContract.AddCommOpcUaBusinessNodeMaps(inpuDto);
            return Json(result);
        }

        [Description("通讯管理-通讯业务点表与通讯点表关联管理-删除")]
        public async Task<IHttpActionResult> Remove(String idStr)
        {
            Guid id = Guid.Parse(idStr);
            var result = await CommOpcUaBusinessNodeMapContract.DeleteCommOpcUaBusinessNodeMaps(id);
            return Json(result);
        }

        [Description("通讯管理-通讯业务点表与通讯点表关联管理-修改")]
        public async Task<IHttpActionResult> Update(CommOpcUaBusinessNodeMapInputDto inputDto)
        {
            var result = await CommOpcUaBusinessNodeMapContract.EditCommOpcUaBusinessNodeMaps(inputDto);
            return Json(result);
        }

    }
}