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
    public class EquSparePartsInfoService : IEquSparePartsInfoContract
    {

        public IRepository<EquSparePartTypeInfo, Guid> EquipmentInfoRepository { get; set; }
        public IRepository<EquSparePartsInfo, Guid> EquSparePartsInfoRepository { get; set; }
        public IRepository<EquSparePartTypeInfo, Guid> EquSparePartTypeInfoRepository { get; set; }

        /// <summary>
        /// 查询备件信息
        /// </summary>
        public IQueryable<EquSparePartsInfo> EquSparePartsInfos
        {
            get { return EquSparePartsInfoRepository.Entities; }
        }

        /// <summary>
        /// 增加备件信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddEquSpareParts(params EquSparePartsInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.SparePartCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写备件编号！");
                if (string.IsNullOrEmpty(dtoData.SparePartName))
                    return new OperationResult(OperationResultType.Error, "请正确填写备件名称！");
                if (EquSparePartsInfoRepository.CheckExists(x => x.SparePartCode == dtoData.SparePartCode))
                    return new OperationResult(OperationResultType.Error, "该备件编号已存在，无法保存！");
                if (EquSparePartsInfoRepository.CheckExists(x => x.SparePartName == dtoData.SparePartName))
                    return new OperationResult(OperationResultType.Error, "该备件名称已存在，无法保存！");
                if (dtoData.EquSparePartType_Id == Guid.Empty)
                {
                    return new OperationResult(OperationResultType.Error, "请选择备件类别！");
                }
                dtoData.Equspareparttypeinfo = EquSparePartTypeInfoRepository.GetByKey(dtoData.EquSparePartType_Id);
                if (Equals(dtoData.Equspareparttypeinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的备件类别不存在,无法保存！");
                }
                if (dtoData.Quantity == 0)
                {
                    return new OperationResult(OperationResultType.Error, "备件数量应大于0，无法保存！");
                }

            }
            EquSparePartsInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquSparePartsInfoRepository.InsertAsync(inputDtos);
            EquSparePartsInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 检查备件信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的备件信息编号</param>
        /// <returns>备件信息是否存在</returns>
        public bool CheckEquSparePartsExists(Expression<Func<EquSparePartsInfo, bool>> predicate, Guid id)
        {
            return EquSparePartsInfoRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 物理删除备件信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteEquSpareParts(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquSparePartsInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquSparePartsInfoRepository.DeleteAsync(ids);
            EquSparePartsInfoRepository.UnitOfWork.Commit();
            return result;
        }


        public async Task<OperationResult> UpdateEquSpareParts(params EquSparePartsInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.SparePartCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写备件编号！");
                if (string.IsNullOrEmpty(dtoData.SparePartName))
                    return new OperationResult(OperationResultType.Error, "请正确填写备件名称！");
                if (EquSparePartsInfoRepository.CheckExists(x => x.SparePartCode == dtoData.SparePartCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该备件编号已存在，无法保存！");
                if (EquSparePartsInfoRepository.CheckExists(x => x.SparePartName == dtoData.SparePartName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该备件名称已存在，无法保存！");
                if (dtoData.EquSparePartType_Id == Guid.Empty)
                {
                    return new OperationResult(OperationResultType.Error, "请选择备件类别！");
                }
                dtoData.Equspareparttypeinfo = EquSparePartTypeInfoRepository.GetByKey(dtoData.EquSparePartType_Id);
                if (Equals(dtoData.Equspareparttypeinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的备件类别不存在,无法保存！");
                }
                if (dtoData.Quantity == 0)
                {
                    return new OperationResult(OperationResultType.Error, "备件数量应大于0，无法保存！");
                }
            }
            EquSparePartsInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquSparePartsInfoRepository.UpdateAsync(inputDtos);
            EquSparePartsInfoRepository.UnitOfWork.Commit();
            return result;

        }
    }
}
