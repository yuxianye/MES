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
    public class ProductTypeInfoService : IProductTypeInfoContract
    {
        /// <summary>
        /// 产品类别信息实体仓储
        /// </summary>
        public IRepository<ProductTypeInfo, Guid> ProductTypeInfoRepository { get; set; }
        public IRepository<ProductInfo, Guid> ProductInfoRepository { get; set; }

        /// <summary>
        /// 查询产品类别信息
        /// </summary>
        public IQueryable<ProductTypeInfo> ProductTypeInfos
        {
            get { return ProductTypeInfoRepository.Entities; }
        }
        /// <summary>
        /// 增加产品类别
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params ProductTypeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写产品类别编号！");
                if (string.IsNullOrEmpty(dtoData.ProductTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写产品类别名称！");
                if (ProductTypeInfoRepository.CheckExists(x => x.ProductTypeCode == dtoData.ProductTypeCode))
                    return new OperationResult(OperationResultType.Error, "该产品类别编号已存在，无法保存！");
                if (ProductTypeInfoRepository.CheckExists(x => x.ProductTypeName == dtoData.ProductTypeName))
                    return new OperationResult(OperationResultType.Error, "该产品类别名称已存在，无法保存！");
            }
            ProductTypeInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductTypeInfoRepository.InsertAsync(inputDtos);
            ProductTypeInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<ProductTypeInfo, bool>> predicate, Guid id)
        {
            return ProductTypeInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除产品类别信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count = ProductInfoRepository.Entities.Where(m => m.ProductType.Id == id).Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, "产品类别数据关联产品信息，不能被删除。");
                }
            }
            ProductTypeInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductTypeInfoRepository.DeleteAsync(ids);
            ProductTypeInfoRepository.UnitOfWork.Commit();
            return result;
        }

        ///// <summary>
        ///// 逻辑删除产品类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicDelete(params ProductTypeInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;
        //    try
        //    {
        //        ProductTypeInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await ProductTypeInfoRepository.RecycleAsync(enterinfos);
        //        ProductTypeInfoRepository.UnitOfWork.Commit();
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
        //    foreach (var data in enterinfos)
        //    {
        //        names.Add(data.ProductTypeName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        ///// <summary>
        ///// 逻辑还原产品类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicRestore(params ProductTypeInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;

        //    try
        //    {
        //        ProductTypeInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await ProductTypeInfoRepository.RestoreAsync(enterinfos);
        //        ProductTypeInfoRepository.UnitOfWork.Commit();
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
        //    foreach (var data in enterinfos)
        //    {
        //        names.Add(data.ProductTypeName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        /// <summary>
        /// 更新产品类别信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params ProductTypeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写产品类别编号！");
                if (string.IsNullOrEmpty(dtoData.ProductTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写产品类别名称！");
                if (ProductTypeInfoRepository.CheckExists(x => x.ProductTypeCode == dtoData.ProductTypeCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该产品类别编号已存在，无法保存！");
                if (ProductTypeInfoRepository.CheckExists(x => x.ProductTypeName == dtoData.ProductTypeName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该产品类别名称已存在，无法保存！");
            }
            ProductTypeInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductTypeInfoRepository.UpdateAsync(inputDtos);
            ProductTypeInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
