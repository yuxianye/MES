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
    /// 任务服务
    /// </summary>
    public class TaskInfoService : ITaskInfoContract
    {
        /// <summary>
        /// 任务仓储
        /// </summary>
        public IRepository<TaskInfo, Guid> TaskInfoRepository { get; set; }

        /// <summary>
        /// 路径仓储
        /// </summary>
        public IRepository<RouteInfo, Guid> RouteInfoRepository { get; set; }

        /// <summary>
        /// 路段仓储
        /// </summary>
        public IRepository<RoadInfo, Guid> RoadInfoRepository { get; set; }

        /// <summary>
        /// 地标点仓储
        /// </summary>
        public IRepository<MarkPointInfo, Guid> MarkPointInfoRepository { get; set; }

        /// <summary>
        /// 小车仓储
        /// </summary>
        public IRepository<AgvInfo, Guid> AgvInfoRepository { get; set; }

        /// <summary>
        /// 查询数据集
        /// </summary>
        public IQueryable<TaskInfo> TaskInfos
        {
            get { return TaskInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckTaskInfoExists(Expression<Func<TaskInfo, bool>> predicate, Guid id)
        {
            return TaskInfoRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 增加任务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params TaskInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.TaskName))
                    return new OperationResult(OperationResultType.Error, "请正确填写任务名称，该组数据不被存储。");
                if (TaskInfoRepository.CheckExists(x => x.TaskNo == dtoData.TaskNo /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, $"任务编号 {dtoData.TaskNo} 的数据已存在，该组数据不被存储。");
                if (TaskInfoRepository.CheckExists(x => x.TaskName == dtoData.TaskName/* && x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, $"任务名称 {dtoData.TaskName} 的数据已存在，该组数据不被存储。");
                dtoData.TargetPointInfo = MarkPointInfoRepository.GetByKey(dtoData.TargetPointInfo_Id);
                if (Equals(dtoData.TargetPointInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的目的地标点不存在,该组数据不被存储。");
                }
                dtoData.TaskCar = AgvInfoRepository.GetByKey(dtoData.TaskCar_Id);
                if (Equals(dtoData.TaskCar, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的AGV小车不存在,该组数据不被存储。");
                }
                dtoData.RouteInfo = RouteInfoRepository.GetByKey(dtoData.RouteInfo_Id);
                if (Equals(dtoData.RouteInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的路径不存在,该组数据不被存储。");
                }
            }

            TaskInfoRepository.UnitOfWork.BeginTransaction();
            var result = await TaskInfoRepository.InsertAsync(inputDtos);
            TaskInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新任务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params TaskInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.TaskName))
                    return new OperationResult(OperationResultType.Error, "请正确填写任务名称，该组数据不被存储。");
                if (TaskInfoRepository.CheckExists(x => x.TaskNo == dtoData.TaskNo && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"任务编号 {dtoData.TaskNo} 的数据已存在，该组数据不被存储。");
                if (TaskInfoRepository.CheckExists(x => x.TaskName == dtoData.TaskName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"任务名称 {dtoData.TaskName} 的数据已存在，该组数据不被存储。");

                dtoData.TargetPointInfo = MarkPointInfoRepository.GetByKey(dtoData.TargetPointInfo_Id);
                if (Equals(dtoData.TargetPointInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的目的地标点不存在,该组数据不被存储。");
                }
                dtoData.TaskCar = AgvInfoRepository.GetByKey(dtoData.TaskCar_Id);
                if (Equals(dtoData.TaskCar, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的AGV小车不存在,该组数据不被存储。");
                }
                dtoData.RouteInfo = RouteInfoRepository.GetByKey(dtoData.RouteInfo_Id);
                if (Equals(dtoData.RouteInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的路径不存在,该组数据不被存储。");
                }
            }
            TaskInfoRepository.UnitOfWork.BeginTransaction();
            var result = await TaskInfoRepository.UpdateAsync(inputDtos);
            TaskInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除任务信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            TaskInfoRepository.UnitOfWork.BeginTransaction();
            var result = await TaskInfoRepository.DeleteAsync(ids);
            TaskInfoRepository.UnitOfWork.Commit();
            return result;
        }

    }
}
