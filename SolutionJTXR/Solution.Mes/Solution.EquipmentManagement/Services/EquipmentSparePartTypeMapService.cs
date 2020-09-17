using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EquipmentManagement.Dtos;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Solution.EquipmentManagement.Services
{
    public class EquipmentSparePartTypeMapService : IEquipmentSparePartTypeMapContract
    {

        public IRepository<EquipmentSparePartTypeMap, Guid> EquipmentSparePartTypeMapRepository { get; set; }
        public IRepository<EquEquipmentInfo, Guid> EquipmentInfoRepository { get; set; }
        public IRepository<EquSparePartTypeInfo, Guid> EquSparePartTypeInfoRepository { get; set; }

        /// <summary>
        /// 查询装备信息
        /// </summary>
        public IQueryable<EquipmentSparePartTypeMap> EquipmentSparePartTypeMaps
        {
            get { return EquipmentSparePartTypeMapRepository.Entities; }
        }
        /// <summary>
        /// 增加刀具信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddEquipmentSparePartTypeMap(params EquipmentSparePartTypeMapInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            EquipmentSparePartTypeMapRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentSparePartTypeMapRepository.InsertAsync(inputDtos);
            EquipmentSparePartTypeMapRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 检查刀具信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的刀具信息编号</param>
        /// <returns>刀具信息是否存在</returns>
        public bool CheckEquipmentSparePartTypeMapExists(Expression<Func<EquipmentSparePartTypeMap, bool>> predicate, Guid id)
        {
            return EquipmentSparePartTypeMapRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 物理删除刀具信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteEquipmentSparePartTypeMap(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquipmentSparePartTypeMapRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentSparePartTypeMapRepository.DeleteAsync(ids);
            EquipmentSparePartTypeMapRepository.UnitOfWork.Commit();
            return result;
        }



        public async Task<OperationResult> UpdateEquipmentSparePartTypeMap(params EquipmentSparePartTypeMapInputDto[] inputDtos)
        {

            inputDtos.CheckNotNull("inputDtos");
            EquipmentSparePartTypeMapRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentSparePartTypeMapRepository.UpdateAsync(inputDtos);
            EquipmentSparePartTypeMapRepository.UnitOfWork.Commit();
            return result;



        }
    }
}
