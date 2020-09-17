using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using Solution.EquipmentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EquipmentManagement.Services
{
    public class EquipmentTypeInfoService : IEquipmentTypeInfoContract
    {
        /// <summary>
        /// 设备类别信息实体仓储
        /// </summary>
        public IRepository<EquipmentTypeInfo, Guid> EquipmentTypeRepository { get; set; }

        /// <summary>
        /// 查询设备类别信息
        /// </summary>
        public IQueryable<EquipmentTypeInfo> EquipmentTypeInfos => EquipmentTypeRepository.Entities;

        /// <summary>
        /// 检查设备类别信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备类别信息编号</param>
        /// <returns>设备信息是否存在</returns>
        public bool CheckEquipmentTypeExists(Expression<Func<EquipmentTypeInfo, bool>> predicate, Guid id) => EquipmentTypeRepository.CheckExists(predicate, id);



        /// <summary>
        /// 增加设备类别信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddEquipmentType(params EquipmentTypeInfoInputDto[] inputDtos)
        {

            inputDtos.CheckNotNull("inputDtos");
            foreach (EquipmentTypeInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.EquipmentTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写设备类别编号！");
                if (string.IsNullOrEmpty(dtoData.EquipmentTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写设备类别名称！");
                if (EquipmentTypeRepository.CheckExists(x => x.EquipmentTypeCode == dtoData.EquipmentTypeCode /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该设备类别编号已存在，无法保存！");
                if (EquipmentTypeRepository.CheckExists(x => x.EquipmentTypeName == dtoData.EquipmentTypeName && x.Id != dtoData.Id/* && x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该设备类别名称已存在，无法保存！");
            }
            EquipmentTypeRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentTypeRepository.InsertAsync(inputDtos);
            EquipmentTypeRepository.UnitOfWork.Commit();
            return result;
        }
       
        /// <summary>
        /// 物理删除设备类别信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteEquipmentType(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquipmentTypeRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentTypeRepository.DeleteAsync(ids);
            EquipmentTypeRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 逻辑删除设备类别信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        //public async Task<OperationResult> LogicDeleteEquipmentType(params EquipmentTypeInfo[] equipmentTypeinfo)
        //{
        //    equipmentTypeinfo.CheckNotNull("equipmentTypeinfo");
        //    int count = 0;
        //    try
        //    {
        //        EquipmentTypeRepository.UnitOfWork.BeginTransaction();
        //        count = await EquipmentTypeRepository.RecycleAsync(equipmentTypeinfo);
        //        EquipmentTypeRepository.UnitOfWork.Commit();
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
        //    foreach (var data in equipmentTypeinfo)
        //    {
        //        names.Add(data.EquipmentTypeName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}
        /// <summary>
        /// 逻辑还原设备类别信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        //public async Task<OperationResult> LogicRestoreEquipmentType(params EquipmentTypeInfo[] equiinfo)
        //{
        //    equiinfo.CheckNotNull("equiinfo");
        //    int count = 0;

        //    try
        //    {
        //        EquipmentTypeRepository.UnitOfWork.BeginTransaction();
        //        count = await EquipmentTypeRepository.RestoreAsync(equiinfo);
        //        EquipmentTypeRepository.UnitOfWork.Commit();
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
        //    foreach (var data in equiinfo)
        //    {
        //        names.Add(data.EquipmentTypeName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}
        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateEquipmentType(params EquipmentTypeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EquipmentTypeInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.EquipmentTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写设备类别编号！");
                if (string.IsNullOrEmpty(dtoData.EquipmentTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写设备类别名称！");
                if (EquipmentTypeRepository.CheckExists(x => x.EquipmentTypeCode == dtoData.EquipmentTypeCode && x.Id != dtoData.Id/*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该设备类别编号已存在，无法保存！");
                if (EquipmentTypeRepository.CheckExists(x => x.EquipmentTypeName == dtoData.EquipmentTypeName && x.Id != dtoData.Id/* && x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该设备类别名称已存在，无法保存！");
            }
            EquipmentTypeRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentTypeRepository.UpdateAsync(inputDtos);
            EquipmentTypeRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
