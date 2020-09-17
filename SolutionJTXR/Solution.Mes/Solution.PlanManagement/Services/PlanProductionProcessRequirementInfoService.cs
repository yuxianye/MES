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
    public class PlanProductionProcessRequirementInfoService : IPlanProductionProcessRequirementInfoContract
    {
        /// <summary>
        /// 工序需求信息实体仓储
        /// </summary>
        public IRepository<PlanProductionProcessRequirementInfo, Guid> PlanProductionProcessRequirementInfoRepository { get; set; }

        public IRepository<PlanProductionScheduleInfo, Guid> PlanProductionScheduleInfoRepository { get; set; }

        public IRepository<ProductionProcessInfo, Guid> ProductionProcessInfoRepository { get; set; }

        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }
        public IRepository<PlanEquipmentRequirementInfo, Guid> PlanEquipmentRequirementInfoRepository { get; set; }
        public IRepository<PlanMaterialRequirementInfo, Guid> PlanMaterialRequirementInfoRepository { get; set; }

        /// <summary>
        /// 查询工序需求信息
        /// </summary>
        public IQueryable<PlanProductionProcessRequirementInfo> PlanProductionProcessRequirementInfos
        {
            get { return PlanProductionProcessRequirementInfoRepository.Entities; }
        }
        /// <summary>
        /// 增加工序需求
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params PlanProductionProcessRequirementInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.ProductionSchedule = PlanProductionScheduleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionSchedule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionSchedule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的生产计划不存在,无法保存！");
                }
                dtoData.ProductionProcess = ProductionProcessInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionProcess_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionProcess, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的工序不存在,无法保存！");
                }
            }
            PlanProductionProcessRequirementInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanProductionProcessRequirementInfoRepository.InsertAsync(inputDtos);
            PlanProductionProcessRequirementInfoRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<PlanProductionProcessRequirementInfo, bool>> predicate, Guid id)
        {
            return PlanProductionProcessRequirementInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除工序需求信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count1 = PlanEquipmentRequirementInfoRepository.Entities.Where(m => m.ProductionProcessRequirement.Id == id).Count();
                if (count1 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "工序需求数据关联设备需求信息，不能被删除。");
                }
                int count2 = PlanMaterialRequirementInfoRepository.Entities.Where(m => m.ProductionProcessRequirement.Id == id).Count();
                if (count2 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "工序需求数据关联物料需求信息，不能被删除。");
                }
            }
            PlanProductionProcessRequirementInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanProductionProcessRequirementInfoRepository.DeleteAsync(ids);
            PlanProductionProcessRequirementInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新工序需求信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params PlanProductionProcessRequirementInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.ProductionSchedule = PlanProductionScheduleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionSchedule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionSchedule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的生产计划不存在,无法保存！");
                }
                dtoData.ProductionProcess = ProductionProcessInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionProcess_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionProcess, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的工序不存在,无法保存！");
                }
            }
            PlanProductionProcessRequirementInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanProductionProcessRequirementInfoRepository.UpdateAsync(inputDtos);
            PlanProductionProcessRequirementInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
