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
    /// 路段服务
    /// </summary>
    public class RoadInfoService : IRoadInfoContract
    {
        /// <summary>
        /// 路段仓储
        /// </summary>
        public IRepository<RoadInfo, Guid> RoadInfoRepository { get; set; }

        /// <summary>
        /// 地标点仓储
        /// </summary>
        public IRepository<MarkPointInfo, Guid> MarkPointInfoRepository { get; set; }

        /// <summary>
        /// 路段数据集
        /// </summary>
        public IQueryable<RoadInfo> RoadInfos
        {
            get { return RoadInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckRoadInfoExists(Expression<Func<RoadInfo, bool>> predicate, Guid id)
        {
            return RoadInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加路段信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params RoadInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.RoadNo))
                    return new OperationResult(OperationResultType.Error, "请正确填写路段编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.RoadName))
                    return new OperationResult(OperationResultType.Error, "请正确填写路段名称，该组数据不被存储。");
                if (RoadInfoRepository.CheckExists(x => x.RoadNo == dtoData.RoadNo))
                    return new OperationResult(OperationResultType.Error, $"路段编号 {dtoData.RoadNo} 的数据已存在，该组数据不被存储。");
                if (RoadInfoRepository.CheckExists(x => x.RoadName == dtoData.RoadName))
                    return new OperationResult(OperationResultType.Error, $"路段名称 {dtoData.RoadName} 的数据已存在，该组数据不被存储。");
                dtoData.StartMarkPointInfo = MarkPointInfoRepository.GetByKey(dtoData.StartMarkPointInfo_Id);
                if (Equals(dtoData.StartMarkPointInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的开始地标点不存在,该组数据不被存储。");
                }
                dtoData.EndMarkPointInfo = MarkPointInfoRepository.GetByKey(dtoData.EndMarkPointInfo_Id);
                if (Equals(dtoData.EndMarkPointInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的结束地标点不存在,该组数据不被存储。");
                }
            }
            RoadInfoRepository.UnitOfWork.BeginTransaction();
            var result = await RoadInfoRepository.InsertAsync(inputDtos);
            RoadInfoRepository.UnitOfWork.Commit();
            return result;

        }

        /// <summary>
        /// 更新路段信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params RoadInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.RoadNo))
                    return new OperationResult(OperationResultType.Error, "请正确填写路段编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.RoadName))
                    return new OperationResult(OperationResultType.Error, "请正确填写路段名称，该组数据不被存储。");
                if (RoadInfoRepository.CheckExists(x => x.RoadNo == dtoData.RoadNo && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"路段编号 {dtoData.RoadNo} 的数据已存在，该组数据不被存储。");
                if (RoadInfoRepository.CheckExists(x => x.RoadName == dtoData.RoadName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"路段名称 {dtoData.RoadName} 的数据已存在，该组数据不被存储。");
                dtoData.StartMarkPointInfo = MarkPointInfoRepository.GetByKey(dtoData.StartMarkPointInfo_Id);
                if (Equals(dtoData.StartMarkPointInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的开始地标点不存在,该组数据不被存储。");
                }
                dtoData.EndMarkPointInfo = MarkPointInfoRepository.GetByKey(dtoData.EndMarkPointInfo_Id);
                if (Equals(dtoData.EndMarkPointInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的结束地标点不存在,该组数据不被存储。");
                }
            }
            RoadInfoRepository.UnitOfWork.BeginTransaction();
            var result = await RoadInfoRepository.UpdateAsync(inputDtos);
            RoadInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除路段信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            RoadInfoRepository.UnitOfWork.BeginTransaction();
            var result = await RoadInfoRepository.DeleteAsync(ids);
            RoadInfoRepository.UnitOfWork.Commit();
            return result;
        }


        /// <summary>
        /// 逻辑删除路段信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        public async Task<OperationResult> LogicDelete(params RoadInfo[] enterinfos)
        {
            enterinfos.CheckNotNull("enterinfos");
            int count = 0;
            try
            {
                RoadInfoRepository.UnitOfWork.BeginTransaction();
                count = await RoadInfoRepository.RecycleAsync(enterinfos);
                RoadInfoRepository.UnitOfWork.Commit();
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
            foreach (var data in enterinfos)
            {
                names.Add(data.RoadName);
            }
            return count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑删除成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);
        }


        /// <summary>
        /// 逻辑还原路段信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        public async Task<OperationResult> LogicRestore(params RoadInfo[] enterinfos)
        {
            enterinfos.CheckNotNull("enterinfos");
            int count = 0;

            try
            {
                RoadInfoRepository.UnitOfWork.BeginTransaction();
                count = await RoadInfoRepository.RestoreAsync(enterinfos);
                RoadInfoRepository.UnitOfWork.Commit();
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
            foreach (var data in enterinfos)
            {
                names.Add(data.RoadName);
            }
            return count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑还原成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);
        }

    }
}
