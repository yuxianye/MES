using OSharp.Core.Dependency;
using Solution.StereoscopicWarehouseManagement.Dtos;
using System;
using System.Collections.Generic;

namespace Solution.StereoscopicWarehouseManagement.Contracts
{
    /// <summary>
    /// 仓位图信息契约接口
    /// </summary>
    public interface IMatWareHousAreaLocationInfoContract : IScopeDependency
    {
        #region 仓位图信息业务

        /// <summary>
        /// 初始化仓位图信息
        /// </summary>
        /// <returns></returns>
        List<MatWareHousAreaLocationInfoOutputDto> Ini1(Guid id);

        /// <summary>
        /// 初始化仓位图信息
        /// </summary>
        /// <returns></returns>
        List<MatWareHousAreaLocationItemInfoOutputDto> Ini2(string id);
        
        #endregion
    }
}
