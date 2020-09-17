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
    /// 供应商信息服务
    /// </summary>
    public class MatSupplierInfoService : IMatSupplierInfoContract
    {

        /// <summary>
        /// 供应商信息实体仓储
        /// </summary>
        public IRepository<MatSupplierInfo, Guid> MatSupplierRepository { get; set; }
        //

        /// <summary>
        /// 查询供应商信息
        /// </summary>
        public IQueryable<MatSupplierInfo> MatSupplierInfos
        {
            get { return MatSupplierRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MatSupplierInfo, bool>> predicate, Guid id)
        {
            return MatSupplierRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加供应商信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatSupplierInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.SupplierCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写供应商编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.SupplierName))
                    return new OperationResult(OperationResultType.Error, "请正确填写供应商名称，该组数据不被存储。");
                //
                if (MatSupplierRepository.CheckExists(x => x.SupplierCode == dtoData.SupplierCode))
                    return new OperationResult(OperationResultType.Error, $"供应商编号 {dtoData.SupplierCode} 的数据已存在，该组数据不被存储。");
                if (MatSupplierRepository.CheckExists(x => x.SupplierName == dtoData.SupplierName))
                    return new OperationResult(OperationResultType.Error, $"供应商名称 {dtoData.SupplierName} 的数据已存在，该组数据不被存储。");
            }
            MatSupplierRepository.UnitOfWork.BeginTransaction();
            var result = await MatSupplierRepository.InsertAsync(inputDtos);
            MatSupplierRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新供应商信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MatSupplierInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatSupplierInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.SupplierCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写供应商编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.SupplierName))
                    return new OperationResult(OperationResultType.Error, "请正确填写供应商名称，该组数据不被存储。");
                //
                if (MatSupplierRepository.CheckExists(x => x.SupplierCode == dtoData.SupplierCode && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"供应商编号 {dtoData.SupplierCode} 的数据已存在，该组数据不被存储。");
                if (MatSupplierRepository.CheckExists(x => x.SupplierName == dtoData.SupplierName && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"供应商名称 {dtoData.SupplierName} 的数据已存在，该组数据不被存储。");
            }
            //
            MatSupplierRepository.UnitOfWork.BeginTransaction();
            var result = await MatSupplierRepository.UpdateAsync(inputDtos);
            MatSupplierRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除供应商信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatSupplierRepository.UnitOfWork.BeginTransaction();
            var result = await MatSupplierRepository.DeleteAsync(ids);
            MatSupplierRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
