using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
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
    /// 分步教学任务调度主信息服务
    /// </summary>
    public class DisTaskDispatchInfoService : IDisTaskDispatchInfoContract
    {

        /// <summary>
        /// 分步教学任务调度主信息实体仓储
        /// </summary>
        public IRepository<DisTaskDispatchInfo, Guid> DisTaskDispatchRepository { get; set; }

        //

        /// <summary>
        /// 查询分步教学任务调度主信息
        /// </summary>
        public IQueryable<DisTaskDispatchInfo> DisTaskDispatchInfos
        {
            get { return DisTaskDispatchRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<DisTaskDispatchInfo, bool>> predicate, Guid id)
        {
            return DisTaskDispatchRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加分步教学任务调度主信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params DisTaskDispatchInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.TaskCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写任务编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseTypeName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库类型名称，该组数据不被存储。");
                //
                if (DisTaskDispatchRepository.CheckExists(x => x.TaskCode == dtoData.TaskCode))
                    return new OperationResult(OperationResultType.Error, $"任务编号 {dtoData.TaskCode} 的数据已存在，该组数据不被存储。");
                //if (DisTaskDispatchRepository.CheckExists(x => x.WareHouseTypeName == dtoData.WareHouseTypeName))
                //    return new OperationResult(OperationResultType.Error, $"仓库类型名称 {dtoData.WareHouseTypeName} 的数据已存在，该组数据不被存储。");
            }
            DisTaskDispatchRepository.UnitOfWork.BeginTransaction();
            var result = await DisTaskDispatchRepository.InsertAsync(inputDtos);
            DisTaskDispatchRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新分步教学任务调度主信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params DisTaskDispatchInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (DisTaskDispatchInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.TaskCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写任务编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseTypeName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库类型名称，该组数据不被存储。");
                //
                if (DisTaskDispatchRepository.CheckExists(x => x.TaskCode == dtoData.TaskCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"任务编号 {dtoData.TaskCode} 的数据已存在，该组数据不被存储。");
                //if (DisTaskDispatchRepository.CheckExists(x => x.WareHouseTypeName == dtoData.WareHouseTypeName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库类型名称 {dtoData.WareHouseTypeName} 的数据已存在，该组数据不被存储。");
            }
            //
            DisTaskDispatchRepository.UnitOfWork.BeginTransaction();
            var result = await DisTaskDispatchRepository.UpdateAsync(inputDtos);
            DisTaskDispatchRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除分步教学任务调度主信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            DisTaskDispatchRepository.UnitOfWork.BeginTransaction();
            var result = await DisTaskDispatchRepository.DeleteAsync(ids);
            DisTaskDispatchRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
