using OSharp.Core.Data;
using OSharp.Core.Mapping;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Services
{
    /// <summary>
    /// 业务点表服务
    /// </summary>
    public class BusinessNodeService : IBusinessNodeContract
    {
        /// <summary>
        /// 业务点表仓储
        /// </summary>
        public IRepository<BusinessNode, Guid> BusinessNodeRepository { get; set; }

        /// <summary>
        /// 业务点表map仓储
        /// </summary>
        public IRepository<ProductionProcessEquipmentBusinessNodeMap, Guid> ProductionProcessEquipmentBusinessNodeMapRepository { get; set; }

        /// <summary>
        /// 获取业务点表查询数据集
        /// </summary>
        public IQueryable<BusinessNode> BusinessNodes => BusinessNodeRepository.Entities;

        /// <summary>
        /// 添加业务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params BusinessNodeInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (BusinessNodeInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.BusinessName))
                    return new OperationResult(OperationResultType.Error, "请正确填写业务名称，业务名称不能为空。");
                if (BusinessNodeRepository.CheckExists(x => x.BusinessName == dtoData.BusinessName))
                    return new OperationResult(OperationResultType.Error, $"业务点名称 {dtoData.BusinessName} 的数据已存在，请使用其他名称。");
            }
            var result = await BusinessNodeRepository.InsertAsync(inputDtos);
            return result;
        }

        /// <summary>
        /// 更新业务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params BusinessNodeInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (BusinessNodeInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.BusinessName))
                    return new OperationResult(OperationResultType.Error, "请正确填写业务名称，业务名称不能为空。");
                if (BusinessNodeRepository.CheckExists(x => x.BusinessName == dtoData.BusinessName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"业务点名称 {dtoData.BusinessName} 的数据已被使用，请使用其他名称。");
            }
            var result = await BusinessNodeRepository.UpdateAsync(inputDtos);
            return result;
        }

        /// <summary>
        /// 检查业务点信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的业务点编号</param>
        /// <returns>业务点数据是否存在</returns>
        public bool CheckBusinessNodeExists(Expression<Func<BusinessNode, bool>> predicate, Guid id) => BusinessNodeRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除业务点数据
        /// </summary>
        /// <param name="ids">要删除的业务点数据编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteBusinessNodes(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {

                if (ProductionProcessEquipmentBusinessNodeMapRepository.CheckExists(a => a.BusinessNode.Id == id))
                {
                    return new OperationResult(OperationResultType.Error, $"该业务点数据已被其他业务使用不能删除。");
                }
            }
            var result = await BusinessNodeRepository.DeleteAsync(ids);
            return result;
        }

    }
}
