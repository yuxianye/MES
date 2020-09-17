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
    /// 业务点表Map契约接口
    /// </summary>
    public interface IProductionProcessEquipmentBusinessNodeMapContract : IScopeDependency
    {
        #region 工序设备业务点表业务
        /// <summary>
        /// 获取设备业务点表查询数据集
        /// </summary>
        IQueryable<ProductionProcessEquipmentBusinessNodeMap> BusinessNodeMaps { get; }

        /// <summary>
        /// 检查业务点表map数据关联的信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的业务点表map信息编号</param>
        /// <returns>OpcUa业务数据关联数据点信息是否存在</returns>
        bool CheckBusinessNodeMapExists(Expression<Func<ProductionProcessEquipmentBusinessNodeMap, bool>> predicate, Guid id);

        /// <summary>
        /// 添加业务点表map信息
        /// </summary>
        /// <param name="inputDtos">要添加的业务点表mapDTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddBusinessNodeMaps(params ProductionProcessEquipmentBusinessNodeMapInputDto[] inputDtos);

        /// <summary>
        /// 更新业务点表map信息
        /// </summary>
        /// <param name="inputDtos">包含更新的业务点表map信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditBusinessNodeMaps(params ProductionProcessEquipmentBusinessNodeMapInputDto[] inputDtos);

        /// <summary>
        /// 删除业务点表map信息
        /// </summary>
        /// <param name="ids">要删除的业务点表map信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteBusinessNodeMaps(params Guid[] ids);

        /// <summary>
        /// 配置业务点表map
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Setting(params EquipmentBusinessNodeMapManageInputDto[] inputDtos);

        #endregion
    }
}
