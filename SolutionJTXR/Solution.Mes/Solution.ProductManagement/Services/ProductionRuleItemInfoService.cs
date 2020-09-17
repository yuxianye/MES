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
    public class ProductionRuleItemInfoService : IProductionRuleItemInfoContract
    {

        /// <summary>
        /// 配方明细信息实体仓储
        /// </summary>
        public IRepository<ProductionRuleItemInfo, Guid> ProductionRuleItemInfoRepository { get; set; }

        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }

        public IRepository<ProductionProcessInfo, Guid> ProductionProcessInfoRepository { get; set; }

        /// <summary>
        /// 查询配方明细信息
        /// </summary>
        public IQueryable<ProductionRuleItemInfo> ProductionRuleItemInfos
        {
            get { return ProductionRuleItemInfoRepository.Entities; }
        }
        /// <summary>
        /// 增加配方明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params ProductionRuleItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.ProductionRule = ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方不存在,无法保存！");
                }
                dtoData.ProductionProcess = ProductionProcessInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionProcess_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionProcess, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的工序不存在,无法保存！");
                }
                if (Equals(dtoData.ProductionProcessOrder, null) || dtoData.ProductionProcessOrder <= 0)
                {
                    return new OperationResult(OperationResultType.Error, "请正确输入工序排序！");
                }
                if (Equals(dtoData.Duration, null) || dtoData.Duration < 0)
                {
                    return new OperationResult(OperationResultType.Error, "配方时长应大于0，无法保存！");
                }
                var list = ProductionRuleItemInfoRepository.Entities.Where(m => m.ProductionRule.Id == dtoData.ProductionRule_Id && m.ProductionProcessOrder == dtoData.ProductionProcessOrder).ToList();
                int count = list.Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, $"该排序的工序'{list[0].ProductionProcess.ProductionProcessName}'已存在，不能重复添加。");
                }
            }
            ProductionRuleItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleItemInfoRepository.InsertAsync(inputDtos);
            ProductionRuleItemInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<ProductionRuleItemInfo, bool>> predicate, Guid id)
        {
            return ProductionRuleItemInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除配方明细信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            ProductionRuleItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleItemInfoRepository.DeleteAsync(ids);
            ProductionRuleItemInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新配方明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params ProductionRuleItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.ProductionRule = ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方不存在,无法保存！");
                }
                dtoData.ProductionProcess = ProductionProcessInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionProcess_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionProcess, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的工序不存在,无法保存！");
                }
                if (Equals(dtoData.ProductionProcessOrder, null) || dtoData.ProductionProcessOrder <= 0)
                {
                    return new OperationResult(OperationResultType.Error, "请正确输入工序排序！");
                }
                if (Equals(dtoData.Duration, null) || dtoData.Duration < 0)
                {
                    return new OperationResult(OperationResultType.Error, "配方时长应大于0，无法保存！");
                }
                var list = ProductionRuleItemInfoRepository.Entities.Where(m => m.ProductionRule.Id == dtoData.ProductionRule_Id && m.ProductionProcessOrder == dtoData.ProductionProcessOrder && m.Id != dtoData.Id).ToList();
                int count = list.Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, $"该排序的工序'{list[0].ProductionProcess.ProductionProcessName}'已存在，不能重复添加。");
                }
            }
            ProductionRuleItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleItemInfoRepository.UpdateAsync(inputDtos);
            ProductionRuleItemInfoRepository.UnitOfWork.Commit();
            return result;
        }

    }
}
