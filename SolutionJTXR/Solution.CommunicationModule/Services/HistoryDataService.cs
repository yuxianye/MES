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
    public class HistoryDataService : IHistoryDataContract
    {
        public IRepository<HistoryData, long> HistoryDataRepository { get; set; }

        /// <summary>
        /// 获取历史数据查询数据集
        /// </summary>
        public IQueryable<HistoryData> HistoryDatas => HistoryDataRepository.Entities;

        /// <summary>
        /// 添加历史数据
        /// </summary>
        /// <param name="inputDtos">要添加的Opc Ua 历史数据DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> Add(params HistoryDataInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //foreach (BusinessNodeInputDto dtoData in inputDtos)
            //{
            //    if (string.IsNullOrEmpty(dtoData.BusinessName))
            //        return new OperationResult(OperationResultType.Error, "请正确填写业务名称，该组数据不被存储。");
            //    if (HistoryDataRepository.CheckExists(x => x.BusinessName == dtoData.BusinessName))
            //        return new OperationResult(OperationResultType.Error, $"业务名称 {dtoData.BusinessName} 的数据已存在，该组数据不被存储。");
            //}
            HistoryDataRepository.UnitOfWork.BeginTransaction();
            var result = await HistoryDataRepository.InsertAsync(inputDtos);
            HistoryDataRepository.UnitOfWork.Commit();
            return result;

            //inputDtos.CheckNotNull("inputDtos");
            //HistoryDataRepository.UnitOfWork.BeginTransaction();
            //List<Guid> ids = new List<Guid>();
            //List<HistoryData> commOpcUaHistories = new List<HistoryData>();
            //for (int i = 0; i < inputDtos.Length; i++)
            //{
            //    HistoryData commOpcUaHistory = inputDtos[i].MapTo<HistoryData>();
            //    ids.Add(commOpcUaHistory.Id);
            //    commOpcUaHistories.Add(commOpcUaHistory);
            //}

            //int count = HistoryDataRepository.InsertAsync(commOpcUaHistories.AsEnumerable());
            //HistoryDataRepository.UnitOfWork.Commit();

            //return count > 0 ? new OperationResult(OperationResultType.Success,
            //                    ids.Count > 0
            //                        ? "信息“{0}”逻辑删除成功".FormatWith(ids.ExpandAndToString())
            //                        : "{0}个信息逻辑删除成功".FormatWith(count))
            //                    : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 检查历史数据信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的历史数据编号</param>
        /// <returns>历史数据是否存在</returns>
        public bool CheckHistoryDataExists(Expression<Func<HistoryData, bool>> predicate, long id) => HistoryDataRepository.CheckExists(predicate, id);

        /// <summary>
        /// 物理删除历史数据
        /// </summary>
        /// <param name="ids">要删除的历史数据编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> Delete(params long[] ids)
        {
            ids.CheckNotNull("ids");

            HistoryDataRepository.UnitOfWork.BeginTransaction();
            var result = await HistoryDataRepository.DeleteAsync(ids);
            HistoryDataRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 更新历史数据信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的历史数据DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> Edit(params HistoryDataInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            HistoryDataRepository.UnitOfWork.BeginTransaction();
            var result = await HistoryDataRepository.UpdateAsync(inputDtos);
            HistoryDataRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 逻辑删除Opc Ua 历史数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        //public async Task<OperationResult> RecycleCommOpcUaHistory(params CommOpcUaHistory[] data)
        //{
        //    data.CheckNotNull("data");
        //    int count = 0;
        //    try
        //    {
        //        CommOpcUaHistoryRepository.UnitOfWork.BeginTransaction();
        //        count = await CommOpcUaHistoryRepository.RecycleAsync(data);
        //        CommOpcUaHistoryRepository.UnitOfWork.Commit();
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
        //    foreach (var entity in data )
        //    {
        //        names.Add(entity.NodeId.ToString());
        //    }
        //    return  count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        /// <summary>
        /// 恢复逻辑删除Opc Ua 历史数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        //public async Task<OperationResult> RestoreCommOpcUaHistory(params CommOpcUaHistory[] data)
        //{
        //    data.CheckNotNull("data");
        //    int count = 0;
        //    try
        //    {
        //        CommOpcUaHistoryRepository.UnitOfWork.BeginTransaction();
        //        count = await CommOpcUaHistoryRepository.RestoreAsync(data);
        //        CommOpcUaHistoryRepository.UnitOfWork.Commit();
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
        //    foreach (var entity in data )
        //    {
        //        names.Add(entity.NodeId.ToString());
        //    }
        //    return  count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}
    }
}
