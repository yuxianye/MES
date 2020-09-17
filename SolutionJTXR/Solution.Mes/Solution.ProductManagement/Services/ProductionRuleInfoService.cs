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
    public class ProductionRuleInfoService : IProductionRuleInfoContract
    {
        /// <summary>
        /// 配方信息实体仓储
        /// </summary>
        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }

        public IRepository<ProductInfo, Guid> ProductInfoRepository { get; set; }

        public IRepository<ProductionRuleStatusInfo, Guid> ProductionRuleStatusInfoRepository { get; set; }

        public IRepository<ProductionRuleItemInfo, Guid> ProductionRuleItemInfoRepository { get; set; }

        public IRepository<ProManufacturingBOMBillItemInfo, Guid> ProManufacturingBOMBillItemInfoRepository { get; set; }

        public IRepository<ProManufacturingBORBillItemInfo, Guid> ProManufacturingBORBillItemInfoRepository { get; set; }
        public IRepository<ProManufacturingBillInfo, Guid> ProManufacturingBillInfoRepository { get; set; }

        /// <summary>
        /// 查询配方信息
        /// </summary>
        public IQueryable<ProductionRuleInfo> ProductionRuleInfos
        {
            get { return ProductionRuleInfoRepository.Entities; }
        }

        /// <summary>
        /// 增加配方信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params ProductionRuleInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductionRuleVersion))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方版本号！");
                if (string.IsNullOrEmpty(dtoData.ProductionRuleName))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方名称！");
                if (ProductionRuleInfoRepository.CheckExists(x => x.ProductionRuleVersion == dtoData.ProductionRuleVersion))
                    return new OperationResult(OperationResultType.Error, "该配方版本号已存在，无法保存！");
                if (ProductionRuleInfoRepository.CheckExists(x => x.ProductionRuleName == dtoData.ProductionRuleName))
                    return new OperationResult(OperationResultType.Error, "该配方名称已存在，无法保存！");
                dtoData.Product = ProductInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Product_Id).FirstOrDefault();
                if (Equals(dtoData.Product, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的产品不存在,无法保存！");
                }
                //默认新增加的配方信息 配方状态="未审核"
                dtoData.ProductionRuleStatus = ProductionRuleStatusInfoRepository.TrackEntities.Where(m => m.ProductionRuleStatusCode == "UnCommit").FirstOrDefault();
                if (Equals(dtoData.ProductionRuleStatus, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的配方状态不存在,无法保存！");
                }
                dtoData.ProductionRuleStatus_Id = dtoData.ProductionRuleStatus.Id;
                if (Equals(dtoData.EffectiveDate, null) || Equals(dtoData.ExpirationDate, null))
                    return new OperationResult(OperationResultType.Error, "请正确填写生效时间或失效时间！");
                if (dtoData.EffectiveDate < DateTime.Now)
                    return new OperationResult(OperationResultType.Error, "生效时间小于系统当前时间，无法保存！");
                if (dtoData.ExpirationDate < dtoData.EffectiveDate)
                    return new OperationResult(OperationResultType.Error, "失效时间小于生效时间，无法保存！");
                if (Equals(dtoData.Duration, null) || dtoData.Duration < 0)
                {
                    return new OperationResult(OperationResultType.Error, "配方时长应大于0，无法保存！");
                }
            }
            ProductionRuleInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleInfoRepository.InsertAsync(inputDtos);
            ProductionRuleInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<ProductionRuleInfo, bool>> predicate, Guid id)
        {
            return ProductionRuleInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除配方信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count1 = ProductionRuleItemInfoRepository.Entities.Where(m => m.ProductionRule.Id == id).Count();
                if (count1 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "配方数据关联配方明细信息，不能被删除。");
                }
                int count2 = ProManufacturingBillInfoRepository.Entities.Where(m => m.ProductionRule.Id == id).Count();
                if (count2 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "配方数据关联制造清单信息，不能被删除。");
                }
            }
            ProductionRuleInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleInfoRepository.DeleteAsync(ids);
            ProductionRuleInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新配方信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params ProductionRuleInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductionRuleVersion))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方版本号！");
                if (string.IsNullOrEmpty(dtoData.ProductionRuleName))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方名称！");
                if (ProductionRuleInfoRepository.CheckExists(x => x.ProductionRuleVersion == dtoData.ProductionRuleVersion && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该配方版本号已存在，无法保存！");
                if (ProductionRuleInfoRepository.CheckExists(x => x.ProductionRuleName == dtoData.ProductionRuleName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该配方名称已存在，无法保存！");
                dtoData.Product = ProductInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Product_Id).FirstOrDefault();
                if (Equals(dtoData.Product, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的产品不存在,无法保存！");
                }
                dtoData.ProductionRuleStatus = ProductionRuleStatusInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRuleStatus_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRuleStatus, null) || dtoData.ProductionRuleStatus.ProductionRuleStatusCode != "UnCommit")
                {
                    return new OperationResult(OperationResultType.Error, $"对应的配方状态不存在，或配方状态为{dtoData.ProductionRuleStatus.ProductionRuleStatusName},无法保存！");
                }
                if (Equals(dtoData.EffectiveDate, null) || Equals(dtoData.ExpirationDate, null))
                    return new OperationResult(OperationResultType.Error, "请正确填写生效时间或失效时间！");
                if (dtoData.EffectiveDate < DateTime.Now)
                    return new OperationResult(OperationResultType.Error, "生效时间小于系统当前时间，无法保存！");
                if (dtoData.ExpirationDate < dtoData.EffectiveDate)
                    return new OperationResult(OperationResultType.Error, "失效时间小于生效时间，无法保存！");
                if (Equals(dtoData.Duration, null) || dtoData.Duration < 0)
                {
                    return new OperationResult(OperationResultType.Error, "配方时长应大于0，无法保存！");
                }
            }
            ProductionRuleInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleInfoRepository.UpdateAsync(inputDtos);
            ProductionRuleInfoRepository.UnitOfWork.Commit();
            return result;
        }


        /// <summary>
        /// 审核配方信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Audit(params ProductionRuleInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductionRuleVersion))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方版本号！");
                if (string.IsNullOrEmpty(dtoData.ProductionRuleName))
                    return new OperationResult(OperationResultType.Error, "请正确填写配方名称！");
                if (ProductionRuleInfoRepository.CheckExists(x => x.ProductionRuleVersion == dtoData.ProductionRuleVersion && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该配方版本号已存在，无法保存！");
                if (ProductionRuleInfoRepository.CheckExists(x => x.ProductionRuleName == dtoData.ProductionRuleName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该配方名称已存在，无法保存！");
                dtoData.Product = ProductInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Product_Id).FirstOrDefault();
                if (Equals(dtoData.Product, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的产品不存在,无法保存！");
                }
                dtoData.ProductionRuleStatus = ProductionRuleStatusInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionRuleStatus_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionRuleStatus, null) || dtoData.ProductionRuleStatus.ProductionRuleStatusCode == "UnCommit")
                {
                    return new OperationResult(OperationResultType.Error, $"对应的配方状态不存在，或配方状态为{dtoData.ProductionRuleStatus.ProductionRuleStatusName},无法保存！");
                }
                int count1 = ProductionRuleItemInfoRepository.Entities.Where(m => m.ProductionRule.Id == dtoData.Id).Count();
                int count2 = ProManufacturingBOMBillItemInfoRepository.Entities.Where(m => m.ProManufacturingBill.ProductionRule.Id == dtoData.Id).Count();
                int count3 = ProManufacturingBORBillItemInfoRepository.Entities.Where(m => m.ProManufacturingBill.ProductionRule.Id == dtoData.Id).Count();
                if (count1 < 1 || count2 < 1 || count3 < 1)
                { return new OperationResult(OperationResultType.Error, $"该配方对应的配方明细个数为{count1},BOM清单个数为{count2},BOR清单个数为{count3},无法保存！"); }
            }
            ProductionRuleInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionRuleInfoRepository.UpdateAsync(inputDtos);
            ProductionRuleInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
