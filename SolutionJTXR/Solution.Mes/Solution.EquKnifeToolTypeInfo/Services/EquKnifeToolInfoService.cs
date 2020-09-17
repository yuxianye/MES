using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EquipKnifeToolInfo.Contracts;
using Solution.EquipKnifeToolInfo.Dtos;
using Solution.EquipKnifeToolInfo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EquipKnifeToolInfo.Services
{
    public class EquKnifeToolInfoService : IEquKnifeToolInfoContract
    {

        public IRepository<EquKnifeToolInfo, Guid> EquKnifeToolInfoRepository { get; set; }
        public IRepository<KnifeToolTypeInfo, Guid> KnifeToolTypeInfoRepository { get; set; }

        /// <summary>
        /// 查询刀具信息
        /// </summary>
        public IQueryable<EquKnifeToolInfo> EquKnifeToolInfos
        {
            get { return EquKnifeToolInfoRepository.Entities; }
        }



        /// <summary>
        /// 增加刀具信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddKnifeTool(params EquKnifeToolInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.KnifeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写刀具信息编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.KnifeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写刀具存在问题信息，该组数据不被存储。");
                if (EquKnifeToolInfoRepository.CheckExists(x => x.KnifeCode == dtoData.KnifeCode))
                    return new OperationResult(OperationResultType.Error, $"刀具信息编号 {dtoData.KnifeCode} 的数据已存在，该组数据不被存储。");
                if (EquKnifeToolInfoRepository.CheckExists(x => x.KnifeName == dtoData.KnifeName))
                    return new OperationResult(OperationResultType.Error, $"刀具问题信息名称 {dtoData.KnifeName} 的数据已存在，该组数据不被存储。");
                dtoData.Knifetooltypeinfo = KnifeToolTypeInfoRepository.GetByKey(dtoData.KnifeToolTypeInfo_Id);
                if (Equals(dtoData.Knifetooltypeinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的刀具状态信息不存在,该组数据不被存储。");
                }

            }
            EquKnifeToolInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquKnifeToolInfoRepository.InsertAsync(inputDtos);
            EquKnifeToolInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 检查刀具信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的刀具信息编号</param>
        /// <returns>刀具信息是否存在</returns>
        public bool CheckKnifeToolExists(Expression<Func<EquKnifeToolInfo, bool>> predicate, Guid id)
        {
            return EquKnifeToolInfoRepository.CheckExists(predicate, id);
        }


        /// <summary>
        /// 物理删除刀具信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteKnifeTool(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            EquKnifeToolInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquKnifeToolInfoRepository.DeleteAsync(ids);
            EquKnifeToolInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 逻辑删除刀具信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        //public async Task<OperationResult> LogicDeleteKnifeTool(params KnifeToolInfo[] equiinfo)
        //{
        //    equiinfo.CheckNotNull("equiinfo");
        //    int count = 0;
        //    try
        //    {
        //        EquKnifeToolInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await EquKnifeToolInfoRepository.RecycleAsync(equiinfo);
        //        EquKnifeToolInfoRepository.UnitOfWork.Commit();
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
        //        names.Add(data.KnifeToolName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}
        /// <summary>
        /// 逻辑还原刀具信息
        /// </summary>
        /// <param name="enterinfo"></param>
        /// <returns></returns>
        //public async Task<OperationResult> LogicRestoreKnifeTool(params KnifeToolInfo[] equiinfo)
        //{
        //    equiinfo.CheckNotNull("equiinfo");
        //    int count = 0;

        //    try
        //    {
        //        EquKnifeToolInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await EquKnifeToolInfoRepository.RestoreAsync(equiinfo);
        //        EquKnifeToolInfoRepository.UnitOfWork.Commit();
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
        //        names.Add(data.KnifeToolName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        public async Task<OperationResult> UpdateKnifeTool(params EquKnifeToolInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.KnifeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写刀具信息编号，该组数据不被存储。");
                if (string.IsNullOrEmpty(dtoData.KnifeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写刀具存在问题信息，该组数据不被存储。");
                if (EquKnifeToolInfoRepository.CheckExists(x => x.KnifeCode == dtoData.KnifeCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"刀具信息编号 {dtoData.KnifeCode} 的数据已存在，该组数据不被存储。");
                if (EquKnifeToolInfoRepository.CheckExists(x => x.KnifeName == dtoData.KnifeName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"刀具问题信息名称 {dtoData.KnifeName} 的数据已存在，该组数据不被存储。");
                dtoData.Knifetooltypeinfo = KnifeToolTypeInfoRepository.GetByKey(dtoData.KnifeToolTypeInfo_Id);
                if (Equals(dtoData.Knifetooltypeinfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的刀具状态信息不存在,该组数据不被存储。");
                }

            }
            EquKnifeToolInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EquKnifeToolInfoRepository.UpdateAsync(inputDtos);
            EquKnifeToolInfoRepository.UnitOfWork.Commit();
            return result;



        }
    }
}
