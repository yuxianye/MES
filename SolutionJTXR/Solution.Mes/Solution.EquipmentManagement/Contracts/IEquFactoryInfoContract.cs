using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.EquipmentManagement.Dtos;
using Solution.EquipmentManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EquipmentManagement.Contracts
{
    public interface IEquFactoryInfoContract : IScopeDependency
    {
        #region 设备厂家信息
        /// <summary>
        ///设备厂家信息查询数据集
        /// </summary>
        IQueryable<EquFactoryInfo> EquFactoryInfos { get; }

        /// <summary>
        ///设备厂家信息查询数据集
        /// </summary>
        IQueryable<EquFactoryInfo> EquFactoryTrackInfos { get; }

        /// <summary>
        /// 检查设备厂家信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新设备厂家信息编号</param>
        /// <returns>设备厂家信息是否存在</returns>
        bool CheckEquFactoryInfoExists(Expression<Func<EquFactoryInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 增加设备厂家信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>    AlarmInfoIntputDto
        Task<OperationResult> Add(params EquFactoryInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新设备厂家信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params EquFactoryInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除设备厂家信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        #endregion

    }
}
