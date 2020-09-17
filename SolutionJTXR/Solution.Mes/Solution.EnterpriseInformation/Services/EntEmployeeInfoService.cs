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
    public class EntEmployeeInfoService : IEntEmployeeInfoContract
    {
        public IRepository<EntEmployeeInfo, Guid> EntEmployeeInfoRepository { get; set; }
        public IRepository<EntSiteInfo, Guid> EntSiteInfoRepository { get; set; }
        public IRepository<EnterpriseInfo, Guid> EnterpriseInfoRepository { get; set; }
        public IRepository<EntDepartmentInfo, Guid> EntDepartmentInfoRepository { get; set; }

        public IQueryable<EntEmployeeInfo> EntEmployeeInfo
        {
            get { return EntEmployeeInfoRepository.Entities; }
        }

        /// <summary>
        /// 增加车间信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Add(params EntEmployeeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.EntEmployeeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写员工编号！");
                if (string.IsNullOrEmpty(dtoData.EntEmployeeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写员工名称！");
                if (EntEmployeeInfoRepository.CheckExists(x => x.EntEmployeeCode == dtoData.EntEmployeeCode))
                    return new OperationResult(OperationResultType.Error, "该员工编号已存在，无法保存！");
                if (EntEmployeeInfoRepository.CheckExists(x => x.EntEmployeeName == dtoData.EntEmployeeName))
                    return new OperationResult(OperationResultType.Error, "该员工名称已存在，无法保存！");
                dtoData.DepartmentInfo = EntDepartmentInfoRepository.TrackEntities.Where(m => m.Id == dtoData.DepartmentCode).FirstOrDefault();
                //if (Equals(dtoData.DepartmentInfo, null))
                //{
                //    return new OperationResult(OperationResultType.Error, "对应的厂区不存在,无法保存！");
                //}
            }
            EntEmployeeInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntEmployeeInfoRepository.InsertAsync(inputDtos);
            EntEmployeeInfoRepository.UnitOfWork.Commit();
            return result;

        }
        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckEntEmployeeInfoExists(Expression<Func<EntEmployeeInfo, bool>> predicate, Guid id)
        {
            return EntEmployeeInfoRepository.CheckExists(predicate, id);
        }

        /// <summary>
        /// 物理删除车间信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            //foreach (var id in ids)
            //{
            //    int count = EntProductionLineInfoRepository.Entities.Where(m => m.EntArea.Id == id).Count();
            //    if (count > 0)
            //    {
            //        return new OperationResult(OperationResultType.Error, "车间数据关联生产线信息，不能被删除。");
            //    }
            //}
            EntEmployeeInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntEmployeeInfoRepository.DeleteAsync(ids);
            EntEmployeeInfoRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 更新车间信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(params EntEmployeeInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.EntEmployeeCode))
                    return new OperationResult(OperationResultType.Error, "请正确填写员工编号！");
                if (string.IsNullOrEmpty(dtoData.EntEmployeeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写车间名称！");
                if (EntEmployeeInfoRepository.CheckExists(x => x.EntEmployeeCode == dtoData.EntEmployeeCode && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该车间编号已存在，无法保存！");
                if (EntEmployeeInfoRepository.CheckExists(x => x.EntEmployeeName == dtoData.EntEmployeeName && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, "该车间名称已存在，无法保存！");
                dtoData.DepartmentInfo = EntDepartmentInfoRepository.TrackEntities.Where(m => m.Id == dtoData.DepartmentCode).FirstOrDefault();
                if (Equals(dtoData.DepartmentInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, "对应的厂区不存在,无法保存！");
                }
            }
            EntEmployeeInfoRepository.UnitOfWork.BeginTransaction();
            var result = await EntEmployeeInfoRepository.UpdateAsync(inputDtos);
            EntEmployeeInfoRepository.UnitOfWork.Commit();
            return result;
        }
    }
}
