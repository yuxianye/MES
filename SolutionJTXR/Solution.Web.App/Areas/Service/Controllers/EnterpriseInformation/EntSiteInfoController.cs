using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using OSharp.Utility.Filter;
using Solution.Core.Identity;
using Solution.Core.Models.Identity;
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
    [Description("服务-厂区管理")]
    public class EntSiteInfoController : ServiceBaseApiController
    {
        public UserManager<User, int> userManager { get; set; }
        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }

        /// <summary>
        /// 厂区信息契约
        /// </summary>
        public IEntSiteInfoContract EntSiteInfoContract { get; set; }

        [HttpPost]
        [Description("服务-厂区管理-根据ID查询")]
        public IHttpActionResult GetEntSiteInfo(EntSiteInfo entsiteinfo)
        {
            EntSiteInfo entsiteInfo = EntSiteInfoContract.EntSiteInfo.ToList().Find(s =>
            {
                return s.Id == entsiteinfo.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取企业厂区数据成功！", entsiteInfo));
        }

        [HttpPost]
        [Description("服务-厂区管理-获取某一企业下的厂区列表")]
        public IHttpActionResult GetEntSiteListByEnterpriseID(EnterpriseInfo enterpriseInfo)
        {
            try
            {

                var page = GetPageResult(EntSiteInfoContract.EntSiteInfo.Where(m => m.Enterprise.Id == enterpriseInfo.Id), m => new
                {
                    m.Id,
                    m.SiteName,
                    m.SiteCode,
                    m.SitePhone,
                    m.SiteManager,
                    Enterprise_Id = m.Enterprise.Id,
                    m.Enterprise.EnterpriseName,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                });
                return Json(new OperationResult(OperationResultType.Success, "读取某企业下厂区信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取某企业下厂区信息列表数据失败！", ex.ToString()));
            }
        }


        [HttpPost]
        [Description("服务-厂区管理-分页数据")]
        public IHttpActionResult GetEntSiteList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EntSiteInfoContract.EntSiteInfo, m => new
                {
                    m.Id,
                    m.SiteName,
                    m.SiteCode,
                    m.SitePhone,
                    m.SiteManager,
                    Enterprise_Id = m.Enterprise.Id,
                    m.Enterprise.EnterpriseName,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取厂区信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取厂区信息列表数据失败！", ex.ToString()));
            }
        }



        [Description("服务-厂区管理-增加")]

        public async Task<IHttpActionResult> Add(params EntSiteInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EntSiteInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-厂区管理-修改")]
        public async Task<IHttpActionResult> Update(params EntSiteInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EntSiteInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-厂区管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await EntSiteInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
