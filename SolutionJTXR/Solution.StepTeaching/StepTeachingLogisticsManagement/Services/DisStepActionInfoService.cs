using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EnterpriseInformation.Models;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
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
    /// 分步操作信息服务
    /// </summary>
    public class DisStepActionInfoService : IDisStepActionInfoContract
    {

        /// <summary>
        /// 分步操作信息实体仓储
        /// </summary>
        public IRepository<DisStepActionInfo, Guid> DisStepActionInfoRepository { get; set; }
        //

        /// <summary>
        /// 查询仓库类型信息
        /// </summary>
        public IQueryable<DisStepActionInfo> DisStepActionInfos
        {
            get { return DisStepActionInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<DisStepActionInfo, bool>> predicate, Guid id)
        {
            return DisStepActionInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加分步操作信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params DisStepActionInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.StepActionCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写分步操作编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.StepActionName))
                    return new OperationResult(OperationResultType.Error, "请正确填写分步操作名称，该组数据不被存储。");
                //
                if (DisStepActionInfoRepository.CheckExists(x => x.StepActionCode == dtoData.StepActionCode))
                    return new OperationResult(OperationResultType.Error, $"分步操作编号 {dtoData.StepActionCode} 的数据已存在，该组数据不被存储。");
                if (DisStepActionInfoRepository.CheckExists(x => x.StepActionName == dtoData.StepActionName))
                    return new OperationResult(OperationResultType.Error, $"分步操作名称 {dtoData.StepActionName} 的数据已存在，该组数据不被存储。");
            }
            DisStepActionInfoRepository.UnitOfWork.BeginTransaction();
            var result = await DisStepActionInfoRepository.InsertAsync(inputDtos);
            DisStepActionInfoRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新分步操作信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params DisStepActionInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (DisStepActionInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.StepActionCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写分步操作编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.StepActionName))
                    return new OperationResult(OperationResultType.Error, "请正确填写分步操作名称，该组数据不被存储。");
                //
                if (DisStepActionInfoRepository.CheckExists(x => x.StepActionCode == dtoData.StepActionCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"分步操作编号 {dtoData.StepActionCode} 的数据已存在，该组数据不被存储。");
                if (DisStepActionInfoRepository.CheckExists(x => x.StepActionName == dtoData.StepActionName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"分步操作名称 {dtoData.StepActionName} 的数据已存在，该组数据不被存储。");
            }
            //
            DisStepActionInfoRepository.UnitOfWork.BeginTransaction();
            var result = await DisStepActionInfoRepository.UpdateAsync(inputDtos);
            DisStepActionInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除分步操作信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            DisStepActionInfoRepository.UnitOfWork.BeginTransaction();
            var result = await DisStepActionInfoRepository.DeleteAsync(ids);
            DisStepActionInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
