using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Contracts;
using Solution.StoredInWarehouseManagement.Dtos;
using Solution.StoredInWarehouseManagement.Models;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.StoredInWarehouseManagement.Services
{
    /// <summary>
    /// 物料批次信息服务
    /// </summary>
    public class MaterialBatchInfoService : IMaterialBatchInfoContract
    {

        /// <summary>
        /// 物料批次信息实体仓储
        /// </summary>
        public IRepository<MaterialBatchInfo, Guid> MaterialBatchInfoRepository { get; set; }
        //
        //public IRepository<EntAreaInfo, Guid> EntAreaInfoRepository { get; set; }
        ////
        //public IRepository<MaterialBatchInfoTypeInfo, Guid> MaterialBatchInfoTypeInfoRepository { get; set; }
        //

        /// <summary>
        /// 查询物料批次信息
        /// </summary>
        public IQueryable<MaterialBatchInfo> MaterialBatchInfos
        {
            get { return MaterialBatchInfoRepository.Entities; }
        }
        public IQueryable<MaterialBatchInfo> MaterialTrackBatchInfos
        {
            get { return MaterialBatchInfoRepository.TrackEntities; }
        }
        /// <summary>
        /// 查询物料批次信息2
        /// </summary>
        public IQueryable<MaterialBatchInfoOutputDto> MaterialBatch2Infos
        {
            get
            {
                var aa = MaterialBatchInfoRepository.UnitOfWork.SqlQuery<MaterialBatchInfoOutputDto>("select  MaterialCode  as BatchCode ,1.0 as Quantity from MaterialInfoes");
                //aa.Any();
                //var a1 = aa.Select(m => m.BatchCode);
                return aa.AsQueryable();
            }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MaterialBatchInfo, bool>> predicate, Guid id)
        {
            return MaterialBatchInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加物料批次信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MaterialBatchInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                //if (string.IsNullOrEmpty(dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MaterialBatchInfoRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MaterialBatchInfoRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                ////
                //dtoData.EntArea = EntAreaInfoRepository.TrackEntities.Where(m => m.Id == dtoData.EntArea_Id).FirstOrDefault();
                //if (Equals(dtoData.EntArea, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的所属区域不存在,该组数据不被存储。");
                //}
                ////
                //dtoData.MaterialBatchInfoType = MaterialBatchInfoTypeInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MaterialBatchInfoType_Id).FirstOrDefault();
                //if (Equals(dtoData.MaterialBatchInfoType, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的仓库类型不存在,该组数据不被存储。");
                //}
            }
            MaterialBatchInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MaterialBatchInfoRepository.InsertAsync(inputDtos);
            MaterialBatchInfoRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新物料批次信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MaterialBatchInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MaterialBatchInfoInputDto dtoData in inputDtos)
            {
                //if (string.IsNullOrEmpty(dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MaterialBatchInfoRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MaterialBatchInfoRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName && x.Id != dtoData.Id ))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
            }
            //
            MaterialBatchInfoRepository.UnitOfWork.BeginTransaction();
            //
            //EntAreaInfo info = new EntAreaInfo();
            //foreach (var item in inputDtos)
            //{
            //    info = EntAreaInfoRepository.TrackEntities.Where(m => m.Id == item.EntArea_Id).FirstOrDefault();
            //    item.EntArea = info;
            //}
            ////
            //MaterialBatchInfoTypeInfo matwarehousetypeinfo = new MaterialBatchInfoTypeInfo();
            //foreach (var item in inputDtos)
            //{
            //    matwarehousetypeinfo = MaterialBatchInfoTypeInfoRepository.TrackEntities.Where(m => m.Id == item.MaterialBatchInfoType_Id).FirstOrDefault();
            //    item.MaterialBatchInfoType = matwarehousetypeinfo;
            //}
            //
            var result = await MaterialBatchInfoRepository.UpdateAsync(inputDtos);
            MaterialBatchInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除物料批次信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MaterialBatchInfoRepository.UnitOfWork.BeginTransaction();
            var result = await MaterialBatchInfoRepository.DeleteAsync(ids);
            MaterialBatchInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
