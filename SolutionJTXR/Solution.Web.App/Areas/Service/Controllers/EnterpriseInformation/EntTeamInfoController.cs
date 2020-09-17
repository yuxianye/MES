using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Contracts;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-班组管理")]
    public class EntTeamInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 班组信息契约
        /// </summary>
        public IEntTeamInfoContract EntTeamInfoContract { get; set; }
        public IEntTeamMapInfoContract EntTeamMapInfoContract { get; set; }

        [HttpPost]
        [Description("服务-班组管理-根据ID查询")]
        public IHttpActionResult GetEntTeamInfo(EntTeamInfo info)
        {
            EntTeamInfo Info = EntTeamInfoContract.EntTeamInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取班组数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-班组管理-分页数据")]
        public IHttpActionResult GetEntTeamInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EntTeamInfoContract.EntTeamInfos, m => new
                {
                    m.Id,
                    m.TeamName,
                    m.TeamCode,
                    m.TeamNumber,
                    m.Description,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取班组信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取班组信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-班组管理-增加")]

        public async Task<IHttpActionResult> Add(params EntTeamInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EntTeamInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-班组管理-修改")]
        public async Task<IHttpActionResult> Update(params EntTeamInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EntTeamInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-班组管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await EntTeamInfoContract.Delete(ids);
            return Json(result);
        }

        [Description("服务-班组管理-人员配置")]
        public async Task<IHttpActionResult> Setting(params EntTeamMapManageInfoInputDto[] inputDtos)
        {
            inputDtos?.ToList().ForEach((a) =>
            {
                a.Id = CombHelper.NewComb();
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EntTeamMapInfoContract.Setting(inputDtos);
            return Json(result);
        }
    }
}
