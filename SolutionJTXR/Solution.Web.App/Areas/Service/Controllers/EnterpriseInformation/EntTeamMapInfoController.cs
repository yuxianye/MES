using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Contracts;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers.EnterpriseInformation
{
    [Description("服务-班组关联管理")]
    public class EntTeamMapInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 班组关联信息契约
        /// </summary>
        public IEntTeamMapInfoContract EntTeamMapInfoContract { get; set; }

        [HttpPost]
        [Description("服务-班组关联管理-根据ID查询")]
        public IHttpActionResult GetEntTeamMapInfo(EntTeamMapInfo info)
        {
            EntTeamMapInfo Info = EntTeamMapInfoContract.EntTeamMapInfos.ToList().Find(s =>
            {
                return s.Id == info.Id;
            });
            return Json(new OperationResult(OperationResultType.Success, "读取班组关联数据成功！", Info));
        }

        [HttpPost]
        [Description("服务-班组关联管理-分页数据")]
        public IHttpActionResult GetEntTeamMapInfoList(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(EntTeamMapInfoContract.EntTeamMapInfos, m => new
                {
                    m.Id,
                    m.EntEmployeeInfo,
                    m.EntTeamInfo,
                    EntTeamInfo_Id = m.EntTeamInfo.Id,
                    m.EntTeamInfo.TeamName,
                    m.EntTeamInfo.TeamCode,
                    EntEmployeeInfo_Id = m.EntEmployeeInfo.Id,
                    m.EntEmployeeInfo.EntEmployeeCode,
                    m.EntEmployeeInfo.EntEmployeeName,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取班组关联信息列表数据成功！", page));
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取班组关联信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-班组关联管理-增加")]

        public async Task<IHttpActionResult> Add(params EntTeamMapInfoInputDto[] dto)
        {
            //创建和修改的人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.CreatorUserId = User.Identity.Name;
                a.CreatedTime = DateTime.Now;
                a.LastUpdatedTime = a.CreatedTime;
                a.LastUpdatorUserId = a.CreatorUserId;
            });
            var result = await EntTeamMapInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-班组关联管理-修改")]
        public async Task<IHttpActionResult> Update(params EntTeamMapInfoInputDto[] dto)
        {
            //修改人员、时间
            dto?.ToList().ForEach((a) =>
            {
                a.LastUpdatedTime = DateTime.Now;
                a.LastUpdatorUserId = User.Identity.Name;
            });
            var result = await EntTeamMapInfoContract.Update(dto);
            return Json(result);
        }

        [HttpPost]
        [Description("服务-班组关联管理-删除")]
        public async Task<IHttpActionResult> Delete(params Guid[] ids)
        {
            var result = await EntTeamMapInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
