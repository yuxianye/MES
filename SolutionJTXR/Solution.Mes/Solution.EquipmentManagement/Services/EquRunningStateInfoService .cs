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
using Solution.EnterpriseInformation.Models;

namespace Solution.EquipmentManagement.Services
{
    public class EquRunningStateInfoService : IEquRunningStateInfoContract
    {

        public IRepository<EquEquipmentInfo, Guid> EquipmentInfoRepository { get; set; }
        public IRepository<EquRunningStateInfo, Guid> EquRunningStateInfoRepository { get; set; }

        /// <summary>
        /// 查询设备运行状态信息
        /// </summary>
        public IQueryable<EquRunningStateInfo> EquRunningStateInfos
        {
            get { return EquRunningStateInfoRepository.Entities; }
        }
        /// <summary>
        /// 增加设备运行状态信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddEquRunningStateInfo(params EquRunningStateInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (Equals(dtoData.EquRunningStateTypes, null))
                    return new OperationResult(OperationResultType.Error, "请正确填写设备运行状态类别！");
                if (Equals(dtoData.RunningStateTime, DateTime.MinValue))
                    return new OperationResult(OperationResultType.Error, "设备采集时间不应为空，无法保存！");
                if (string.IsNullOrEmpty(dtoData.FaultCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写故障代码！");
                if (dtoData.EquipmentInfo_Id == Guid.Empty)
                {
                    return new OperationResult(OperationResultType.Error, "请选择设备名称，无法保存！");
                }
                dtoData.Equipmentinfo = EquipmentInfoRepository.GetByKey(dtoData.EquipmentInfo_Id);
                if (Equals(dtoData.Equipmentinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的设备不存在,无法保存！");
                }
                if (dtoData.EquRunningStateTypes == 0)
                {
                    return new OperationResult(OperationResultType.Error, "请选择设备运行状态类型！");
                }
            }
            EquRunningStateInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquRunningStateInfoRepository.InsertAsync(inputDtos);
            EquRunningStateInfoRepository.UnitOfWork.Commit();
            return result;
        }



        /// <summary>
        /// 检查设备运行状态信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备运行状态信息编号</param>
        /// <returns>设备运行状态信息是否存在</returns>
        public bool CheckEquRunningStateInfoExists(Expression<Func<EquRunningStateInfo, bool>> predicate, Guid id)
        {
            return EquRunningStateInfoRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 物理删除设备运行状态信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteEquRunningStateInfo(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquRunningStateInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquRunningStateInfoRepository.DeleteAsync(ids);
            EquRunningStateInfoRepository.UnitOfWork.Commit();
            return result;
        }


        public async Task<OperationResult> UpdateEquRunningStateInfo(params EquRunningStateInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.FaultCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写故障代码！");
                if (dtoData.EquipmentInfo_Id == Guid.Empty)
                {
                    return new OperationResult(OperationResultType.Error, "请选择设备名称！");
                }
                dtoData.Equipmentinfo = EquipmentInfoRepository.GetByKey(dtoData.EquipmentInfo_Id);
                if (Equals(dtoData.Equipmentinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的设备不存在,无法保存！");
                }
                if (dtoData.EquRunningStateTypes == 0)
                {
                    return new OperationResult(OperationResultType.Error, "请选择设备运行状态类型！");
                }
            }
            EquRunningStateInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquRunningStateInfoRepository.UpdateAsync(inputDtos);
            EquRunningStateInfoRepository.UnitOfWork.Commit();
            return result;

        }
    }
}
