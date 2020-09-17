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
    /// 盘点明细信息服务
    /// </summary>
    public class MatInventoryItemInfoService : IMatInventoryItemInfoContract
    {

        /// <summary>
        /// 盘点明细信息实体仓储
        /// </summary>
        public IRepository<MatInventoryItemInfo, Guid> MatInventoryItemRepository { get; set; }
        //
        public IRepository<MatInventoryInfo, Guid> MatInventoryInfoRepository { get; set; }
        //
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationInfoRepository { get; set; }
        //
        //
        public IRepository<MaterialInfo, Guid> MaterialInfoRepository { get; set; }
        //
        public IRepository<MaterialBatchInfo, Guid> MaterialBatchInfoRepository { get; set; }
        //
        public IRepository<MaterialStorageLogInfo, Guid> MaterialStorageLogInfoRepository { get; set; }

        /// <summary>
        /// 查询盘点明细信息
        /// </summary>
        public IQueryable<MatInventoryItemInfo> MatInventoryItemInfos
        {
            get { return MatInventoryItemRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MatInventoryItemInfo, bool>> predicate, Guid id)
        {
            return MatInventoryItemRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加盘点明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatInventoryItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                //if (string.IsNullOrEmpty(dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                //

                if (dtoData.ActualAmount > 500)
                    return new OperationResult(OperationResultType.Error, $"实际数量不能大于500，该组数据不被存储。");

                if (dtoData.ActualAmount > dtoData.FullPalletQuantity)
                    return new OperationResult(OperationResultType.Error, "实际数量(" + dtoData.ActualAmount.ToString() + ")大于满盘数量(" + dtoData.FullPalletQuantity.ToString() + ")，该组数据不被存储。");
                //
                ////
                //if (MatInventoryItemRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MatInventoryItemRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                ////
                //dtoData.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Material_Id).FirstOrDefault();
                //if (Equals(dtoData.Material, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的物料信息不存在,该组数据不被存储。");
                //}
                //
                dtoData.MatInventory = MatInventoryInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MatInventory_Id).FirstOrDefault();
                if (Equals(dtoData.MatInventory, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的盘点单不存在,该组数据不被存储。");
                }
                //
                dtoData.MaterialBatch = MaterialBatchInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MaterialBatch_Id).FirstOrDefault();
                if (Equals(dtoData.MaterialBatch, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的批次不存在,该组数据不被存储。");
                }
            }
            MatInventoryItemRepository.UnitOfWork.BeginTransaction();
            var result = await MatInventoryItemRepository.InsertAsync(inputDtos);
            MatInventoryItemRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新盘点明细信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MatInventoryItemInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatInventoryItemInfoInputDto dtoData in inputDtos)
            {
                //if (string.IsNullOrEmpty(dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                //

                if (dtoData.ActualAmount > 500)
                    return new OperationResult(OperationResultType.Error, $"实际数量不能大于500，该组数据不被存储。");

                if (dtoData.ActualAmount > dtoData.FullPalletQuantity)
                    return new OperationResult(OperationResultType.Error, "实际数量(" + dtoData.ActualAmount.ToString() + ")大于满盘数量(" + dtoData.FullPalletQuantity.ToString() + ")，该组数据不被存储。");
                //
                ////
                //if (MatInventoryItemRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MatInventoryItemRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
            }
            //
            MatInventoryItemRepository.UnitOfWork.BeginTransaction();
            //
            //MaterialInfo info = new MaterialInfo();
            //foreach (var item in inputDtos)
            //{
            //    info = MaterialInfoRepository.TrackEntities.Where(m => m.Id == item.Material_Id).FirstOrDefault();
            //    item.Material = info;
            //}
            ////
            MatInventoryInfo matinventoryinfo = new MatInventoryInfo();
            foreach (var item in inputDtos)
            {
                matinventoryinfo = MatInventoryInfoRepository.TrackEntities.Where(m => m.Id == item.MatInventory_Id).FirstOrDefault();
                item.MatInventory = matinventoryinfo;
            }
            //            
            MaterialBatchInfo materialbatchinfo = new MaterialBatchInfo();
            foreach (var item in inputDtos)
            {
                materialbatchinfo = MaterialBatchInfoRepository.TrackEntities.Where(m => m.Id == item.MaterialBatch_Id).FirstOrDefault();
                item.MaterialBatch = materialbatchinfo;
            }
            //
            var result = await MatInventoryItemRepository.UpdateAsync(inputDtos);
            MatInventoryItemRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除盘点明细信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatInventoryItemRepository.UnitOfWork.BeginTransaction();
            var result = await MatInventoryItemRepository.DeleteAsync(ids);
            MatInventoryItemRepository.UnitOfWork.Commit();
            return result;
        }

        ///////////
        /// <summary>
        /// 更新作业单据信息
        /// </summary>
        /// <param name="dtos">包含更新信息的物料DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddTask(params MatInventoryItemInfoInputDto[] dtos)
        {
            List<string> names = new List<string>();
            //
            MatInventoryItemRepository.UnitOfWork.BeginTransaction();
            foreach (MatInventoryItemInfoInputDto dto in dtos)
            {
                names.Add(dto.Id.ToString());
                List<Guid> MaterialBatchIds = new List<Guid>();
                //
                var setResult = await SetMaterialOutStorageMaterialBatch(dto.Id, dto, dto.UserName);
                if (setResult.ResultType.Equals(OperationResultType.Error))
                {
                    return setResult;
                }
                //
                dto.InventoryTime = DateTime.Now;
                //盘点结束
                dto.InventoryState = (int)InventoryStateEnumModel.InventoryState.InventoryFinishState;
                //
                int count = 0;
                //
                MatInventoryItemInfo materialoutstorageInfo = new MatInventoryItemInfo();
                materialoutstorageInfo = dto.MapTo(materialoutstorageInfo);
                //
                count += await MatInventoryItemRepository.UpdateAsync(materialoutstorageInfo);
            }
            MatInventoryItemRepository.UnitOfWork.Commit();
            return new OperationResult(OperationResultType.Success, "库位“{0}”更新成功".FormatWith(names.ExpandAndToString()));
        }


        public async Task<OperationResult> SetMaterialOutStorageMaterialBatch(Guid MaterialOutStorageId, MatInventoryItemInfoInputDto MaterialOutStorageDtos, string UserName)
        {
            int count = 0;
            //
            //////////////批次表
            MaterialBatchInfo materialbatchInfo = MaterialBatchInfoRepository.TrackEntities.Where(m => m.Id == MaterialOutStorageDtos.MaterialBatch_Id).FirstOrDefault();
            decimal dMaterialBatchQuantity = materialbatchInfo.Quantity.Value;
            //
            materialbatchInfo.Quantity = MaterialOutStorageDtos.ActualAmount;
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
            materialstoragelogInfo.ChangedAmount = MaterialOutStorageDtos.DifferenceAmount;
            //
            materialstoragelogInfo.OriginalAmount = dMaterialBatchQuantity;
            materialstoragelogInfo.ChangedAmount = MaterialOutStorageDtos.DifferenceAmount;
            materialstoragelogInfo.CurrentAmount = materialbatchInfo.Quantity;
            //
            //盘点
            materialstoragelogInfo.StorageChangeType = (int)StorageChangeTypeEnumModel.StorageChangeType.InventoryStorageChangeType;
            //
            materialstoragelogInfo.CreatorUserId = UserName;
            materialstoragelogInfo.CreatedTime = DateTime.Now;
            materialstoragelogInfo.LastUpdatorUserId = materialstoragelogInfo.CreatorUserId;
            materialstoragelogInfo.LastUpdatedTime = materialstoragelogInfo.CreatedTime;
            //
            count += await MaterialStorageLogInfoRepository.InsertAsync(materialstoragelogInfo);
            //
            return count > 0
                ? new OperationResult(OperationResultType.Success, "出库任务“{0}”指派库位批次操作成功".FormatWith("Test"))
                : OperationResult.NoChanged;
        }

    }
}
