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
    public class EntTeamMapInfoService : IEntTeamMapInfoContract
    {
        /// <summary>
        /// 班组关联信息实体仓储
        /// </summary>
        public IRepository<EntTeamMapInfo, Guid> EntTeamMapInfoRepository { get; set; }
        /// <summary>
        /// 班组信息实体仓储
        /// </summary>
        public IRepository<EntTeamInfo, Guid> EntTeamInfoRepository { get; set; }
        /// <summary>
        /// 人员信息实体仓储
        /// </summary>
        public IRepository<EntEmployeeInfo, Guid> EntEmployeeInfoRepository { get; set; }
        /// <summary>
        /// 查询班组关联信息
        /// </summary>
        public IQueryable<EntTeamMapInfo> EntTeamMapInfos
        {
            get { return EntTeamMapInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查班组关联信息是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEnterpriseExists(Expression<Func<EntTeamMapInfo, bool>> predicate, Guid id)
        {
            return EntTeamMapInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 添加班组关联信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EntTeamMapInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            EntTeamMapInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntTeamMapInfoRepository.InsertAsync(inputDtos);
            EntTeamMapInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新班组关联信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EntTeamMapInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            EntTeamMapInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntTeamMapInfoRepository.UpdateAsync(inputDtos);
            EntTeamMapInfoRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 物理删除班组关联信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EntTeamMapInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntTeamMapInfoRepository.DeleteAsync(ids);
            EntTeamMapInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 班组与人员关联信息配置
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Setting(params EntTeamMapManageInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            OperationResult result = new OperationResult();
            OperationResult result2 = new OperationResult();
            foreach (var inputDto in inputDtos)
            {
                try
                {
                    int count = inputDto.EntEmployeeInfoList.Count();
                    if (count >= 0)
                    {
                        EntTeamMapInfoRepository.UnitOfWork.BeginTransaction();
                        var oldmaplist = EntTeamMapInfoRepository.TrackEntities.Where(x => x.EntTeamInfo.Id == inputDto.EntTeamInfo.Id);
                        int count0 = oldmaplist.Count();
                        if (count0 > 0)
                        {
                            Guid[] mapIds = new Guid[count0];
                            mapIds = oldmaplist.Select(x => x.Id).ToArray();
                            result2 = await EntTeamMapInfoRepository.DeleteAsync(mapIds);
                        }
                        if (count == 0 && count0 > 0)
                        {
                            result = result2;
                        }
                        if ((result2.Successed || count0 == 0) && count > 0)
                        {
                            EntTeamMapInfoInputDto[] map_dtos = new EntTeamMapInfoInputDto[count];
                            for (int i = 0; i < count; i++)
                            {
                                EntTeamMapInfoInputDto edto = new EntTeamMapInfoInputDto();
                                edto.EntTeamInfo = EntTeamInfoRepository.TrackEntities.Where(m => m.Id == inputDto.EntTeamInfo.Id).FirstOrDefault();
                                var id = inputDto.EntEmployeeInfoList[i].Id;
                                edto.EntEmployeeInfo = EntEmployeeInfoRepository.TrackEntities.Where(m => m.Id == id).FirstOrDefault();
                                edto.CreatorUserId = inputDto.CreatorUserId;
                                edto.CreatedTime = inputDto.CreatedTime;
                                edto.LastUpdatedTime = inputDto.LastUpdatedTime;
                                edto.LastUpdatorUserId = inputDto.LastUpdatorUserId;
                                if (Equals(edto.EntTeamInfo, null))
                                {
                                    return new OperationResult(OperationResultType.Error, "对应的班组信息不存在,无法保存！");
                                }
                                if (Equals(edto.EntEmployeeInfo, null))
                                {
                                    return new OperationResult(OperationResultType.Error, "对应的人员信息不存在,无法保存！");
                                }
                                map_dtos[i] = edto;
                            }
                            result = await EntTeamMapInfoRepository.InsertAsync(map_dtos);
                        }
                        EntTeamMapInfoRepository.UnitOfWork.Commit();
                    }
                    else
                    {
                        return new OperationResult(OperationResultType.Error, "人员列表数据异常,无法保存！");
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
