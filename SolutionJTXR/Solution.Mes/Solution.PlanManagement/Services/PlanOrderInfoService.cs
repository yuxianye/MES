using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.PlanManagement.Contracts;
using Solution.PlanManagement.Dtos;
using Solution.PlanManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.PlanManagement.Services
{
    public class PlanOrderInfoService : IPlanOrderInfoContract
    {
        /// <summary>
        /// 订单信息实体仓储
        /// </summary>
        public IRepository<PlanOrderInfo, Guid> PlanOrderInfoRepository { get; set; }

        public IRepository<PlanOrderItemInfo, Guid> PlanOrderItemInfoRepository { get; set; }

        /// <summary>
        /// 查询订单信息
        /// </summary>
        public IQueryable<PlanOrderInfo> PlanOrderInfos
        {
            get { return PlanOrderInfoRepository.Entities; }
        }


        /// <summary>
        /// 增加订单信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params PlanOrderInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.OrderCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写订单编号！");
                if (string.IsNullOrEmpty(dtoData.OrderName))
                    return new OperationResult(OperationResultType.Error, "请正确填写订单名称！");
                if (PlanOrderInfoRepository.CheckExists(x => x.OrderCode == dtoData.OrderCode))
                    return new OperationResult(OperationResultType.Error, "该订单编号已存在，无法保存！");
                if (PlanOrderInfoRepository.CheckExists(x => x.OrderName == dtoData.OrderName))
                    return new OperationResult(OperationResultType.Error, "该订单名称已存在，无法保存！");
                if (Equals(dtoData.DeliveryTime, null) || Equals(dtoData.ExpectedFinishTime, null))
                    return new OperationResult(OperationResultType.Error, "交货时间或预计完成时间不应为空，无法保存！");
                if (dtoData.ExpectedFinishTime < DateTime.Now)
                    return new OperationResult(OperationResultType.Error, "预期完成时间小于系统当前时间，无法保存！");
                if (dtoData.DeliveryTime < dtoData.ExpectedFinishTime)
                    return new OperationResult(OperationResultType.Error, "预期完成时间大于交货时间，无法保存！");
            }
            PlanOrderInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanOrderInfoRepository.InsertAsync(inputDtos);
            PlanOrderInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<PlanOrderInfo, bool>> predicate, Guid id)
        {
            return PlanOrderInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除订单信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count = PlanOrderItemInfoRepository.Entities.Where(m => m.Order.Id == id).Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, "订单数据关联订单明细信息，不能被删除。");
                }
            }
            PlanOrderInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanOrderInfoRepository.DeleteAsync(ids);
            PlanOrderInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新订单信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params PlanOrderInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.OrderCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写订单编号！");
                if (string.IsNullOrEmpty(dtoData.OrderName))
                    return new OperationResult(OperationResultType.Error, "请正确填写订单名称！");
                if (PlanOrderInfoRepository.CheckExists(x => x.OrderCode == dtoData.OrderCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该订单编号已存在，无法保存！");
                if (PlanOrderInfoRepository.CheckExists(x => x.OrderName == dtoData.OrderName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该订单名称已存在，无法保存！");
                if (Equals(dtoData.DeliveryTime, null) || Equals(dtoData.ExpectedFinishTime, null))
                    return new OperationResult(OperationResultType.Error, "交货时间或预计完成时间不应为空，无法保存！");
                if (dtoData.ExpectedFinishTime < DateTime.Now)
                    return new OperationResult(OperationResultType.Error, "预期完成时间小于系统当前时间，无法保存！");
                if (dtoData.DeliveryTime < dtoData.ExpectedFinishTime)
                    return new OperationResult(OperationResultType.Error, "预期完成时间大于交货时间，无法保存！");
            }
            PlanOrderInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanOrderInfoRepository.UpdateAsync(inputDtos);
            PlanOrderInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
