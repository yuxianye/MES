using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Contracts
{
    /// <summary>
    /// 历史数据契约接口
    /// </summary>
    public interface IHistoryDataContract : IScopeDependency
    {
        #region 历史数据业务
        /// <summary>
        /// 获取历史数据查询数据集
        /// </summary>
        IQueryable<HistoryData> HistoryDatas { get; }

        /// <summary>
        /// 检查历史数据信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的历史数据编号</param>
        /// <returns>历史数据是否存在</returns>
        bool CheckHistoryDataExists(Expression<Func<HistoryData, bool>> predicate, long id);

        /// <summary>
        /// 添加历史数据
        /// </summary>
        /// <param name="inputDtos">要添加的历史数据DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params HistoryDataInputDto[] inputDtos);

        /// <summary>
        /// 更新历史数据信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的历史数据DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Edit(params HistoryDataInputDto[] inputDtos);

        /// <summary>
        /// 物理删除历史数据
        /// </summary>
        /// <param name="ids">要删除的历史数据编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params long[] ids);

        /// <summary>
        /// 逻辑删除Opc Ua 历史数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        //Task<OperationResult> RecycleHistoryData(params CommOpcUaHistory[] data);

        /// <summary>
        /// 恢复逻辑删除Opc Ua 历史数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        //Task<OperationResult> RestoreHistoryData(params CommOpcUaHistory[] data);

        #endregion
    }
}
