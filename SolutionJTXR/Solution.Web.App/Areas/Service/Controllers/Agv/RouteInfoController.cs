using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using Solution.Agv.Contracts;
using Solution.Agv.Models;
using Solution.Core.Models.Identity;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Collections.Generic;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-路径管理")]
    public class RouteInfoController : ServiceBaseApiController
    {
        /// <summary>
        /// 路径信息契约
        /// </summary>
        public IRouteInfoContract RouteInfoContract { get; set; }

        public IRoadInfoContract RoadInfoContract { get; set; }

        public IMarkPointInfoContract MarkPointInfoContract { get; set; }

        /// <summary>
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
        [Description("服务-路径管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(RouteInfoContract.RouteInfos, m => new
                {
                    m.Id,
                    m.RouteNo,
                    m.RouteName,
                    m.MarkPoints,
                    m.TotalDistance,
                    StartMarkPointInfo_Id = m.StartMarkPointInfo.Id,
                    EndMarkPointInfo_Id = m.EndMarkPointInfo.Id,
                    StartMarkPointInfoName = m.StartMarkPointInfo.MarkPointName,
                    EndMarkPointInfoName = m.EndMarkPointInfo.MarkPointName,
                    m.Remark,
                    m.CreatedTime,
                    m.CreatorUserId,
                    m.LastUpdatedTime,
                    m.LastUpdatorUserId,
                }, request);
                return Json(new OperationResult(OperationResultType.Success, "读取路径信息列表数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "读取路径信息列表数据失败！" + ex.ToString()));
            }
        }

        [Description("服务-路径管理-增加路径信息")]
        public async Task<IHttpActionResult> Add(params Agv.Dtos.RouteInfoInputDto[] dto)
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
            var result = await RouteInfoContract.Add(dto);
            return Json(result);
        }

        [Description("服务-路径管理-自动生成所有路径信息")]
        public async Task<IHttpActionResult> Generate(params Agv.Dtos.RouteInfoInputDto[] dto)
        {
            //地标点和路径信息全获取
            List<MarkPointInfo> TmpMarkPointList = MarkPointInfoContract.MarkPointInfos?.ToList();

            //起始点
            MarkPointInfo StartMarkPoint = new MarkPointInfo();
            //终止点
            MarkPointInfo EndMarkPoint = new MarkPointInfo();
            //中间层起始点
            MarkPointInfo BeforeMarkPoint = new MarkPointInfo();
            List<RoadInfo> AllRoads = new List<RoadInfo>();
            List<RoadInfo> PassedRoads = new List<RoadInfo>();

            List<RoadInfo> GetRoadInfoList = RoadInfoContract.RoadInfos.ToList();
            //反向所有路段
            List<RoadInfo> GetReverseRoadInfoList = RoadInfoContract.RoadInfos.ToList();
            MarkPointInfo TmproadInfo = new MarkPointInfo();

            Agv.Dtos.RouteInfoInputDto RouteInfo = new Agv.Dtos.RouteInfoInputDto();
            OperationResult result = new OperationResult();
            dto = new Agv.Dtos.RouteInfoInputDto[1] { RouteInfo };
            int StartMarkPointOfMarkPoints = 0;
            int EndMarkPointOfMarkPoints = -1;
            //相同起点和终点的路段编号
            int RoutesNum = 0;
            if (!Equals(TmpMarkPointList, null))
            {
                //得到无相路段
                GetReverseRoadInfoList.ForEach((a) =>
                {
                    TmproadInfo = a.StartMarkPointInfo;
                    a.StartMarkPointInfo = a.EndMarkPointInfo;
                    a.EndMarkPointInfo = TmproadInfo;
                });
                GetRoadInfoList.AddRange(GetReverseRoadInfoList);
                //起始点和终止点全排列
                for (int i = 0; i < TmpMarkPointList.Count; i++)
                {
                    for (int j = 0; j < TmpMarkPointList.Count; j++)
                    {
                        if (i != j)
                        {
                            RoutesNum = 0;
                            StartMarkPoint = TmpMarkPointList[i];
                            BeforeMarkPoint = TmpMarkPointList[i];
                            EndMarkPoint = TmpMarkPointList[j];

                            if (!Equals(AllRoads, null))
                            {
                                AllRoads.Clear();
                            }
                            if (!Equals(PassedRoads, null))
                            {
                                PassedRoads.Clear();
                            }
                            //用递归找到可以到达的路径的集合
                            var V = await RouteInfoContract.GetCombination(StartMarkPoint, EndMarkPoint, BeforeMarkPoint, AllRoads, PassedRoads, GetRoadInfoList);

                            if (AllRoads.Count > 0)
                            {
                                //list翻转
                                AllRoads.Reverse();
                                //用来区分路段集合中的多段路径
                                for (int k = 0; k < AllRoads.Count; k++)
                                {
                                    //记录每一次开始地标点存在的位置在MarkPoints
                                    if (Equals(AllRoads[k].StartMarkPointInfo, StartMarkPoint))
                                    {
                                        StartMarkPointOfMarkPoints = k;
                                    }
                                    //记录每一次结束地标点存在的位置在MarkPoints
                                    if (Equals(AllRoads[k].EndMarkPointInfo, EndMarkPoint))
                                    {
                                        EndMarkPointOfMarkPoints = k;
                                    }
                                    //每次找到结束点添加多段路径的表中
                                    if (EndMarkPointOfMarkPoints >= 0)
                                    {
                                        ++RoutesNum;
                                        dto?.ToList().ForEach((a) =>
                                        {
                                            a.TotalDistance = 0;
                                            a.MarkPoints = "";
                                            a.Id = CombHelper.NewComb();
                                            a.CreatorUserId = User.Identity.Name;
                                            a.CreatedTime = DateTime.Now;
                                            a.LastUpdatedTime = a.CreatedTime;
                                            a.LastUpdatorUserId = a.CreatorUserId;

                                            for (int m = StartMarkPointOfMarkPoints; m <= EndMarkPointOfMarkPoints; m++)
                                            {

                                                a.RouteStatus = AllRoads[m].RoadStatus;
                                                a.TotalDistance += AllRoads[m].Distance;
                                                //if (a.MarkPoints.IndexOf(MarkPoints[m].StartMarkPointInfo.MarkPointName) == -1)
                                                //{
                                                a.MarkPoints += AllRoads[m].StartMarkPointInfo.MarkPointName + ",";
                                                //}
                                            }
                                            a.RouteNo = "Route" + "(" + StartMarkPoint.MarkPointName + "-" + EndMarkPoint.MarkPointName + ")" + "=" + RoutesNum.ToString();
                                            a.RouteName = "路段" + "(" + StartMarkPoint.MarkPointName + "-" + EndMarkPoint.MarkPointName + ")" + "=" + RoutesNum.ToString();
                                            //if (a.MarkPoints.IndexOf(EndMarkPoint.MarkPointName) == -1)
                                            //{
                                            a.MarkPoints += EndMarkPoint.MarkPointName;
                                            //}
                                            a.StartMarkPointInfo = StartMarkPoint;
                                            a.EndMarkPointInfo = EndMarkPoint;
                                            a.StartMarkPointInfo_Id = StartMarkPoint.Id;
                                            a.EndMarkPointInfo_Id = EndMarkPoint.Id;

                                        });
                                        EndMarkPointOfMarkPoints = -1;
                                        result = await RouteInfoContract.Generate(dto);
                                        if (!result.Successed)
                                        {
                                            return Json(result);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return null;
            }

            return Json(result);
        }

        [Description("服务-路径管理-修改路径信息")]
        public async Task<IHttpActionResult> Update(params Agv.Dtos.RouteInfoInputDto[] dto)
        {
            var result = await RouteInfoContract.Update(dto);
            return Json(result);
        }

        [Description("服务-路径管理-物理删除路径信息")]
        public async Task<IHttpActionResult> Remove(params Guid[] ids)
        {
            var result = await RouteInfoContract.Delete(ids);
            return Json(result);
        }
    }
}
