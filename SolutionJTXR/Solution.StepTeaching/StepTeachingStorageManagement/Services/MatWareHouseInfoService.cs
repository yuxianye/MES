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
    /// 仓库信息服务
    /// </summary>
    public class MatWareHouseInfoService : IMatWareHouseInfoContract
    {

        /// <summary>
        /// 仓库信息实体仓储
        /// </summary>
        public IRepository<MatWareHouseInfo, Guid> MatWareHouseRepository { get; set; }
        //
        public IRepository<EntAreaInfo, Guid> EntAreaInfoRepository { get; set; }
        //
        public IRepository<MatWareHouseTypeInfo, Guid> MatWareHouseTypeInfoRepository { get; set; }
        //

        /// <summary>
        /// 查询仓库信息
        /// </summary>
        public IQueryable<MatWareHouseInfo> MatWareHouseInfos
        {
            get { return MatWareHouseRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckMatWareHouseExists(Expression<Func<MatWareHouseInfo, bool>> predicate, Guid id)
        {
            return MatWareHouseRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加仓库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatWareHouseInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.WareHouseCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.WareHouseName))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                //
                if (MatWareHouseRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode))
                    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                if (MatWareHouseRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName))
                    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                //
                dtoData.EntArea = EntAreaInfoRepository.TrackEntities.Where(m => m.Id == dtoData.EntArea_Id).FirstOrDefault();
                if (Equals(dtoData.EntArea, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的所属车间不存在,该组数据不被存储。");
                }
                //
                dtoData.MatWareHouseType = MatWareHouseTypeInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MatWareHouseType_Id).FirstOrDefault();
                if (Equals(dtoData.MatWareHouseType, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的仓库类型不存在,该组数据不被存储。");
                }
            }
            MatWareHouseRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseRepository.InsertAsync(inputDtos);
            MatWareHouseRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新仓库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateMatWareHouses(params MatWareHouseInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatWareHouseInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.WareHouseCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.WareHouseName))
                    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                //
                if (MatWareHouseRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                if (MatWareHouseRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
            }
            //
            MatWareHouseRepository.UnitOfWork.BeginTransaction();
            //
            EntAreaInfo info = new EntAreaInfo();
            foreach (var item in inputDtos)
            {
                info = EntAreaInfoRepository.TrackEntities.Where(m => m.Id == item.EntArea_Id).FirstOrDefault();
                item.EntArea = info;
            }
            //
            MatWareHouseTypeInfo matwarehousetypeinfo = new MatWareHouseTypeInfo();
            foreach (var item in inputDtos)
            {
                matwarehousetypeinfo = MatWareHouseTypeInfoRepository.TrackEntities.Where(m => m.Id == item.MatWareHouseType_Id).FirstOrDefault();
                item.MatWareHouseType = matwarehousetypeinfo;
            }
            //
            var result = await MatWareHouseRepository.UpdateAsync(inputDtos);
            MatWareHouseRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除仓库信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteMatWareHouses(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatWareHouseRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseRepository.DeleteAsync(ids);
            MatWareHouseRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
