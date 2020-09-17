using OSharp.Core.Data;
using OSharp.Core.Security;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using Solution.Core.Contracts;
using Solution.Core.Dtos.Security;
using Solution.Core.Models.Security;
using Solution.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("系统管理-模块管理")]
    public class ModuleManagerController : ServiceBaseApiController
    {
        /// <summary>
        /// 模块服务契约
        /// </summary>
        public IModuleContract ModuleContract { get; set; }

        /// <summary>
        /// 安全服务
        /// </summary>
        public SecurityService SecurityService { get; set; }

        /// <summary>
        /// 获取或设置 模块仓储对象
        /// </summary>
        public IRepository<Module, int> ModuleRepository { protected get; set; }

        [Description("系统管理-模块管理-分页数据")]
        public IHttpActionResult PageData(PageRepuestParams requestParams)
        {
            try
            {
                GridRequest request = new GridRequest(requestParams);
                var page = GetPageResult(ModuleContract.Modules, m => new
                {
                    m.Id,
                    m.Name,
                    m.Remark,
                    m.OrderCode,
                    m.TreePathString,
                    m.Icon,
                    ParentId = m.Parent.Id.ToString()
                }, request);


                return Json(new OperationResult(OperationResultType.Success, "查询模块数据成功！", page));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询模块数据失败！" + ex.ToString()));
            }
        }

        [Description("系统管理-模块管理-功能列表数据")]
        public IHttpActionResult GetFunctionList(int id)
        {
            try
            {
                Module module = ModuleRepository.GetByKey(id);
                if (module == null)
                {
                    return Json(new OperationResult(OperationResultType.Error, "编号为“{0}”的模块信息不存在".FormatWith(id)));
                }
                int[] keys = module.TreePathIds;
                List<Function> functions = new List<Function>();
                if (keys.Length >= 2)
                {
                    int key = keys[keys.Length - 1];
                    functions = ModuleRepository.Entities.Where(m => key.Equals(m.Id)).SelectMany(m => m.Functions).DistinctBy(m => m.Id).ToList();
                }

                return Json(new OperationResult(OperationResultType.Success, "查询模块对应功能数据成功！", functions));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询模块对应功能数据失败！" + ex.ToString()));
            }
        }

        [Description("系统管理-模块管理-角色模块列表数据")]
        public IHttpActionResult GetRolesByModuleId(int id)
        {
            try
            {
                List<int> modules = SecurityService.SecurityManager.Modules.Where(m => m.Roles.Select(n => n.Id).Contains(id)).Select(m => m.Id).ToList();
                return Json(new OperationResult(OperationResultType.Success, "查询角色对应模块数据成功！", modules));

            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "查询角色对应模块数据失败！" + ex.ToString()));
            }
        }

        [Description("系统管理-模块管理-增加")]
        public async Task<IHttpActionResult> Add(ModuleInputDto dto)
        {
            try
            {
                var result = await ModuleContract.AddModules(dto);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "添加模块失败！" + ex.ToString()));
            }
        }

        [Description("系统管理-模块管理-修改")]
        public async Task<IHttpActionResult> Update(ModuleInputDto dto)
        {
            try
            {
                var result = await ModuleContract.EditModules(dto);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "修改模块失败！" + ex.ToString()));
            }
        }

        [Description("系统管理-模块管理-删除")]
        public async Task<IHttpActionResult> Remove(params int[] id)
        {
            try
            {
                OperationResult result = await ModuleContract.DeleteModules(id[0]);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new OperationResult(OperationResultType.Error, "删除模块失败！" + ex.ToString()));
            }
        }
    }
}