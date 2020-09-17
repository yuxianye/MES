using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.PlanManagement.Contracts;
using Solution.PlanManagement.Dtos;
using Solution.PlanManagement.Models;
using Solution.ProductManagement.Models;
using Solution.ProductManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.PlanManagement.Services
{
    public class PlanProductionScheduleInfoService : IPlanProductionScheduleInfoContract
    {
        /// <summary>
        /// 生产计划信息实体仓储
        /// </summary>
        public IRepository<PlanProductionScheduleInfo, Guid> PlanProductionScheduleInfoRepository { get; set; }

        public IRepository<ProductInfo, Guid> ProductInfoRepository { get; set; }

        public IRepository<PlanOrderInfo, Guid> PlanOrderInfoRepository { get; set; }

        public IRepository<PlanOrderItemInfo, Guid> PlanOrderItemInfoRepository { get; set; }

        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }

        public IRepository<ProductionRuleItemInfo, Guid> ProductionRuleItemInfoRepository { get; set; }
        public IRepository<ProManufacturingBOMBillItemInfo, Guid> ProManufacturingBOMBillItemInfoRepository { get; set; }
        public IRepository<ProManufacturingBORBillItemInfo, Guid> ProManufacturingBORBillItemInfoRepository { get; set; }
        public IRepository<PlanProductionProcessRequirementInfo, Guid> PlanProductionProcessRequirementInfoRepository { get; set; }
        public IRepository<PlanMaterialRequirementInfo, Guid> PlanMaterialRequirementInfoRepository { get; set; }
        public IRepository<PlanEquipmentRequirementInfo, Guid> PlanEquipmentRequirementInfoRepository { get; set; }

        /// <summary>
        /// 查询生产计划信息
        /// </summary>
        public IQueryable<PlanProductionScheduleInfo> PlanProductionScheduleInfos
        {
            get { return PlanProductionScheduleInfoRepository.Entities; }
        }

        /// <summary>
        /// 增加生产计划
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params PlanProductionScheduleInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ScheduleCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写生产计划编号！");
                if (string.IsNullOrEmpty(dtoData.ScheduleName))
                    return new OperationResult(OperationResultType.Error, "请正确填写生产计划名称！");
                if (PlanProductionScheduleInfoRepository.CheckExists(x => x.ScheduleCode == dtoData.ScheduleCode))
                    return new OperationResult(OperationResultType.Error, "该生产计划编号已存在，无法保存！");
                if (PlanProductionScheduleInfoRepository.CheckExists(x => x.ScheduleName == dtoData.ScheduleName))
                    return new OperationResult(OperationResultType.Error, "该生产计划名称已存在，无法保存！");
                dtoData.OrderItem = PlanOrderItemInfoRepository.TrackEntities.Where(m => m.Id == dtoData.OrderItem_Id).FirstOrDefault();
                if (Equals(dtoData.OrderItem, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的订单明细不存在,无法保存！");
                }
                if (dtoData.OrderItem.RemainQuantity < dtoData.Quantity)
                {
                    return new OperationResult(OperationResultType.Error, $"剩余排产数量{dtoData.OrderItem.RemainQuantity}小于计划排产数量{dtoData.Quantity},无法保存！");
                }
                dtoData.OrderItem.RemainQuantity -= dtoData.Quantity;
                dtoData.OrderItem.OrderState = 2;
                dtoData.OrderItem.Order.OrderState = 2;
                dtoData.ProductionRule = ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方不存在,无法保存！");
                }
                if (Equals(dtoData.StartTime, null) || Equals(dtoData.EndTime))
                {
                    return new OperationResult(OperationResultType.Error, "开始时间或结束时间为空，无法保存！");
                }
                if (dtoData.StartTime < DateTime.Now)
                {
                    return new OperationResult(OperationResultType.Error, "开始时间应大于系统当前时间，无法保存！");
                }
                if (dtoData.StartTime >= dtoData.EndTime)
                {
                    return new OperationResult(OperationResultType.Error, "开始时间应小于结束时间，无法保存！");
                }
            }
            PlanProductionScheduleInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanProductionScheduleInfoRepository.InsertAsync(inputDtos);
            PlanProductionScheduleInfoRepository.UnitOfWork.Commit();
            return result;

        }

        /// <summary>
        /// 增加工单
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddWorkList(params PlanProductionScheduleInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            var result1 = new OperationResult();
            var result2 = new OperationResult();
            var result3 = new OperationResult();
            PlanProductionScheduleInfoRepository.UnitOfWork.BeginTransaction();
            foreach (var dtoData in inputDtos)
            {
                dtoData.OrderItem = PlanOrderItemInfoRepository.TrackEntities.Where(m => m.Id == dtoData.OrderItem_Id).FirstOrDefault();
                if (Equals(dtoData.OrderItem, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的订单明细不存在,不能生成工单。");
                }
                dtoData.ProductionRule = ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方不存在,不能生成工单。");
                }
                if (dtoData.ScheduleStatus != 1)
                {
                    return new OperationResult(OperationResultType.Error, "只有生产计划状态为未开始的才能生成工单。");
                }
                int count1 = ProductionRuleItemInfoRepository.Entities.Where(m => m.ProductionRule.Id == dtoData.ProductionRule.Id).Count();
                if (count1 > 0)
                {
                    var ruleitemlist = ProductionRuleItemInfoRepository.TrackEntities.Where(m => m.ProductionRule.Id == dtoData.ProductionRule.Id).OrderBy(m => m.ProductionProcessOrder).ToList();
                    PlanProductionProcessRequirementInfoInputDto[] processRequires = new PlanProductionProcessRequirementInfoInputDto[count1];
                    for (int i = 0; i < count1; i++)
                    {
                        PlanProductionProcessRequirementInfoInputDto info = new PlanProductionProcessRequirementInfoInputDto();
                        info.Id = CombHelper.NewComb();
                        info.ProductionProcess_Id = ruleitemlist[i].ProductionProcess.Id;
                        info.ProductionSchedule_Id = dtoData.Id;
                        info.ProductionProcess = ruleitemlist[i].ProductionProcess;
                        info.ProductionProcessOrder = ruleitemlist[i].ProductionProcessOrder;
                        if (i == 0)
                        {
                            info.StartTime = dtoData.StartTime;
                        }
                        else
                        {
                            info.StartTime = processRequires[i - 1].EndTime.AddSeconds(10);
                        }
                        info.EndTime = info.StartTime.AddSeconds(double.Parse(ruleitemlist[i].Duration.ToString()));
                        info.ProductionSchedule = PlanProductionScheduleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Id).FirstOrDefault();
                        info.Duration = ruleitemlist[i].Duration;
                        info.DurationUnit = ruleitemlist[i].DurationUnit;
                        info.Remark = ruleitemlist[i].Remark;
                        info.CreatedTime = dtoData.CreatedTime;
                        info.CreatorUserId = dtoData.CreatorUserId;
                        info.LastUpdatedTime = dtoData.LastUpdatedTime;
                        info.LastUpdatorUserId = dtoData.LastUpdatorUserId;
                        processRequires[i] = info;
                    }
                    // PlanProductionProcessRequirementInfoRepository.UnitOfWork.BeginTransaction();
                    result1 = await PlanProductionProcessRequirementInfoRepository.InsertAsync(processRequires);
                    //  PlanProductionProcessRequirementInfoRepository.UnitOfWork.Commit();
                }
                int count2 = ProManufacturingBOMBillItemInfoRepository.Entities.Where(m => m.ProManufacturingBill.ProductionRule.Id == dtoData.ProductionRule.Id).Count();
                if (count2 > 0)
                {
                    PlanMaterialRequirementInfoInputDto[] materialRequires = new PlanMaterialRequirementInfoInputDto[count2];
                    var bomlist = ProManufacturingBOMBillItemInfoRepository.TrackEntities.Where(m => m.ProManufacturingBill.ProductionRule.Id == dtoData.ProductionRule.Id).ToList();
                    ProManufacturingBOMBillItemInfoInputDto[] bomRequires = new ProManufacturingBOMBillItemInfoInputDto[count2];
                    for (int i1 = 0; i1 < count2; i1++)
                    {
                        PlanMaterialRequirementInfoInputDto info1 = new PlanMaterialRequirementInfoInputDto();
                        info1.Id = CombHelper.NewComb();
                        info1.Material = bomlist[i1].Material;
                        var id = bomlist[i1].ProductionProcess.Id;
                        info1.ProductionProcessRequirement = PlanProductionProcessRequirementInfoRepository.TrackEntities.Where(m => m.ProductionProcess.Id == id && m.ProductionSchedule.Id == dtoData.Id).FirstOrDefault();
                        info1.RequireQuantity = bomlist[i1].Quantity * dtoData.Quantity;
                        info1.Remark = bomlist[i1].Remark;
                        info1.CreatedTime = dtoData.CreatedTime;
                        info1.CreatorUserId = dtoData.CreatorUserId;
                        info1.LastUpdatedTime = dtoData.LastUpdatedTime;
                        info1.LastUpdatorUserId = dtoData.LastUpdatorUserId;
                        materialRequires[i1] = info1;
                    }
                    // PlanMaterialRequirementInfoRepository.UnitOfWork.BeginTransaction();
                    result2 = await PlanMaterialRequirementInfoRepository.InsertAsync(materialRequires);
                    // PlanMaterialRequirementInfoRepository.UnitOfWork.Commit();
                }
                int count3 = ProManufacturingBORBillItemInfoRepository.Entities.Where(m => m.ProManufacturingBill.ProductionRule.Id == dtoData.ProductionRule.Id).Count();
                if (count3 > 0)
                {
                    PlanEquipmentRequirementInfoInputDto[] equipmentRequires = new PlanEquipmentRequirementInfoInputDto[count3];
                    var borlist = ProManufacturingBORBillItemInfoRepository.TrackEntities.Where(m => m.ProManufacturingBill.ProductionRule.Id == dtoData.ProductionRule.Id).ToList();
                    ProManufacturingBORBillItemInfoInputDto[] borRequires = new ProManufacturingBORBillItemInfoInputDto[count3];
                    for (int i2 = 0; i2 < count3; i2++)
                    {
                        PlanEquipmentRequirementInfoInputDto info2 = new PlanEquipmentRequirementInfoInputDto();
                        info2.Id = CombHelper.NewComb();
                        info2.Equipment = borlist[i2].Equipment;
                        var id = borlist[i2].ProductionProcess.Id;
                        info2.ProductionProcessRequirement = PlanProductionProcessRequirementInfoRepository.TrackEntities.Where(m => m.ProductionProcess.Id == id && m.ProductionSchedule.Id == dtoData.Id).FirstOrDefault();
                        info2.RequireQuantity = borlist[i2].Quantity;
                        info2.Remark = borlist[i2].Remark;
                        info2.CreatedTime = dtoData.CreatedTime;
                        info2.CreatorUserId = dtoData.CreatorUserId;
                        info2.LastUpdatedTime = dtoData.LastUpdatedTime;
                        info2.LastUpdatorUserId = dtoData.LastUpdatorUserId;
                        equipmentRequires[i2] = info2;
                    }
                    // PlanEquipmentRequirementInfoRepository.UnitOfWork.BeginTransaction();
                    result3 = await PlanEquipmentRequirementInfoRepository.InsertAsync(equipmentRequires);
                    // PlanEquipmentRequirementInfoRepository.UnitOfWork.Commit();
                }
                if (result1.Successed && result2.Successed && result1.Successed)
                {
                    dtoData.ScheduleStatus = 2;
                }
            }
            // PlanProductionScheduleInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanProductionScheduleInfoRepository.UpdateAsync(inputDtos);
            PlanProductionScheduleInfoRepository.UnitOfWork.Commit();
            return result;

        }

        /// <summary>
        /// 工单下达
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> DistributeWorkOrder(params PlanProductionScheduleInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.OrderItem = PlanOrderItemInfoRepository.TrackEntities.Where(m => m.Id == dtoData.OrderItem_Id).FirstOrDefault();
                if (Equals(dtoData.OrderItem, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的订单明细不存在，不能下达计划。");
                }
                dtoData.ProductionRule = ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方不存在,不能下达计划。");
                }
                if (dtoData.ScheduleStatus != 2)
                {
                    return new OperationResult(OperationResultType.Error, "只有生产计划状态为'已生成工单'的才能下达计划。");
                }
                dtoData.ScheduleStatus = 3;
            }
            PlanProductionScheduleInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanProductionScheduleInfoRepository.UpdateAsync(inputDtos);
            PlanProductionScheduleInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<PlanProductionScheduleInfo, bool>> predicate, Guid id)
        {
            return PlanProductionScheduleInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除生产计划信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count = PlanProductionProcessRequirementInfoRepository.Entities.Where(m => m.ProductionSchedule.Id == id).Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, "生产计划数据关联工序需求信息，不能被删除。");
                }
            }
            PlanProductionScheduleInfoRepository.UnitOfWork.BeginTransaction();
            foreach (var id in ids)
            {
                PlanProductionScheduleInfo info = PlanProductionScheduleInfoRepository.TrackEntities.Where(m => m.Id == id).FirstOrDefault();
                if (info.OrderItem.OrderQuantity >= info.OrderItem.RemainQuantity + info.Quantity)
                {
                    info.OrderItem.RemainQuantity += info.Quantity;
                }
            }
            var result = await PlanProductionScheduleInfoRepository.DeleteAsync(ids);
            PlanProductionScheduleInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新生产计划信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params PlanProductionScheduleInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ScheduleCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写生产计划编号！");
                if (string.IsNullOrEmpty(dtoData.ScheduleName))
                    return new OperationResult(OperationResultType.Error, "请正确填写生产计划名称！");
                if (PlanProductionScheduleInfoRepository.CheckExists(x => x.ScheduleCode == dtoData.ScheduleCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该生产计划编号已存在，无法保存！");
                if (PlanProductionScheduleInfoRepository.CheckExists(x => x.ScheduleName == dtoData.ScheduleName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该生产计划名称已存在，无法保存！");
                dtoData.OrderItem = PlanOrderItemInfoRepository.TrackEntities.Where(m => m.Id == dtoData.OrderItem_Id).FirstOrDefault();
                if (Equals(dtoData.OrderItem, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的订单明细不存在,无法保存！");
                }
                dtoData.ProductionRule = ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方不存在,无法保存！");
                }
                if (Equals(dtoData.ProductionRule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方不存在,无法保存！");
                }
                if (Equals(dtoData.StartTime, null) || Equals(dtoData.EndTime))
                {
                    return new OperationResult(OperationResultType.Error, "开始时间或结束时间为空，无法保存！");
                }
                if (dtoData.StartTime < DateTime.Now)
                {
                    return new OperationResult(OperationResultType.Error, "开始时间应大于系统当前时间，无法保存！");
                }
                if (dtoData.StartTime >= dtoData.EndTime)
                {
                    return new OperationResult(OperationResultType.Error, "开始时间应小于结束时间，无法保存！");
                }
            }
            PlanProductionScheduleInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanProductionScheduleInfoRepository.UpdateAsync(inputDtos);
            PlanProductionScheduleInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
