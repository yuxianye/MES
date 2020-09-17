using OSharp.Utility.Data;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-设备运行状态")]
    public class EquRunningStateInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 设备信息契约
        /// </summary>
        public IEquRunningStateInfoContract EquRunningStateInfoContract { get; set; }


        [Description("服务-设备运行状态-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {

                GridRequest request = new GridRequest(requestParams);

                var page = GetPageResult(EquRunningStateInfoContract.EquRunningStateInfos, m => new
                {
                    m.Id,
                    EquipmentInfo_Id = m.Equipmentinfo.Id,
                    m.Equipmentinfo.EquipmentName,
                    m.Equipmentinfo.EquipmentCode,
                    m.EquRunningStateTypes,
                    m.FaultInfo,
                    m.FaultCode,
                    m.RunningStateTime,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                    m.Remark
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取设备运行状态信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取设备运行状态信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-设备运行状态-增加")]
        public async Task<IHttpActionResult> Add(params EquRunningStateInfoInputDto[] dto)
        {
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EquRunningStateInfoContract.AddEquRunningStateInfo(dto);
            return Json(result);
        }


        [Description("服务-设备运行状态-修改")]
        public async Task<IHttpActionResult> Update(params EquRunningStateInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EquRunningStateInfoContract.UpdateEquRunningStateInfo(dto);
            return Json(result);
        }

        [Description("服务-设备运行状态-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await EquRunningStateInfoContract.DeleteEquRunningStateInfo(ids);
            return Json(result);
        }

    }
}
