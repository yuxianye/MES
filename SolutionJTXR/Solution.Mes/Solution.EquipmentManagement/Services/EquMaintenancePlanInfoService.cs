using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using Solution.EquipmentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EquipmentManagement.Services
{
    public class EquMaintenancePlanInfoService : IEquMaintenancePlanInfoContract
    {
        //设备类型实体仓储
        public IRepository<EquMaintenancePlanInfo, Guid> EquipmentTypeInfoRepository { get; set; }
        //设备信息实体仓储
        public IRepository<EquEquipmentInfo, Guid> EquipmentInfoRepository { get; set; }
        //部门信息实体仓储
        public IRepository<EntDepartmentInfo, Guid> DepartmentInfoRepository { get; set; }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        public IQueryable<EquMaintenancePlanInfo> EquMaintenancePlanInfos => EquipmentTypeInfoRepository.Entities; 
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EquMaintenancePlanInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (EquipmentTypeInfoRepository.CheckExists(x => x.MaintenanceCode == dtoData.MaintenanceCode))
                    return new OperationResult(OperationResultType.Error, "该设备编号已存在，无法保存！");
                dtoData.CheckDepartment = DepartmentInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.CheckDepartment.Id);
                if (Equals(dtoData.CheckDepartment, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的部门信息不存在,数据修改失败。");
                }
                dtoData.EquipmentInfo = EquipmentInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.EquipmentInfo.Id);
                if (Equals(dtoData.EquipmentInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备信息不存在,数据修改失败。");
                }
            }
            EquipmentInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentTypeInfoRepository.InsertAsync(inputDtos);
            EquipmentInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 检查设备信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备信息编号</param>
        /// <returns>设备信息是否存在</returns>
        public bool CheckEquipmentExists(Expression<Func<EquMaintenancePlanInfo, bool>> predicate, Guid id)
        {
            return EquipmentTypeInfoRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 物理删除设备信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquipmentTypeInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentTypeInfoRepository.DeleteAsync(ids);
            EquipmentTypeInfoRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EquMaintenancePlanInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.CheckDepartment = DepartmentInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.CheckDepartment.Id);
                if (Equals(dtoData.CheckDepartment, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的部门信息不存在,数据修改失败。");
                }
                dtoData.EquipmentInfo = EquipmentInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.EquipmentInfo.Id);
                if (Equals(dtoData.EquipmentInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备信息不存在,数据修改失败。");
                }
            }

            EquipmentInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquipmentInfoRepository.UpdateAsync(inputDtos);
            EquipmentInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
