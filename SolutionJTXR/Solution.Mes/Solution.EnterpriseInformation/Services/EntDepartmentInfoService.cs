using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Contracts;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.Services
{
    /// <summary>
    /// 部门信息服务
    /// </summary>
    public class EntDepartmentInfoService : IEntDepartmentInfoContract
    {
        /// <summary>
        /// 部门信息实体仓储
        /// </summary>
        public IRepository<EntDepartmentInfo, Guid> EntDepartmenInfoRepository { get; set; }

        /// <summary>
        /// 查询部门信息
        /// </summary>
        public IQueryable<EntDepartmentInfo> EntDepartmentInfos
        {
            get { return EntDepartmenInfoRepository.Entities; }
        }

        /// <summary>
        /// 检查部门信息是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEntDepartmentInfosExists(Expression<Func<EntDepartmentInfo, bool>> predicate, Guid id)
        {
            return EntDepartmenInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddEntDepartmentInfos(params EntDepartmentInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EntDepartmentInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.DepartmentName))
                    return new OperationResult(OperationResultType.Error, "请正确填写部门名称！");
                if (string.IsNullOrEmpty(dtoData.DepartmentCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写部门编号！");
                if (EntDepartmenInfoRepository.CheckExists(x => x.DepartmentName == dtoData.DepartmentName /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该部门名称已存在，无法保存！");
                if (EntDepartmenInfoRepository.CheckExists(x => x.DepartmentCode == dtoData.DepartmentCode/* && x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该部门编号已存在，无法保存！");
            }
            EntDepartmenInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntDepartmenInfoRepository.InsertAsync(inputDtos);
            EntDepartmenInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateEntDepartmentInfos(params EntDepartmentInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (EntDepartmentInfoInputDto dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.DepartmentName))
                    return new OperationResult(OperationResultType.Error, "请正确填写部门名称！");
                if (string.IsNullOrEmpty(dtoData.DepartmentCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写部门编号！");
                if (EntDepartmenInfoRepository.CheckExists(x => x.DepartmentName == dtoData.DepartmentName && x.Id != dtoData.Id /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该部门名称已存在，无法保存！");
                if (EntDepartmenInfoRepository.CheckExists(x => x.DepartmentCode == dtoData.DepartmentCode && x.Id != dtoData.Id /*&& x.IsDeleted == false*/))
                    return new OperationResult(OperationResultType.Error, "该部门编号已存在，无法保存！");
            }
            EntDepartmenInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntDepartmenInfoRepository.UpdateAsync(inputDtos);
            EntDepartmenInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 物理删除部门信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteEntDepartmentInfos(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            //foreach (var id in ids)
            //{
            //    int count = EntSiteInfoRepository.Entities.Where(m => m.Enterprise.Id == id).Count();
            //    if (count > 0)
            //    {
            //        return new OperationResult(OperationResultType.Error, "部门数据关联厂区信息，不能被删除。");
            //    }
            //}
            EntDepartmenInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntDepartmenInfoRepository.DeleteAsync(ids);
            EntDepartmenInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
