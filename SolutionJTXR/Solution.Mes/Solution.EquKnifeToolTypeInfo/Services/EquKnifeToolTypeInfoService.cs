using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EquipKnifeToolInfo.Dtos;
using Solution.EquipKnifeToolInfo.Contracts;
using Solution.EquipKnifeToolInfo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EquipKnifeToolInfo.Services
{
    public class EquKnifeToolTypeInfoService : IKnifeToolTypeInfoContract
    {
        //public IQueryable<EquKnifeToolTypeInfo> EquKnifeToolTypeInfos => throw new NotImplementedException();
        /// <summary>
        /// 刀具类别信息实体仓储
        /// </summary>
        public IRepository<KnifeToolTypeInfo, Guid> KnifeToolTypeRepository { get; set; }

        /// <summary>
        /// 查询刀具类别信息
        /// </summary>
        public IQueryable<KnifeToolTypeInfo> KnifeToolTypeInfos
        {
            get { return KnifeToolTypeRepository.Entities; }
        }

        /// <summary>
        /// 增加刀具类别信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddKnifeToolType(params KnifeToolTypeInfoInputDto[] inputDtos)
        {

            inputDtos.CheckNotNull("inputDtos");
            foreach (KnifeToolTypeInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.KnifeToolTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写刀具类别编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.KnifeToolTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写刀具类别名称，该组数据不被存储。");
                if (KnifeToolTypeRepository.CheckExists(x => x.KnifeToolTypeCode == dtoData.KnifeToolTypeCode /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, $"刀具类别编号 {dtoData.KnifeToolTypeCode} 的数据已存在，该组数据不被存储。");
                if (KnifeToolTypeRepository.CheckExists(x => x.KnifeToolTypeName == dtoData.KnifeToolTypeName /* && x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, $"刀具类别名称 {dtoData.KnifeToolTypeName} 的数据已存在，该组数据不被存储。");
            }
            KnifeToolTypeRepository.UnitOfWork.BeginTransaction();
            var result = await KnifeToolTypeRepository.InsertAsync(inputDtos);
            KnifeToolTypeRepository.UnitOfWork.Commit();
            return result;
        }
        /// <summary>
        /// 检查刀具类别信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的刀具类别信息编号</param>
        /// <returns>刀具信息是否存在</returns>
        public bool CheckKnifeToolTypeExists(Expression<Func<KnifeToolTypeInfo, bool>> predicate, Guid id)
        {
            return KnifeToolTypeRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 物理删除刀具类别信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteKnifeToolType(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            KnifeToolTypeRepository.UnitOfWork.BeginTransaction();
            var result = await KnifeToolTypeRepository.DeleteAsync(ids);
            KnifeToolTypeRepository.UnitOfWork.Commit();
            return result;


        }

        /// <summary>
        /// 逻辑删除刀具类别信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        //public async Task<OperationResult> LogicDeleteKnifeToolType(params EquKnifeToolTypeInfo[] EquKnifeToolTypeInfo)
        //{
        //    EquKnifeToolTypeInfo.CheckNotNull("EquKnifeToolTypeInfo");
        //    int count = 0;
        //    try
        //    {
        //        KnifeToolTypeRepository.UnitOfWork.BeginTransaction();
        //        count = await KnifeToolTypeRepository.RecycleAsync(EquKnifeToolTypeInfo);
        //        KnifeToolTypeRepository.UnitOfWork.Commit();
        //    }
        //    catch (DataException dataException)
        //    {
        //        return new OperationResult(OperationResultType.Error, dataException.Message);
        //    }
        //    catch (OSharpException osharpException)
        //    {
        //        return new OperationResult(OperationResultType.Error, osharpException.Message);
        //    }

        //    List<string> names = new List<string>();
        //    foreach (var data in EquKnifeToolTypeInfo)
        //    {
        //        names.Add(data.KnifeToolTypeName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}
        /// <summary>
        /// 逻辑还原刀具类别信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        //public async Task<OperationResult> LogicRestoreKnifeToolType(params EquKnifeToolTypeInfo[] equiinfo)
        //{
        //    equiinfo.CheckNotNull("equiinfo");
        //    int count = 0;

        //    try
        //    {
        //        KnifeToolTypeRepository.UnitOfWork.BeginTransaction();
        //        count = await KnifeToolTypeRepository.RestoreAsync(equiinfo);
        //        KnifeToolTypeRepository.UnitOfWork.Commit();
        //    }
        //    catch (DataException dataException)
        //    {
        //        return new OperationResult(OperationResultType.Error, dataException.Message);
        //    }
        //    catch (OSharpException osharpException)
        //    {
        //        return new OperationResult(OperationResultType.Error, osharpException.Message);
        //    }

        //    List<string> names = new List<string>();
        //    foreach (var data in equiinfo)
        //    {
        //        names.Add(data.KnifeToolTypeName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}
        /// <summary>
        /// 更新刀具信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateKnifeToolType(params KnifeToolTypeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (KnifeToolTypeInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.KnifeToolTypeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写刀具类别编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.KnifeToolTypeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写刀具类别名称，该组数据不被存储。");
                if (KnifeToolTypeRepository.CheckExists(x => x.KnifeToolTypeCode == dtoData.KnifeToolTypeCode && x.Id != dtoData.Id/*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, $"刀具类别编号 {dtoData.KnifeToolTypeCode} 的数据已存在，该组数据不被存储。");
                if (KnifeToolTypeRepository.CheckExists(x => x.KnifeToolTypeName == dtoData.KnifeToolTypeName && x.Id != dtoData.Id/* && x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, $"刀具类别名称 {dtoData.KnifeToolTypeName} 的数据已存在，该组数据不被存储。");
            }
            KnifeToolTypeRepository.UnitOfWork.BeginTransaction();
            var result = await KnifeToolTypeRepository.UpdateAsync(inputDtos);
            KnifeToolTypeRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
