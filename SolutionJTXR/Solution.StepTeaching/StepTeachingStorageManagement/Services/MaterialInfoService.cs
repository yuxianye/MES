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
    /// 物料信息服务
    /// </summary>
    public class MaterialInfoService : IMaterialInfoContract
    {

        /// <summary>
        /// 物料信息实体仓储
        /// </summary>
        public IRepository<MaterialInfo, Guid> MaterialRepository { get; set; }

        /// <summary>
        /// 查询物料信息
        /// </summary>
        public IQueryable<MaterialInfo> MaterialInfos
        {
            get { return MaterialRepository.Entities; }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckMaterialExists(Expression<Func<MaterialInfo, bool>> predicate, Guid id)
        {
            return MaterialRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 增加物料信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params MaterialInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.MaterialCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写物料类型编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.MaterialName))
                    return new OperationResult(OperationResultType.Error, "请正确填写物料类型名称，该组数据不被存储。");
                //
                if (MaterialRepository.CheckExists(x => x.MaterialCode == dtoData.MaterialCode))
                    return new OperationResult(OperationResultType.Error, $"物料类型编号 {dtoData.MaterialCode} 的数据已存在，该组数据不被存储。");
                if (MaterialRepository.CheckExists(x => x.MaterialName == dtoData.MaterialName))
                    return new OperationResult(OperationResultType.Error, $"物料类型名称 {dtoData.MaterialName} 的数据已存在，该组数据不被存储。");
                //
                if ( dtoData.FullPalletQuantity == 0 )
                    return new OperationResult(OperationResultType.Error, $"物料满盘数量不能为0，该组数据不被存储。");

                if ( dtoData.FullPalletQuantity > 10 )
                    return new OperationResult(OperationResultType.Error, $"物料满盘数量不能大于10，该组数据不被存储。");

                if (dtoData.UnitWeight > 100 )
                    return new OperationResult(OperationResultType.Error, $"物料单位重量不能大于100，该组数据不被存储。");
            }
            MaterialRepository.UnitOfWork.BeginTransaction();
            var result = await MaterialRepository.InsertAsync(inputDtos);
            MaterialRepository.UnitOfWork.Commit();
            //
            return result;
        }

        /// <summary>
        /// 更新物料信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateMaterials(params MaterialInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            //
            foreach (MaterialInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.MaterialCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写物料编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.MaterialName))
                    return new OperationResult(OperationResultType.Error, "请正确填写物料名称，该组数据不被存储。");
                //
                if (MaterialRepository.CheckExists(x => x.MaterialCode == dtoData.MaterialCode && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"物料编号 {dtoData.MaterialCode} 的数据已存在，该组数据不被存储。");
                if (MaterialRepository.CheckExists(x => x.MaterialName == dtoData.MaterialName && x.Id != dtoData.Id ))
                    return new OperationResult(OperationResultType.Error, $"物料名称 {dtoData.MaterialName} 的数据已存在，该组数据不被存储。");
                //
                if (dtoData.FullPalletQuantity == 0)
                    return new OperationResult(OperationResultType.Error, $"物料满盘数量不能为0，该组数据不被存储。");

                if (dtoData.FullPalletQuantity > 10)
                    return new OperationResult(OperationResultType.Error, $"物料满盘数量不能大于10，该组数据不被存储。");

                if (dtoData.UnitWeight > 100)
                    return new OperationResult(OperationResultType.Error, $"物料单位重量不能大于100，该组数据不被存储。");
            }
            //
            MaterialRepository.UnitOfWork.BeginTransaction();
            //
            var result = await MaterialRepository.UpdateAsync(inputDtos);
            MaterialRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除物料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteMaterials(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            MaterialRepository.UnitOfWork.BeginTransaction();
            var result = await MaterialRepository.DeleteAsync(ids);
            MaterialRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
