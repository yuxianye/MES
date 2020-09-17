using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EnterpriseInformation.Contracts;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.Services
{
    public class EntProductionLineInfoService : IEntProductionLineInfoContract
    {

        public IRepository<EntAreaInfo, Guid> EntAreaInfoRepository { get; set; }
        public IRepository<EntSiteInfo, Guid> EntSiteInfoRepository { get; set; }
        public IRepository<EnterpriseInfo, Guid> EnterpriseInfoRepository { get; set; }
        public IRepository<EntProductionLineInfo, Guid> EntProductionLineInfoRepository { get; set; }

        public IQueryable<EntProductionLineInfo> EntProductionLineInfo
        {
            get { return EntProductionLineInfoRepository.Entities; }
        }

        /// <summary>
        /// 增加生产线信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EntProductionLineInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductionLineCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写生产线编号！");
                if (string.IsNullOrEmpty(dtoData.ProductionLineName))
                    return new OperationResult(OperationResultType.Error, "请正确填写生产线名称！");
                if (EntProductionLineInfoRepository.CheckExists(x => x.ProductionLineCode == dtoData.ProductionLineCode))
                    return new OperationResult(OperationResultType.Error, "该生产线编号已存在，无法保存！");
                if (EntProductionLineInfoRepository.CheckExists(x => x.ProductionLineName == dtoData.ProductionLineName))
                    return new OperationResult(OperationResultType.Error, "该生产线名称已存在，无法保存！");
                dtoData.EntArea = EntAreaInfoRepository.TrackEntities.Where(m => m.Id == dtoData.EntArea_Id).FirstOrDefault();
                if (Equals(dtoData.EntArea, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的车间不存在,无法保存！");
                }
                if (dtoData.Duration == 0)
                {
                    return new OperationResult(OperationResultType.Error, "生产线时长应大于0，无法保存！");
                }
            }
            EntProductionLineInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntProductionLineInfoRepository.InsertAsync(inputDtos);
            EntProductionLineInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEntProductionLineInfoExists(Expression<Func<EntProductionLineInfo, bool>> predicate, Guid id)
        {
            return EntProductionLineInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除生产线信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EntProductionLineInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntProductionLineInfoRepository.DeleteAsync(ids);
            EntProductionLineInfoRepository.UnitOfWork.Commit();
            return result;
        }



        /// <summary>
        /// 更新生产线信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EntProductionLineInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductionLineCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写生产线编号！");
                if (string.IsNullOrEmpty(dtoData.ProductionLineName))
                    return new OperationResult(OperationResultType.Error, "请正确填写生产线名称！");
                if (EntProductionLineInfoRepository.CheckExists(x => x.ProductionLineCode == dtoData.ProductionLineCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该生产线编号已存在，无法保存！");
                if (EntProductionLineInfoRepository.CheckExists(x => x.ProductionLineName == dtoData.ProductionLineName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该生产线名称已存在，无法保存！");
                dtoData.EntArea = EntAreaInfoRepository.TrackEntities.Where(m => m.Id == dtoData.EntArea_Id).FirstOrDefault();
                if (Equals(dtoData.EntArea, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的车间不存在,无法保存！");
                }
                if (dtoData.Duration == 0)
                {
                    return new OperationResult(OperationResultType.Error, "生产线时长应大于0，无法保存！");
                }
            }
            EntProductionLineInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntProductionLineInfoRepository.UpdateAsync(inputDtos);
            EntProductionLineInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
