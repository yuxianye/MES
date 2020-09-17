﻿using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.MatWarehouseStorageManagement.Contracts
{
    /// <summary>
    /// 仓库类型信息契约接口
    /// </summary>
    public interface IMatWareHouseTypeInfoContract : IScopeDependency
    {
        #region 仓库类型信息业务

        /// <summary>
        /// 获取仓库类型信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MatWareHouseTypeInfo> MatWareHouseTypeInfos { get; }

        /// <summary>
        /// 检查组仓库类型信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的仓库类型信息编号</param>
        /// <returns>仓库类型信息是否存在</returns>
        bool CheckExists(Expression<Func<MatWareHouseTypeInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加仓库类型信息1
        /// </summary>
        /// <param name="inputDtos">要添加的仓库类型信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MatWareHouseTypeInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新仓库类型信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的仓库类型信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params MatWareHouseTypeInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除仓库类型信息
        /// </summary>
        /// <param name="ids">要删除的仓库类型信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
         
        #endregion
    }
}
