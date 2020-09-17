using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.ProductManagement.Contracts;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.ProductManagement.Services
{
    public class ProductionRuleStatusInfoService : IProductionRuleStatusInfoContract
    {
        /// <summary>
        /// 配方状态信息实体仓储
        /// </summary>
        public IRepository<ProductionRuleStatusInfo, Guid> ProductionRuleStatusInfoRepository { get; set; }

        public IRepository<ProductTypeInfo, Guid> ProductTypeInfoRepository { get; set; }
        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }

        /// <summary>
        /// 查询配方状态信息
        /// </summary>
        public IQueryable<ProductionRuleStatusInfo> ProductionRuleStatusInfos
        {
            get { return ProductionRuleStatusInfoRepository.Entities; }
        }
        /// <summary>
        /// 增加配方状态
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params ProductionRuleStatusInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductionRuleStatusCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方状态编号！");
                if (string.IsNullOrEmpty(dtoData.ProductionRuleStatusName))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方状态名称！");
                if (ProductionRuleStatusInfoRepository.CheckExists(x => x.ProductionRuleStatusCode == dtoData.ProductionRuleStatusCode))
                    return new OperationResult(OperationResultType.Error, "该配方状态编号已存在，无法保存！");
                if (ProductionRuleStatusInfoRepository.CheckExists(x => x.ProductionRuleStatusName == dtoData.ProductionRuleStatusName))
                    return new OperationResult(OperationResultType.Error, "该配方状态名称已存在，无法保存！");
            }
            ProductionRuleStatusInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleStatusInfoRepository.InsertAsync(inputDtos);
            ProductionRuleStatusInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<ProductionRuleStatusInfo, bool>> predicate, Guid id)
        {
            return ProductionRuleStatusInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除配方状态信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count1 = ProductionRuleInfoRepository.Entities.Where(m => m.ProductionRuleStatus.Id == id).Count();
                if (count1 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "配方状态数据关联配方信息，不能被删除。");
                }
            }
            ProductionRuleStatusInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleStatusInfoRepository.DeleteAsync(ids);
            ProductionRuleStatusInfoRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 更新配方状态信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params ProductionRuleStatusInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductionRuleStatusCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方状态编号！");
                if (string.IsNullOrEmpty(dtoData.ProductionRuleStatusName))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方状态名称！");
                if (ProductionRuleStatusInfoRepository.CheckExists(x => x.ProductionRuleStatusCode == dtoData.ProductionRuleStatusCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该配方状态编号已存在，无法保存！");
                if (ProductionRuleStatusInfoRepository.CheckExists(x => x.ProductionRuleStatusName == dtoData.ProductionRuleStatusName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该配方状态名称已存在，无法保存！");
            }
            ProductionRuleStatusInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleStatusInfoRepository.UpdateAsync(inputDtos);
            ProductionRuleStatusInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
