using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.EnterpriseInformation.Contracts;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.Services
{
    /// <summary>
    /// 企业信息服务
    /// </summary>
    public class EnterpriseInfoService : IEnterpriseInfoContract
    {
        /// <summary>
        /// 企业信息实体仓储
        /// </summary>
        public IRepository<EnterpriseInfo, Guid> EnterpriseRepository { get; set; }
        /// <summary>
        /// 厂区信息仓储
        /// </summary>
        public IRepository<EntSiteInfo, Guid> EntSiteInfoRepository { get; set; }

        /// <summary>
        /// 查询企业信息
        /// </summary>
        public IQueryable<EnterpriseInfo> EnterpriseInfos
        {
            get { return EnterpriseRepository.Entities; }
        }

        /// <summary>
        /// 检查企业信息是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEnterpriseExists(Expression<Func<EnterpriseInfo, bool>> predicate, Guid id)
        {
            return EnterpriseRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 添加企业信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddEnterprises(params EnterpriseInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EnterpriseInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.EnterpriseCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写企业编号！");
                if (string.IsNullOrEmpty(dtoData.EnterpriseName))
                    return new OperationResult(OperationResultType.Error, "请正确填写企业名称！");
                if (EnterpriseRepository.CheckExists(x => x.EnterpriseCode == dtoData.EnterpriseCode /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该企业编号已存在，无法保存！");
                if (EnterpriseRepository.CheckExists(x => x.EnterpriseName == dtoData.EnterpriseName/* && x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该企业名称已存在，无法保存！");
            }
            EnterpriseRepository.UnitOfWork.BeginTransaction();
            var result = await EnterpriseRepository.InsertAsync(inputDtos);
            EnterpriseRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新企业信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateEnterprises(params EnterpriseInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EnterpriseInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.EnterpriseCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写企业编号！");
                if (string.IsNullOrEmpty(dtoData.EnterpriseName))
                    return new OperationResult(OperationResultType.Error, "请正确填写企业名称！");
                if (EnterpriseRepository.CheckExists(x => x.EnterpriseCode == dtoData.EnterpriseCode && x.Id != dtoData.Id /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该企业编号已存在，无法保存！");
                if (EnterpriseRepository.CheckExists(x => x.EnterpriseName == dtoData.EnterpriseName && x.Id != dtoData.Id /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该企业名称已存在，无法保存！");
            }
            EnterpriseRepository.UnitOfWork.BeginTransaction();
            var result = await EnterpriseRepository.UpdateAsync(inputDtos);
            EnterpriseRepository.UnitOfWork.Commit();
            return result;
        }

        ///// <summary>
        ///// 逻辑删除企业信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicDeleteEnterprises(params EnterpriseInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;
        //    try
        //    {
        //        EnterpriseRepository.UnitOfWork.BeginTransaction();
        //        count = await EnterpriseRepository.RecycleAsync(enterinfos);
        //        EnterpriseRepository.UnitOfWork.Commit();
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
        //    foreach (var data in enterinfos)
        //    {
        //        names.Add(data.EnterpriseName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        /// <summary>
        /// 物理删除企业信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteEnterprises(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count = EntSiteInfoRepository.Entities.Where(m => m.Enterprise.Id == id).Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, "企业数据关联厂区信息，不能被删除。");
                }
            }
            EnterpriseRepository.UnitOfWork.BeginTransaction();
            var result = await EnterpriseRepository.DeleteAsync(ids);
            EnterpriseRepository.UnitOfWork.Commit();
            return result;
        }

        ///// <summary>
        ///// 逻辑还原企业信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicRestoreEnterprises(params EnterpriseInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;
        //    try
        //    {
        //        foreach (EnterpriseInfo dtoData in enterinfos)
        //        {
        //            if (EnterpriseRepository.CheckExists(x => x.EnterpriseCode == dtoData.EnterpriseCode && x.Id != dtoData.Id && x.IsDeleted == false))
        //                return new OperationResult(OperationResultType.Error, $"企业编号 {dtoData.EnterpriseCode} 的数据已存在，数据恢复失败，请修改为其它编号之后重试恢复。");
        //            if (EnterpriseRepository.CheckExists(x => x.EnterpriseName == dtoData.EnterpriseName && x.Id != dtoData.Id && x.IsDeleted == false))
        //                return new OperationResult(OperationResultType.Error, $"企业名称 {dtoData.EnterpriseCode} 的数据已存在，数据恢复失败，请修改为其它编号之后重试恢复。");
        //        }

        //        EnterpriseRepository.UnitOfWork.BeginTransaction();
        //        count = await EnterpriseRepository.RestoreAsync(enterinfos);
        //        EnterpriseRepository.UnitOfWork.Commit();
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
        //    foreach (var data in enterinfos)
        //    {
        //        names.Add(data.EnterpriseName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑恢复成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑恢复成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

    }
}
