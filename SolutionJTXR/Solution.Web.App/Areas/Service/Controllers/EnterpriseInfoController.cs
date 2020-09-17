using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Filter;
using OSharp.Web.Http.Filters;
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
    /// <summary>
    /// 企业信息API控制器
    /// </summary>
    [Description("服务-企业管理")]
    public class EnterpriseInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 企业信息服务契约接口
        /// </summary>
        public IEnterpriseInfoContract EnterpriseInfoContract { get; set; }

        ///// <summary>
        ///// 用户管理
        ///// </summary>
        //public UserManager<User, int> userManager { get; set; }

        ///// <summary>
        ///// 用户角色映射仓储
        ///// </summary>
        //public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }

        /// <summary>
        /// 分页数据
        /// PageRepuestParams举例：
        /// {
	    ///     "filterGroup":"",
	    ///     "pageIndex":1,
	    ///     "pageSize":5,
	    ///     "sortField":"Id,NodeId,NodeName,NodeUrl,Interval,Description,IsLocked,CreatedTime",
	    ///     "sortOrder":",asc,,,,,,"
        ///}
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        [Description("服务-企业管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                string username = User.Identity.Name;
                //User user = userManager.Users.Where(x => x.UserName.Equals(username)).FirstOrDefault();
                //UserRoleMap userRoleMap = UserRoleMapRepository.Entities.Where(x => x.User.Id == user.Id).First();
                //Role role = userRoleMap.Role;

                GridRequest request = new GridRequest(requestParams);
                //if (role.IsAdmin)//管理员可管理全部数据
                //{
                var page = GetPageResult(EnterpriseInfoContract.EnterpriseInfos, m => new
                {
                    m.Id,
                    m.EnterpriseCode,
                    m.EnterpriseName,
                    m.EnterpriseAddress,
                    m.EnterprisePhone,
                    m.Description,
                    m.Remark,
                    //m.IsDeleted,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取企业信息列表数据成功！", page));
                //}
                //else//普通用户取得未删除为锁定的用户。
                //{
                //    var page = GetPageResult(EnterpriseInfoContract.EnterpriseInfos.Unrecycled(), m => new
                //    {
                //        m.Id,
                //        m.EnterpriseCode,
                //        m.EnterpriseName,
                //        m.EnterpriseAddress,
                //        m.EnterprisePhone,
                //        m.Description,
                //        m.Remark,
                //        m.IsDeleted,
                //        m.CreatedTime,
                //        m.CreatorUserId,
                //        m.LastUpdatedTime,
                //        m.LastUpdatorUserId,
                //    }, request);
                //    return Json(new OperationResult(OperationResultType.Success, "读取企业信息列表数据成功！", page));
                //}
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取企业信息列表数据失败！" + ex.ToString()));
            }
        }

        //[HttpPost]
        //[Description("服务-企业信息管理-获取可用的企业信息列表")]
        //public IHttpActionResult GetEnterpriseInfoList_IsUse()
        //{
        //    try
        //    {
        //        var page = GetPageResult(EnterpriseInfoContract.EnterpriseInfos/*.Unlocked().Unrecycled()*/, m => new
        //        {
        //            m.Id,
        //            m.EnterpriseName,
        //            m.EnterpriseCode,
        //            m.EnterpriseAddress,
        //            m.EnterprisePhone,
        //            m.Description,
        //            m.Remark,
        //            //m.IsLocked,
        //            //m.IsDeleted,
        //            m.CreatedTime
        //        });
        //        return Json(new OperationResult(OperationResultType.Success, "读取可用的企业信息列表成功！", page));

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new OperationResult(OperationResultType.Error, "读取可用的企业信息列表失败！", ex.ToString()));
        //    }
        //}

        [Description("服务-企业管理-增加")]
        public async Task<IHttpActionResult> Add(params EnterpriseInfoInputDto[] dto)
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
            var result = await EnterpriseInfoContract.AddEnterprises(dto);
            return Json(result);
        }

        [Description("服务-企业管理-修改")]
        public async Task<IHttpActionResult> Update(params EnterpriseInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });

            var result = await EnterpriseInfoContract.UpdateEnterprises(dto);
            return Json(result);
        }

        [Description("服务-企业管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await EnterpriseInfoContract.DeleteEnterprises(ids);
            return Json(result);
        }

        //[Description("服务-企业信息管理-逻辑删除企业信息")]
        //public async Task<IHttpActionResult> Recycle(params EnterpriseInfo[] enterinfo)
        //{
        //    //修改人员、时间
        //    enterinfo?.ToList().ForEach((a) =>
        //    {
        //        a.LastUpdatedTime = DateTime.Now;
        //        a.LastUpdatorUserId = User.Identity.Name;
        //    });
        //    var result = await EnterpriseInfoContract.LogicDeleteEnterprises(enterinfo);
        //    return Json(result);
        //}

        //[Description("服务-企业信息管理-逻辑还原企业信息")]
        //public async Task<IHttpActionResult> Restore(params EnterpriseInfo[] enterinfo)
        //{
        //    //修改人员、时间
        //    enterinfo?.ToList().ForEach((a) =>
        //    {
        //        a.LastUpdatedTime = DateTime.Now;
        //        a.LastUpdatorUserId = User.Identity.Name;
        //    });

        //    var result = await EnterpriseInfoContract.LogicRestoreEnterprises(enterinfo);
        //    return Json(result);
        //}
    }
}
