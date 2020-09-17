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
    /// 移库信息服务
    /// </summary>
    public class MatStorageMoveInfoService : IMatStorageMoveInfoContract
    {

        /// <summary>
        /// 移库信息实体仓储
        /// </summary>
        public IRepository<MatStorageMoveInfo, Guid> MatStorageMoveInfoRepository { get; set; }
        //
        public IRepository<MaterialInfo, Guid> MaterialInfoRepository { get; set; }
        //
        public IRepository<MaterialBatchInfo, Guid> MaterialBatchInfoRepository { get; set; }
        ////
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationInfoRepository { get; set; }
        //
        public IRepository<MaterialStorageLogInfo, Guid> MaterialStorageLogInfoRepository { get; set; }

        /// <summary>
        /// 查询移库信息
        /// </summary>
        public IQueryable<MatStorageMoveInfo> MatStorageMoveInfos
        {
            get { return MatStorageMoveInfoRepository.Entities; }
        }
        public IQueryable<MatStorageMoveInfo> MatStorageTrackMoveInfos
        {
            get { return MatStorageMoveInfoRepository.TrackEntities; }
        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MatStorageMoveInfo, bool>> predicate, Guid id)
        {
            return MatStorageMoveInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加移库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatStorageMoveInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.StorageMoveCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MatStorageMoveInfoRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MatStorageMoveInfoRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                //
                var FromLocationID = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == dtoData.FromLocationID).FirstOrDefault();
                if (Equals(FromLocationID, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的原库位信息不存在,该组数据不被存储。");
                }
                //
                var ToLocationID = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ToLocationID).FirstOrDefault();
                if (Equals(ToLocationID, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的现库位信息不存在,该组数据不被存储。");
                }
                //
                if (dtoData.FromLocationID.Equals(dtoData.ToLocationID))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的现库位信息与原库位信息相同,该组数据不被存储。");
                }
            }
            MatStorageMoveInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MatStorageMoveInfoRepository.InsertAsync(inputDtos);
            MatStorageMoveInfoRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新移库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MatStorageMoveInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatStorageMoveInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.StorageMoveCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MatStorageMoveInfoRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MatStorageMoveInfoRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                //
                var FromLocationID = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == dtoData.FromLocationID).FirstOrDefault();
                if (Equals(FromLocationID, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的原库位信息不存在,该组数据不被存储。");
                }
                //
                var ToLocationID = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == dtoData.ToLocationID).FirstOrDefault();
                if (Equals(ToLocationID, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的现库位信息不存在,该组数据不被存储。");
                }
                //
                if (dtoData.FromLocationID.Equals(dtoData.ToLocationID))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的现库位信息与原库位信息相同,该组数据不被存储。");
                }
            }
            //
            MatStorageMoveInfoRepository.UnitOfWork.BeginTransaction();
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
            var result = await MatStorageMoveInfoRepository.UpdateAsync(inputDtos);
            MatStorageMoveInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除移库信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatStorageMoveInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MatStorageMoveInfoRepository.DeleteAsync(ids);
            MatStorageMoveInfoRepository.UnitOfWork.Commit();
            return result;
        }

        ///////////
        /// <summary>
        /// 更新作业单据信息
        /// </summary>
        /// <param name="dtos">包含更新信息的物料DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddTask(params MatStorageMoveInfoInputDto[] dtos)
        {
            List<string> names = new List<string>();
            var setResult = new OperationResult();
            //
            MatStorageMoveInfoRepository.UnitOfWork.BeginTransaction();
            foreach (MatStorageMoveInfoInputDto dto in dtos)
            {
                names.Add(dto.StorageMoveCode);
                List<Guid> MaterialBatchIds = new List<Guid>();
                //
                setResult = await SetMaterialOutStorageMaterialBatch(dto.Id, dto, dto.UserName);
                if (setResult.ResultType.Equals(OperationResultType.Error))
                {
                    return setResult;
                }
                //
                //dto.FinishTime = DateTime.Now;
                //移库完成
                dto.StorageMoveState = (int)StorageMoveStateEnumModel.StorageMoveState.StorageMoveFinishState;
                //
                int count = 0;
                //
                MatStorageMoveInfo materialoutstorageInfo = new MatStorageMoveInfo();
                materialoutstorageInfo = dto.MapTo(materialoutstorageInfo);
                //
                count += await MatStorageMoveInfoRepository.UpdateAsync(materialoutstorageInfo);
            }
            MatStorageMoveInfoRepository.UnitOfWork.Commit();
            //return new OperationResult(OperationResultType.Success, "库位“{0}”更新成功".FormatWith(names.ExpandAndToString()));
            return setResult;
        }


        public async Task<OperationResult> SetMaterialOutStorageMaterialBatch(Guid MaterialOutStorageId, MatStorageMoveInfoInputDto MaterialOutStorageDtos, string UserName)
        {
            int count = 0;
            //
            //空托盘 移库
            //托盘及物料 移库
            //
            //////////////库位表
            MatWareHouseLocationInfo frommatwarehouselocationInfo = new MatWareHouseLocationInfo();
            frommatwarehouselocationInfo = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == MaterialOutStorageDtos.FromLocationID).FirstOrDefault();
            if (!Equals(frommatwarehouselocationInfo.PalletID, null))
            {
                Guid PalletID = frommatwarehouselocationInfo.PalletID.Value;
                //
                MatWareHouseLocationInfo tomatwarehouselocationInfo = new MatWareHouseLocationInfo();
                tomatwarehouselocationInfo = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == MaterialOutStorageDtos.ToLocationID).FirstOrDefault();
                //
                //if (tomatwarehouselocationInfo.PalletID == null)
                //修改关永强代码  by cxh
                if (Equals(tomatwarehouselocationInfo.PalletID, null) || Equals(tomatwarehouselocationInfo.PalletID, Guid.Empty))
                {
                    //frommatwarehouselocationInfo.PalletID = null;
                    //tomatwarehouselocationInfo.PalletID = PalletID;
                    ////
                    //count += await MatWareHouseLocationInfoRepository.UpdateAsync(frommatwarehouselocationInfo);
                    //count += await MatWareHouseLocationInfoRepository.UpdateAsync(tomatwarehouselocationInfo);
                    //
                    //批次表 修改 by cxh 20181126 增加条件，解决插入库存流水多条数据的问题
                    // List<MaterialBatchInfo> materialbatchInfoList = MaterialBatchInfoRepository.TrackEntities.Where(m => m.MatWareHouseLocation.Id == MaterialOutStorageDtos.FromLocationID).ToList();
                    List<MaterialBatchInfo> materialbatchInfoList = MaterialBatchInfoRepository.TrackEntities.Where(m => m.MatWareHouseLocation.Id == MaterialOutStorageDtos.FromLocationID && m.MatWareHouseLocation.PalletID != null && m.MatWareHouseLocation.PalletID != Guid.Empty && m.Quantity > 0).ToList();
                    foreach (MaterialBatchInfo materialbatchInfo in materialbatchInfoList)
                    {
                        //////////////库位表
                        MatWareHouseLocationInfo matwarehouselocationInfo = new MatWareHouseLocationInfo();
                        matwarehouselocationInfo = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == MaterialOutStorageDtos.ToLocationID).FirstOrDefault();
                        //
                        materialbatchInfo.MatWareHouseLocation = matwarehouselocationInfo;
                        //
                        materialbatchInfo.LastUpdatorUserId = UserName;
                        materialbatchInfo.LastUpdatedTime = DateTime.Now;
                        //           
                        count += await MaterialBatchInfoRepository.UpdateAsync(materialbatchInfo);
                        //////////////
                        //////////////流水帐
                        MaterialStorageLogInfo materialstoragelogInfo = new MaterialStorageLogInfo();
                        //
                        Guid MaterialBatch_Id = materialbatchInfo.Id;
                        materialstoragelogInfo.MaterialBatch = MaterialBatchInfoRepository.TrackEntities.Where(m => m.Id == MaterialBatch_Id).FirstOrDefault();
                        //
                        Guid MaterialID = materialstoragelogInfo.MaterialBatch.Material.Id;
                        materialstoragelogInfo.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == MaterialID).FirstOrDefault();
                        //
                        materialstoragelogInfo.OriginalAmount = materialbatchInfo.Quantity;
                        //materialstoragelogInfo.ChangedAmount = materialbatchInfo.Quantity;
                        materialstoragelogInfo.ChangedAmount = 0;
                        materialstoragelogInfo.CurrentAmount = materialbatchInfo.Quantity;
                        //
                        //移库
                        materialstoragelogInfo.StorageChangeType = (int)StorageChangeTypeEnumModel.StorageChangeType.MoveStorageChangeType;
                        //
                        materialstoragelogInfo.CreatorUserId = UserName;
                        materialstoragelogInfo.CreatedTime = DateTime.Now;
                        materialstoragelogInfo.LastUpdatorUserId = materialstoragelogInfo.CreatorUserId;
                        materialstoragelogInfo.LastUpdatedTime = materialstoragelogInfo.CreatedTime;
                        //
                        count += await MaterialStorageLogInfoRepository.InsertAsync(materialstoragelogInfo);
                    }
                    frommatwarehouselocationInfo.PalletID = null;
                    tomatwarehouselocationInfo.PalletID = PalletID;
                    //
                    count += await MatWareHouseLocationInfoRepository.UpdateAsync(frommatwarehouselocationInfo);
                    count += await MatWareHouseLocationInfoRepository.UpdateAsync(tomatwarehouselocationInfo);
                }
                else
                {
                    return new OperationResult(OperationResultType.Error, "目标库位不为空库位");
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Error, "原库位为空库位");
            }
            //
            return count > 0
                ? new OperationResult(OperationResultType.Success, "出库任务“{0}”指派库位批次操作成功".FormatWith("Test"))
                : OperationResult.NoChanged;
        }
    }
}
