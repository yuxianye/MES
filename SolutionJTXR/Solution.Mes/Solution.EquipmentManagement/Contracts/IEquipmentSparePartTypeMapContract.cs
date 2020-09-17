using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Solution.EquipmentManagement.Models;
using Solution.EquipmentManagement.Dtos;

namespace Solution.EquipmentManagement.Contracts
{
    public interface IEquipmentSparePartTypeMapContract : IScopeDependency
    {
        #region 设备刀具映射业务
        /// <summary>
        /// 获取设备刀具映射信息
        /// </summary>
        IQueryable<EquipmentSparePartTypeMap> EquipmentSparePartTypeMaps { get; }


        /// <summary>
        /// 检查设备刀具映射信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备刀具映射编号</param>
        /// <returns>设备刀具映射是否存在</returns>
        bool CheckEquipmentSparePartTypeMapExists(Expression<Func<EquipmentSparePartTypeMap, bool>> predicate, Guid id);


        /// <summary>
        /// 添加设备刀具映射信息
        /// </summary>
        /// <param name="inputDtos">要添加的设备刀具映射DTO信息</param>
        /// <returns>业务操作结果</returns>

        Task<OperationResult> AddEquipmentSparePartTypeMap(params EquipmentSparePartTypeMapInputDto[] inputDtos);
        /// <summary>
        /// 更新设备刀具映射信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的设备刀具映射DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateEquipmentSparePartTypeMap(params EquipmentSparePartTypeMapInputDto[] inputDtos);

        /// <summary>
        /// 物理删除设备刀具映射信息
        /// </summary>
        /// <param name="ids">要删除的设备刀具映射信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteEquipmentSparePartTypeMap(params Guid[] ids);

      
        ///// <summary>
        ///// 逻辑删除设备刀具映射
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDeleteEquipment(params EquipmentSparePartTypeMap[] equiinfo);
        ///// <summary>
        ///// 逻辑还原设备刀具映射
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestoreEquipment(params EquipmentSparePartTypeMap[] equiinfo);
   

        #endregion
    }
}
