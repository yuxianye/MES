using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
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

namespace Solution.Web.App.Areas.Service.Controllers.CommOpcUa
{
    [Description("通讯业务点表管理")]
    public class CommOpcUaBusinessController : ServiceBaseApiController
    {
        public ICommOpcUaBusinessContract CommOpcUaBusinessContract { get; set; }

        public ICommOpcUaBusinessNodeMapContract CommOpcUaBusinessNodeMapContract { get; set; }
        public UserManager<User, int> userManager { get; set; }
        public IRepository<UserRoleMap, int> UserRoleMapRepository { get; set; }
        public IRepository<CommOpcUaBusiness, Guid> CommOpcUaBusinessRepository { get; set; }
        public IRepository<DeviceNode, Guid> CommOpcUaNodeRepository { get; set; }
        [HttpPost]
        [Description("通讯管理-通讯业务-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(CommOpcUaBusinessContract.CommOpcUaBusinessInfos, m => new
                {
                    m.Id,
                    m.BusinessName,
                    m.Description,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取通讯业务列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取通讯业务列表数据失败！", ex.ToString()));
            }
        }
        // [Description("通讯管理-通讯业务点表管理-分页数据")]
        //public IHttpActionResult PageData(PageRepuestParams requestParams)
        //{
        //    GridRequest request = new GridRequest(requestParams);
        //    PageRepuestParams localRequestParams = new PageRepuestParams();
        //    localRequestParams.PageIndex = requestParams.PageIndex;
        //    localRequestParams.PageSize = requestParams.PageSize;
        //    localRequestParams.SortField = requestParams.SortField;
        //    localRequestParams.SortOrder = requestParams.SortOrder;
        //    var sourceData = CommOpcUaBusinessContract.CommOpcUaBusinessInfos;
        //    if (requestParams.FilterGroup != null && requestParams.FilterGroup.Rules.Any())
        //    {
        //        var data = requestParams.FilterGroup.Rules.ToList()[0].Value;


        //        sourceData = CommOpcUaBusinessContract.CommOpcUaBusinessInfos.Where(
        //                        x =>
        //                        CommOpcUaBusinessNodeMapContract.CommOpcUaBusinessNodeMapInfos.Where(m => m.OpcUaNode.NodeName.Contains(data.ToString())).Select(a => a.OpcUaBusiness.Id).Contains(x.Id)
        //                        ||
        //                        x.BusinessName.Contains(data.ToString())
        //                        );

        //        request = new GridRequest(localRequestParams);
        //    }

        //    try
        //    {
        //        var page = GetPageResult(sourceData, m => new
        //        {
        //            m.Id,
        //            m.BusinessName,
        //            NodeId = CommOpcUaBusinessNodeMapContract.CommOpcUaBusinessNodeMapInfos.Where(x => x.OpcUaBusiness.Id.Equals(m.Id)).Select(x => x.OpcUaNode.Id).FirstOrDefault(),
        //            NodeName = CommOpcUaBusinessNodeMapContract.CommOpcUaBusinessNodeMapInfos.Where(x => x.OpcUaBusiness.Id.Equals(m.Id)).Select(x => x.OpcUaNode.NodeName).FirstOrDefault(),
        //            m.Description,
        //            m.CreatedTime
        //        }, request);
        //        return Json(new OperationResult(OperationResultType.Success, "查询通讯业务点表数据成功！", page));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new OperationResult(OperationResultType.Error, "查询通讯业务点表数据失败！" + ex.ToString()));
        //    }
        //}

        //[Description("通讯管理-通讯业务点表管理-查询一个")]
        //public IHttpActionResult Get(String id)
        //{
        //    Guid guid = Guid.Parse(id);
        //    CommOpcUaBusiness commOpcUaHistory = CommOpcUaBusinessContract.CommOpcUaBusinessInfos.ToList().Find(delegate (CommOpcUaBusiness data)
        //    {
        //        return data.Id == guid;
        //    });
        //    if (commOpcUaHistory == null)
        //    {
        //        return Json(new OperationResult(OperationResultType.Error, "查询通讯业务点表数据失败！"));
        //    }
        //    return Json(new OperationResult(OperationResultType.Success, "查询通讯业务点表数据成功！", commOpcUaHistory));
        //}

        [Description("通讯管理-通讯业务点表管理-添加")]
        public async Task<IHttpActionResult> Add(params CommOpcUaBusinessInputDto[] inpuDto)
        {
            //创建和修改的人员、时间
            inpuDto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });

            var result = await CommOpcUaBusinessContract.Add(inpuDto);
            return Json(result);
        }

        [Description("通讯管理-通讯业务点表管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {

            var result = await CommOpcUaBusinessContract.DeleteCommOpcUaBusinesss(ids);
            return Json(result);
        }

        [Description("通讯管理-通讯业务点表管理-修改")]
        public async Task<IHttpActionResult> Update(params CommOpcUaBusinessInputDto[] inputDto)
        {
            string name = User.Identity.Name;
            //修改人员、时间
            inputDto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await CommOpcUaBusinessContract.Update(inputDto);

            return Json(result);
        }

        [Description("通讯管理-通讯业务点表管理-设置")]
        public async Task<IHttpActionResult> Setting(params CommOpcUaBusinessManageInputDto[] inputDto)
        {
            inputDto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await CommOpcUaBusinessNodeMapContract.Setting(inputDto);
            return Json(result);
        }
    }
}