using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.PlanManagement.Contracts;
using Solution.PlanManagement.Dtos;
using Solution.PlanManagement.Models;
using Solution.ProductManagement.Models;
using Solution.EquipmentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.PlanManagement.Services
{
    public class PlanEquipmentRequirementInfoService : IPlanEquipmentRequirementInfoContract
    {
        /// <summary>
        /// 工序设备需求信息实体仓储
        /// </summary>
        public IRepository<PlanEquipmentRequirementInfo, Guid> PlanEquipmentRequirementInfoRepository { get; set; }

        public IRepository<PlanProductionProcessRequirementInfo, Guid> PlanProductionProcessRequirementInfoRepository { get; set; }

        public IRepository<ProductionProcessInfo, Guid> ProductionProcessInfoRepository { get; set; }

        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }

        public IRepository<EquEquipmentInfo, Guid> EquipmentInfoRepository { get; set; }

        /// <summary>
        /// 查询工序设备需求信息
        /// </summary>
        public IQueryable<PlanEquipmentRequirementInfo> PlanEquipmentRequirementInfos
        {
            get { return PlanEquipmentRequirementInfoRepository.Entities; }
        }



        ///// <summary>
        ///// 增加工序设备需求
        ///// </summary>
        ///// <param name="inputDtos"></param>
        ///// <returns></returns>
        //public OperationResult Add(params PlanEquipmentRequirementInfoInputDto[] inputDtos)
        //{

        //    inputDtos.CheckNotNull("inputDtos");
        //    OperationResult result = PlanEquipmentRequirementInfoRepository.Insert(inputDtos,
        //        dto =>
        //        {
        //            if (dto.RequireQuantity == null)
        //            {
        //                throw new Exception("存在工序设备需求数量为空的数据，数据不合法，无法保存！");
        //            }
        //        },
        //        (dto, entity) =>
        //        {
        //            if (dto.ProductionProcessRequirement_Id != null)
        //            {
        //                PlanProductionProcessRequirementInfo productionProcessRequirementInfo = PlanProductionProcessRequirementInfoRepository.GetByKey(dto.ProductionProcessRequirement_Id);
        //                if (productionProcessRequirementInfo == null)
        //                {
        //                    throw new Exception("要增加设备需求对应的的工序需求不存在。");
        //                }
        //                entity.ProductionProcessRequirement = productionProcessRequirementInfo;
        //            }
        //            else
        //            {
        //                entity.ProductionProcessRequirement = null;
        //            }
        //            if (dto.ProductionProcess_Id != null)
        //            {
        //                ProductionProcessInfo processInfo = ProductionProcessInfoRepository.GetByKey(dto.ProductionProcess_Id);
        //                if (processInfo == null)
        //                {
        //                    throw new Exception("要增加的设备需求对应的工序不存在。");
        //                }
        //                entity.ProductionProcess = processInfo;
        //            }
        //            else
        //            {
        //                entity.ProductionProcess = null;
        //            }
        //            if (dto.ProductionRule_Id != null)
        //            {
        //                ProductionRuleInfo productionRuleInfo = ProductionRuleInfoRepository.GetByKey(dto.ProductionRule_Id);
        //                if (productionRuleInfo == null)
        //                {
        //                    throw new Exception("要增加的设备需求对应的配方不存在。");
        //                }
        //                entity.ProductionRule = productionRuleInfo;
        //            }
        //            else
        //            {
        //                entity.ProductionRule = null;
        //            }
        //            return entity;
        //        });
        //    return result;
        //}
        /// <summary>
        /// 增加工序设备需求信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params PlanEquipmentRequirementInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.ProductionProcessRequirement = PlanProductionProcessRequirementInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionProcessRequirement_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionProcessRequirement, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的工序请求不存在,无法保存！");
                }
                dtoData.Equipment = EquipmentInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Equipment_Id).FirstOrDefault();
                if (Equals(dtoData.Equipment, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的设备不存在,无法保存！");
                }
            }
            PlanEquipmentRequirementInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanEquipmentRequirementInfoRepository.InsertAsync(inputDtos);
            PlanEquipmentRequirementInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<PlanEquipmentRequirementInfo, bool>> predicate, Guid id)
        {
            return PlanEquipmentRequirementInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除工序设备需求信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            PlanEquipmentRequirementInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanEquipmentRequirementInfoRepository.DeleteAsync(ids);
            PlanEquipmentRequirementInfoRepository.UnitOfWork.Commit();
            return result;
        }

        ///// <summary>
        ///// 逻辑删除工序设备需求信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicDelete(params PlanEquipmentRequirementInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;
        //    try
        //    {
        //        PlanEquipmentRequirementInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await PlanEquipmentRequirementInfoRepository.RecycleAsync(enterinfos);
        //        PlanEquipmentRequirementInfoRepository.UnitOfWork.Commit();
        //    }
        //    catch (DataException dataException)
        //    {
        //        return new OperationResult(OperationResultType.Error, dataException.Message);
        //    }
        //    catch (OSharpException osharpException)
        //    {
        //        return new OperationResult(OperationResultType.Error, osharpException.Message);
        //    }

        //    List<string> names = new List<string>();
        //    foreach (var data in enterinfos)
        //    {
        //        names.Add(data.Id.ToString());
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        ///// <summary>
        ///// 逻辑还原工序设备需求信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicRestore(params PlanEquipmentRequirementInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;

        //    try
        //    {
        //        PlanEquipmentRequirementInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await PlanEquipmentRequirementInfoRepository.RestoreAsync(enterinfos);
        //        PlanEquipmentRequirementInfoRepository.UnitOfWork.Commit();
        //    }
        //    catch (DataException dataException)
        //    {
        //        return new OperationResult(OperationResultType.Error, dataException.Message);
        //    }
        //    catch (OSharpException osharpException)
        //    {
        //        return new OperationResult(OperationResultType.Error, osharpException.Message);
        //    }

        //    List<string> names = new List<string>();
        //    foreach (var data in enterinfos)
        //    {
        //        names.Add(data.Id.ToString());
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        /// <summary>
        /// 更新工序设备需求信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params PlanEquipmentRequirementInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.ProductionProcessRequirement = PlanProductionProcessRequirementInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionProcessRequirement_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionProcessRequirement, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的工序请求不存在,无法保存！");
                }
                dtoData.Equipment = EquipmentInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Equipment_Id).FirstOrDefault();
                if (Equals(dtoData.Equipment, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的设备不存在,无法保存！");
                }
            }
            PlanEquipmentRequirementInfoRepository.UnitOfWork.BeginTransaction();
            var result = await PlanEquipmentRequirementInfoRepository.UpdateAsync(inputDtos);
            PlanEquipmentRequirementInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
