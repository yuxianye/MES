using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.EquipKnifeToolInfo.Dtos;
using Solution.EquipKnifeToolInfo.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EquipKnifeToolInfo.Contracts
{
    public interface IEquKnifeToolInfoContract : IScopeDependency
    {
        #region 刀具信息业务
        /// <summary>
        /// 获取刀具维护状态信息
        /// </summary>
        IQueryable<EquKnifeToolInfo> EquKnifeToolInfos { get; }


        /// <summary>
        /// 检查刀具维护状态信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的刀具信息编号</param>
        /// <returns>刀具信息是否存在</returns>
        bool CheckKnifeToolExists(Expression<Func<EquKnifeToolInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加刀具维护状态信息
        /// </summary>
        /// <param name="inputDtos">要添加的刀具信息DTO信息</param>
        /// <returns>业务操作结果</returns>

        Task<OperationResult> AddKnifeTool(params EquKnifeToolInfoInputDto[] inputDtos);
        /// <summary>
        /// 更新刀具维护状态信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的刀具信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateKnifeTool(params EquKnifeToolInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除刀具维护状态信息
        /// </summary>
        /// <param name="ids">要删除的刀具维护状态信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteKnifeTool(params Guid[] ids);


        ///// <summary>
        ///// 逻辑删除刀具信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDeleteEquipment(params EquKnifeToolInfo[] equiinfo);
        ///// <summary>
        ///// 逻辑还原刀具信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestoreEquipment(params EquKnifeToolInfo[] equiinfo);


        #endregion
    }
}
