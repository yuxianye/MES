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
    /// 物料库存日志信息服务
    /// </summary>
    public class MaterialStorageLogInfoService : IMaterialStorageLogInfoContract
    {

        /// <summary>
        /// 物料库存日志信息实体仓储
        /// </summary>
        public IRepository<MaterialStorageLogInfo, Guid> MatWareHouseRepository { get; set; }
        //

        /// <summary>
        /// 查询物料库存日志信息
        /// </summary>
        public IQueryable<MaterialStorageLogInfo> MaterialStorageLogInfos
        {
            get { return MatWareHouseRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckExists(Expression<Func<MaterialStorageLogInfo, bool>> predicate, Guid id)
        {
            return MatWareHouseRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加物料库存日志信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MaterialStorageLogInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                //if (string.IsNullOrEmpty(dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库编号，该组数据不被存储。");
                //if (string.IsNullOrEmpty(dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, "请正确填写仓库名称，该组数据不被存储。");
                ////
                //if (MatWareHouseRepository.CheckExists(x => x.WareHouseCode == dtoData.WareHouseCode))
                //    return new OperationResult(OperationResultType.Error, $"仓库编号 {dtoData.WareHouseCode} 的数据已存在，该组数据不被存储。");
                //if (MatWareHouseRepository.CheckExists(x => x.WareHouseName == dtoData.WareHouseName))
                //    return new OperationResult(OperationResultType.Error, $"仓库名称 {dtoData.WareHouseName} 的数据已存在，该组数据不被存储。");
                ////
                //dtoData.EntArea = EntAreaInfoRepository.TrackEntities.Where(m => m.Id == dtoData.EntArea_Id).FirstOrDefault();
                //if (Equals(dtoData.EntArea, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的所属区域不存在,该组数据不被存储。");
                //}
                ////
                //dtoData.MatWareHouseType = MatWareHouseTypeInfoRepository.TrackEntities.Where(m => m.Id == dtoData.MatWareHouseType_Id).FirstOrDefault();
                //if (Equals(dtoData.MatWareHouseType, null))
                //{
                //    return new OperationResult(OperationResultType.Error, $"对应的仓库类型不存在,该组数据不被存储。");
                //}
            }
            MatWareHouseRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseRepository.InsertAsync(inputDtos);
            MatWareHouseRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新物料库存日志信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params MaterialStorageLogInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MaterialStorageLogInfoInputDto dtoData in inputDtos)
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
            MatWareHouseRepository.UnitOfWork.BeginTransaction();
            //
            //EntAreaInfo info = new EntAreaInfo();
            //foreach (var item in inputDtos)
            //{
            //    info = EntAreaInfoRepository.TrackEntities.Where(m => m.Id == item.EntArea_Id).FirstOrDefault();
            //    item.EntArea = info;
            //}
            ////
            //MatWareHouseTypeInfo matwarehousetypeinfo = new MatWareHouseTypeInfo();
            //foreach (var item in inputDtos)
            //{
            //    matwarehousetypeinfo = MatWareHouseTypeInfoRepository.TrackEntities.Where(m => m.Id == item.MatWareHouseType_Id).FirstOrDefault();
            //    item.MatWareHouseType = matwarehousetypeinfo;
            //}
            //
            var result = await MatWareHouseRepository.UpdateAsync(inputDtos);
            MatWareHouseRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除物料库存日志信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatWareHouseRepository.UnitOfWork.BeginTransaction();
            var result = await MatWareHouseRepository.DeleteAsync(ids);
            MatWareHouseRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
