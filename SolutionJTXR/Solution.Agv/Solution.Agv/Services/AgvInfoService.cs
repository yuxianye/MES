using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.Agv.Contracts;
using Solution.Agv.Dtos;
using Solution.Agv.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.Agv.Services
{
    /// <summary>
    /// 车辆信息服务
    /// </summary>
    public class AgvInfoService : IAgvInfoContract
    {
        /// <summary>
        /// 车辆信息实体仓储
        /// </summary>
        public IRepository<AgvInfo, Guid> AgvInfoRepository { get; set; }

        /// <summary>
        /// 查询企业信息数据集
        /// </summary>
        public IQueryable<AgvInfo> AgvInfos
        {
            get { return AgvInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckAgvInfoExists(Expression<Func<AgvInfo, bool>> predicate, Guid id)
        {
            return AgvInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加agv小车信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params AgvInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.CarNo))
                    return new OperationResult(OperationResultType.Error, "请正确填写车辆编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.CarName))
                    return new OperationResult(OperationResultType.Error, "请正确填写车辆名称，该组数据不被存储。");
                if (AgvInfoRepository.CheckExists(x => x.CarNo == dtoData.CarNo))
                    return new OperationResult(OperationResultType.Error, $"车辆编号 {dtoData.CarNo} 的数据已存在，该组数据不被存储。");
                if (AgvInfoRepository.CheckExists(x => x.CarName == dtoData.CarName))
                    return new OperationResult(OperationResultType.Error, $"车辆名称 {dtoData.CarName} 的数据已存在，该组数据不被存储。");
            }
            AgvInfoRepository.UnitOfWork.BeginTransaction();
            var result = await AgvInfoRepository.InsertAsync(inputDtos);
            AgvInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新agv小车信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params AgvInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (AgvInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.CarNo))
                    return new OperationResult(OperationResultType.Error, "请正确填写车辆编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.CarNo))
                    return new OperationResult(OperationResultType.Error, "请正确填写车辆名称，该组数据不被存储。");
                if (AgvInfoRepository.CheckExists(x => x.CarNo == dtoData.CarNo && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"车辆编号 {dtoData.CarNo} 的数据已存在，该组数据不被存储。");
                if (AgvInfoRepository.CheckExists(x => x.CarName == dtoData.CarName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"车辆名称 {dtoData.CarName} 的数据已存在，该组数据不被存储。");
            }
            AgvInfoRepository.UnitOfWork.BeginTransaction();
            var result = await AgvInfoRepository.UpdateAsync(inputDtos);
            AgvInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除agv小车信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            AgvInfoRepository.UnitOfWork.BeginTransaction();
            var result = await AgvInfoRepository.DeleteAsync(ids);
            AgvInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
