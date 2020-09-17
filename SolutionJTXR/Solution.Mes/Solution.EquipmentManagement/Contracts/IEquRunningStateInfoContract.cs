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
    public interface IEquRunningStateInfoContract : IScopeDependency
    {
        #region 设备信息业务
        /// <summary>
        /// 获取设备运行状态信息
        /// </summary>
        IQueryable<EquRunningStateInfo> EquRunningStateInfos { get; }


        /// <summary>
        /// 检查设备运行状态信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备信息编号</param>
        /// <returns>设备信息是否存在</returns>
        bool CheckEquRunningStateInfoExists(Expression<Func<EquRunningStateInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加设备运行状态信息
        /// </summary>
        /// <param name="inputDtos">要添加的设备信息DTO信息</param>
        /// <returns>业务操作结果</returns>

        Task<OperationResult> AddEquRunningStateInfo(params EquRunningStateInfoInputDto[] inputDtos);
        /// <summary>
        /// 更新设备运行状态信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的设备信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateEquRunningStateInfo(params EquRunningStateInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除设备运行状态信息
        /// </summary>
        /// <param name="ids">要删除的设备运行状态信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteEquRunningStateInfo(params Guid[] ids);


        ///// <summary>
        /////设备运行状态信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDeleteEquipment(params EquRunningStateInfo[] equiinfo);
        ///// <summary>
        ///// 逻辑还原设备运行状态
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestoreEquipment(params EquRunningStateInfo[] equiinfo);


        #endregion
    }
}
