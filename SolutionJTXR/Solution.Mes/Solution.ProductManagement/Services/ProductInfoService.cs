using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.ProductManagement.Contracts;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.ProductManagement.Services
{
    public class ProductInfoService : IProductInfoContract
    {
        /// <summary>
        /// 产品信息实体仓储
        /// </summary>
        public IRepository<ProductInfo, Guid> ProductInfoRepository { get; set; }

        public IRepository<ProductTypeInfo, Guid> ProductTypeInfoRepository { get; set; }
        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }

        /// <summary>
        /// 查询产品信息
        /// </summary>
        public IQueryable<ProductInfo> ProductInfos
        {
            get { return ProductInfoRepository.Entities; }
        }
        /// <summary>
        /// 增加产品信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params ProductInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写产品编号！");
                if (string.IsNullOrEmpty(dtoData.ProductName))
                    return new OperationResult(OperationResultType.Error, "请正确填写产品名称！");
                if (ProductInfoRepository.CheckExists(x => x.ProductCode == dtoData.ProductCode))
                    return new OperationResult(OperationResultType.Error, "该产品编号已存在，无法保存！");
                if (ProductInfoRepository.CheckExists(x => x.ProductName == dtoData.ProductName))
                    return new OperationResult(OperationResultType.Error, "该产品名称已存在，无法保存！");
                dtoData.ProductType = ProductTypeInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductType_Id).FirstOrDefault();
                if (Equals(dtoData.ProductType, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的产品类别不存在,无法保存！");
                }
            }
            ProductInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductInfoRepository.InsertAsync(inputDtos);
            ProductInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<ProductInfo, bool>> predicate, Guid id)
        {
            return ProductInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除产品信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count = ProductionRuleInfoRepository.Entities.Where(m => m.Product.Id == id).Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, "产品数据关联配方信息，不能被删除。");
                }
            }
            ProductInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductInfoRepository.DeleteAsync(ids);
            ProductInfoRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params ProductInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写产品编号！");
                if (string.IsNullOrEmpty(dtoData.ProductName))
                    return new OperationResult(OperationResultType.Error, "请正确填写产品名称！");
                if (ProductInfoRepository.CheckExists(x => x.ProductCode == dtoData.ProductCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该产品编号已存在，无法保存！");
                if (ProductInfoRepository.CheckExists(x => x.ProductName == dtoData.ProductName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该产品名称已存在，无法保存！");
                dtoData.ProductType = ProductTypeInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductType_Id).FirstOrDefault();
                if (Equals(dtoData.ProductType, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的产品类别不存在,无法保存！");
                }
            }
            ProductInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductInfoRepository.UpdateAsync(inputDtos);
            ProductInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
