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
    public interface IEquipmentTypeInfoContract : IScopeDependency
    {
        #region 设备类别信息业务
        /// <summary>
        /// 获取设备类别查询数据集
        /// </summary>
        IQueryable<EquipmentTypeInfo> EquipmentTypeInfos { get; }

        /// <summary>
        /// 检查设备类别信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备类别信息编号</param>
        /// <returns>设备类别信息是否存在</returns>
        bool CheckEquipmentTypeExists(Expression<Func<EquipmentTypeInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加设备类别信息
        /// </summary>
        /// <param name="inputDtos">要添加的设备类别信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddEquipmentType(params EquipmentTypeInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新设备类别信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的设备类别信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateEquipmentType(params EquipmentTypeInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除设备类别信息
        /// </summary>
        /// <param name="ids">要删除的设备类别信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteEquipmentType(params Guid[] ids);


        ///// <summary>
        ///// 逻辑删除设备类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDeleteEquipmentType(params EquipmentTypeInfo[] equiinfo);
        ///// <summary>
        ///// 逻辑还原设备类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestoreEquipmentType(params EquipmentTypeInfo[] equiinfo);


        #endregion
    }
}
