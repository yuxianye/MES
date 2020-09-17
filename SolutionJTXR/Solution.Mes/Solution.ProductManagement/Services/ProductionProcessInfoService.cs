using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.ProductManagement.Contracts;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using Solution.EnterpriseInformation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.ProductManagement.Services
{
    public class ProductionProcessInfoService : IProductionProcessInfoContract
    {
        /// <summary>
        /// 工序信息信息实体仓储
        /// </summary>
        public IRepository<ProductionProcessInfo, Guid> ProductionProcessInfoRepository { get; set; }


        public IRepository<EntProductionLineInfo, Guid> EntProductionLineInfoRepository { get; set; }
        public IRepository<ProductionRuleItemInfo, Guid> ProductionRuleItemInfoRepository { get; set; }
        public IRepository<ProManufacturingBOMBillItemInfo, Guid> ProManufacturingBOMBillItemInfoRepository { get; set; }
        public IRepository<ProManufacturingBORBillItemInfo, Guid> ProManufacturingBORBillItemInfoRepository { get; set; }

        /// <summary>
        /// 查询工序信息信息
        /// </summary>
        public IQueryable<ProductionProcessInfo> ProductionProcessInfos
        {
            get { return ProductionProcessInfoRepository.Entities; }
        }
        /// <summary>
        /// 增加工序信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params ProductionProcessInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductionProcessCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写工序编号！");
                if (string.IsNullOrEmpty(dtoData.ProductionProcessName))
                    return new OperationResult(OperationResultType.Error, "请正确填写工序名称！");
                if (ProductionProcessInfoRepository.CheckExists(x => x.ProductionProcessCode == dtoData.ProductionProcessCode))
                    return new OperationResult(OperationResultType.Error, "该工序编号已存在，无法保存！");
                if (ProductionProcessInfoRepository.CheckExists(x => x.ProductionProcessName == dtoData.ProductionProcessName))
                    return new OperationResult(OperationResultType.Error, "该工序名称已存在，无法保存！");
                dtoData.EntProductionLine = EntProductionLineInfoRepository.TrackEntities.Where(m => m.Id == dtoData.EntProductionLine_Id).FirstOrDefault();
                if (Equals(dtoData.EntProductionLine, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的生产线不存在,无法保存！");
                }
            }
            ProductionProcessInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionProcessInfoRepository.InsertAsync(inputDtos);
            ProductionProcessInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<ProductionProcessInfo, bool>> predicate, Guid id)
        {
            return ProductionProcessInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除工序信息信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count1 = ProductionRuleItemInfoRepository.Entities.Where(m => m.ProductionProcess.Id == id).Count();
                if (count1 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "工序数据关联配方明细信息，不能被删除。");
                }
                int count2 = ProManufacturingBOMBillItemInfoRepository.Entities.Where(m => m.ProductionProcess.Id == id).Count();
                if (count2 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "工序数据关联BOM明细信息，不能被删除。");
                }
                int count3 = ProManufacturingBORBillItemInfoRepository.Entities.Where(m => m.ProductionProcess.Id == id).Count();
                if (count3 > 0)
                {
                    return new OperationResult(OperationResultType.Error, "工序数据关联BOR明细信息，不能被删除。");
                }
            }
            ProductionProcessInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionProcessInfoRepository.DeleteAsync(ids);
            ProductionProcessInfoRepository.UnitOfWork.Commit();
            return result;
        }

        ///// <summary>
        ///// 逻辑删除工序信息信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicDelete(params ProductionProcessInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;
        //    try
        //    {
        //        ProductionProcessInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await ProductionProcessInfoRepository.RecycleAsync(enterinfos);
        //        ProductionProcessInfoRepository.UnitOfWork.Commit();
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
        //        names.Add(data.ProductionProcessName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        ///// <summary>
        ///// 逻辑还原工序信息信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicRestore(params ProductionProcessInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;

        //    try
        //    {
        //        ProductionProcessInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await ProductionProcessInfoRepository.RestoreAsync(enterinfos);
        //        ProductionProcessInfoRepository.UnitOfWork.Commit();
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
        //        names.Add(data.ProductionProcessName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        /// <summary>
        /// 更新工序信息信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params ProductionProcessInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.ProductionProcessCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写工序编号！");
                if (string.IsNullOrEmpty(dtoData.ProductionProcessName))
                    return new OperationResult(OperationResultType.Error, "请正确填写工序名称！");
                if (ProductionProcessInfoRepository.CheckExists(x => x.ProductionProcessCode == dtoData.ProductionProcessCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该工序编号已存在，无法保存！");
                if (ProductionProcessInfoRepository.CheckExists(x => x.ProductionProcessName == dtoData.ProductionProcessName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该工序名称已存在，无法保存！");
                dtoData.EntProductionLine = EntProductionLineInfoRepository.TrackEntities.Where(m => m.Id == dtoData.EntProductionLine_Id).FirstOrDefault();
                if (Equals(dtoData.EntProductionLine, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的生产线不存在,无法保存！");
                }
            }
            ProductionProcessInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProductionProcessInfoRepository.UpdateAsync(inputDtos);
            ProductionProcessInfoRepository.UnitOfWork.Commit();
            return result;
        }

    }
}
