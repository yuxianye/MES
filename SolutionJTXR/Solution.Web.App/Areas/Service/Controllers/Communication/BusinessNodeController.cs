using Microsoft.AspNet.Identity;
using OSharp.Utility.Data;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Dtos;
using Solution.Core.Models.Identity;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("业务点表管理")]
    public class BusinessNodeController : ServiceBaseApiController
    {
        public IBusinessNodeContract BusinessNodeContract { get; set; }

        public IProductionProcessEquipmentBusinessNodeMapContract ProductionProcessEquipmentBusinessNodeMapContract { get; set; }

        public UserManager<User, int> userManager { get; set; }

        [HttpPost]
        [Description("通讯管理-业务点表管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(BusinessNodeContract.BusinessNodes, m => new
                {
                    m.Id,
                    m.BusinessName,
                    m.Description,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取业务点表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取业务点表数据失败！", ex.ToString()));
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

        [Description("通讯管理-业务点表管理-添加")]
        public async Task<IHttpActionResult> Add(params BusinessNodeInputDto[] inpuDto)
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

            var result = await BusinessNodeContract.Add(inpuDto);
            return Json(result);
        }

        [Description("通讯管理-业务点表管理-删除")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {

            var result = await BusinessNodeContract.DeleteBusinessNodes(ids);
            return Json(result);
        }

        [Description("通讯管理-业务点表管理-修改")]
        public async Task<IHttpActionResult> Update(params BusinessNodeInputDto[] inputDto)
        {
            string name = User.Identity.Name;
            //修改人员、时间
            inputDto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await BusinessNodeContract.Update(inputDto);

            return Json(result);
        }

        [Description("通讯管理-业务点表管理-设置")]
        public async Task<IHttpActionResult> Setting(params EquipmentBusinessNodeMapManageInputDto[] inputDto)
        {
            inputDto?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await ProductionProcessEquipmentBusinessNodeMapContract.Setting(inputDto);
            return Json(result);
        }
    }
}