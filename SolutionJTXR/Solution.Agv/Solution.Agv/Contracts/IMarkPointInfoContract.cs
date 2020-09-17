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
    public interface IMarkPointInfoContract : IScopeDependency
    {
        #region 地标点信息业务
        /// <summary>
        /// 获取地标点信息查询数据集
        /// </summary>
        IQueryable<MarkPointInfo> MarkPointInfos { get; }

        /// <summary>
        /// 检查组地标点信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的地标点信息编号</param>
        /// <returns>地标点信息是否存在</returns>
        bool CheckMarkPointInfoExists(Expression<Func<MarkPointInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 增加地标点信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params MarkPointInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新地标点信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params MarkPointInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除地标点信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        #endregion
    }
}
