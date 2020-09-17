using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using Solution.EquipmentManagement.Models;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EquipmentManagement.Services
{
    public class EquEquipmentInfoService : IEquEquipmentInfoContract
    {
        //设备类型实体仓储
        public IRepository<EquipmentTypeInfo, Guid> EquipmentTypeInfoRepository { get; set; }
        //设备信息实体仓储
        public IRepository<EquEquipmentInfo, Guid> EquipmentInfoRepository { get; set; }
        //部门信息实体仓储
        public IRepository<EntDepartmentInfo, Guid> DepartmentInfoRepository { get; set; }

        public IRepository<EquFactoryInfo, Guid> FactoryInfoRepository { get; set; }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        public IQueryable<EquEquipmentInfo> EquipmentInfos
        {
            get { return EquipmentInfoRepository.Entities; }
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EquEquipmentInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (EquipmentInfoRepository.CheckExists(x => x.EquipmentCode == dtoData.EquipmentCode))
                    return new OperationResult(OperationResultType.Error, "该设备编号已存在，无法保存！");
                dtoData.Equipmenttype = EquipmentTypeInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.Equipmenttype.Id);
                if (Equals(dtoData.Equipmenttype, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备类型不存在,数据添加失败。");
                }
                dtoData.DepartmentInfo = DepartmentInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.DepartmentInfo.Id);
                if (Equals(dtoData.DepartmentInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的部门信息不存在,数据添加失败。");
                }
                dtoData.EquFactoryInfo = FactoryInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.EquFactoryInfo.Id);
                if (Equals(dtoData.EquFactoryInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备厂家信息不存在,数据添加失败。");
                }
            }
            EquipmentInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentInfoRepository.InsertAsync(inputDtos);
            EquipmentInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 检查设备信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备信息编号</param>
        /// <returns>设备信息是否存在</returns>
        public bool CheckEquipmentExists(Expression<Func<EquEquipmentInfo, bool>> predicate, Guid id)
        {
            return EquipmentInfoRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 物理删除设备信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquipmentInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentInfoRepository.DeleteAsync(ids);
            EquipmentInfoRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EquEquipmentInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {

                dtoData.Equipmenttype = EquipmentTypeInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.Equipmenttype.Id);
                if (Equals(dtoData.Equipmenttype, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备类型不存在,数据修改失败。");
                }
                dtoData.DepartmentInfo = DepartmentInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.DepartmentInfo.Id);
                if (Equals(dtoData.DepartmentInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的部门信息不存在,数据修改失败。");
                }
                dtoData.EquFactoryInfo = FactoryInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.EquFactoryInfo.Id);
                if (Equals(dtoData.EquFactoryInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备厂家信息不存在,数据修改失败。");
                }
            }

            EquipmentInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentInfoRepository.UpdateAsync(inputDtos);
            EquipmentInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
