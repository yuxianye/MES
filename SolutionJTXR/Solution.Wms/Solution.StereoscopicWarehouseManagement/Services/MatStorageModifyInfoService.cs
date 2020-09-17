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
    /// 库存调整信息服务
    /// </summary>
    public class MatStorageModifyInfoService : IMatStorageModifyInfoContract
    {

        /// <summary>
        /// 库存调整信息实体仓储
        /// </summary>
        public IRepository<MatStorageModifyInfo, Guid> MatStorageModifyInfoRepository { get; set; }
        //
        public IRepository<MaterialInfo, Guid> MaterialInfoRepository { get; set; }
        //
        public IRepository<MaterialBatchInfo, Guid> MaterialBatchInfoRepository { get; set; }
        ////
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationInfoRepository { get; set; }
        //
        public IRepository<MaterialStorageLogInfo, Guid> MaterialStorageLogInfoRepository { get; set; }

        /// <summary>
        /// 查询库存调整信息
        /// </summary>
        public IQueryable<MatStorageModifyInfo> MatStorageModifyInfos
        {
            get { return MatStorageModifyInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MatStorageModifyInfo, bool>> predicate, Guid id)
        {
            return MatStorageModifyInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加库存调整信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatStorageModifyInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.StorageModifyCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                //
                if (dtoData.CurrentAmount > 500)
                    return new OperationResult(OperationResultType.Error, $"调整数量不能大于500，该组数据不被存储。");

                if ( dtoData.CurrentAmount > dtoData.FullPalletQuantity)
                    return new OperationResult(OperationResultType.Error, "调整数量("+ dtoData.CurrentAmount.ToString() + ")大于满盘数量(" + dtoData.FullPalletQuantity.ToString() + ")，该组数据不被存储。");
                //
                ////
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                ////
                //dtoData.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Material_Id).FirstOrDefault();
                //if (Equals(dtoData.Material, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的物料信息不存在,该组数据不被存储。");
                //}
                ////
                //dtoData.MaterialInStorageType = MaterialInStorageTypeInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MaterialInStorageType_Id).FirstOrDefault();
                //if (Equals(dtoData.MaterialInStorageType, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的仓库类型不存在,该组数据不被存储。");
                //}
                //
                dtoData.MaterialBatch = MaterialBatchInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MaterialBatch_Id).FirstOrDefault();
                if (Equals(dtoData.MaterialBatch, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的批次不存在,该组数据不被存储。");
                }
            }
            MatStorageModifyInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MatStorageModifyInfoRepository.InsertAsync(inputDtos);
            MatStorageModifyInfoRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新库存调整信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MatStorageModifyInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatStorageModifyInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.StorageModifyCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                //
                if (dtoData.CurrentAmount > 500)
                    return new OperationResult(OperationResultType.Error, $"调整数量不能大于500，该组数据不被存储。");

                if (dtoData.CurrentAmount > dtoData.FullPalletQuantity)
                    return new OperationResult(OperationResultType.Error, "调整数量(" + dtoData.CurrentAmount.ToString() + ")大于满盘数量(" + dtoData.FullPalletQuantity.ToString() + ")，该组数据不被存储。");
                //
                ////
                //if (MatStorageModifyInfoRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
            }
            //
            MatStorageModifyInfoRepository.UnitOfWork.BeginTransaction();
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
            //            
            MaterialBatchInfo materialbatchinfo = new MaterialBatchInfo();
            foreach (var item in inputDtos)
            {
                materialbatchinfo = MaterialBatchInfoRepository.TrackEntities.Where(m => m.Id == item.MaterialBatch_Id).FirstOrDefault();
                item.MaterialBatch = materialbatchinfo;
            }
            //
            var result = await MatStorageModifyInfoRepository.UpdateAsync(inputDtos);
            MatStorageModifyInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除库存调整信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatStorageModifyInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MatStorageModifyInfoRepository.DeleteAsync(ids);
            MatStorageModifyInfoRepository.UnitOfWork.Commit();
            return result;
        }

        ///////////
        /// <summary>
        /// 更新作业单据信息
        /// </summary>
        /// <param name="dtos">包含更新信息的物料DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddTask(params MatStorageModifyInfoInputDto[] dtos)
        {
            List<string> names = new List<string>();
            //
            MatStorageModifyInfoRepository.UnitOfWork.BeginTransaction();
            foreach (MatStorageModifyInfoInputDto dto in dtos)
            {
                names.Add(dto.StorageModifyCode);
                List<Guid> MaterialBatchIds = new List<Guid>();
                //
                var setResult = await SetMaterialOutStorageMaterialBatch(dto.Id, dto, dto.UserName);
                if (setResult.ResultType.Equals(OperationResultType.Error))
                {
                    return setResult;
                }
                //
                dto.FinishTime = DateTime.Now;
                //调整结束
                dto.StorageModifyState = (int)StorageModifyStateEnumModel.StorageModifyState.StorageModifyFinishState;
                //
                int count = 0;
                //
                MatStorageModifyInfo materialoutstorageInfo = new MatStorageModifyInfo();
                materialoutstorageInfo = dto.MapTo(materialoutstorageInfo);
                //
                count += await MatStorageModifyInfoRepository.UpdateAsync(materialoutstorageInfo);
            }
            MatStorageModifyInfoRepository.UnitOfWork.Commit();
            return new OperationResult(OperationResultType.Success, "库位“{0}”更新成功".FormatWith(names.ExpandAndToString()));
        }


        public async Task<OperationResult> SetMaterialOutStorageMaterialBatch(Guid MaterialOutStorageId, MatStorageModifyInfoInputDto MaterialOutStorageDtos, string UserName)
        {
            int count = 0;
            //
            //////////////批次表
            MaterialBatchInfo materialbatchInfo = MaterialBatchInfoRepository.TrackEntities.Where(m => m.Id == MaterialOutStorageDtos.MaterialBatch_Id).FirstOrDefault();
            decimal dMaterialBatchQuantity = materialbatchInfo.Quantity.Value;
            //
            materialbatchInfo.Quantity = MaterialOutStorageDtos.CurrentAmount;
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
            materialstoragelogInfo.OriginalAmount = dMaterialBatchQuantity;
            materialstoragelogInfo.ChangedAmount = MaterialOutStorageDtos.ChangedAmount;
            materialstoragelogInfo.CurrentAmount = materialbatchInfo.Quantity;
            //
            //调整
            materialstoragelogInfo.StorageChangeType = (int)StorageChangeTypeEnumModel.StorageChangeType.ModifyStorageChangeType;
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
