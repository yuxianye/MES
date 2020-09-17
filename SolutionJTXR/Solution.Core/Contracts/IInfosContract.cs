using System;
using System.Linq;

using OSharp.Core.Dependency;
using Solution.Core.Dtos.Infos;
using Solution.Core.Models.Infos;
using OSharp.Utility.Data;
using System.Collections.Generic;
namespace Solution.Core.Contracts
{
    /// <summary>
    /// 业务契约——系统信息模块
    /// </summary>
    public interface IInfosContract : IScopeDependency
    {
        #region 字典信息业务
        /// <summary>
        /// 获取 字典信息查询数据集
        /// </summary>
        IQueryable<SysDicInfo> SysDicInfo { get; }


        /// <summary>
        /// 添加字典信息
        /// </summary>
        /// <param name="dic">要添加的字典信息</param>
        /// <returns>业务操作结果</returns>
        bool AddDic(SysDicInfo dic);
        /// <summary>
        /// 修改字典信息
        /// </summary>
        /// <param name="new_dic"></param>
        /// <returns></returns>
        bool EditDic(SysDicInfo new_dic);
        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeletDic(string id);
        /// <summary>
        /// 获取字典带分页列表
        /// </summary>
        /// <param name="typeid"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        object GetDicList(string typeid, int page = 1, int rows = 10);
        /// <summary>
        /// 获取字典节点
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        List<SysDicInfo> GetDicNode(string pid);

        /// <summary>
        /// 获取某一条字典信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SysDicInfo GetDic(string id);
        #endregion
    }
}
