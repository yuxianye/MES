using OSharp.Core.Data;
using OSharp.Core.Mapping;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StereoscopicWarehouseManagement.Contracts;
using Solution.StereoscopicWarehouseManagement.Dtos;
using Solution.StereoscopicWarehouseManagement.Models;
using Solution.StoredInWarehouseManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.StereoscopicWarehouseManagement.Services
{
    /// <summary>
    /// 盘点信息服务
    /// </summary>
    public class MatInventoryInfoService : IMatInventoryInfoContract 
    {

        /// <summary>
        /// 盘点信息实体仓储
        /// </summary>
        public IRepository<MatInventoryInfo, Guid> MatInventoryInfoRepository { get; set; }
        //
        public IRepository<MaterialInfo, Guid> MaterialInfoRepository { get; set; }
        //
        public IRepository<MatWareHouseInfo, Guid> MatWareHouseInfoRepository { get; set; }
        public IRepository<MaterialBatchInfo, Guid> MaterialBatchInfoRepository { get; set; }
        ////
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationInfoRepository { get; set; }
        //
        public IRepository<MaterialStorageLogInfo, Guid> MaterialStorageLogInfoRepository { get; set; }

        /// <summary>
        /// 查询盘点信息
        /// </summary>
        public IQueryable<MatInventoryInfo> MatInventoryInfos
        {
            get { return MatInventoryInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MatInventoryInfo, bool>> predicate, Guid id)
        {
            return MatInventoryInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加盘点信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatInventoryInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.InventoryCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");

                //
                ////
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                //
                if (dtoData.MatWareHouse_Id == Guid.Empty)
                    return new OperationResult(OperationResultType.Error, "请选择所属仓库，该组数据不被存储。");
                //
                dtoData.MatWareHouse = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MatWareHouse_Id).FirstOrDefault();
                if (Equals(dtoData.MatWareHouse, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的仓库不存在,该组数据不被存储。");
                }
                //
                if (dtoData.InventoryType == 0)
                    return new OperationResult(OperationResultType.Error, "请正确选择盘点类型，该组数据不被存储。");
                //
                //dtoData.MaterialInStorageType = MaterialInStorageTypeInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MaterialInStorageType_Id).FirstOrDefault();
                //if (Equals(dtoData.MaterialInStorageType, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的仓库类型不存在,该组数据不被存储。");
                //}
            }
            MatInventoryInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MatInventoryInfoRepository.InsertAsync(inputDtos);
            MatInventoryInfoRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新盘点信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MatInventoryInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatInventoryInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.InventoryCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MatInventoryInfoRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                //
                if (dtoData.MatWareHouse_Id == Guid.Empty )
                    return new OperationResult(OperationResultType.Error, "请选择所属仓库，该组数据不被存储。");
                //
                dtoData.MatWareHouse = MatWareHouseInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MatWareHouse_Id).FirstOrDefault();
                if (Equals(dtoData.MatWareHouse, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的仓库不存在,该组数据不被存储。");
                }
            }
            //
            MatInventoryInfoRepository.UnitOfWork.BeginTransaction();
            //
            //MaterialInfo info = new MaterialInfo();
            //foreach (var item in inputDtos)
            //{
            //    info = MaterialInfoRepository.TrackEntities.Where(m => m.Id == item.Material_Id).FirstOrDefault();
            //    item.Material = info;
            //}
            ////
            //MaterialInStorageTypeInfo matwarehousetypeinfo = new MaterialInStorageTypeInfo();
            //foreach (var item in inputDtos)
            //{
            //    matwarehousetypeinfo = MaterialInStorageTypeInfoRepository.TrackEntities.Where(m => m.Id == item.MaterialInStorageType_Id).FirstOrDefault();
            //    item.MaterialInStorageType = matwarehousetypeinfo;
            //}
            //
            var result = await MatInventoryInfoRepository.UpdateAsync(inputDtos);
            MatInventoryInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除盘点信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatInventoryInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MatInventoryInfoRepository.DeleteAsync(ids);
            MatInventoryInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
