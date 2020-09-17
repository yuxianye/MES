using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.Agv.Dtos;
using Solution.Agv.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Solution.Agv.Contracts
{
    public interface IRouteInfoContract : IScopeDependency
    {
        #region 路径信息业务
        /// <summary>
        /// 获取路径信息查询数据集
        /// </summary>
        IQueryable<RouteInfo> RouteInfos { get; }

        /// <summary>
        /// 检查路径信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的路径信息编号</param>
        /// <returns>路径信息是否存在</returns>
        bool CheckRouteInfoExists(Expression<Func<RouteInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 递归查找起始点到终点的所有路段集合
        /// </summary>
        /// <param name="StartMarkPoint"></param>
        /// <param name="EndMarkPoint"></param>
        /// <param name="BeforeMarkPoint"></param>
        /// <param name="MarkPoints"></param>
        /// <param name="MarkPointsWithOut"></param>
        Task<Boolean> GetCombination(MarkPointInfo StartMarkPoint, MarkPointInfo EndMarkPoint, MarkPointInfo BeforeMarkPoint, List<RoadInfo> AllRoads, List<RoadInfo> PassedRoads, List<RoadInfo> GetRoadInfoList);

        /// <summary>
        /// 增加路径信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params RouteInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新路径信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params RouteInfoInputDto[] inputDtos);

        /// <summary>
        /// 自动生成路径信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Generate(params RouteInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除路径信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        #endregion
    }
}
