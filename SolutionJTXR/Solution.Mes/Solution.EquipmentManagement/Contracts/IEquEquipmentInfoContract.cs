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
    public interface IEquEquipmentInfoContract : IScopeDependency
    {
        #region 设备信息业务
        /// <summary>
        /// 获取设备信息查询数据集
        /// </summary>
        IQueryable<EquEquipmentInfo> EquipmentInfos { get; }

    
        /// <summary>
        /// 检查设备信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备信息编号</param>
        /// <returns>设备信息是否存在</returns>
        bool CheckEquipmentExists(Expression<Func<EquEquipmentInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加设备信息
        /// </summary>
        /// <param name="inputDtos">要添加的设备信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        
        Task<OperationResult> Add(params EquEquipmentInfoInputDto[] inputDtos);
        /// <summary>
        /// 更新设备信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的设备信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params EquEquipmentInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除设备信息
        /// </summary>
        /// <param name="ids">要删除的设备信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        #endregion
    }
}
