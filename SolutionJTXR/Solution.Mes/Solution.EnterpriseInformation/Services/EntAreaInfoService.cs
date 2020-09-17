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
    public class EntAreaInfoService : IEntAreaInfoContract
    {
        public IRepository<EntAreaInfo, Guid> EntAreaInfoRepository { get; set; }
        public IRepository<EntSiteInfo, Guid> EntSiteInfoRepository { get; set; }
        public IRepository<EnterpriseInfo, Guid> EnterpriseInfoRepository { get; set; }
        public IRepository<EntProductionLineInfo, Guid> EntProductionLineInfoRepository { get; set; }

        public IQueryable<EntAreaInfo> EntAreaInfo
        {
            get { return EntAreaInfoRepository.Entities; }
        }

        /// <summary>
        /// 增加车间信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EntAreaInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.AreaCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写车间编号！");
                if (string.IsNullOrEmpty(dtoData.AreaName))
                    return new OperationResult(OperationResultType.Error, "请正确填写车间名称！");
                if (EntAreaInfoRepository.CheckExists(x => x.AreaCode == dtoData.AreaCode))
                    return new OperationResult(OperationResultType.Error, "该车间编号已存在，无法保存！");
                if (EntAreaInfoRepository.CheckExists(x => x.AreaName == dtoData.AreaName))
                    return new OperationResult(OperationResultType.Error, "该车间名称已存在，无法保存！");
                dtoData.EntSite = EntSiteInfoRepository.TrackEntities.Where(m => m.Id == dtoData.EntSite_Id).FirstOrDefault();
                if (Equals(dtoData.EntSite, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的厂区不存在,无法保存！");
                }
            }
            EntAreaInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntAreaInfoRepository.InsertAsync(inputDtos);
            EntAreaInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEntAreaInfoExists(Expression<Func<EntAreaInfo, bool>> predicate, Guid id)
        {
            return EntAreaInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除车间信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count = EntProductionLineInfoRepository.Entities.Where(m => m.EntArea.Id == id).Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, "车间数据关联生产线信息，不能被删除。");
                }
            }
            EntAreaInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntAreaInfoRepository.DeleteAsync(ids);
            EntAreaInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新车间信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EntAreaInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.AreaCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写车间编号！");
                if (string.IsNullOrEmpty(dtoData.AreaName))
                    return new OperationResult(OperationResultType.Error, "请正确填写车间名称！");
                if (EntAreaInfoRepository.CheckExists(x => x.AreaCode == dtoData.AreaCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该车间编号已存在，无法保存！");
                if (EntAreaInfoRepository.CheckExists(x => x.AreaName == dtoData.AreaName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该车间名称已存在，无法保存！");
                dtoData.EntSite = EntSiteInfoRepository.TrackEntities.Where(m => m.Id == dtoData.EntSite_Id).FirstOrDefault();
                if (Equals(dtoData.EntSite, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的厂区不存在,无法保存！");
                }
            }
            EntAreaInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntAreaInfoRepository.UpdateAsync(inputDtos);
            EntAreaInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
