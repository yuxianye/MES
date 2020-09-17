using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EnterpriseInformation.Models;
using Solution.StepTeachingDispatchManagement.Contracts;
using Solution.StepTeachingDispatchManagement.Dtos;
using Solution.StepTeachingDispatchManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Solution.ProductManagement.Models;

namespace Solution.StepTeachingDispatchManagement.Services
{
    /// <summary>
    /// 分步操作与工序关联信息服务
    /// </summary>
    public class DisStepActionProcessMapInfoService : IDisStepActionProcessMapInfoContract
    {
        public IRepository<DisStepActionProcessMapInfo, Guid> DisStepActionProcessMapRepository { get; set; }
        public IRepository<DisStepActionInfo, Guid> DisStepActionRepository { get; set; }
        public IRepository<ProductionProcessInfo, Guid> ProductionProcessRepository { get; set; }

        /// <summary>
        /// 获取分步操作与工序关联信息查询数据集
        /// </summary>
        public IQueryable<DisStepActionProcessMapInfo> DisStepActionProcessMapInfos => DisStepActionProcessMapRepository.Entities;

        /// <summary>
        /// 添加分步操作与工序关联信息
        /// </summary>
        /// <param name="inputDtos">要添加的分步操作与工序关联信息信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddDisStepActionProcessMaps(params DisStepActionProcessMapInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            DisStepActionProcessMapRepository.UnitOfWork.BeginTransaction();
            var result = await DisStepActionProcessMapRepository.InsertAsync(inputDtos);
            DisStepActionProcessMapRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 检查分步操作与工序关联信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的分步操作与工序关联信息编号</param>
        /// <returns>分步操作与工序关联信息是否存在</returns>
        public bool CheckDisStepActionProcessMapExists(Expression<Func<DisStepActionProcessMapInfo, bool>> predicate, Guid id) => DisStepActionProcessMapRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除分步操作与工序关联信息
        /// </summary>
        /// <param name="ids">要删除的分步操作与工序关联信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteDisStepActionProcessMaps(params Guid[] ids)
        {
            ids.CheckNotNull("ids");

            DisStepActionProcessMapRepository.UnitOfWork.BeginTransaction();
            var result = await DisStepActionProcessMapRepository.DeleteAsync(ids);
            DisStepActionProcessMapRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 更新分步操作与工序关联信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的分步操作与工序关联信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditDisStepActionProcessMaps(params DisStepActionProcessMapInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            DisStepActionProcessMapRepository.UnitOfWork.BeginTransaction();
            var result = await DisStepActionProcessMapRepository.UpdateAsync(inputDtos);
            DisStepActionProcessMapRepository.UnitOfWork.Commit();

            return result;
        }
        /// <summary>
        /// 分步操作与工序关联信息配置
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Setting(params DisStepActionProcessMapManageInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            OperationResult result = new OperationResult();
            OperationResult result2 = new OperationResult();
            foreach (var inputDto in inputDtos)
            {
                try
                {
                    int count = inputDto.ProductionProcessInfoList.Count();
                    if (count >= 0)
                    {
                        DisStepActionProcessMapRepository.UnitOfWork.BeginTransaction();
                        var oldmaplist = DisStepActionProcessMapRepository.TrackEntities.Where(x => x.DisStepActionInfo.Id == inputDto.DisStepActionInfo.Id);
                        int count0 = oldmaplist.Count();
                        if (count0 > 0)
                        {
                            Guid[] mapIds = new Guid[count0];
                            mapIds = oldmaplist.Select(x => x.Id).ToArray();
                            result2 = await DisStepActionProcessMapRepository.DeleteAsync(mapIds);
                        }
                        if (count == 0 && count0 > 0)
                        {
                            result = result2;
                        }
                        if ((result2.Successed || count0 == 0) && count > 0)
                        {
                            DisStepActionProcessMapInfoInputDto[] map_dtos = new DisStepActionProcessMapInfoInputDto[count];
                            for (int i = 0; i < count; i++)
                            {
                                DisStepActionProcessMapInfoInputDto edto = new DisStepActionProcessMapInfoInputDto();
                                edto.DisStepActionInfo = DisStepActionRepository.TrackEntities.Where(m => m.Id == inputDto.DisStepActionInfo.Id).FirstOrDefault();
                                var id = inputDto.ProductionProcessInfoList[i].Id;
                                edto.ProductionProcessInfo = ProductionProcessRepository.TrackEntities.Where(m => m.Id == id).FirstOrDefault();
                                edto.CreatorUserId = inputDto.CreatorUserId;
                                edto.CreatedTime = inputDto.CreatedTime;
                                edto.LastUpdatedTime = inputDto.LastUpdatedTime;
                                edto.LastUpdatorUserId = inputDto.LastUpdatorUserId;
                                if (Equals(edto.DisStepActionInfo, null))
                                {
                                    return new OperationResult(OperationResultType.Error, "对应的分步操作信息不存在,该组数据不被存储。");
                                }
                                if (Equals(edto.ProductionProcessInfo, null))
                                {
                                    return new OperationResult(OperationResultType.Error, "对应的工序信息不存在,该组数据不被存储。");
                                }
                                map_dtos[i] = edto;
                            }
                            result = await DisStepActionProcessMapRepository.InsertAsync(map_dtos);
                        }
                        DisStepActionProcessMapRepository.UnitOfWork.Commit();
                    }
                    else
                    {
                        return new OperationResult(OperationResultType.Error, $"工序表数据异常,该组数据不被存储。");
                    }
                }
                catch (DataException dataException)
                {
                    return new OperationResult(OperationResultType.Error, dataException.Message);
                }
            }
            return result;
        }
    }
}
