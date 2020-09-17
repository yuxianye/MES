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
    /// <summary>
    /// 备件类别信息契约接口
    /// </summary>
    public interface IEquSparePartTypeInfoContract : IScopeDependency
    {
        #region 备件类别信息业务

        /// <summary>
        /// 获取备件类别信息查询数据集 《注意拼写单复数。》
        /// </summary>
        IQueryable<EquSparePartTypeInfo> EquSparePartTypeInfos { get; }

        /// <summary>
        /// 检查备件类别信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的备件类别信息编号</param>
        /// <returns>备件类别信息是否存在</returns>
        bool CheckEquSparePartTypeExists(Expression<Func<EquSparePartTypeInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加备件类别信息
        /// </summary>
        /// <param name="inputDtos">要添加的备件类别信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddEquSparePartType(params EquSparePartTypeInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新备件类别信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的备件类别信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateEquSparePartType(params EquSparePartTypeInfoInputDto[] inputDtos);

        ///// <summary>
        ///// 逻辑删除备件类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDeleteEnterprises(params EquSparePartTypeInfo[] enterinfo);

        /// <summary>
        /// 物理删除备件类别信息
        /// </summary>
        /// <param name="ids">要删除的备件类别信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteEquSparePartType(params Guid[] ids);

        ///// <summary>
        ///// 逻辑还原备件类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestoreEnterprises(params EquSparePartTypeInfo[] enterinfo);

        #endregion

    }
}
