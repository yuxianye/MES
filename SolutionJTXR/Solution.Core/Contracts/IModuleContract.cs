using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.Core.Dtos.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Contracts
{
    public interface IModuleContract : IScopeDependency
    {
        /// <summary>
        /// 获取 模块信息查询数据集
        /// </summary>
        IQueryable<Solution.Core.Models.Security.Module> Modules { get; }

        /// <summary>
        /// 检查模块信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的功能信息编号</param>
        /// <returns>功能信息是否存在</returns>
        Task<bool> CheckModuleExists(Expression<Func<Solution.Core.Models.Security.Module, bool>> predicate, int id = default(int));

        /// <summary>
        /// 添加模块信息信息
        /// </summary>
        /// <param name="inputDtos">要添加的模块信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddModules(ModuleInputDto inputDto);

        /// <summary>
        /// 更新模块信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的功能信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditModules(ModuleInputDto inputDtos);

        /// <summary>
        /// 删除模块信息信息
        /// </summary>
        /// <param name="ids">要删除的模块信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteModules(int id);
    }
}
