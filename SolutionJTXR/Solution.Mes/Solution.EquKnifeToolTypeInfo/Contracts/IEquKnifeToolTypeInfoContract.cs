using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.EquipKnifeToolInfo.Models;
using Solution.EquipKnifeToolInfo.Dtos;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EquipKnifeToolInfo.Contracts
{
    public interface IKnifeToolTypeInfoContract : IScopeDependency
    {
        #region 刀具类别类别信息业务
        /// <summary>
        /// 获取刀具类别查询数据集
        /// </summary>
        IQueryable<KnifeToolTypeInfo> KnifeToolTypeInfos { get; }

        #region ww 注释：该方法刀具类别信息管理中可能不需要，后续需要的话，添加
        /// <summary>
        /// 检查刀具类别信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的刀具类别信息编号</param>
        /// <returns>刀具类别信息是否存在</returns>
        bool CheckKnifeToolTypeExists(Expression<Func<KnifeToolTypeInfo, bool>> predicate, Guid id);
        #endregion

        /// <summary>
        /// 添加刀具类别信息
        /// </summary>
        /// <param name="inputDtos">要添加的刀具类别信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddKnifeToolType(params KnifeToolTypeInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新刀具类别信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的刀具类别信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateKnifeToolType(params KnifeToolTypeInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除刀具类别信息
        /// </summary>
        /// <param name="ids">要删除的刀具类别信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteKnifeToolType(params Guid[] ids);


        ///// <summary>
        ///// 逻辑删除刀具类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDeleteKnifeToolType(params KnifeToolTypeInfo[] equiinfo);
        ///// <summary>
        ///// 逻辑还原刀具类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestoreKnifeToolType(params KnifeToolTypeInfo[] equiinfo);


        #endregion
    }
}
