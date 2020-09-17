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
    /// 地标点服务
    /// </summary>
    public class MarkPointInfoService : IMarkPointInfoContract
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        public IRepository<MarkPointInfo, Guid> MarkPointInfoRepository { get; set; }

        /// <summary>
        /// 查询数据集
        /// </summary>
        public IQueryable<MarkPointInfo> MarkPointInfos
        {
            get { return MarkPointInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckMarkPointInfoExists(Expression<Func<MarkPointInfo, bool>> predicate, Guid id)
        {
            return MarkPointInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加地标点信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MarkPointInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.MarkPointNo))
                    return new OperationResult(OperationResultType.Error, "请正确填写地标点编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.MarkPointName))
                    return new OperationResult(OperationResultType.Error, "请正确填写地标点名称，该组数据不被存储。");
                if (MarkPointInfoRepository.CheckExists(x => x.MarkPointNo == dtoData.MarkPointNo))
                    return new OperationResult(OperationResultType.Error, $"地标点编号 {dtoData.MarkPointNo} 的数据已存在，该组数据不被存储。");
                if (MarkPointInfoRepository.CheckExists(x => x.MarkPointName == dtoData.MarkPointName))
                    return new OperationResult(OperationResultType.Error, $"地标点名称 {dtoData.MarkPointName} 的数据已存在，该组数据不被存储。");
            }

            MarkPointInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MarkPointInfoRepository.InsertAsync(inputDtos);
            MarkPointInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新地标点信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MarkPointInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (MarkPointInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.MarkPointNo))
                    return new OperationResult(OperationResultType.Error, "请正确填写地标点编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.MarkPointName))
                    return new OperationResult(OperationResultType.Error, "请正确填写地标点名称，该组数据不被存储。");
                if (MarkPointInfoRepository.CheckExists(x => x.MarkPointNo == dtoData.MarkPointNo && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"地标点编号 {dtoData.MarkPointNo} 的数据已存在，该组数据不被存储。");
                if (MarkPointInfoRepository.CheckExists(x => x.MarkPointName == dtoData.MarkPointName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"地标点名称 {dtoData.MarkPointName} 的数据已存在，该组数据不被存储。");
            }
            MarkPointInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MarkPointInfoRepository.UpdateAsync(inputDtos);
            MarkPointInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除地标点信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MarkPointInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MarkPointInfoRepository.DeleteAsync(ids);
            MarkPointInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
