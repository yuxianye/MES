using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using Solution.EquipmentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EquipmentManagement.Services
{
    public class EquFactoryInfoService : IEquFactoryInfoContract
    {  
        /// <summary>
        /// 设备厂家信息实体仓储
        /// </summary>
        public IRepository<EquFactoryInfo, Guid> EquFactoryRepository { get; set; }

        /// <summary>
        /// 查询企业信息数据集
        /// </summary>
        public IQueryable<EquFactoryInfo> EquFactoryInfos => EquFactoryRepository.Entities;

        /// <summary>
        ///设备厂家信息查询数据集
        /// </summary>
        public IQueryable<EquFactoryInfo> EquFactoryTrackInfos => EquFactoryRepository.TrackEntities;

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEquFactoryInfoExists(Expression<Func<EquFactoryInfo, bool>> predicate, Guid id)
        {
            return EquFactoryRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加设备厂家信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EquFactoryInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.FactoryCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写设备厂家编号！");
                if (string.IsNullOrEmpty(dtoData.FactoryName))
                    return new OperationResult(OperationResultType.Error, "请正确填写设备厂家名称！");
                if (EquFactoryRepository.CheckExists(x => x.FactoryCode == dtoData.FactoryCode))
                    return new OperationResult(OperationResultType.Error, "该设备厂家编号已存在，无法保存！");
                if (EquFactoryRepository.CheckExists(x => x.FactoryName == dtoData.FactoryName))
                    return new OperationResult(OperationResultType.Error, "该设备厂家名称已存在，无法保存！");
            }
            EquFactoryRepository.UnitOfWork.BeginTransaction();
            var result = await EquFactoryRepository.InsertAsync(inputDtos);
            EquFactoryRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新设备厂家信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EquFactoryInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EquFactoryInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.FactoryCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写设备厂家编号！");
                if (string.IsNullOrEmpty(dtoData.FactoryName))
                    return new OperationResult(OperationResultType.Error, "请正确填写设备厂家名称！");
                if (EquFactoryRepository.CheckExists(x => x.FactoryCode == dtoData.FactoryCode))
                    return new OperationResult(OperationResultType.Error, "该设备厂家编号已存在，无法保存！");
                if (EquFactoryRepository.CheckExists(x => x.FactoryName == dtoData.FactoryName))
                    return new OperationResult(OperationResultType.Error, "该设备厂家名称已存在，无法保存！");
            }
            EquFactoryRepository.UnitOfWork.BeginTransaction();
            var result = await EquFactoryRepository.UpdateAsync(inputDtos);
            EquFactoryRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除设备厂家信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquFactoryRepository.UnitOfWork.BeginTransaction();
            var result = await EquFactoryRepository.DeleteAsync(ids);
            EquFactoryRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
