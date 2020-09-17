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
    /// <summary>
    /// 备件类别信息服务
    /// </summary>
    public class EquSparePartTypeInfoService : IEquSparePartTypeInfoContract
    {
        /// <summary>
        /// 备件类别信息实体仓储
        /// </summary>
        public IRepository<EquSparePartTypeInfo, Guid> EquSparePartTypeRepository { get; set; }

        /// <summary>
        /// 查询备件类别信息
        /// </summary>
        public IQueryable<EquSparePartTypeInfo> EquSparePartTypeInfos
        {
            get { return EquSparePartTypeRepository.Entities; }
        }

        /// <summary>
        /// 检查备件类别信息是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEquSparePartTypeExists(Expression<Func<EquSparePartTypeInfo, bool>> predicate, Guid id)
        {
            return EquSparePartTypeRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 添加备件类别信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddEquSparePartType(params EquSparePartTypeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EquSparePartTypeInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.EquSparePartTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写备件类别编号！");
                if (string.IsNullOrEmpty(dtoData.EquSparePartTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写备件类别名称！");
                if (EquSparePartTypeRepository.CheckExists(x => x.EquSparePartTypeCode == dtoData.EquSparePartTypeCode /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该备件类别编号已存在，无法保存！");
                if (EquSparePartTypeRepository.CheckExists(x => x.EquSparePartTypeName == dtoData.EquSparePartTypeName /* && x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该备件类别名称已存在，无法保存！");
            }
            EquSparePartTypeRepository.UnitOfWork.BeginTransaction();
            var result = await EquSparePartTypeRepository.InsertAsync(inputDtos);
            EquSparePartTypeRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新备件类别信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateEquSparePartType(params EquSparePartTypeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EquSparePartTypeInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.EquSparePartTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写备件类别编号！");
                if (string.IsNullOrEmpty(dtoData.EquSparePartTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写备件类别名称！");
                if (EquSparePartTypeRepository.CheckExists(x => x.EquSparePartTypeCode == dtoData.EquSparePartTypeCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该备件类别编号已存在，无法保存！");
                if (EquSparePartTypeRepository.CheckExists(x => x.EquSparePartTypeName == dtoData.EquSparePartTypeName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该备件类别名称已存在，无法保存！");
            }
            EquSparePartTypeRepository.UnitOfWork.BeginTransaction();
            var result = await EquSparePartTypeRepository.UpdateAsync(inputDtos);
            EquSparePartTypeRepository.UnitOfWork.Commit();
            return result;

        }

        /// <summary>
        /// 物理删除备件类别信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteEquSparePartType(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquSparePartTypeRepository.UnitOfWork.BeginTransaction();
            var result = await EquSparePartTypeRepository.DeleteAsync(ids);
            EquSparePartTypeRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
