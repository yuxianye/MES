using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.Agv.ControlService
{
    /// <summary>
    /// Agv控制契约接口
    /// </summary>
    public interface IAgvControlContract : ISingletonDependency
    {
        /// <summary>
        /// 初始化工作
        /// </summary>
        /// <returns></returns>
        void Initialize();

        //#region agv小车信息业务

        ///// <summary>
        ///// 获取agv小车信息查询数据集
        ///// </summary>
        //IQueryable<AgvInfo> AgvInfos { get; }

        ///// <summary>
        ///// 检查agv小车信息是否存在
        ///// </summary>
        ///// <param name="predicate">检查谓语表达式</param>
        ///// <param name="id">更新的agv小车信息编号</param>
        ///// <returns>agv小车信息是否存在</returns>
        //bool CheckAgvInfoExists(Expression<Func<AgvInfo, bool>> predicate, Guid id);

        ///// <summary>
        ///// 添加agv小车信息
        ///// </summary>
        ///// <param name="inputDtos">要添加的agv小车信息DTO信息</param>
        ///// <returns>业务操作结果</returns>
        //Task<OperationResult> Add(params AgvInfoInputDto[] inputDtos);

        ///// <summary>
        ///// 更新agv小车信息信息
        ///// </summary>
        ///// <param name="inputDtos">包含更新信息的agv小车信息DTO信息</param>
        ///// <returns>业务操作结果</returns>
        //Task<OperationResult> Update(params AgvInfoInputDto[] inputDtos);

        ///// <summary>
        ///// 物理删除agv小车信息
        ///// </summary>
        ///// <param name="ids">要删除的agv小车信息编号</param>
        ///// <returns>业务操作结果</returns>
        //Task<OperationResult> Delete(params Guid[] ids);

        //#endregion
    }
}
