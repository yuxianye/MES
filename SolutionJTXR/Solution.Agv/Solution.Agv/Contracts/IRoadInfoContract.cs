using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.Agv.Dtos;
using Solution.Agv.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.Agv.Contracts
{
    public interface IRoadInfoContract : IScopeDependency
    {
        #region 路段信息业务
        /// <summary>
        /// 获取路段信息查询数据集
        /// </summary>
        IQueryable<RoadInfo> RoadInfos { get; }

        /// <summary>
        /// 检查路段信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的路段信息编号</param>
        /// <returns>路段信息是否存在</returns>
        bool CheckRoadInfoExists(Expression<Func<RoadInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 增加路段信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params RoadInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新路段信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params RoadInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除路段信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        #endregion
    }
}
