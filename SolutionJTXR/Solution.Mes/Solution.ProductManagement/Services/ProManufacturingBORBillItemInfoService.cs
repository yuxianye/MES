using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.ProductManagement.Contracts;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using Solution.EquipmentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.ProductManagement.Services
{
    public class ProManufacturingBORBillItemInfoService : IProManufacturingBORBillItemInfoContract
    {
        /// <summary>
        /// BOR清单明细信息实体仓储
        /// </summary>
        public IRepository<ProManufacturingBORBillItemInfo, Guid> ProManufacturingBORBillItemInfoRepository { get; set; }

        public IRepository<ProManufacturingBillInfo, Guid> ProManufacturingBillInfoRepository { get; set; }

        public IRepository<ProductionRuleInfo, Guid> ProductionRuleInfoRepository { get; set; }

        public IRepository<ProductionProcessInfo, Guid> ProductionProcessInfoRepository { get; set; }

        public IRepository<EquEquipmentInfo, Guid> EquipmentInfoRepository { get; set; }

        /// <summary>
        /// 查询BOR清单明细信息
        /// </summary>
        public IQueryable<ProManufacturingBORBillItemInfo> ProManufacturingBORBillItemInfos
        {
            get { return ProManufacturingBORBillItemInfoRepository.Entities; }
        }



        ///// <summary>
        ///// 增加BOR清单明细
        ///// </summary>
        ///// <param name="inputDtos"></param>
        ///// <returns></returns>
        //public OperationResult Add(params ProManufacturingBORBillItemInfoInputDto[] inputDtos)
        //{

        //    inputDtos.CheckNotNull("inputDtos");
        //    OperationResult result = ProManufacturingBORBillItemInfoRepository.Insert(inputDtos,
        //        dto =>
        //        {
        //            ///因为BOR清单明细表中 材料ID和设备ID两个字段还没加（需要等到设备管理和物料管理中加表之后再加），后续再校验数据
        //            //if (ProManufacturingBORBillItemInfoRepository.CheckExists(m => m.ProductName == dto.ProductName))
        //            //{
        //            //    throw new Exception("产品名称为“{0}”的数据已存在，不能重复添加。".FormatWith(dto.ProductName));
        //            //}
        //            //if (ProManufacturingBORBillItemInfoRepository.CheckExists(m => m.ProductCode == dto.ProductCode))
        //            //{
        //            //    throw new Exception("产品编号为“{0}”的数据已存在，不能重复添加。".FormatWith(dto.ProductCode));
        //            //}

        //            //if (string.IsNullOrEmpty(dto.ProductName) || string.IsNullOrEmpty(dto.ProductCode))
        //            //{
        //            //    throw new Exception("存在产品名称或者产品编号为空的数据，数据不合法，无法保存！");
        //            //}
        //        },
        //        (dto, entity) =>
        //        {
        //            if (dto.ProManufacturingBill_Id != null)
        //            {
        //                ProManufacturingBillInfo proManufacturingBillInfo = ProManufacturingBORBillInfoRepository.GetByKey(dto.ProManufacturingBill_Id);
        //                if (proManufacturingBillInfo == null)
        //                {
        //                    throw new Exception("要增加BOR清单明细对应的BOR清单不存在。");
        //                }
        //                entity.ProManufacturingBill = proManufacturingBillInfo;
        //            }
        //            else
        //            {
        //                entity.ProductionRule = null;
        //            }
        //            if (dto.ProductionRule_Id != null)
        //            {
        //                ProductionRuleInfo productionRuleInfo = ProductionRuleInfoRepository.GetByKey(dto.ProductionRule_Id);
        //                if (productionRuleInfo == null)
        //                {
        //                    throw new Exception("要增加BOR清单明细对应的配方不存在。");
        //                }
        //                entity.ProductionRule = productionRuleInfo;
        //            }
        //            else
        //            {
        //                entity.ProductionRule = null;
        //            }
        //            if (dto.ProductionProcess_Id != null)
        //            {
        //                ProductionProcessInfo productionProcessInfo = ProductionProcessInfoRepository.GetByKey(dto.ProductionProcess_Id);
        //                if (productionProcessInfo == null)
        //                {
        //                    throw new Exception("要增加BOR清单明细对应的工序不存在。");
        //                }
        //                entity.ProductionProcess = productionProcessInfo;
        //            }
        //            else
        //            {
        //                entity.ProductionRule = null;
        //            }
        //            return entity;
        //        });
        //    return result;
        //}
        /// <summary>
        /// 增加BOR明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params ProManufacturingBORBillItemInfoInputDto[] inputDtos)
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
                dtoData.Equipment = EquipmentInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Equipment_Id).FirstOrDefault();
                if (Equals(dtoData.Equipment, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的设备不存在,无法保存！");
                }
                if (Equals(dtoData.Quantity, null) || dtoData.Quantity <= 0)
                {
                    return new OperationResult(OperationResultType.Error, "设备数量应大于0,无法保存！");
                }
            }
            ProManufacturingBORBillItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProManufacturingBORBillItemInfoRepository.InsertAsync(inputDtos);
            ProManufacturingBORBillItemInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<ProManufacturingBORBillItemInfo, bool>> predicate, Guid id)
        {
            return ProManufacturingBORBillItemInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除BOR清单明细信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            ProManufacturingBORBillItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProManufacturingBORBillItemInfoRepository.DeleteAsync(ids);
            ProManufacturingBORBillItemInfoRepository.UnitOfWork.Commit();
            return result;
        }

        ///// <summary>
        ///// 逻辑删除BOR清单明细信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicDelete(params ProManufacturingBORBillItemInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;
        //    try
        //    {
        //        ProManufacturingBORBillItemInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await ProManufacturingBORBillItemInfoRepository.RecycleAsync(enterinfos);
        //        ProManufacturingBORBillItemInfoRepository.UnitOfWork.Commit();
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
        ///// 逻辑还原BOR清单明细信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicRestore(params ProManufacturingBORBillItemInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;

        //    try
        //    {
        //        ProManufacturingBORBillItemInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await ProManufacturingBORBillItemInfoRepository.RestoreAsync(enterinfos);
        //        ProManufacturingBORBillItemInfoRepository.UnitOfWork.Commit();
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
        /// 更新BOR清单明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params ProManufacturingBORBillItemInfoInputDto[] inputDtos)
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
                dtoData.Equipment = EquipmentInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Equipment_Id).FirstOrDefault();
                if (Equals(dtoData.Equipment, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的设备不存在,无法保存！");
                }
                if (Equals(dtoData.Quantity, null) || dtoData.Quantity <= 0)
                {
                    return new OperationResult(OperationResultType.Error, "设备数量应大于0,无法保存！");
                }
            }
            ProManufacturingBORBillItemInfoRepository.UnitOfWork.BeginTransaction();
            var result = await ProManufacturingBORBillItemInfoRepository.UpdateAsync(inputDtos);
            ProManufacturingBORBillItemInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
