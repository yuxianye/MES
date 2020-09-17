using OSharp.Core.Data;
using OSharp.Core.Mapping;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Models;
using Solution.StoredInWarehouseManagement.Dtos;
using Solution.TakeOutWarehouseManagement.Contracts;
using Solution.TakeOutWarehouseManagement.Dtos;
using Solution.TakeOutWarehouseManagement.Models;
using Solution.StepTeachingDispatchManagement.Models;
using Solution.StepTeachingDispatchManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.TakeOutWarehouseManagement.Services
{
    /// <summary>
    /// 物料出库信息服务
    /// </summary>
    public class MaterialOutStorageInfoService : IMaterialOutStorageInfoContract
    {
        /// <summary>
        /// 物料出库信息实体仓储
        /// </summary>
        public IRepository<MaterialOutStorageInfo, Guid> MaterialOutStorageRepository { get; set; }
        public IRepository<MaterialBatchInfo, Guid> MaterialBatchRepository { get; set; }
        public IRepository<MaterialStorageLogInfo, Guid> MaterialStorageLogRepository { get; set; }
        //
        public IRepository<MaterialInfo, Guid> MaterialInfoRepository { get; set; }
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationInfoRepository { get; set; }
        //
        public IRepository<MatWareHouseInfo, Guid> MatWareHouseInfoRepository { get; set; }
        public IRepository<DisTaskDispatchInfo, Guid> DisTaskDispatchInfoRepository { get; set; }
        public IRepository<DisStepActionInfo, Guid> DisStepActionInfoRepository { get; set; }

        /// <summary>
        /// 查询物料出库信息
        /// </summary>
        public IQueryable<MaterialOutStorageInfo> MaterialOutStorageInfos
        {
            get { return MaterialOutStorageRepository.Entities; }
        }
        public IQueryable<MaterialOutStorageInfo> MaterialOutStorageTrackInfos
        {
            get { return MaterialOutStorageRepository.TrackEntities; }
        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MaterialOutStorageInfo, bool>> predicate, Guid id)
        {
            return MaterialOutStorageRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加物料出库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MaterialOutStorageInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.OutStorageBillCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MatWareHouseRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MatWareHouseRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                ////
                //dtoData.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Material_Id).FirstOrDefault();
                //if (Equals(dtoData.Material, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的物料信息不存在,该组数据不被存储。");
                //}
                ////
                //dtoData.MatWareHouseType = MatWareHouseTypeInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MatWareHouseType_Id).FirstOrDefault();
                //if (Equals(dtoData.MatWareHouseType, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的仓库类型不存在,该组数据不被存储。");
                //}
                //空托盘出库
                if (dtoData.OutStorageType == (int)OutStorageTypeEnumModel.OutStorageType.PalletOutStorageType &&
                    dtoData.PalletID == null)
                    return new OperationResult(OperationResultType.Error, "请正确选择托盘信息，该组数据不被存储。");

                //成品手动出库
                if (dtoData.OutStorageType == (int)OutStorageTypeEnumModel.OutStorageType.ProductManuallyOutStorageType)
                {
                    if (dtoData.MaterialID == null)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料信息，该组数据不被存储。");

                    if (dtoData.Quantity == null || dtoData.Quantity == decimal.Zero)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料出库数量，该组数据不被存储。");

                    if (dtoData.Quantity > 500)
                        return new OperationResult(OperationResultType.Error, $"物料出库数量不能大于500，该组数据不被存储。");
                }
                //原料自动出库演示
                if (dtoData.OutStorageType == (int)OutStorageTypeEnumModel.OutStorageType.MaterialAutoShowOutStorageType)
                {
                    if (dtoData.MaterialID == null)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料信息，该组数据不被存储。");

                    if (dtoData.Quantity == null || dtoData.Quantity == decimal.Zero)
                        return new OperationResult(OperationResultType.Error, "请正确填写物料出库数量，该组数据不被存储。");

                    var fullPalletQuqntity = MaterialInfoRepository.Entities.Where(m => m.Id == dtoData.MaterialID).FirstOrDefault().FullPalletQuantity;
                    if (dtoData.Quantity > fullPalletQuqntity)
                        return new OperationResult(OperationResultType.Error, $"物料出库数量不能大于满盘数量，该组数据不被存储。");
                }
            }
            MaterialOutStorageRepository.UnitOfWork.BeginTransaction();
            var result = await MaterialOutStorageRepository.InsertAsync(inputDtos);
            MaterialOutStorageRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新物料出库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MaterialOutStorageInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MaterialOutStorageInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.OutStorageBillCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MatWareHouseRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MatWareHouseRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                //空托盘出库
                if (dtoData.OutStorageType == (int)OutStorageTypeEnumModel.OutStorageType.PalletOutStorageType &&
                    dtoData.PalletID == null)
                    return new OperationResult(OperationResultType.Error, "请正确选择托盘信息，该组数据不被存储。");
                //成品手动出库
                if (dtoData.OutStorageType == (int)OutStorageTypeEnumModel.OutStorageType.ProductManuallyOutStorageType)
                {
                    if (dtoData.MaterialID == null)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料信息，该组数据不被存储。");

                    if (dtoData.Quantity == null || dtoData.Quantity == decimal.Zero)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料出库数量，该组数据不被存储。");

                    if (dtoData.Quantity > 500)
                        return new OperationResult(OperationResultType.Error, $"物料出库数量不能大于500，该组数据不被存储。");
                }
                if (dtoData.OutStorageType == (int)OutStorageTypeEnumModel.OutStorageType.MaterialAutoShowOutStorageType)
                {
                    if (dtoData.MaterialID == null)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料信息，该组数据不被存储。");

                    if (dtoData.Quantity == null || dtoData.Quantity == decimal.Zero)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料出库数量，该组数据不被存储。");

                    var fullPalletQuqntity = MaterialInfoRepository.Entities.Where(m => m.Id == dtoData.MaterialID).FirstOrDefault().FullPalletQuantity;
                    if (dtoData.Quantity > fullPalletQuqntity)
                        return new OperationResult(OperationResultType.Error, $"物料出库数量不能大于满盘数量，该组数据不被存储。");
                }
            }
            //
            MaterialOutStorageRepository.UnitOfWork.BeginTransaction();
            //
            //MaterialInfo info = new MaterialInfo();
            //foreach (var item in inputDtos)
            //{
            //    info = MaterialInfoRepository.TrackEntities.Where(m => m.Id == item.Material_Id).FirstOrDefault();
            //    item.Material = info;
            //}
            ////
            //MatWareHouseTypeInfo matwarehousetypeinfo = new MatWareHouseTypeInfo();
            //foreach (var item in inputDtos)
            //{
            //    matwarehousetypeinfo = MatWareHouseTypeInfoRepository.TrackEntities.Where(m => m.Id == item.MatWareHouseType_Id).FirstOrDefault();
            //    item.MatWareHouseType = matwarehousetypeinfo;
            //}
            //
            var result = await MaterialOutStorageRepository.UpdateAsync(inputDtos);
            MaterialOutStorageRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 审核物料出库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Audit(params MaterialOutStorageInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MaterialOutStorageInfoInputDto dtoData in inputDtos)
            {
                //if (string.IsNullOrEmpty(dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MatWareHouseRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MatWareHouseRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
            }
            //
            MaterialOutStorageRepository.UnitOfWork.BeginTransaction();
            //
            //MaterialInfo info = new MaterialInfo();
            //foreach (var item in inputDtos)
            //{
            //    info = MaterialInfoRepository.TrackEntities.Where(m => m.Id == item.Material_Id).FirstOrDefault();
            //    item.Material = info;
            //}
            ////
            //MatWareHouseTypeInfo matwarehousetypeinfo = new MatWareHouseTypeInfo();
            //foreach (var item in inputDtos)
            //{
            //    matwarehousetypeinfo = MatWareHouseTypeInfoRepository.TrackEntities.Where(m => m.Id == item.MatWareHouseType_Id).FirstOrDefault();
            //    item.MatWareHouseType = matwarehousetypeinfo;
            //}
            //
            var result = await MaterialOutStorageRepository.UpdateAsync(inputDtos);
            MaterialOutStorageRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除物料出库信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MaterialOutStorageRepository.UnitOfWork.BeginTransaction();
            var result = await MaterialOutStorageRepository.DeleteAsync(ids);
            MaterialOutStorageRepository.UnitOfWork.Commit();
            return result;
        }

        ///////////
        /// <summary>
        /// 更新作业单据信息
        /// </summary>
        /// <param name="dtos">包含更新信息的物料DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddTask(params MaterialOutStorageInfoInputDto[] dtos)
        {
            List<string> names = new List<string>();
            //
            MaterialOutStorageRepository.UnitOfWork.BeginTransaction();
            foreach (MaterialOutStorageInfoInputDto dto in dtos)
            {
                //IdentityResult result;
                //User user = UserManager.FindById(dto.Id);
                //if (user == null)
                //{
                //    return new OperationResult(OperationResultType.QueryNull);
                //}

                //user = dto.MapTo(user);
                //result = await UserManager.UpdateAsync(user);
                //if (!result.Succeeded)
                //{
                //    return new OperationResult(OperationResultType.Error, result.Errors.ExpandAndToString());
                //}
                names.Add(dto.OutStorageBillCode);

                //User savedUser = UserManager.Users.Where(x => x.UserName.Equals(dto.UserName) &&
                //                                           x.NickName.Equals(dto.NickName) &&
                //                                           x.Email.Equals(dto.Email) &&
                //                                           x.PhoneNumber.Equals(dto.PhoneNumber)).FirstOrDefault();
                //if (savedUser != null)
                //{
                List<Guid> MaterialBatchIds = new List<Guid>();
                MaterialBatchIds = dto.MaterialBatchs.Select(x => x.Id).ToList();
                //
                //空托盘出库
                if (dto.OutStorageType == (int)OutStorageTypeEnumModel.OutStorageType.PalletOutStorageType)
                {
                    var setResult = await SetPalletOutStorageWareHouseLocation(dto.Id, dto, MaterialBatchIds.ToArray());
                    if (setResult.ResultType.Equals(OperationResultType.Error))
                    {
                        return setResult;
                    }
                }
                //成品手动出库
                else if (dto.OutStorageType == (int)OutStorageTypeEnumModel.OutStorageType.ProductManuallyOutStorageType)
                {
                    var setResult = await SetMaterialOutStorageMaterialBatch(dto.Id, dto, MaterialBatchIds.ToArray(), dto.UserName);
                    if (setResult.ResultType.Equals(OperationResultType.Error))
                    {
                        return setResult;
                    }
                }
            }
            MaterialOutStorageRepository.UnitOfWork.Commit();
            return new OperationResult(OperationResultType.Success, "库位“{0}”更新成功".FormatWith(names.ExpandAndToString()));
        }


        /// <summary>
        /// 设置出库任务的空托盘库位
        /// </summary>
        /// <param name="id">入库任务编号</param>
        /// <param name="roleIds">库位编号集合</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetPalletOutStorageWareHouseLocation(Guid MaterialOutStorageId, MaterialOutStorageInfoInputDto MaterialOutStorageDtos, Guid[] WareHouseLocationIds)
        {
            int count = 0;
            //////////////库位表
            foreach (Guid WareHouseLocationId in WareHouseLocationIds)
            {
                MatWareHouseLocationInfo matwarehouselocationInfo = new MatWareHouseLocationInfo();
                //
                matwarehouselocationInfo = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == WareHouseLocationId).FirstOrDefault();
                //
                matwarehouselocationInfo.PalletID = null;
                //
                count += await MatWareHouseLocationInfoRepository.UpdateAsync(matwarehouselocationInfo);
            }
            //
            //出库单
            MaterialOutStorageDtos.OutStorageTime = DateTime.Now;
            MaterialOutStorageDtos.FinishTime = DateTime.Now;
            //已完成
            MaterialOutStorageDtos.OutStorageStatus = (int)OutStorageStatusEnumModel.OutStorageStatus.OutStorageFinishStatus;
            //
            MaterialOutStorageInfo materialoutstorageInfo = new MaterialOutStorageInfo();
            materialoutstorageInfo = MaterialOutStorageDtos.MapTo(materialoutstorageInfo);
            //
            count += await MaterialOutStorageRepository.UpdateAsync(materialoutstorageInfo);
            //
            return count > 0
                ? new OperationResult(OperationResultType.Success, "空托盘出库任务“{0}”指派库位操作成功".FormatWith("Test"))
                : OperationResult.Success;
        }

        /// <summary>
        /// 设置出库任务的库位
        /// </summary>
        /// <param name="id">入库任务编号</param>
        /// <param name="roleIds">库位编号集合</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetMaterialOutStorageMaterialBatch(Guid MaterialOutStorageId, MaterialOutStorageInfoInputDto MaterialOutStorageDtos, Guid[] MaterialBatchIds, string UserName)
        {
            int count = 0;
            //
            //////////////出库单
            //MaterialOutStorageInfo materialoutstorageInfo = new MaterialOutStorageInfo();
            //materialoutstorageInfo = MaterialOutStorageRepository.TrackEntities.Where(m => m.Id == MaterialOutStorageId).FirstOrDefault();
            //////////////
            decimal dQuantity = MaterialOutStorageDtos.Quantity.Value;
            decimal dChangedQuantity = 0;
            //
            decimal dPalletQuantity = MaterialOutStorageDtos.PalletQuantity.Value;
            dPalletQuantity = 0;
            //
            decimal dPartQuantity = 0;
            //////////////库位表
            foreach (Guid MaterialBatchId in MaterialBatchIds)
            {
                MaterialBatchInfo materialbatchInfo = new MaterialBatchInfo();
                //
                materialbatchInfo = MaterialBatchRepository.TrackEntities.Where(m => m.Id == MaterialBatchId).FirstOrDefault();
                //
                decimal dMaterialBatchQuantity = materialbatchInfo.Quantity.Value;
                //
                if (dQuantity >= dMaterialBatchQuantity)
                {
                    materialbatchInfo.Quantity = 0;
                    dQuantity = dQuantity - dMaterialBatchQuantity;
                    dChangedQuantity = dMaterialBatchQuantity;
                    //
                    dPalletQuantity++;
                }
                else if (dQuantity > 0)
                {
                    dPartQuantity = 0;
                    materialbatchInfo.Quantity = dMaterialBatchQuantity - dQuantity;
                    //部分出库 库位清零
                    dPartQuantity = materialbatchInfo.Quantity.Value;
                    materialbatchInfo.Quantity = 0;
                    //
                    dChangedQuantity = dQuantity;
                    dQuantity = 0;
                    //
                    dPalletQuantity++;
                }
                else
                {
                    break;
                }
                //
                materialbatchInfo.LastUpdatorUserId = UserName;
                materialbatchInfo.LastUpdatedTime = DateTime.Now;
                //               
                count += await MaterialBatchRepository.UpdateAsync(materialbatchInfo);

                //////////////
                //////////////流水帐

                MaterialStorageLogInfo materialstoragelogInfo = new MaterialStorageLogInfo();
                //
                Guid MaterialBatch_Id = materialbatchInfo.Id;
                materialstoragelogInfo.MaterialBatch = MaterialBatchRepository.TrackEntities.Where(m => m.Id == MaterialBatch_Id).FirstOrDefault();
                //
                Guid MaterialID = MaterialOutStorageDtos.MaterialID.Value;
                materialstoragelogInfo.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == MaterialID).FirstOrDefault();
                //
                materialstoragelogInfo.OutStorageID = MaterialOutStorageDtos.Id;
                //
                materialstoragelogInfo.OriginalAmount = dMaterialBatchQuantity;
                //materialstoragelogInfo.ChangedAmount = dChangedQuantity;
                //修改 关永强 Bug 成品出库改变数量应为负数
                materialstoragelogInfo.ChangedAmount = -dChangedQuantity;
                materialstoragelogInfo.CurrentAmount = materialbatchInfo.Quantity;
                //
                //出库
                materialstoragelogInfo.StorageChangeType = (int)StorageChangeTypeEnumModel.StorageChangeType.OutStorageChangeType;
                //
                if (dPartQuantity > 0)
                {
                    materialstoragelogInfo.Remark = "部分出库，应进行退库操作！数量为：" + dPartQuantity.ToString();
                }
                //
                materialstoragelogInfo.CreatorUserId = UserName;
                materialstoragelogInfo.CreatedTime = DateTime.Now;
                materialstoragelogInfo.LastUpdatorUserId = materialstoragelogInfo.CreatorUserId;
                materialstoragelogInfo.LastUpdatedTime = materialstoragelogInfo.CreatedTime;
                //
                count += await MaterialStorageLogRepository.InsertAsync(materialstoragelogInfo);

                //////////////库位表
                MatWareHouseLocationInfo matwarehouselocationInfo = new MatWareHouseLocationInfo();
                //
                matwarehouselocationInfo = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == materialbatchInfo.MatWareHouseLocation.Id).FirstOrDefault();
                //
                matwarehouselocationInfo.PalletID = null;
                //
                count += await MatWareHouseLocationInfoRepository.UpdateAsync(matwarehouselocationInfo);
            }
            //
            //出库单
            MaterialOutStorageDtos.OutStorageTime = DateTime.Now;
            MaterialOutStorageDtos.FinishTime = DateTime.Now;
            //已完成
            MaterialOutStorageDtos.OutStorageStatus = (int)OutStorageStatusEnumModel.OutStorageStatus.OutStorageFinishStatus;
            MaterialOutStorageDtos.PalletQuantity = dPalletQuantity;
            //
            MaterialOutStorageInfo materialoutstorageInfo = new MaterialOutStorageInfo();
            materialoutstorageInfo = MaterialOutStorageDtos.MapTo(materialoutstorageInfo);
            //
            count += await MaterialOutStorageRepository.UpdateAsync(materialoutstorageInfo);
            //
            return count > 0
                ? new OperationResult(OperationResultType.Success, "出库任务“{0}”指派库位批次操作成功".FormatWith("Test"))
                : OperationResult.Success;
        }

        /// <summary>
        /// 分步教学-原料自动出库演示操作
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> MaterialOutStorageShowTask(params MaterialOutStorageInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            OperationResult result1 = new OperationResult();
            OperationResult result2 = new OperationResult();
            int count0 = inputDtos.Count();
            DisTaskDispatchInfoInputDto[] dispatchInfos = new DisTaskDispatchInfoInputDto[count0];
            MaterialStorageLogInfoInputDto[] logInfos = new MaterialStorageLogInfoInputDto[count0];
            MaterialStorageLogRepository.UnitOfWork.BeginTransaction();
            int count1 = 0;
            for (int i = 0; i < inputDtos.Count(); i++)
            {
                int count = inputDtos[i].MaterialBatchs.Count();
                if (count == 1)
                {
                    DisTaskDispatchInfoInputDto dispatchInfo = new DisTaskDispatchInfoInputDto();
                    dispatchInfo.DisStepAction = DisStepActionInfoRepository.TrackEntities.Where(m => m.StepActionCode == "StepAction_MaterialOutStorageShow").FirstOrDefault();
                    dispatchInfo.TaskCode = "Task_MaterialOutStorageShow" + DateTime.Now.ToString("yyyyMMddhhmmss");
                    dispatchInfo.CreatedTime = inputDtos[i].CreatedTime;
                    dispatchInfo.TaskResult = "成功";
                    dispatchInfo.FinishTime = DateTime.Now;
                    dispatchInfo.LastUpdatedTime = inputDtos[i].LastUpdatedTime;
                    dispatchInfo.LastUpdatorUserId = inputDtos[i].LastUpdatorUserId;
                    dispatchInfo.CreatorUserId = inputDtos[i].CreatorUserId;
                    dispatchInfo.Description = "原料自动出库演示";
                    dispatchInfo.OutStorage_Id = inputDtos[i].Id;
                    dispatchInfos[i] = dispatchInfo;
                    //更新库位表
                    MatWareHouseLocationInfo locationInfo = new MatWareHouseLocationInfo();
                    var bid = inputDtos[i].MaterialBatchs[0].Id;
                    Guid locationid = MaterialBatchRepository.TrackEntities.Where(x => x.Id == bid).FirstOrDefault().MatWareHouseLocation.Id;
                    locationInfo = MatWareHouseLocationInfoRepository.TrackEntities.Where(x => x.Id == locationid).FirstOrDefault();
                    Guid? palletid = locationInfo.PalletID;
                    locationInfo.PalletID = null;
                    count1 += await MatWareHouseLocationInfoRepository.UpdateAsync(locationInfo);
                    //更新出库单
                    MaterialOutStorageInfo outstorageInfo = new MaterialOutStorageInfo();
                    var oid = inputDtos[i].Id;
                    outstorageInfo = MaterialOutStorageRepository.TrackEntities.Where(x => x.Id == oid).FirstOrDefault();
                    outstorageInfo.PalletID = palletid;
                    outstorageInfo.FinishTime = DateTime.Now;
                    outstorageInfo.OutStorageStatus = 2;
                    outstorageInfo.PalletQuantity = 1;
                    outstorageInfo.LastUpdatedTime = inputDtos[i].LastUpdatedTime;
                    outstorageInfo.LastUpdatorUserId = inputDtos[i].LastUpdatorUserId;
                    count1 += await MaterialOutStorageRepository.UpdateAsync(outstorageInfo);
                    //更新批次表
                    MaterialBatchInfo batchInfo = new MaterialBatchInfo();
                    batchInfo = MaterialBatchRepository.TrackEntities.Where(x => x.Id == bid).FirstOrDefault();
                    if (batchInfo.Quantity != outstorageInfo.Quantity)
                    {
                        return new OperationResult(OperationResultType.Error, "Service:选择库位库存与出库单数量不一致！");
                    }
                    batchInfo.Quantity = 0;
                    batchInfo.LastUpdatedTime = inputDtos[i].LastUpdatedTime;
                    batchInfo.LastUpdatorUserId = inputDtos[i].LastUpdatorUserId;
                    count1 += await MaterialBatchRepository.UpdateAsync(batchInfo);
                    //插入库存日志表
                    MaterialStorageLogInfoInputDto logInfo = new MaterialStorageLogInfoInputDto();
                    logInfo.MaterialBatch = MaterialBatchRepository.TrackEntities.Where(x => x.Id == bid).FirstOrDefault();
                    var mid = outstorageInfo.MaterialID;
                    logInfo.Material = MaterialInfoRepository.TrackEntities.Where(x => x.Id == mid).FirstOrDefault();
                    logInfo.StorageChangeType = 2;
                    logInfo.OriginalAmount = outstorageInfo.Quantity;
                    logInfo.ChangedAmount = -outstorageInfo.Quantity;
                    logInfo.CurrentAmount = 0;
                    logInfo.OutStorageID = oid;
                    logInfo.CreatorUserId = inputDtos[i].CreatorUserId;
                    logInfo.CreatedTime = inputDtos[i].CreatedTime;
                    logInfo.LastUpdatedTime = inputDtos[i].LastUpdatedTime;
                    logInfo.LastUpdatorUserId = inputDtos[i].LastUpdatorUserId;
                    logInfos[i] = logInfo;
                }
                else
                {
                    return new OperationResult(OperationResultType.Error, "选择库位数据异常,该组数据不被存储。");
                }
            }
            result1 = await DisTaskDispatchInfoRepository.InsertAsync(dispatchInfos);
            result2 = await MaterialStorageLogRepository.InsertAsync(logInfos);
            MaterialStorageLogRepository.UnitOfWork.Commit();
            return count1 >= 3
               ? new OperationResult(OperationResultType.Success, "原料自动出库任务操作成功！")
               : new OperationResult(OperationResultType.Error, "原料自动出库任务操作失败！");
            // return new OperationResult(OperationResultType.Success, "原料自动出库任务操作成功");
        }

    }
}
