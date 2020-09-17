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
using Solution.MatWarehouseStorageManagement.Models;

namespace Solution.Agv.Services
{
    /// <summary>
    /// 任务服务
    /// </summary>
    public class TaskDetailedInfoService : ITaskDetailedInfoContract
    {
        /// <summary>
        /// 任务详细仓储
        /// </summary>
        public IRepository<TaskDetailedInfo, Guid> TaskDetailedInfoRepository { get; set; }

        /// <summary>
        /// 任务仓储
        /// </summary>
        public IRepository<TaskInfo, Guid> TaskInfoRepository { get; set; }

        /// <summary>
        /// 任务仓储
        /// </summary>
        public IRepository<MaterialInfo, Guid> MaterialInfoRepository { get; set; }

        /// <summary>
        /// 查询数据集
        /// </summary>
        public IQueryable<TaskDetailedInfo> TaskDetailedInfos
        {
            get { return TaskDetailedInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckTaskDetailedInfoExists(Expression<Func<TaskDetailedInfo, bool>> predicate, Guid id)
        {
            return TaskDetailedInfoRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 增加任务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params TaskDetailedInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.TaskInfo = TaskInfoRepository.GetByKey(dtoData.TaskInfo_Id);
                if (Equals(dtoData.TaskInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的Agv任务不存在,该组数据不被存储。");
                }
                dtoData.Material = MaterialInfoRepository.GetByKey(dtoData.Material_Id);
                if (Equals(dtoData.Material, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的物料不存在,该组数据不被存储。");
                }
            }

            TaskDetailedInfoRepository.UnitOfWork.BeginTransaction();
            var result = await TaskDetailedInfoRepository.InsertAsync(inputDtos);
            TaskDetailedInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新任务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params TaskDetailedInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.TaskInfo = TaskInfoRepository.GetByKey(dtoData.TaskInfo_Id);
                if (Equals(dtoData.TaskInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的Agv任务不存在,该组数据不被存储。");
                }
                dtoData.Material = MaterialInfoRepository.GetByKey(dtoData.Material_Id);
                if (Equals(dtoData.Material, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的物料不存在,该组数据不被存储。");
                }
            }
            TaskDetailedInfoRepository.UnitOfWork.BeginTransaction();
            var result = await TaskDetailedInfoRepository.UpdateAsync(inputDtos);
            TaskDetailedInfoRepository.UnitOfWork.Commit();
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
            TaskDetailedInfoRepository.UnitOfWork.BeginTransaction();
            var result = await TaskDetailedInfoRepository.DeleteAsync(ids);
            TaskDetailedInfoRepository.UnitOfWork.Commit();
            return result;
        }

    }
}
