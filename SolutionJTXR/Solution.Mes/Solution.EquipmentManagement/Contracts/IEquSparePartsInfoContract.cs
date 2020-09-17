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
    public interface IEquSparePartsInfoContract : IScopeDependency
    {
        #region 设备信息业务
        /// <summary>
        /// 获取设备备件信息
        /// </summary>
        IQueryable<EquSparePartsInfo> EquSparePartsInfos { get; }


        /// <summary>
        /// 检查设备备件信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备信息编号</param>
        /// <returns>设备信息是否存在</returns>
        bool CheckEquSparePartsExists(Expression<Func<EquSparePartsInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加设备备件信息
        /// </summary>
        /// <param name="inputDtos">要添加的设备信息DTO信息</param>
        /// <returns>业务操作结果</returns>

        Task<OperationResult> AddEquSpareParts(params EquSparePartsInfoInputDto[] inputDtos);
        /// <summary>
        /// 更新设备备件信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的设备信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateEquSpareParts(params EquSparePartsInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除设备备件信息
        /// </summary>
        /// <param name="ids">要删除的设备备件信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteEquSpareParts(params Guid[] ids);


        ///// <summary>
        ///// 逻辑删除设备信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDeleteEquipment(params EquSparePartsInfo[] equiinfo);
        ///// <summary>
        ///// 逻辑还原设备信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestoreEquipment(params EquSparePartsInfo[] equiinfo);


        #endregion
    }
}
