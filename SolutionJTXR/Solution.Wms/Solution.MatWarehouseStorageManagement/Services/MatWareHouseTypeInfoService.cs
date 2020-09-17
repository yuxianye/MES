using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EnterpriseInformation.Models;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.MatWarehouseStorageManagement.Services
{
    /// <summary>
    /// 仓库类型信息服务
    /// </summary>
    public class MatWareHouseTypeInfoService : IMatWareHouseTypeInfoContract
    {

        /// <summary>
        /// 仓库类型信息实体仓储
        /// </summary>
        public IRepository<MatWareHouseTypeInfo, Guid> MatWareHouseTypeRepository { get; set; }
        //

        /// <summary>
        /// 查询仓库类型信息
        /// </summary>
        public IQueryable<MatWareHouseTypeInfo> MatWareHouseTypeInfos
        {
            get { return MatWareHouseTypeRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MatWareHouseTypeInfo, bool>> predicate, Guid id)
        {
            return MatWareHouseTypeRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加仓库类型信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatWareHouseTypeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.WareHouseTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库类型编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.WareHouseTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库类型名称，该组数据不被存储。");
                //
                if (MatWareHouseTypeRepository.CheckExists(x => x.WareHouseTypeCode == dtoData.WareHouseTypeCode))
                    return new OperationResult(OperationResultType.Error, $"仓库类型编号 {dtoData.WareHouseTypeCode} 的数据已存在，该组数据不被存储。");
                if (MatWareHouseTypeRepository.CheckExists(x => x.WareHouseTypeName == dtoData.WareHouseTypeName))
                    return new OperationResult(OperationResultType.Error, $"仓库类型名称 {dtoData.WareHouseTypeName} 的数据已存在，该组数据不被存储。");
            }
            MatWareHouseTypeRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseTypeRepository.InsertAsync(inputDtos);
            MatWareHouseTypeRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新仓库类型信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MatWareHouseTypeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatWareHouseTypeInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.WareHouseTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库类型编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.WareHouseTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库类型名称，该组数据不被存储。");
                //
                if (MatWareHouseTypeRepository.CheckExists(x => x.WareHouseTypeCode == dtoData.WareHouseTypeCode && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"仓库类型编号 {dtoData.WareHouseTypeCode} 的数据已存在，该组数据不被存储。");
                if (MatWareHouseTypeRepository.CheckExists(x => x.WareHouseTypeName == dtoData.WareHouseTypeName && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"仓库类型名称 {dtoData.WareHouseTypeName} 的数据已存在，该组数据不被存储。");
            }
            //
            MatWareHouseTypeRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseTypeRepository.UpdateAsync(inputDtos);
            MatWareHouseTypeRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除仓库类型信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatWareHouseTypeRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseTypeRepository.DeleteAsync(ids);
            MatWareHouseTypeRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
