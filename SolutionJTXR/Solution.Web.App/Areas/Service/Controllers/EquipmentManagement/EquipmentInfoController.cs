using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Contracts;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("设备管理-设备信息")]
    public class EquipmentInfoController : ServiceBaseApiController
    {
        //设备信息契约
        public IEquEquipmentInfoContract EquipmentInfoContract { get; set; }
        //设备厂家信息契约
        public IEquFactoryInfoContract FactoryInfoContract { get; set; }
        //设备类型信息契约
        public IEquipmentTypeInfoContract EquipmentTypeInfoContract { get; set; }
        //部门信息契约
        public IEntDepartmentInfoContract DepartmentInfoContract { get; set; }

        [Description("设备管理-设备信息-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {

                GridRequest request = new GridRequest(requestParams);

                var page = GetPageResult(EquipmentInfoContract.EquipmentInfos, m => new
                {
                    m.Id,
                    m.EquipmentName,
                    m.EquipmentCode,
                    EquipmentType_Id = m.Equipmenttype.Id,
                    EquipmentType_Name = m.Equipmenttype.EquipmentTypeName,
                    m.Specifications,
                    m.ModelNumber,
                    DepartmentInfo_Id = m.DepartmentInfo.Id,
                    DepartmentInfo_Name = m.DepartmentInfo.DepartmentName,
                    m.EquipmentState,
                    m.StartusingTime,
                    m.ResponsiblePerson,
                    EquFactoryInfo_Id = m.EquFactoryInfo.Id,
                    EquFactoryInfo_Name = m.EquFactoryInfo.FactoryName,
                    m.FactoryNumber,
                    m.ProductionDate,
                    m.AbcType,
                    m.OriginalValue,
                    m.DepreciationYears,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取设备信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取设备信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("设备管理-设备信息-增加")]
        public async Task<IHttpActionResult> Add(params EquEquipmentInfoInputDto[] inputDtos)
        {
            inputDtos?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EquipmentInfoContract.Add(inputDtos);
            return Json(result);
        }

        [Description("设备管理-设备信息-修改")]
        public async Task<IHttpActionResult> Update(params EquEquipmentInfoInputDto[] dto)
        {
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EquipmentInfoContract.Update(dto);
            return Json(result);
        }

        [Description("设备管理-设备信息-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await EquipmentInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
