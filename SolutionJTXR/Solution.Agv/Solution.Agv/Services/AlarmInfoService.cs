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
    /// 报警信息服务
    /// </summary>
    public class AlarmInfoService : IAlarmInfoContract
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        public IRepository<AlarmInfo, Guid> AlarmInfoRepository { get; set; }

        /// <summary>
        /// 查询数据集
        /// </summary>
        public IQueryable<AlarmInfo> AlarmInfos
        {
            get { return AlarmInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckAlarmInfoExists(Expression<Func<AlarmInfo, bool>> predicate, Guid id)
        {
            return AlarmInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加报警信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params AlarmInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            AlarmInfoRepository.UnitOfWork.BeginTransaction();
            var result = await AlarmInfoRepository.InsertAsync(inputDtos);
            AlarmInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新报警信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params AlarmInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            AlarmInfoRepository.UnitOfWork.BeginTransaction();
            var result = await AlarmInfoRepository.UpdateAsync(inputDtos);
            AlarmInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除报警信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            AlarmInfoRepository.UnitOfWork.BeginTransaction();
            var result = await AlarmInfoRepository.DeleteAsync(ids);
            AlarmInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
