using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.Core.Contracts;
using Solution.Core.Dtos.Security;
using Solution.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Services
{
    public class ModuleService : IModuleContract
    {
        public IRepository<Solution.Core.Models.Security.Module, int> ModuleRepository { get; set; }

        /// <summary>
        /// 安全服务
        /// </summary>
        public SecurityService SecurityService { get; set; }
        public IQueryable<Solution.Core.Models.Security.Module> Modules
        {
            get { return SecurityService.SecurityManager.Modules; }
        }

        public async Task<OperationResult> AddModules(ModuleInputDto inputDto)
        {
            ModuleRepository.UnitOfWork.BeginTransaction();
            OperationResult result = await SecurityService.SecurityManager.CreateModule(inputDto);
            if (result.Successed)
            {
                Solution.Core.Models.Security.Module moduleData = SecurityService.SecurityManager.Modules.Where(x => x.Name.Equals(inputDto.Name)).FirstOrDefault();

                List<Guid> functionIds = new List<Guid>();
                foreach (var data in inputDto.FunctionModels)
                {
                    functionIds.Add(data.Id);
                }
                result = await SecurityService.SecurityManager.SetModuleFunctions(moduleData.Id, functionIds.ToArray());
            }
            ModuleRepository.UnitOfWork.Commit();
            if (result.ResultType == OperationResultType.Error)
            {
                return result;
            }
            else
            {
                return new OperationResult(OperationResultType.Success,"添加成功！");
            }
        }

        public async Task<bool> CheckModuleExists(Expression<Func<Models.Security.Module, bool>> predicate, int id = 0)
        {
            var result = await SecurityService.SecurityManager.CheckModuleExists(predicate);
            return result;
        }

        public async Task<OperationResult> DeleteModules(int id)
        {
            ModuleRepository.UnitOfWork.BeginTransaction();
            var result = await SecurityService.SecurityManager.DeleteModule(id);
            ModuleRepository.UnitOfWork.Commit();
            return result;
        }

        public async Task<OperationResult> EditModules(ModuleInputDto inputDto)
        {
            ModuleRepository.UnitOfWork.BeginTransaction();
            var result = await SecurityService.SecurityManager.UpdateModule(inputDto);
            if (result.Successed || result.ResultType == OperationResultType.NoChanged)
            {
                List<Guid> functionIds = new List<Guid>();
                foreach (var data in inputDto.FunctionModels)
                {
                    functionIds.Add(data.Id);
                }
                result = await SecurityService.SecurityManager.SetModuleFunctions(inputDto.Id, functionIds.ToArray());
            }
            ModuleRepository.UnitOfWork.Commit();
            if (result.ResultType == OperationResultType.Error)
            {
                return result;
            }
            else
            {
                return new OperationResult(OperationResultType.Success, "编辑成功！");
            }
        }
    }
}
