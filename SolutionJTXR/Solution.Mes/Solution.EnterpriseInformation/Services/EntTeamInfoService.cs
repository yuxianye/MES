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
    public class EntTeamInfoService : IEntTeamInfoContract
    {
        /// <summary>
        /// 班组信息实体仓储
        /// </summary>
        public IRepository<EntTeamInfo, Guid> EntTeamInfoRepository { get; set; }
        public IRepository<EntTeamMapInfo, Guid> EntTeamMapInfoRepository { get; set; }

        /// <summary>
        /// 查询班组信息
        /// </summary>
        public IQueryable<EntTeamInfo> EntTeamInfos
        {
            get { return EntTeamInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查班组信息是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEnterpriseExists(Expression<Func<EntTeamInfo, bool>> predicate, Guid id)
        {
            return EntTeamInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 添加班组信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EntTeamInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EntTeamInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.TeamCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写班组编号！");
                if (string.IsNullOrEmpty(dtoData.TeamName))
                    return new OperationResult(OperationResultType.Error, "请正确填写班组名称！");
                if (EntTeamInfoRepository.CheckExists(x => x.TeamCode == dtoData.TeamCode))
                    return new OperationResult(OperationResultType.Error, "该班组编号已存在，无法保存！");
                if (EntTeamInfoRepository.CheckExists(x => x.TeamName == dtoData.TeamName))
                    return new OperationResult(OperationResultType.Error, "该班组名称已存在，无法保存！");
            }
            EntTeamInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntTeamInfoRepository.InsertAsync(inputDtos);
            EntTeamInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新班组信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EntTeamInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EntTeamInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.TeamCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写班组编号！");
                if (string.IsNullOrEmpty(dtoData.TeamName))
                    return new OperationResult(OperationResultType.Error, "请正确填写班组名称！");
                if (EntTeamInfoRepository.CheckExists(x => x.TeamCode == dtoData.TeamCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该班组编号已存在，无法保存！");
                if (EntTeamInfoRepository.CheckExists(x => x.TeamName == dtoData.TeamName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该班组名称已存在，无法保存！");
            }
            EntTeamInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntTeamInfoRepository.UpdateAsync(inputDtos);
            EntTeamInfoRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 物理删除班组信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count = EntTeamMapInfoRepository.Entities.Where(m => m.EntTeamInfo.Id == id).Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, "班组数据关联人员配置信息，不能被删除。");
                }
            }
            EntTeamInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntTeamInfoRepository.DeleteAsync(ids);
            EntTeamInfoRepository.UnitOfWork.Commit();
            return result;
        }

    }
}
