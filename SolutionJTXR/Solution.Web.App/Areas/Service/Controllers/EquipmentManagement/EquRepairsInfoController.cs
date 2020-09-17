using OSharp.Utility.Data;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers.EquipmentManagement
{
    [Description("设备管理-设备维护")]
    public class EquRepairsInfoController : ServiceBaseApiController
    {
        //设备信息契约
        public IEquRepairsInfoContract EquRepairsInfoContract { get; set; }

        [Description("设备管理-设备维护-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EquRepairsInfoContract.EquRepairsInfos, m => new
                {
                    m.Id,
                    EquipmentInfo_Id = m.EquipmentInfo.Id,
                    EquipmentName = m.EquipmentInfo.EquipmentName,
                    m.RepairCode,
                    m.RepairDate,
                    m.FinishDate,
                    m.StopDuration,
                    m.DiagnosisDuration,
                    m.SupportDuration,
                    m.FaultRemovingDuration,
                    m.SparePartDuration,
                    SparePartsInfo_Id = m.SparePartsInfo.Id,
                    SparePartName = m.SparePartsInfo.SparePartName,
                    m.SparePartQuantity,
                    m.SparePartCost,
                    m.faultType,
                    m.FaultDescription,
                    m.FaultAnalysis,
                    m.FaultReason,
                    m.ManhoursCost,
                    m.FaultLossCost,
                    m.Repairman,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取设备维修信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取设备维修信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("设备管理-设备维护-增加")]
        public async Task<IHttpActionResult> Add(params EquRepairsInfoInputDto[] inputDtos)
        {
            inputDtos?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EquRepairsInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("设备管理-设备维护-修改")]
        public async Task<IHttpActionResult> Update(params EquRepairsInfoInputDto[] dto)
        {
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EquRepairsInfoContract.Update(dto);
            return Json(result);
        }

        [Description("设备管理-设备维护-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await EquRepairsInfoContract.Delete(ids);
            return Json(result);
        }
    }
}