using OSharp.Core.Data;
using OSharp.Core.Mapping;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Contracts;
using Solution.StoredInWarehouseManagement.Dtos;
using Solution.StoredInWarehouseManagement.Models;
using Solution.StepTeachingDispatchManagement.Models;
using Solution.StepTeachingDispatchManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Solution.StoredInWarehouseManagement.Services
{
    /// <summary>
    /// 物料入库信息服务
    /// </summary>
    public class MaterialInStorageInfoService : IMaterialInStorageInfoContract
    {
        /// <summary>
        /// 物料入库信息实体仓储
        /// </summary>
        public IRepository<MaterialInStorageInfo, Guid> MaterialInStorageRepository { get; set; }
        public IRepository<MaterialBatchInfo, Guid> MaterialBatchInfoRepository { get; set; }        //
        public IRepository<MaterialStorageLogInfo, Guid> MaterialStorageLogInfoRepository { get; set; }
        //
        public IRepository<MaterialInfo, Guid> MaterialInfoRepository { get; set; }        //
        //public IRepository<MatSupplierInfo, Guid> MatSupplierInfoRepository { get; set; }
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationInfoRepository { get; set; }
        public IRepository<MaterialStorageLogInfo, Guid> MaterialStorageLogRepository { get; set; }
        public IRepository<DisStepActionInfo, Guid> DisStepActionInfoRepository { get; set; }
        public IRepository<DisTaskDispatchInfo, Guid> DisTaskDispatchInfoRepository { get; set; }
        public IRepository<MaterialBatchInfo, Guid> MaterialBatchRepository { get; set; }
        public IRepository<MatPalletInfo, Guid> MatPalletInfoRepository { get; set; }

        /// <summary>
        /// 查询物料入库信息
        /// </summary>
        public IQueryable<MaterialInStorageInfo> MaterialInStorageInfos
        {
            get { return MaterialInStorageRepository.Entities; }
        }
        public IQueryable<MaterialInStorageInfo> MaterialInStorageTrackInfos
        {
            get { return MaterialInStorageRepository.TrackEntities; }
        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MaterialInStorageInfo, bool>> predicate, Guid id)
        {
            return MaterialInStorageRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加物料入库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MaterialInStorageInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.InStorageBillCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
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
                //空托盘入库
                if (dtoData.InStorageType == (int)InStorageTypeEnumModel.InStorageType.PalletInStorageType &&
                     dtoData.PalletID == null)
                    return new OperationResult(OperationResultType.Error, "请正确选择托盘信息，该组数据不被存储。");

                //原料手动入库
                if (dtoData.InStorageType == (int)InStorageTypeEnumModel.InStorageType.MaterialManuallyInStorageType)
                {
                    if (dtoData.MaterialID == null)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料信息，该组数据不被存储。");

                    if (dtoData.Quantity == null || dtoData.Quantity == decimal.Zero)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料入库数量，该组数据不被存储。");

                    if (dtoData.Quantity > 500)
                        return new OperationResult(OperationResultType.Error, $"物料入库数量不能大于500，该组数据不被存储。");
                }
            }
            MaterialInStorageRepository.UnitOfWork.BeginTransaction();
            var result = await MaterialInStorageRepository.InsertAsync(inputDtos);
            MaterialInStorageRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新物料入库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MaterialInStorageInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MaterialInStorageInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.InStorageBillCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写单据编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                //
                //空托盘入库
                if (dtoData.InStorageType == (int)InStorageTypeEnumModel.InStorageType.PalletInStorageType &&
                     dtoData.PalletID == null)
                    return new OperationResult(OperationResultType.Error, "请正确选择托盘信息，该组数据不被存储。");

                //原料手动入库
                if (dtoData.InStorageType == (int)InStorageTypeEnumModel.InStorageType.MaterialManuallyInStorageType)
                {
                    if (dtoData.MaterialID == null)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料信息，该组数据不被存储。");

                    if (dtoData.Quantity == null || dtoData.Quantity == decimal.Zero)
                        return new OperationResult(OperationResultType.Error, "请正确选择物料入库数量，该组数据不被存储。");

                    if (dtoData.Quantity > 500)
                        return new OperationResult(OperationResultType.Error, $"物料入库数量不能大于500，该组数据不被存储。");
                }
            }
            //
            MaterialInStorageRepository.UnitOfWork.BeginTransaction();
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
            var result = await MaterialInStorageRepository.UpdateAsync(inputDtos);
            MaterialInStorageRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 审核物料入库信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Audit(params MaterialInStorageInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MaterialInStorageInfoInputDto dtoData in inputDtos)
            {
                //if (string.IsNullOrEmpty(dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MaterialInStorageRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
            }
            //
            MaterialInStorageRepository.UnitOfWork.BeginTransaction();
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
            var result = await MaterialInStorageRepository.UpdateAsync(inputDtos);
            MaterialInStorageRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除物料入库信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MaterialInStorageRepository.UnitOfWork.BeginTransaction();
            var result = await MaterialInStorageRepository.DeleteAsync(ids);
            MaterialInStorageRepository.UnitOfWork.Commit();
            return result;
        }

        ///////////
        /// <summary>
        /// 更新作业单据信息
        /// </summary>
        /// <param name="dtos">包含更新信息的物料DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddTask(params MaterialInStorageInfoInputDto[] dtos)
        {
            List<string> names = new List<string>();
            //
            MaterialInStorageRepository.UnitOfWork.BeginTransaction();
            foreach (MaterialInStorageInfoInputDto dto in dtos)
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
                names.Add(dto.InStorageBillCode);

                //User savedUser = UserManager.Users.Where(x => x.UserName.Equals(dto.UserName) &&
                //                                           x.NickName.Equals(dto.NickName) &&
                //                                           x.Email.Equals(dto.Email) &&
                //                                           x.PhoneNumber.Equals(dto.PhoneNumber)).FirstOrDefault();
                //if (savedUser != null)
                //{
                //
                List<Guid> WareHouseLocationIds = new List<Guid>();
                WareHouseLocationIds = dto.MatWareHouseLocations.Select(x => x.Id).ToList();
                //
                //空托盘入库
                if (dto.InStorageType == (int)InStorageTypeEnumModel.InStorageType.PalletInStorageType)
                {
                    var setResult = await SetPalletInStorageWareHouseLocation(dto.Id, dto, WareHouseLocationIds.ToArray());
                    if (setResult.ResultType.Equals(OperationResultType.Error))
                    {
                        return setResult;
                    }
                }
                //原料手动入库
                else if (dto.InStorageType == (int)InStorageTypeEnumModel.InStorageType.MaterialManuallyInStorageType)
                {
                    var setResult = await SetMaterialInStorageWareHouseLocation(dto.Id, dto, WareHouseLocationIds.ToArray(), dto.UserName);
                    if (setResult.ResultType.Equals(OperationResultType.Error))
                    {
                        return setResult;
                    }
                }
                //
                dto.InStorageTime = DateTime.Now;
                dto.FinishTime = DateTime.Now;
                //已完成
                dto.InStorageStatus = (int)InStorageStatusEnumModel.InStorageStatus.InStorageFinishStatus;
                //
                int count = 0;
                //
                MaterialInStorageInfo materialinstorageInfo = new MaterialInStorageInfo();
                materialinstorageInfo = dto.MapTo(materialinstorageInfo);
                //
                count += await MaterialInStorageRepository.UpdateAsync(materialinstorageInfo);
            }
            MaterialInStorageRepository.UnitOfWork.Commit();
            //
            return new OperationResult(OperationResultType.Success, "库位“{0}”更新成功".FormatWith(names.ExpandAndToString()));
        }

        /// <summary>
        /// 设置入库任务的空托盘库位
        /// </summary>
        /// <param name="id">入库任务编号</param>
        /// <param name="roleIds">库位编号集合</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetPalletInStorageWareHouseLocation(Guid MaterialInStorageId, MaterialInStorageInfoInputDto MaterialInStorageDtos, Guid[] WareHouseLocationIds)
        {
            Guid[] existIds = MaterialBatchInfoRepository.Entities.Where(m => m.MaterialInStorage.Id == MaterialInStorageId).Select(m => m.Id).ToArray();
            Guid[] addIds = WareHouseLocationIds.Except(existIds).ToArray();
            Guid[] removeIds = existIds.Except(WareHouseLocationIds).ToArray();
            //
            //MaterialBatchInfoRepository.UnitOfWork.BeginTransaction();
            ////
            int count = 0;
            //foreach (Guid addId in addIds)
            //{
            //    //Role role = await RoleRepository.GetByKeyAsync(addId);
            //    //if (role == null)
            //    //{
            //    //    return new OperationResult(OperationResultType.QueryNull, "指定编号的角色信息不存在");
            //    //}

            //    MaterialBatchInfo map = new MaterialBatchInfo() { Id = addId, BatchCode = "2018/07/27" };
            //    //
            //    count += await MaterialBatchInfoRepository.InsertAsync(map);
            //}
            //count += await MaterialBatchInfoRepository.DeleteAsync(m => m.MaterialInStorage.Id == id);
            //MaterialBatchInfoRepository.UnitOfWork.Commit();
            //
            //////////////
            //MatWareHouseTypeInfo matwarehousetypeInfo = new MatWareHouseTypeInfo();
            //matwarehousetypeInfo.WareHouseTypeCode = "0001";
            //matwarehousetypeInfo.WareHouseTypeName = "库位1";
            //await MatWareHouseTypeInfoRepository.InsertAsync(matwarehousetypeInfo);

            //MaterialOutStorageInfo materialoutstorageInfo = new MaterialOutStorageInfo();
            //materialoutstorageInfo.Remark = "0001";
            ////
            //Guid Material_Id = Guid.Parse("3C973F9E-1F8B-E811-895C-005056C00008");
            //materialoutstorageInfo.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == Material_Id).FirstOrDefault();
            //if (Equals(materialoutstorageInfo.Material, null))
            //{
            //    return new OperationResult(OperationResultType.Error, $"对应的物料信息不存在,该组数据不被存储。");
            //}
            ////materialoutstorageInfo.WareHouseTypeName = "库位1";
            //count += await MaterialOutStorageInfoRepository.InsertAsync(materialoutstorageInfo);

            //////////////库位表
            foreach (Guid WareHouseLocationId in WareHouseLocationIds)
            {
                MatWareHouseLocationInfo matwarehouselocationInfo = new MatWareHouseLocationInfo();
                //
                matwarehouselocationInfo = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == WareHouseLocationId).FirstOrDefault();
                //
                matwarehouselocationInfo.PalletID = MaterialInStorageDtos.PalletID;
                //
                count += await MatWareHouseLocationInfoRepository.UpdateAsync(matwarehouselocationInfo);
            }
            //////////////
            return count > 0
            ? new OperationResult(OperationResultType.Success, "空托盘入库任务“{0}”指派库位操作成功".FormatWith("Test"))
            : OperationResult.NoChanged;
        }


        /// <summary>
        /// 设置入库任务的库位
        /// </summary>
        /// <param name="id">入库任务编号</param>
        /// <param name="roleIds">库位编号集合</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetMaterialInStorageWareHouseLocation(Guid MaterialInStorageId, MaterialInStorageInfoInputDto MaterialInStorageDtos, Guid[] WareHouseLocationIds, string UserName)
        {
            //User user = await UserRepository.GetByKeyAsync(id);
            //if (user == null)
            //{
            //    return new OperationResult(OperationResultType.QueryNull, "指定编号的用户信息不存在");
            //}
            ////
            //
            //MaterialBatchInfoRepository.UnitOfWork.BeginTransaction();
            ////
            int count = 0;
            //foreach (Guid addId in addIds)
            //{
            //    //Role role = await RoleRepository.GetByKeyAsync(addId);
            //    //if (role == null)
            //    //{
            //    //    return new OperationResult(OperationResultType.QueryNull, "指定编号的角色信息不存在");
            //    //}

            //    MaterialBatchInfo map = new MaterialBatchInfo() { Id = addId, BatchCode = "2018/07/27" };
            //    //
            //    count += await MaterialBatchInfoRepository.InsertAsync(map);
            //}
            //count += await MaterialBatchInfoRepository.DeleteAsync(m => m.MaterialInStorage.Id == id);
            //MaterialBatchInfoRepository.UnitOfWork.Commit();
            //
            //////////////
            //MatWareHouseTypeInfo matwarehousetypeInfo = new MatWareHouseTypeInfo();
            //matwarehousetypeInfo.WareHouseTypeCode = "0001";
            //matwarehousetypeInfo.WareHouseTypeName = "库位1";
            //await MatWareHouseTypeInfoRepository.InsertAsync(matwarehousetypeInfo);

            //MaterialOutStorageInfo materialoutstorageInfo = new MaterialOutStorageInfo();
            //materialoutstorageInfo.Remark = "0001";
            ////
            //Guid Material_Id = Guid.Parse("3C973F9E-1F8B-E811-895C-005056C00008");
            //materialoutstorageInfo.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == Material_Id).FirstOrDefault();
            //if (Equals(materialoutstorageInfo.Material, null))
            //{
            //    return new OperationResult(OperationResultType.Error, $"对应的物料信息不存在,该组数据不被存储。");
            //}
            ////materialoutstorageInfo.WareHouseTypeName = "库位1";
            //count += await MaterialOutStorageInfoRepository.InsertAsync(materialoutstorageInfo);

            //////////////入库单
            MaterialInStorageInfo materialinstorageInfo = new MaterialInStorageInfo();
            materialinstorageInfo = MaterialInStorageRepository.TrackEntities.Where(m => m.Id == MaterialInStorageId).FirstOrDefault();
            //////////////
            string sBatchCode = GetInStorageBatchCode();
            decimal dQuantity = materialinstorageInfo.Quantity.Value;
            //
            //decimal dPalletQuantity = materialinstorageInfo.PalletQuantity.Value;
            //decimal dFullPalletQuantity = 2;
            //
            Guid MaterialID0 = MaterialInStorageDtos.MaterialID.Value;
            int dFullPalletQuantity = MaterialInfoRepository.TrackEntities.Where(m => m.Id == MaterialID0).Select(m => m.FullPalletQuantity).FirstOrDefault().Value;
            //////////////库位表
            foreach (Guid WareHouseLocationId in WareHouseLocationIds)
            {
                MaterialBatchInfo materialbatchInfo = new MaterialBatchInfo();
                //
                Guid MaterialInStorage_Id = MaterialInStorageId;
                materialbatchInfo.MaterialInStorage = MaterialInStorageRepository.TrackEntities.Where(m => m.Id == MaterialInStorage_Id).FirstOrDefault();
                //
                Guid MaterialID = MaterialInStorageDtos.MaterialID.Value;
                materialbatchInfo.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == MaterialID).FirstOrDefault();
                //
                //Guid MatSupplier_Id = Guid.Parse("750B08C8-BC8F-E811-8BE2-005056C00008");
                //materialbatchInfo.MatSupplier = MatSupplierInfoRepository.TrackEntities.Where(m => m.Id == MatSupplier_Id).FirstOrDefault();
                //
                Guid MatWareHouseLocation_Id = WareHouseLocationId;
                materialbatchInfo.MatWareHouseLocation = MatWareHouseLocationInfoRepository.TrackEntities.Where(m => m.Id == MatWareHouseLocation_Id).FirstOrDefault();
                //
                materialbatchInfo.BatchCode = sBatchCode;
                //
                if (dQuantity > dFullPalletQuantity)
                {
                    materialbatchInfo.Quantity = dFullPalletQuantity;
                    dQuantity = dQuantity - dFullPalletQuantity;
                }
                else
                {
                    materialbatchInfo.Quantity = dQuantity;
                }
                //
                materialbatchInfo.Description = "";
                materialbatchInfo.MatSupplierID = materialinstorageInfo.MatSupplierID;
                //
                materialbatchInfo.CreatorUserId = UserName;
                materialbatchInfo.CreatedTime = DateTime.Now;
                materialbatchInfo.LastUpdatorUserId = materialbatchInfo.CreatorUserId;
                materialbatchInfo.LastUpdatedTime = materialbatchInfo.CreatedTime;
                //
                count += await MaterialBatchInfoRepository.InsertAsync(materialbatchInfo);

                //////////////
                //////////////流水帐

                MaterialStorageLogInfo materialstoragelogInfo = new MaterialStorageLogInfo();
                //
                Guid MaterialBatch_Id = materialbatchInfo.Id;
                materialstoragelogInfo.MaterialBatch = MaterialBatchInfoRepository.TrackEntities.Where(m => m.Id == MaterialBatch_Id).FirstOrDefault();
                //
                MaterialID = MaterialInStorageDtos.MaterialID.Value;
                materialstoragelogInfo.Material = MaterialInfoRepository.TrackEntities.Where(m => m.Id == MaterialID).FirstOrDefault();
                //
                materialstoragelogInfo.InStorageID = materialinstorageInfo.Id;
                //
                materialstoragelogInfo.OriginalAmount = 0;
                materialstoragelogInfo.ChangedAmount = materialbatchInfo.Quantity;
                materialstoragelogInfo.CurrentAmount = materialbatchInfo.Quantity;
                //
                //入库
                materialstoragelogInfo.StorageChangeType = (int)StorageChangeTypeEnumModel.StorageChangeType.InStorageChangeType;
                //
                materialstoragelogInfo.CreatorUserId = UserName;
                materialstoragelogInfo.CreatedTime = DateTime.Now;
                materialstoragelogInfo.LastUpdatorUserId = materialstoragelogInfo.CreatorUserId;
                materialstoragelogInfo.LastUpdatedTime = materialstoragelogInfo.CreatedTime;
                //
                count += await MaterialStorageLogInfoRepository.InsertAsync(materialstoragelogInfo);
            }
            //
            return count > 0
                ? new OperationResult(OperationResultType.Success, "入库任务“{0}”指派库位操作成功".FormatWith("Test"))
                : OperationResult.NoChanged;
        }

        /// <summary>
        /// 获取入库批次号
        /// </summary>
        public string GetInStorageBatchCode()
        {
            try
            {
                MaterialInStorageInfo MaterialInStorageInfoList = new MaterialInStorageInfo();
                //string sBatchCode = MaterialBatchInfoRepository.TrackEntities.Max(m => m.BatchCode);
                string sBatchCode = "Batch" + string.Format($"{DateTime.Now:yyyyMMddHHmmss}", DateTime.Now);
                //if (string.IsNullOrEmpty(sBatchCode))
                //{
                //    sBatchCode = "2018000001";
                //}
                //else
                //{
                //    int iBatchCode = Convert.ToInt32(sBatchCode) + 1;
                //    sBatchCode = iBatchCode.ToString();
                //}
                return sBatchCode;
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }
        /// <summary>
        /// 成品自动入库任务
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> ProductInStorageShowTask(params MaterialInStorageInfoInputDto[] inputDtos)
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
                int count = inputDtos[i].MatWareHouseLocations.Count();
                if (count == 1)
                {   //成品自动入库，若需要校验RFID扫描的托盘编号和初始值是否一致，后期可以放开这个条件。目前测试不需要这个条件
                    //Guid? pid = inputDtos[i].PalletID;
                    //string palletcode = MatPalletInfoRepository.TrackEntities.Where(m => m.Id == pid).FirstOrDefault().PalletCode;
                    //if (palletcode == inputDtos[i].PalletCode)
                    //{
                    DisTaskDispatchInfoInputDto dispatchInfo = new DisTaskDispatchInfoInputDto();
                    dispatchInfo.DisStepAction = DisStepActionInfoRepository.TrackEntities.Where(m => m.StepActionCode == "StepAction_ProductInStorage").FirstOrDefault();
                    dispatchInfo.TaskCode = "Task_ProductInStorageShow" + DateTime.Now.ToString("yyyyMMddhhmmss");
                    dispatchInfo.CreatedTime = inputDtos[i].CreatedTime;
                    dispatchInfo.TaskResult = "成功";
                    dispatchInfo.FinishTime = DateTime.Now;
                    dispatchInfo.LastUpdatedTime = inputDtos[i].LastUpdatedTime;
                    dispatchInfo.LastUpdatorUserId = inputDtos[i].LastUpdatorUserId;
                    dispatchInfo.CreatorUserId = inputDtos[i].CreatorUserId;
                    dispatchInfo.Description = "成品自动回库演示";
                    dispatchInfo.InStorage_Id = inputDtos[i].Id;
                    dispatchInfos[i] = dispatchInfo;
                    //更新库位表
                    MatWareHouseLocationInfo locationInfo = new MatWareHouseLocationInfo();
                    Guid locationid = inputDtos[i].MatWareHouseLocations[0].Id;
                    locationInfo = MatWareHouseLocationInfoRepository.TrackEntities.Where(x => x.Id == locationid).FirstOrDefault();
                    locationInfo.PalletID = inputDtos[i].PalletID;
                    count1 += await MatWareHouseLocationInfoRepository.UpdateAsync(locationInfo);
                    //插入出库单
                    MaterialInStorageInfo instorageInfo = new MaterialInStorageInfo();
                    instorageInfo.MaterialID = MaterialInfoRepository.TrackEntities.Where(x => x.MaterialType == 3).FirstOrDefault().Id;
                    instorageInfo.InStorageBillCode = "InStorage" + DateTime.Now.ToString("yyyyMMddhhmmss");
                    instorageInfo.Quantity = inputDtos[i].Quantity;
                    instorageInfo.PalletID = inputDtos[i].PalletID;
                    instorageInfo.InStorageStatus = 2;
                    instorageInfo.AuditStatus = 3;
                    instorageInfo.InStorageTime = inputDtos[i].CreatedTime;
                    instorageInfo.AuditPerson = inputDtos[i].CreatorUserId;
                    instorageInfo.FinishTime = DateTime.Now;
                    instorageInfo.InStorageType = 3;
                    instorageInfo.PalletQuantity = 1;
                    instorageInfo.CreatedTime = inputDtos[i].CreatedTime;
                    instorageInfo.CreatorUserId = inputDtos[i].CreatorUserId;
                    instorageInfo.LastUpdatedTime = inputDtos[i].LastUpdatedTime;
                    instorageInfo.LastUpdatorUserId = inputDtos[i].LastUpdatorUserId;
                    count1 += await MaterialInStorageRepository.InsertAsync(instorageInfo);
                    //插入批次表
                    MaterialBatchInfo batchInfo = new MaterialBatchInfo();
                    batchInfo.BatchCode = "Batch" + DateTime.Now.ToString("yyyyMMddhhmmss");
                    batchInfo.Material = MaterialInfoRepository.TrackEntities.Where(x => x.MaterialType == 3).FirstOrDefault();
                    batchInfo.MatWareHouseLocation = MatWareHouseLocationInfoRepository.TrackEntities.Where(x => x.Id == locationid).FirstOrDefault();
                    batchInfo.MaterialInStorage = instorageInfo;
                    batchInfo.Quantity = inputDtos[i].Quantity;
                    batchInfo.CreatorUserId = inputDtos[i].CreatorUserId;
                    batchInfo.LastUpdatedTime = inputDtos[i].LastUpdatedTime;
                    batchInfo.LastUpdatorUserId = inputDtos[i].LastUpdatorUserId;
                    count1 += await MaterialBatchRepository.InsertAsync(batchInfo);
                    //插入库存日志表
                    MaterialStorageLogInfoInputDto logInfo = new MaterialStorageLogInfoInputDto();
                    logInfo.MaterialBatch = batchInfo;
                    var mid = inputDtos[i].MaterialID;
                    logInfo.Material = MaterialInfoRepository.TrackEntities.Where(x => x.MaterialType == 3).FirstOrDefault();
                    logInfo.StorageChangeType = 1;
                    logInfo.OriginalAmount = 0;
                    logInfo.ChangedAmount = inputDtos[i].Quantity;
                    logInfo.CurrentAmount = inputDtos[i].Quantity;
                    logInfo.InStorageID = instorageInfo.Id;
                    logInfo.CreatorUserId = inputDtos[i].CreatorUserId;
                    logInfo.CreatedTime = inputDtos[i].CreatedTime;
                    logInfo.LastUpdatedTime = inputDtos[i].LastUpdatedTime;
                    logInfo.LastUpdatorUserId = inputDtos[i].LastUpdatorUserId;
                    logInfos[i] = logInfo;
                    //}
                    //else
                    //{
                    //    return new OperationResult(OperationResultType.Error, "托盘编号与初始值不一致！" + inputDtos[i].PalletCode + ";原PalletCode:" + palletcode);
                    //}
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
               ? new OperationResult(OperationResultType.Success, "成品自动回库任务操作成功！")
               : new OperationResult(OperationResultType.Error, "成品自动回库任务操作失败！");
        }
    }
}
