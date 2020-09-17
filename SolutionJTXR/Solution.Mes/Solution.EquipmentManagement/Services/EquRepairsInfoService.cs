using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using Solution.EquipmentManagement.Contracts;
using Solution.EquipmentManagement.Dtos;
using Solution.EquipmentManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EquipmentManagement.Services
{
    public class EquRepairsInfoService : IEquRepairsInfoContract
    {
        //设备类型实体仓储
        public IRepository<EquRepairsInfo, Guid> EquRepairsInfoRepository { get; set; }

        //设备信息实体仓储
        public IRepository<EquEquipmentInfo, Guid> EquipmentInfoRepository { get; set; }

        //备件信息实体仓储
        public IRepository<EquSparePartsInfo, Guid> EquSparePartsInfoRepository { get; set; }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        public IQueryable<EquRepairsInfo> EquRepairsInfos => EquRepairsInfoRepository.Entities;


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EquRepairsInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (EquRepairsInfoRepository.CheckExists(x => x.RepairCode == dtoData.RepairCode))
                    return new OperationResult(OperationResultType.Error, "该维修单号已存在，无法保存！");
                dtoData.EquipmentInfo = EquipmentInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.EquipmentInfo.Id);
                if (Equals(dtoData.EquipmentInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备信息不存在,数据添加失败。");
                }
                dtoData.SparePartsInfo = EquSparePartsInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.SparePartsInfo.Id);
                if (Equals(dtoData.SparePartsInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的备件信息不存在,数据添加失败。");
                }
            }
            EquRepairsInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquRepairsInfoRepository.InsertAsync(inputDtos);
            EquRepairsInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 检查设备信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备信息编号</param>
        /// <returns>设备信息是否存在</returns>
        public bool CheckEquipmentExists(Expression<Func<EquRepairsInfo, bool>> predicate, Guid id)
        {
            return EquRepairsInfoRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 物理删除设备信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquRepairsInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquRepairsInfoRepository.DeleteAsync(ids);
            EquRepairsInfoRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EquRepairsInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                dtoData.EquipmentInfo = EquipmentInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.EquipmentInfo.Id);
                if (Equals(dtoData.EquipmentInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备信息不存在,数据修改失败。");
                }
                dtoData.SparePartsInfo = EquSparePartsInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.SparePartsInfo.Id);
                if (Equals(dtoData.SparePartsInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的备件信息不存在,数据修改失败。");
                }
            }

            EquRepairsInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquRepairsInfoRepository.UpdateAsync(inputDtos);
            EquRepairsInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
