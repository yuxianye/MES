
using System;

using OSharp.Core.Data;
using OSharp.Core.Security;
using Solution.Core.Contracts;
using Solution.Core.Models.Infos;

namespace Solution.Core.Services
{
    /// <summary>
    /// 业务实现——系统信息模块
    /// </summary>
    public partial class InfosService:IInfosContract
    {
        /// <summary>
        /// 获取或设置 组织机构信息仓储操作对象
        /// </summary>
        public IRepository<SysDicInfo, Guid> SysDicInfoRepository { protected get; set; }
    }
}
