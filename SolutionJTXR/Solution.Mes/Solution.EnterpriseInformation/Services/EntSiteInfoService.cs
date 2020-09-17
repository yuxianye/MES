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
    public class EntSiteInfoService : IEntSiteInfoContract
    {
        public IRepository<EntSiteInfo, Guid> EntSiteInfoRepository { get; set; }
        public IRepository<EnterpriseInfo, Guid> EnterpriseInfoRepository { get; set; }
        public IRepository<EntAreaInfo, Guid> EntAreaInfoRepository { get; set; }

        public IQueryable<EntSiteInfo> EntSiteInfo
        {
            get { return EntSiteInfoRepository.Entities; }
        }
        ///// <summary>
        ///// 增加厂区信息
        ///// </summary>
        ///// <param name="inputDtos"></param>
        ///// <returns></returns>
        //public OperationResult Add(params EntSiteInfoInputDto[] inputDtos)
        //{

        //    inputDtos.CheckNotNull("inputDtos");
        //    OperationResult result = EntSiteInfoRepository.Insert(inputDtos,
        //        dto =>
        //        {
        //            if (EntSiteInfoRepository.CheckExists(m => m.SiteName == dto.SiteName))
        //            {
        //                throw new Exception("厂区名称为“{0}”的数据已存在，不能重复添加。".FormatWith(dto.SiteName));
        //            }
        //            if (EntSiteInfoRepository.CheckExists(m => m.SiteCode == dto.SiteCode))
        //            {
        //                throw new Exception("厂区编号为“{0}”的数据已存在，不能重复添加。".FormatWith(dto.SiteCode));
        //            }

        //            if (string.IsNullOrEmpty(dto.SiteName) || string.IsNullOrEmpty(dto.SiteCode))
        //            {
        //                throw new Exception("存在厂区名称或者厂区编号为空的数据，数据不合法，该组数据不被存储。");
        //            }
        //        },
        //        (dto, entity) =>
        //        {
        //            if (dto.Enterprise_Id != null)
        //            {
        //                EnterpriseInfo enterpriseInfo = EnterpriseInfoRepository.GetByKey(dto.Enterprise_Id);
        //                if (enterpriseInfo == null)
        //                {
        //                    throw new Exception("要加入的企业不存在。");
        //                }
        //                entity.Enterprise = enterpriseInfo;
        //            }
        //            else
        //            {
        //                entity.Enterprise = null;
        //            }
        //            return entity;
        //        });
        //    return result;
        //}

        /// <summary>
        /// 增加厂区信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EntSiteInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.SiteCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写厂区编号！");
                if (string.IsNullOrEmpty(dtoData.SiteName))
                    return new OperationResult(OperationResultType.Error, "请正确填写厂区名称！");
                if (EntSiteInfoRepository.CheckExists(x => x.SiteCode == dtoData.SiteCode))
                    return new OperationResult(OperationResultType.Error, "该厂区编号已存在，无法保存！");
                if (EntSiteInfoRepository.CheckExists(x => x.SiteName == dtoData.SiteName))
                    return new OperationResult(OperationResultType.Error, "该厂区名称已存在，无法保存！");
                dtoData.Enterprise = EnterpriseInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Enterprise_Id).FirstOrDefault();
                if (Equals(dtoData.Enterprise, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的企业不存在,无法保存！");
                }
            }
            EntSiteInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntSiteInfoRepository.InsertAsync(inputDtos);
            EntSiteInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEntSiteInfoExists(Expression<Func<EntSiteInfo, bool>> predicate, Guid id)
        {
            return EntSiteInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除厂区信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                int count = EntAreaInfoRepository.Entities.Where(m => m.EntSite.Id == id).Count();
                if (count > 0)
                {
                    return new OperationResult(OperationResultType.Error, "厂区数据关联车间信息，不能被删除。");
                }
            }
            EntSiteInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntSiteInfoRepository.DeleteAsync(ids);
            EntSiteInfoRepository.UnitOfWork.Commit();
            return result;
        }

        ///// <summary>
        ///// 逻辑删除厂区信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicDelete(params EntSiteInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;
        //    try
        //    {
        //        EntSiteInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await EntSiteInfoRepository.RecycleAsync(enterinfos);
        //        EntSiteInfoRepository.UnitOfWork.Commit();
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
        //        names.Add(data.SiteName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑删除成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        ///// <summary>
        ///// 逻辑还原厂区信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //public async Task<OperationResult> LogicRestore(params EntSiteInfo[] enterinfos)
        //{
        //    enterinfos.CheckNotNull("enterinfos");
        //    int count = 0;

        //    try
        //    {
        //        EntSiteInfoRepository.UnitOfWork.BeginTransaction();
        //        count = await EntSiteInfoRepository.RestoreAsync(enterinfos);
        //        EntSiteInfoRepository.UnitOfWork.Commit();
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
        //        names.Add(data.SiteName);
        //    }
        //    return count > 0
        //            ? new OperationResult(OperationResultType.Success,
        //                names.Count > 0
        //                    ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
        //                    : "{0}个信息逻辑还原成功".FormatWith(count))
        //            : new OperationResult(OperationResultType.NoChanged);
        //}

        /// <summary>
        /// 更新厂区信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EntSiteInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.SiteCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写厂区编号！");
                if (string.IsNullOrEmpty(dtoData.SiteName))
                    return new OperationResult(OperationResultType.Error, "请正确填写厂区名称！");
                if (EntSiteInfoRepository.CheckExists(x => x.SiteCode == dtoData.SiteCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该厂区编号已存在，无法保存！");
                if (EntSiteInfoRepository.CheckExists(x => x.SiteName == dtoData.SiteName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该厂区名称已存在，无法保存！");
                dtoData.Enterprise = EnterpriseInfoRepository.TrackEntities.Where(m => m.Id == dtoData.Enterprise_Id).FirstOrDefault();
                if (Equals(dtoData.Enterprise, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的企业不存在,无法保存！");
                }
            }
            EntSiteInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntSiteInfoRepository.UpdateAsync(inputDtos);
            EntSiteInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
