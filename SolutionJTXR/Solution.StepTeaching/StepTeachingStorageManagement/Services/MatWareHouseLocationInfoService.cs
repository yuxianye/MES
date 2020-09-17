using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EnterpriseInformation.Models;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.MatWarehouseStorageManagement.Services
{
    /// <summary>
    /// 库位信息服务
    /// </summary>
    public class MatWareHouseLocationInfoService : IMatWareHouseLocationInfoContract
    {

        /// <summary>
        /// 库位信息实体仓储
        /// </summary>
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationRepository { get; set; }
        //
        public IRepository<MatWareHouseInfo, Guid> MatWareHouseInfoRepository { get; set; }
        //
        public IRepository<MatWareHouseAreaInfo, Guid> MatWareHouseAreaInfoRepository { get; set; }


        /// <summary>
        /// 查询库位信息
        /// </summary>
        public IQueryable<MatWareHouseLocationInfo> MatWareHouseLocationInfos
        {
            get { return MatWareHouseLocationRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckMatWareHouseLocationExists(Expression<Func<MatWareHouseLocationInfo, bool>> predicate, Guid id)
        {
            return MatWareHouseLocationRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加库位信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatWareHouseLocationInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.WareHouseLocationCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写库位编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.WareHouseLocationName))
                    return new OperationResult(OperationResultType.Error, "请正确填写库位名称，该组数据不被存储。");
                //
                if (MatWareHouseLocationRepository.CheckExists(x => x.WareHouseLocationCode == dtoData.WareHouseLocationCode))
                    return new OperationResult(OperationResultType.Error, $"库位编号 {dtoData.WareHouseLocationCode} 的数据已存在，该组数据不被存储。");
                if (MatWareHouseLocationRepository.CheckExists(x => x.WareHouseLocationName == dtoData.WareHouseLocationName))
                    return new OperationResult(OperationResultType.Error, $"库位名称 {dtoData.WareHouseLocationName} 的数据已存在，该组数据不被存储。");
                //
                dtoData.MatWareHouse = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MatWareHouse_Id).FirstOrDefault();
                if (Equals(dtoData.MatWareHouse, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的所属仓库不存在,该组数据不被存储。");
                }
                //
                dtoData.MatWareHouseArea = MatWareHouseAreaInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MatWareHouseArea_Id).FirstOrDefault();
                if (Equals(dtoData.MatWareHouseArea, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的所属仓库货区不存在,该组数据不被存储。");
                }
            }
            MatWareHouseLocationRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseLocationRepository.InsertAsync(inputDtos);
            MatWareHouseLocationRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新库位信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateMatWareHouseLocations(params MatWareHouseLocationInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatWareHouseLocationInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.WareHouseLocationCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写库位编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.WareHouseLocationName))
                    return new OperationResult(OperationResultType.Error, "请正确填写库位名称，该组数据不被存储。");
                //
                if (MatWareHouseLocationRepository.CheckExists(x => x.WareHouseLocationCode == dtoData.WareHouseLocationCode && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"库位编号 {dtoData.WareHouseLocationCode} 的数据已存在，该组数据不被存储。");
                if (MatWareHouseLocationRepository.CheckExists(x => x.WareHouseLocationName == dtoData.WareHouseLocationName && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"库位名称 {dtoData.WareHouseLocationName} 的数据已存在，该组数据不被存储。");
            }
            //
            MatWareHouseLocationRepository.UnitOfWork.BeginTransaction();
            //
            MatWareHouseInfo info = new MatWareHouseInfo();
            foreach (var item in inputDtos)
            {
                info = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == item.MatWareHouse_Id).FirstOrDefault();
                item.MatWareHouse = info;
            }
            //
            MatWareHouseAreaInfo matwarehouseareaInfo = new MatWareHouseAreaInfo();
            foreach (var item in inputDtos)
            {
                matwarehouseareaInfo = MatWareHouseAreaInfoRepository.TrackEntities.Where(m => m.Id == item.MatWareHouseArea_Id).FirstOrDefault();
                item.MatWareHouseArea = matwarehouseareaInfo;
            }
            //
            var result = await MatWareHouseLocationRepository.UpdateAsync(inputDtos);
            MatWareHouseLocationRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除库位信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteMatWareHouseLocations(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatWareHouseLocationRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseLocationRepository.DeleteAsync(ids);
            MatWareHouseLocationRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
