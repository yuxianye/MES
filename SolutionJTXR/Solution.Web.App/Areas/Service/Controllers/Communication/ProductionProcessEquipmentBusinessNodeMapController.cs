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
    [Description("工序设备业务点表关联")]
    public class ProductionProcessEquipmentBusinessNodeMapController : ServiceBaseApiController
    {
        public IProductionProcessEquipmentBusinessNodeMapContract ProductionProcessEquipmentBusinessNodeMapContract { get; set; }

        public UserManager<User, int> userManager { get; set; }
        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }
        public IRepository<ProductionProcessEquipmentBusinessNodeMap, Guid> ProductionProcessEquipmentBusinessNodeMapRepository { get; set; }

        [Description("通讯管理-通讯业务点表与通讯点表关联管理-分页数据")]
        public IHttpActionResult GridData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ProductionProcessEquipmentBusinessNodeMapContract.BusinessNodeMaps, m => new
                {
                    m.Id,
                    BusinessNode_Id = m.BusinessNode.Id,
                    m.BusinessNode.BusinessName,
                    m.BusinessNode.Description,
                    DeviceNode_Id = m.DeviceNode.Id,
                    m.DeviceNode.NodeName,
                    m.DeviceNode.NodeUrl,
                    m.DeviceNode.DataType,
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
        public async Task<IHttpActionResult> Add(ProductionProcessEquipmentBusinessNodeMapInputDto inpuDto)
        {
            var result = await ProductionProcessEquipmentBusinessNodeMapContract.AddBusinessNodeMaps(inpuDto);
            return Json(result);
        }

        [Description("通讯管理-通讯业务点表与通讯点表关联管理-删除")]
        public async Task<IHttpActionResult> Remove(String idStr)
        {
            Guid id = Guid.Parse(idStr);
            var result = await ProductionProcessEquipmentBusinessNodeMapContract.DeleteBusinessNodeMaps(id);
            return Json(result);
        }

        [Description("通讯管理-通讯业务点表与通讯点表关联管理-修改")]
        public async Task<IHttpActionResult> Update(ProductionProcessEquipmentBusinessNodeMapInputDto inputDto)
        {
            var result = await ProductionProcessEquipmentBusinessNodeMapContract.EditBusinessNodeMaps(inputDto);
            return Json(result);
        }

    }
}