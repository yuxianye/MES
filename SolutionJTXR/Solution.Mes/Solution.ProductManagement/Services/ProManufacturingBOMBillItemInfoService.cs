using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.ProductManagement.Contracts;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.ProductManagement.Services
{
   public  class ProManufacturingBOMBillItemInfoService:IProManufacturingBOMBillItemInfoContract
    {
        /// <summary>
        /// 制造清单明细信息实体仓储
        /// </summary>
        public IRepository<ProManufacturingBOMBillItemInfo, Guid> ProManufacturingBOMBillItemInfoRepository { get; set; }

        public IRepository<ProManufacturingBillInfo, Guid> ProManufacturingBillInfoRepository { get; set; }

        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }

        public IRepository<ProductionProcessInfo, Guid> ProductionProcessInfoRepository { get; set; }

        public IRepository<MaterialInfo, Guid> MaterialInfoRepository { get; set; }

        /// <summary>
        /// 查询制造清单明细信息
        /// </summary>
        public IQueryable<ProManufacturingBOMBillItemInfo> ProManufacturingBOMBillItemInfos
        {
            get { return ProManufacturingBOMBillItemInfoRepository.Entities; }
        }



        ///// <summary>
        ///// 增加制造清单明细
        ///// </summary>
        ///// <param name="inputDtos"></param>
        ///// <returns></returns>
        //public OperationResult Add(params ProManufacturingBOMBillItemInfoInputDto[] inputDtos)
        //{

        //    inputDtos.CheckNotNull("inputDtos");
        //    OperationResult result = ProManufacturingBOMBillItemInfoRepository.Insert(inputDtos,
        //        dto =>
        //        {
        //            ///因为制造清单明细表中 材料ID和设备ID两个字段还没加（需要等到设备管理和物料管理中加表之后再加），后续再校验数据
        //            //if (ProManufacturingBOMBillItemInfoRepository.CheckExists(m => m.ProductName == dto.ProductName))
        //            //{
        //            //    throw new Exception("产品名称为“{0}”的数据已存在，不能重复添加。".FormatWith(dto.ProductName));
        //            //}
        //            //if (ProManufacturingBOMBillItemInfoRepository.CheckExists(m => m.ProductCode == dto.ProductCode))
        //            //{
        //            //    throw new Exception("产品编号为“{0}”的数据已存在，不能重复添加。".FormatWith(dto.ProductCode));
        //            //}

        //            //if (string.IsNullOrEmpty(dto.ProductName) || string.IsNullOrEmpty(dto.ProductCode))
        //            //{
        //            //    throw new Exception("存在产品名称或者产品编号为空的数据，数据不合法，无法保存！");
        //            //}  ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dto.ProductionRule_Id).FirstOrDefault();
        //        },
        //        (dto, entity) =>
        //        {
        //            if (dto.ProManufacturingBill_Id != null)
        //            {
        //                ProManufacturingBillInfo proManufacturingBillInfo = ProManufacturingBillInfoRepository.TrackEntities.Where(m => m.Id == dto.ProManufacturingBill_Id).FirstOrDefault();
        //                if (proManufacturingBillInfo == null)
        //                {
        //                    throw new Exception("要增加制造清单明细对应的制造清单不存在。");
        //                }
        //                entity.ProManufacturingBill = proManufacturingBillInfo;
        //            }
        //            else
        //            {
        //                entity.ProManufacturingBill = null;
        //            }
        //            if (dto.ProductionRule_Id != null)
        //            {
        //                ProductionRuleInfo productionRuleInfo = ProductionRuleInfoRepository.TrackEntities.Where(m => m.Id == dto.ProductionRule_Id).FirstOrDefault();
        //                if (productionRuleInfo == null)
        //                {
        //                    throw new Exception("要增加制造清单明细对应的配方不存在。");
        //                }
        //                entity.ProductionRule = productionRuleInfo;
        //            }
        //            else
        //            {
        //                entity.ProductionRule = null;
        //            }
        //            if (dto.ProductionProcess_Id != null)
        //            {
        //                ProductionProcessInfo productionProcessInfo = ProductionProcessInfoRepository.TrackEntities.Where(m => m.Id == dto.ProductionProcess_Id).FirstOrDefault();
        //                if (productionProcessInfo == null)
        //                {
        //                    throw new Exception("要增加制造清单明细对应的工序不存在。");
        //                }
        //                entity.ProductionProcess = productionProcessInfo;
        //            }
        //            else
        //            {
        //                entity.ProductionProcess = null;
        //            }
        //            return entity;
        //        });
        //    return result;
        //}
        /// <summary>
        /// 增加BOM明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params ProManufacturingBOMBillItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.ProManufacturingBill = ProManufacturingBillInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProManufacturingBill_Id).FirstOrDefault();
                if (Equals(dtoData.ProManufacturingBill, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的制造清单不存在,无法保存！");
                }
                dtoData.ProductionProcess = ProductionProcessInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionProcess_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionProcess, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的工序不存在,无法保存！");
                }
                dtoData.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Material_Id).FirstOrDefault();
                if (Equals(dtoData.Material, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的物料不存在,无法保存！");
                }
                if (Equals(dtoData.Quantity, null) || dtoData.Quantity <= 0)
                {
                    return new OperationResult(OperationResultType.Error, "物料数量应大于0,无法保存！");
                }
            }
            ProManufacturingBOMBillItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProManufacturingBOMBillItemInfoRepository.InsertAsync(inputDtos);
            ProManufacturingBOMBillItemInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<ProManufacturingBOMBillItemInfo, bool>> predicate, Guid id)
        {
            return ProManufacturingBOMBillItemInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除制造清单明细信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            ProManufacturingBOMBillItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProManufacturingBOMBillItemInfoRepository.DeleteAsync(ids);
            ProManufacturingBOMBillItemInfoRepository.UnitOfWork.Commit();
            return result;
        }

        ///// <summary>
        ///// 逻辑删除制造清单明细信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicDelete(params ProManufacturingBOMBillItemInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;
        //    try
        //    {
        //        ProManufacturingBOMBillItemInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await ProManufacturingBOMBillItemInfoRepository.RecycleAsync(enterinfos);
        //        ProManufacturingBOMBillItemInfoRepository.UnitOfWork.Commit();
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
        //        names.Add(data.Id.ToString());
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "Id为“{0}”信息逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        ///// <summary>
        ///// 逻辑还原制造清单明细信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicRestore(params ProManufacturingBOMBillItemInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;

        //    try
        //    {
        //        ProManufacturingBOMBillItemInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await ProManufacturingBOMBillItemInfoRepository.RestoreAsync(enterinfos);
        //        ProManufacturingBOMBillItemInfoRepository.UnitOfWork.Commit();
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
        //        names.Add(data.Id.ToString());
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "Id为“{0}”的信息逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        /// <summary>
        /// 更新制造清单明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params ProManufacturingBOMBillItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.ProManufacturingBill = ProManufacturingBillInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProManufacturingBill_Id).FirstOrDefault();
                if (Equals(dtoData.ProManufacturingBill, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的制造清单不存在,无法保存！");
                }
                dtoData.ProductionProcess = ProductionProcessInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ProductionProcess_Id).FirstOrDefault();
                if (Equals(dtoData.ProductionProcess, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的工序不存在,无法保存！");
                }
                dtoData.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Material_Id).FirstOrDefault();
                if (Equals(dtoData.Material, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的物料不存在,无法保存！");
                }
                if (Equals(dtoData.Quantity, null) || dtoData.Quantity <= 0)
                {
                    return new OperationResult(OperationResultType.Error, "物料数量应大于0,无法保存！");
                }
            }
            ProManufacturingBOMBillItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProManufacturingBOMBillItemInfoRepository.UpdateAsync(inputDtos);
            ProManufacturingBOMBillItemInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
