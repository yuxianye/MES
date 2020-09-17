using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.PlanManagement.Dtos;
using Solution.PlanManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.PlanManagement.Contracts
{
    public interface IPlanEquipmentRequirementInfoContract : IScopeDependency
    {
        #region 工序设备需求信息业务
        /// <summary>
        /// 获取工序设备需求查询数据集
        /// </summary>
        IQueryable<PlanEquipmentRequirementInfo> PlanEquipmentRequirementInfos { get; }

        /// <summary>
        /// 检查组工序设备需求信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的工序设备需求编号</param>
        /// <returns>工序设备需求是否存在</returns>
        bool CheckExists(Expression<Func<PlanEquipmentRequirementInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加工序设备需求
        /// </summary>
        /// <param name="inputDtos">要添加的工序设备需求DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params PlanEquipmentRequirementInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新工序设备需求信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的工序设备需求DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params PlanEquipmentRequirementInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除工序设备需求
        /// </summary>
        /// <param name="ids">要删除的工序设备需求Id</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        ///// <summary>
        ///// 逻辑删除工序设备需求
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params PlanEquipmentRequirementInfo[] enterinfo);
        ///// <summary>
        ///// 逻辑还原工序设备需求
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params PlanEquipmentRequirementInfo[] enterinfo);


        #endregion
    }
}
