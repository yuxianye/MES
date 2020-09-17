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
    [Description("设备管理-设备厂家")]
    public class EquFactoryController : ServiceBaseApiController
    {
        /// <summary>
        /// 设备厂家契约
        /// </summary>
        public IEquFactoryInfoContract EquFactoryContract { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Description("设备管理-设备厂家-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EquFactoryContract.EquFactoryInfos, m => new
                {
                    m.Id,
                    m.FactoryCode,
                    m.FactoryName,
                    m.FactoryAddress,
                    m.Contacts,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取设备动作信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取设备动作信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("设备管理-设备厂家-增加")]
        public async Task<IHttpActionResult> Add(params EquFactoryInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EquFactoryContract.Add(dto);
            return Json(result);
        }

        [Description("设备管理-设备厂家-修改")]
        public async Task<IHttpActionResult> Update(params EquFactoryInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });

            var result = await EquFactoryContract.Update(dto);
            return Json(result);
        }

        [Description("设备管理-设备厂家-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await EquFactoryContract.Delete(ids);
            return Json(result);
        }
    }
}

