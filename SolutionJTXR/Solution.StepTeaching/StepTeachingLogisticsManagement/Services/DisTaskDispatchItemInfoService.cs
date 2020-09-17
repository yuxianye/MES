using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using Solution.EquipmentManagement.Models;
using Solution.StepTeachingDispatchManagement.Contracts;
using Solution.StepTeachingDispatchManagement.Dtos;
using Solution.StepTeachingDispatchManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.StepTeachingDispatchManagement.Services
{
    /// <summary>
    /// 分步教学任务调度明细信息服务
    /// </summary>
    public class DisTaskDispatchItemInfoService : IDisTaskDispatchItemInfoContract
    {

        /// <summary>
        /// 分步教学任务调度明细信息实体仓储
        /// </summary>
        public IRepository<DisTaskDispatchItemInfo, Guid> DisTaskDispatchItemRepository { get; set; }
        /// <summary>
        /// 分步教学任务调度主信息实体仓储
        /// </summary>
        public IRepository<DisTaskDispatchInfo, Guid> DisTaskDispatchRepository { get; set; }
        /// <summary>
        /// 设备动作信息实体仓储
        /// </summary>
        public IRepository<EquFactoryInfo, Guid> EquipmentActionInfoRepository { get; set; }
        /// <summary>
        /// 设备名称实体仓储
        /// </summary>
        public IRepository<EquEquipmentInfo, Guid> EquipmentInfoRepository { get; set; }

        /// <summary>
        /// 查询分步教学任务调度明细信息
        /// </summary>
        public IQueryable<DisTaskDispatchItemInfo> DisTaskDispatchItemInfos
        {
            get { return DisTaskDispatchItemRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<DisTaskDispatchItemInfo, bool>> predicate, Guid id)
        {
            return DisTaskDispatchItemRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加分步教学任务调度明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params DisTaskDispatchItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.TaskItemCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写任务明细编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseTypeName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库类型名称，该组数据不被存储。");
                //
                if (DisTaskDispatchItemRepository.CheckExists(x => x.TaskItemCode == dtoData.TaskItemCode))
                    return new OperationResult(OperationResultType.Error, $"任务明细编号 {dtoData.TaskItemCode} 的数据已存在，该组数据不被存储。");
                //if (DisTaskDispatchItemRepository.CheckExists(x => x.WareHouseTypeName == dtoData.WareHouseTypeName))
                //    return new OperationResult(OperationResultType.Error, $"仓库类型名称 {dtoData.WareHouseTypeName} 的数据已存在，该组数据不被存储。");
            }
            DisTaskDispatchItemRepository.UnitOfWork.BeginTransaction();
            var result = await DisTaskDispatchItemRepository.InsertAsync(inputDtos);
            DisTaskDispatchItemRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新分步教学任务调度明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params DisTaskDispatchItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (DisTaskDispatchItemInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.TaskItemCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写任务明细编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseTypeName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库类型名称，该组数据不被存储。");
                //
                if (DisTaskDispatchItemRepository.CheckExists(x => x.TaskItemCode == dtoData.TaskItemCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"任务明细编号 {dtoData.TaskItemCode} 的数据已存在，该组数据不被存储。");
                //if (DisTaskDispatchItemRepository.CheckExists(x => x.WareHouseTypeName == dtoData.WareHouseTypeName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库类型名称 {dtoData.WareHouseTypeName} 的数据已存在，该组数据不被存储。");
            }
            //
            DisTaskDispatchItemRepository.UnitOfWork.BeginTransaction();
            var result = await DisTaskDispatchItemRepository.UpdateAsync(inputDtos);
            DisTaskDispatchItemRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除分步教学任务调度明细信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            DisTaskDispatchItemRepository.UnitOfWork.BeginTransaction();
            var result = await DisTaskDispatchItemRepository.DeleteAsync(ids);
            DisTaskDispatchItemRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
