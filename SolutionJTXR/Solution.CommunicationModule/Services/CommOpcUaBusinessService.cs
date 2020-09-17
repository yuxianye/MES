using OSharp.Core.Data;
using OSharp.Core.Mapping;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Services
{
    public class CommOpcUaBusinessService : ICommOpcUaBusinessContract
    {
        public IRepository<CommOpcUaBusiness, Guid> CommOpcUaBusinessRepository { get; set; }

        public IRepository<DeviceNode, Guid> CommOpcUaNodeRepository { get; set; }

        public IRepository<CommOpcUaBusinessNodeMap, Guid> CommOpcUaBusinessNodeMapRepository { get; set; }

        public ICommOpcUaBusinessNodeMapContract CommOpcUaBusinessNodeMapContract { get; set; }

        /// <summary>
        /// 获取Opc Ua 业务数据查询数据集
        /// </summary>
        public IQueryable<CommOpcUaBusiness> CommOpcUaBusinessInfos => CommOpcUaBusinessRepository.Entities;

        /// <summary>
        /// 添加Opc Ua 业务数据
        /// </summary>
        /// <param name="inputDtos">要添加的Opc Ua 业务数据DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddCommOpcUaBusinesss(params CommOpcUaBusinessInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (CommOpcUaBusinessRepository.CheckExists(x => x.BusinessName.Equals(dtoData.BusinessName)))
                    return new OperationResult(OperationResultType.Error, $"业务名称为{dtoData.BusinessName}的信息已存在，数据不合法，改组数据不被存储");
            }

            CommOpcUaBusinessRepository.UnitOfWork.BeginTransaction();

            CommOpcUaBusinessNodeMapInputDto commOpcUaBusinessNodeMapInputDto = new CommOpcUaBusinessNodeMapInputDto();
            foreach (var dtoData in inputDtos)
            {
                DeviceNode commOpcUaNode = CommOpcUaNodeRepository.TrackEntities.Where(x => x.Id.Equals(dtoData.NodeId)).FirstOrDefault();

                if (commOpcUaNode != null)
                {
                    //var result = await CommOpcUaBusinessContract.AddCommOpcUaBusinesss(dtoData);
                    var result = await CommOpcUaBusinessRepository.InsertAsync(dtoData.MapTo<CommOpcUaBusiness>());
                    if (result > 0)
                    {
                        CommOpcUaBusiness commOpcUaBusiness = CommOpcUaBusinessRepository.TrackEntities.Where(x => x.BusinessName.Equals(dtoData.BusinessName)).FirstOrDefault();
                        commOpcUaBusinessNodeMapInputDto.OpcUaNode = commOpcUaNode;
                        commOpcUaBusinessNodeMapInputDto.OpcUaBusiness = commOpcUaBusiness;

                        commOpcUaBusinessNodeMapInputDto.Id = CombHelper.NewComb();
                        commOpcUaBusinessNodeMapInputDto.CreatorUserId = dtoData.CreatorUserId;
                        commOpcUaBusinessNodeMapInputDto.CreatedTime = DateTime.Now;
                        commOpcUaBusinessNodeMapInputDto.LastUpdatedTime = commOpcUaBusinessNodeMapInputDto.CreatedTime;
                        commOpcUaBusinessNodeMapInputDto.LastUpdatorUserId = commOpcUaBusinessNodeMapInputDto.CreatorUserId;

                        var saveResult = await CommOpcUaBusinessNodeMapContract.AddCommOpcUaBusinessNodeMaps(commOpcUaBusinessNodeMapInputDto);
                        if (saveResult.ResultType == OperationResultType.Error)
                        {
                            return new OperationResult(OperationResultType.Error, $"存储通讯业务点表与通讯点表关联数据失败，取消数据点\"{dtoData.BusinessName}\"存储！");
                        }
                    }
                    else
                    {
                        return new OperationResult(OperationResultType.Error, "存储通讯业务点表数据失败！");
                    }
                }
                else
                {
                    return new OperationResult(OperationResultType.Error, "查询通讯点表数据失败！");
                }
            }

            CommOpcUaBusinessRepository.UnitOfWork.Commit();
            return new OperationResult(OperationResultType.Success, "存储业务点数据成功！");
        }
        /// <summary>
        /// 添加业务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params CommOpcUaBusinessInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (CommOpcUaBusinessInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.BusinessName))
                    return new OperationResult(OperationResultType.Error, "请正确填写业务名称，该组数据不被存储。");
                if (CommOpcUaBusinessRepository.CheckExists(x => x.BusinessName == dtoData.BusinessName))
                    return new OperationResult(OperationResultType.Error, $"业务名称 {dtoData.BusinessName} 的数据已存在，该组数据不被存储。");
            }
            CommOpcUaBusinessRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaBusinessRepository.InsertAsync(inputDtos);
            CommOpcUaBusinessRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新业务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params CommOpcUaBusinessInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (CommOpcUaBusinessInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.BusinessName))
                    return new OperationResult(OperationResultType.Error, "请正确填写业务名称，该组数据不被存储。");
                if (CommOpcUaBusinessRepository.CheckExists(x => x.BusinessName == dtoData.BusinessName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"业务名称 {dtoData.BusinessName} 的数据已存在，该组数据不被存储。");
            }
            CommOpcUaBusinessRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaBusinessRepository.UpdateAsync(inputDtos);
            CommOpcUaBusinessRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 检查组Opc Ua 业务数据信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的Opc Ua 业务数据编号</param>
        /// <returns>Opc Ua 业务数据是否存在</returns>
        public bool CheckCommOpcUaBusinessExists(Expression<Func<CommOpcUaBusiness, bool>> predicate, Guid id) => CommOpcUaBusinessRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除Opc Ua 业务数据
        /// </summary>
        /// <param name="ids">要删除的Opc Ua 业务数据编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteCommOpcUaBusinesss(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            CommOpcUaBusinessRepository.UnitOfWork.BeginTransaction();

            List<Guid> mapIds = new List<Guid>();
            foreach (var id in ids)
            {
                var subList = CommOpcUaBusinessNodeMapRepository.Entities.Where(x => x.OpcUaBusiness.Id.Equals(id)).Select(x => x.Id).ToList();
                mapIds.AddRange(subList.ToArray());
            }

            if (mapIds.Any())
            {
                var recycleMapResult = await CommOpcUaBusinessNodeMapRepository.DeleteAsync(mapIds.ToArray());
                if (!recycleMapResult.Successed)
                {
                    return new OperationResult(OperationResultType.Error, "删除通讯业务与通讯点关联数据失败,取消通讯业务数据物理删除！");
                }
            }

            var result = await CommOpcUaBusinessRepository.DeleteAsync(ids);
            CommOpcUaBusinessRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 逻辑删除Opc Ua 业务数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        public async Task<OperationResult> RecycleCommOpcUaBusinesss(params CommOpcUaBusiness[] commOpcUaBusiness)
        {
            commOpcUaBusiness.CheckNotNull("commOpcUaBusiness");
            int count = 0;
            try
            {
                CommOpcUaBusinessRepository.UnitOfWork.BeginTransaction();
                count = await CommOpcUaBusinessRepository.RecycleAsync(commOpcUaBusiness);
                CommOpcUaBusinessRepository.UnitOfWork.Commit();
            }
            catch (DataException dataException)
            {
                return new OperationResult(OperationResultType.Error, dataException.Message);
            }
            catch (OSharpException osharpException)
            {
                return new OperationResult(OperationResultType.Error, osharpException.Message);
            }

            List<string> names = new List<string>();
            foreach (var data in commOpcUaBusiness)
            {
                names.Add(data.BusinessName);
            }
            return count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑还原成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 恢复逻辑删除Opc Ua 业务数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> RestoreCommOpcUaBusinesss(params CommOpcUaBusiness[] commOpcUaBusiness)
        {
            commOpcUaBusiness.CheckNotNull("commOpcUaBusiness");
            int count = 0;
            try
            {
                CommOpcUaBusinessRepository.UnitOfWork.BeginTransaction();
                count = await CommOpcUaBusinessRepository.RestoreAsync(commOpcUaBusiness);
                CommOpcUaBusinessRepository.UnitOfWork.Commit();
            }
            catch (DataException dataException)
            {
                return new OperationResult(OperationResultType.Error, dataException.Message);
            }
            catch (OSharpException osharpException)
            {
                return new OperationResult(OperationResultType.Error, osharpException.Message);
            }

            List<string> names = new List<string>();
            foreach (var data in commOpcUaBusiness)
            {
                names.Add(data.BusinessName);
            }
            return count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑还原成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 更新Opc Ua 业务数据信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的Opc Ua 业务数据DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditCommOpcUaBusinesss(params CommOpcUaBusinessInputDto[] inputDto)
        {
            inputDto.CheckNotNull("data");
            CommOpcUaBusinessRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaBusinessRepository.UpdateAsync(inputDto);

            if (result.Successed)
            {
                foreach (var data in inputDto)
                {
                    CommOpcUaBusiness commOpcUaBusiness = CommOpcUaBusinessRepository.TrackEntities.Where(x => x.Id.Equals(data.Id)).FirstOrDefault();
                    DeviceNode commOpcUaNode = CommOpcUaNodeRepository.TrackEntities.Where(x => x.Id.Equals(data.NodeId)).FirstOrDefault();
                    CommOpcUaBusinessNodeMap entity = CommOpcUaBusinessNodeMapContract.CommOpcUaBusinessNodeMapInfos.Where(x => x.OpcUaBusiness.Id.Equals(data.Id)).FirstOrDefault();
                    if (entity != null)
                    {
                        CommOpcUaBusinessNodeMapInputDto dto = new CommOpcUaBusinessNodeMapInputDto();
                        dto.Id = entity.Id;
                        dto.CreatedTime = entity.CreatedTime;
                        dto.CreatorUserId = entity.CreatorUserId;
                        dto.OpcUaBusiness = commOpcUaBusiness;
                        dto.OpcUaNode = commOpcUaNode;
                        dto.LastUpdatedTime = data.LastUpdatedTime;
                        dto.LastUpdatorUserId = data.LastUpdatorUserId;
                        try
                        {
                            var EditResult = await CommOpcUaBusinessNodeMapContract.EditCommOpcUaBusinessNodeMaps(dto);
                            if (EditResult.ResultType == OperationResultType.Error)
                            {
                                return EditResult;
                            }
                        }
                        catch (Exception ex)
                        {
                            string sss = ex.ToString();
                        }
                    }
                }
            }
            else
            {
                return result;
            }

            CommOpcUaBusinessRepository.UnitOfWork.Commit();

            return new OperationResult(OperationResultType.Success, "修改业务数据成功！");
        }
    }
}
