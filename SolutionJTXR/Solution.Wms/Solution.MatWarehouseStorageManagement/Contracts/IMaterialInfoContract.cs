using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.MatWarehouseStorageManagement.Contracts
{
    /// <summary>
    /// 物料信息契约接口
    /// </summary>
    public interface IMaterialInfoContract : IScopeDependency
    {
        #region 物料信息业务

        /// <summary>
        /// 获取物料信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MaterialInfo> MaterialInfos { get; }

        /// <summary>
        /// 检查组物料信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的物料信息编号</param>
        /// <returns>物料信息是否存在</returns>
        bool CheckMaterialExists(Expression<Func<MaterialInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加物料信息
        /// </summary>
        /// <param name="inputDtos">要添加的物料信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MaterialInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新物料信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的物料信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateMaterials(params MaterialInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除物料信息
        /// </summary>
        /// <param name="ids">要删除的物料信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteMaterials(params Guid[] ids);
        
        #endregion
    }
}
