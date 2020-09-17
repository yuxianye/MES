using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.MatWarehouseStorageManagement.Services
{
    /// <summary>
    /// 托盘信息服务
    /// </summary>
    public class MatPalletInfoService : IMatPalletInfoContract
    {

        /// <summary>
        /// 托盘信息实体仓储
        /// </summary>
        public IRepository<MatPalletInfo, Guid> MatPalletRepository { get; set; }
        //
        public IRepository<MatWareHouseLocationInfo, Guid> MatWareHouseLocationRepository { get; set; }

        /// <summary>
        /// 查询托盘信息
        /// </summary>
        public IQueryable<MatPalletInfo> MatPalletInfos
        {
            get { return MatPalletRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckMatPalletExists(Expression<Func<MatPalletInfo, bool>> predicate, Guid id)
        {
            return MatPalletRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加托盘信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MatPalletInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.PalletCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写托盘类型编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.PalletName))
                    return new OperationResult(OperationResultType.Error, "请正确填写托盘类型名称，该组数据不被存储。");
                //
                if (MatPalletRepository.CheckExists(x => x.PalletCode == dtoData.PalletCode))
                    return new OperationResult(OperationResultType.Error, $"托盘类型编号 {dtoData.PalletCode} 的数据已存在，该组数据不被存储。");
                if (MatPalletRepository.CheckExists(x => x.PalletName == dtoData.PalletName))
                    return new OperationResult(OperationResultType.Error, $"托盘类型名称 {dtoData.PalletName} 的数据已存在，该组数据不被存储。");

                if (dtoData.PalletMaxWeight > 1000)
                    return new OperationResult(OperationResultType.Error, $"托盘最大承重不能大于1000，该组数据不被存储。");

            }
            MatPalletRepository.UnitOfWork.BeginTransaction();
            var result = await MatPalletRepository.InsertAsync(inputDtos);
            MatPalletRepository.UnitOfWork.Commit();
            //
            return result;
        }
        /// <summary>
        /// 更新托盘信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateMatPallets(params MatPalletInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MatPalletInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.PalletCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写托盘编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.PalletName))
                    return new OperationResult(OperationResultType.Error, "请正确填写托盘名称，该组数据不被存储。");
                //
                if (MatPalletRepository.CheckExists(x => x.PalletCode == dtoData.PalletCode && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"托盘编号 {dtoData.PalletCode} 的数据已存在，该组数据不被存储。");
                if (MatPalletRepository.CheckExists(x => x.PalletName == dtoData.PalletName && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"托盘名称 {dtoData.PalletName} 的数据已存在，该组数据不被存储。");

                if (dtoData.PalletMaxWeight > 1000)
                    return new OperationResult(OperationResultType.Error, $"托盘最大承重不能大于1000，该组数据不被存储。");
            }
            //
            MatPalletRepository.UnitOfWork.BeginTransaction();
            //
            var result = await MatPalletRepository.UpdateAsync(inputDtos);
            MatPalletRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除托盘信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteMatPallets(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MatPalletRepository.UnitOfWork.BeginTransaction();
            //
            OperationResult result = new OperationResult();
            //
            foreach (Guid idsCurrent in ids)
            {
                int matwarehouseareaInfoNew2 = MatWareHouseLocationRepository.TrackEntities
                                                                                .Count(m => m.PalletID != null  && m.PalletID == idsCurrent );
                if (matwarehouseareaInfoNew2 == 0)
                {
                    result = await MatPalletRepository.DeleteAsync(ids);
                }
                else
                {
                    result.Message = "托盘已经被使用，不能删除！";
                }
            }
            //
            MatPalletRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
