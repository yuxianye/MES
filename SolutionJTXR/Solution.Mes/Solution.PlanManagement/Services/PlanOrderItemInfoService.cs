using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.PlanManagement.Contracts;
using Solution.PlanManagement.Dtos;
using Solution.PlanManagement.Models;
using Solution.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.PlanManagement.Services
{
    public class PlanOrderItemInfoService : IPlanOrderItemInfoContract
    {
        /// <summary>
        /// 订单明细信息实体仓储
        /// </summary>
        public IRepository<PlanOrderItemInfo, Guid> PlanOrderItemInfoRepository { get; set; }

        public IRepository<PlanOrderInfo, Guid> PlanOrderInfoRepository { get; set; }

        public IRepository<ProductInfo, Guid> ProductInfoRepository { get; set; }
        public IRepository<PlanProductionScheduleInfo, Guid> PlanProductionScheduleInfoRepository { get; set; }

        /// <summary>
        /// 查询订单明细信息
        /// </summary>
        public IQueryable<PlanOrderItemInfo> PlanOrderItemInfos
        {
            get { return PlanOrderItemInfoRepository.Entities; }
        }
        /// <summary>
        /// 增加产品明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params PlanOrderItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.OrderItemCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写订单明细编号！");
                if (string.IsNullOrEmpty(dtoData.OrderItemName))
                    return new OperationResult(OperationResultType.Error, "请正确填写订单明细名称！");
                if (PlanOrderItemInfoRepository.CheckExists(x => x.OrderItemCode == dtoData.OrderItemCode))
                    return new OperationResult(OperationResultType.Error, "该订单明细编号已存在，无法保存！");
                if (PlanOrderItemInfoRepository.CheckExists(x => x.OrderItemName == dtoData.OrderItemName))
                    return new OperationResult(OperationResultType.Error, "该订单明细名称已存在，无法保存！");
                dtoData.Order = PlanOrderInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Order_Id).FirstOrDefault();
                if (Equals(dtoData.Order, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的主订单不存在,无法保存！");
                }
                dtoData.Product = ProductInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Product_Id).FirstOrDefault();
                if (Equals(dtoData.Product, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的产品不存在,无法保存！");
                }
            }
            PlanOrderItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanOrderItemInfoRepository.InsertAsync(inputDtos);
            PlanOrderItemInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<PlanOrderItemInfo, bool>> predicate, Guid id)
        {
            return PlanOrderItemInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除订单明细信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count = PlanProductionScheduleInfoRepository.Entities.Where(m => m.OrderItem.Id == id).Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, "订单明细数据关联生产计划信息，不能被删除。");
                }
            }
            PlanOrderItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanOrderItemInfoRepository.DeleteAsync(ids);
            PlanOrderItemInfoRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 更新订单明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params PlanOrderItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.OrderItemCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写订单明细编号！");
                if (string.IsNullOrEmpty(dtoData.OrderItemName))
                    return new OperationResult(OperationResultType.Error, "请正确填写订单明细名称！");
                if (PlanOrderItemInfoRepository.CheckExists(x => x.OrderItemCode == dtoData.OrderItemCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该订单明细编号已存在，无法保存！");
                if (PlanOrderItemInfoRepository.CheckExists(x => x.OrderItemName == dtoData.OrderItemName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该订单明细名称已存在，无法保存！");
                dtoData.Order = PlanOrderInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Order_Id).FirstOrDefault();
                if (Equals(dtoData.Order, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的主订单不存在,无法保存！");
                }
                dtoData.Product = ProductInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Product_Id).FirstOrDefault();
                if (Equals(dtoData.Product, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的产品不存在,无法保存！");
                }
            }
            PlanOrderItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanOrderItemInfoRepository.UpdateAsync(inputDtos);
            PlanOrderItemInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
