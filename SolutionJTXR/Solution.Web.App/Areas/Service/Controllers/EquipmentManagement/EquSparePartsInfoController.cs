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
    [Description("服务-备件管理")]
    public class EquSparePartsInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 备件信息契约
        /// </summary>
        public IEquSparePartsInfoContract EquSparePartsInfoContract { get; set; }


        [Description("服务-备件管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EquSparePartsInfoContract.EquSparePartsInfos, m => new
                {
                    m.Id,
                    EquSparePartType_Id = m.Equspareparttypeinfo.Id,
                    m.Equspareparttypeinfo.EquSparePartTypeName,
                    m.SparePartName,
                    m.SparePartCode,
                    m.Specifications,
                    m.ModelNumber,
                    m.Quantity,
                    m.SparePartUnit,
                    m.Price,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId

                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取备件信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取备件信息列表数据失败！" + ex.ToString()));
            }
        }



        [Description("服务-备件管理-增加")]
        public async Task<IHttpActionResult> Add(params EquSparePartsInfoInputDto[] dto)
        {
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EquSparePartsInfoContract.AddEquSpareParts(dto);
            return Json(result);
        }


        [Description("服务-备件管理-修改")]
        public async Task<IHttpActionResult> Update(params EquSparePartsInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EquSparePartsInfoContract.UpdateEquSpareParts(dto);
            return Json(result);
        }

        [Description("服务-备件管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await EquSparePartsInfoContract.DeleteEquSpareParts(ids);
            return Json(result);
        }
    }
}
