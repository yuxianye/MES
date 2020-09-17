using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using OSharp.Utility.Filter;
using Solution.EnterpriseInformation.Contracts;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-车间管理")]
    public class EntAreaInfoController : ServiceBaseApiController
    {

        /// <summary>
        /// 车间信息契约
        /// </summary>
        public IEntAreaInfoContract EntAreaInfoContract { get; set; }

        [HttpPost]
        [Description("服务-车间管理-根据ID查询")]
        public IHttpActionResult GetEntAreaInfo(EntAreaInfo entareainfo)
        {
            EntAreaInfo entareaInfo = EntAreaInfoContract.EntAreaInfo.ToList().Find(s =>
            {
                return s.Id == entareainfo.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取车间数据成功！", entareaInfo));
        }

        [HttpPost]
        [Description("服务-车间管理-分页数据")]
        public IHttpActionResult GetEntAreaList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EntAreaInfoContract.EntAreaInfo, m => new
                {
                    m.Id,
                    m.AreaName,
                    m.AreaCode,
                    m.AreaPhone,
                    m.AreaManager,
                    Enterprise_Id = m.EntSite.Enterprise.Id,
                    m.EntSite.Enterprise.EnterpriseName,
                    EntSite_Id = m.EntSite.Id,
                    m.EntSite.SiteName,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取车间信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取车间信息列表数据失败！", ex.ToString()));
            }
        }

        [HttpPost]
        [Description("服务-车间管理-获取某一厂区下的车间列表")]
        public IHttpActionResult GetEntAreaListByEntSiteID(EntSiteInfo entsiteInfo)
        {
            try
            {

                var page = GetPageResult(EntAreaInfoContract.EntAreaInfo.Where(m => m.EntSite.Id == entsiteInfo.Id), m => new
                {
                    m.Id,
                    m.AreaName,
                    m.AreaCode,
                    EntSite_Id = m.EntSite.Id,
                    m.EntSite.SiteName,
                    Enterprise_Id = m.EntSite.Enterprise.Id,
                    m.EntSite.Enterprise.EnterpriseName,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某厂区下车间信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某厂区下车间信息列表数据失败！", ex.ToString()));
            }
        }

        [Description("服务-车间管理-增加")]

        public async Task<IHttpActionResult> Add(params EntAreaInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EntAreaInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-车间管理-修改")]
        public async Task<IHttpActionResult> Update(params EntAreaInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EntAreaInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-车间管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await EntAreaInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
