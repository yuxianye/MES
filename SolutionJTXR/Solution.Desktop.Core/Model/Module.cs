using System.Collections.Generic;

namespace Solution.Desktop.Core
{
    /// <summary>
    /// 模块信息
    /// </summary>
    public class MenuModule
    {
        /// <summary>
        /// 获取或设置 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 模块备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置 节点内排序码
        /// </summary>
        public double OrderCode { get; set; }

        /// <summary>
        /// 树形和tab页图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 获取或设置 父模块Id
        /// </summary>
        public int? Parent_Id;

        /// <summary>
        /// 获取或设置 功能信息集合
        /// </summary>
        public ICollection<MenuFunction> Functions { get; set; }

    }
}
