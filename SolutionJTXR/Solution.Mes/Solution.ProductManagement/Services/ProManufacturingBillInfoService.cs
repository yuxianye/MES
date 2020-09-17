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
    public class ProManufacturingBillInfoService : IProManufacturingBillInfoContract
    {
        /// <summary>
        /// 制造清单信息实体仓储
        /// </summary>
        public IRepository<ProManufacturingBillInfo, Guid> ProManufacturingBillInfoRepository { get; set; }

        public IRepository<ProductInfo, Guid> ProductInfoRepository { get; set; }

        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }
        public IRepository<ProManufacturingBOMBillItemInfo, Guid> ProManufacturingBOMBillItemInfoRepository { get; set; }
        public IRepository<ProManufacturingBORBillItemInfo, Guid> ProManufacturingBORBillItemInfoRepository { get; set; }

        /// <summary>
        /// 查询制造清单信息
        /// </summary>
        public IQueryable<ProManufacturingBillInfo> ProManufacturingBillInfos
        {
            get { return ProManufacturingBillInfoRepository.Entities; }
        }



        /// <summary>
        /// 增加制造清单信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params ProManufacturingBillInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.BillCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写清单编号！");
                if (string.IsNullOrEmpty(dtoData.BillName))
                    return new OperationResult(OperationResultType.Error, "请正确填写清单名称！");
                if (ProManufacturingBillInfoRepository.CheckExists(x => x.BillCode == dtoData.BillCode))
                    return new OperationResult(OperationResultType.Error, "该清单编号已存在，无法保存！");
                if (ProManufacturingBillInfoRepository.CheckExists(x => x.BillName == dtoData.BillName))
                    return new OperationResult(OperationResultType.Error, "该清单名称已存在，无法保存！");
                dtoData.Product = ProductInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Product_Id).FirstOrDefault();
                if (Equals(dtoData.Product, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的产品不存在,无法保存！");
                }
                dtoData.ProductionRule = ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方不存在,无法保存！");
                }
                if (Equals(dtoData.BillType, null) || dtoData.BillType == 0)
                {
                    return new OperationResult(OperationResultType.Error, "请选择制造清单类型！");
                }
            }
            ProManufacturingBillInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProManufacturingBillInfoRepository.InsertAsync(inputDtos);
            ProManufacturingBillInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<ProManufacturingBillInfo, bool>> predicate, Guid id)
        {
            return ProManufacturingBillInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除制造清单信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count1 = ProManufacturingBOMBillItemInfoRepository.Entities.Where(m => m.ProManufacturingBill.Id == id).Count();
                if (count1 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "制造清单数据关联BOM明细信息，不能被删除。");
                }
                int count2 = ProManufacturingBORBillItemInfoRepository.Entities.Where(m => m.ProManufacturingBill.Id == id).Count();
                if (count2 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "制造清单数据关联BOR明细信息，不能被删除。");
                }
            }
            ProManufacturingBillInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProManufacturingBillInfoRepository.DeleteAsync(ids);
            ProManufacturingBillInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新制造清单信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params ProManufacturingBillInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.BillCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写清单编号！");
                if (string.IsNullOrEmpty(dtoData.BillName))
                    return new OperationResult(OperationResultType.Error, "请正确填写清单名称！");
                if (ProManufacturingBillInfoRepository.CheckExists(x => x.BillCode == dtoData.BillCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该清单编号已存在，无法保存！");
                if (ProManufacturingBillInfoRepository.CheckExists(x => x.BillName == dtoData.BillName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该清单名称已存在，无法保存！");
                dtoData.Product = ProductInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Product_Id).FirstOrDefault();
                if (Equals(dtoData.Product, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的产品不存在,无法保存！");
                }
                dtoData.ProductionRule = ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRule_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRule, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方不存在,无法保存！");
                }
                if (Equals(dtoData.BillType, null) || dtoData.BillType == 0)
                {
                    return new OperationResult(OperationResultType.Error, "请选择制造清单类型！");
                }
            }
            ProManufacturingBillInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProManufacturingBillInfoRepository.UpdateAsync(inputDtos);
            ProManufacturingBillInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
