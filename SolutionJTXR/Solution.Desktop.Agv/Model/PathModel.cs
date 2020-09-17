using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Solution.Desktop.Agv.Model
{
    /// <summary>
    /// PathModel模型
    /// </summary>

    /// <summary>
    /// 路径模型，路线名称，起始点。
    /// </summary>
    public class PathModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 路径名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 开始点
        /// </summary>
        public Point StartPoint { get; set; }

        /// <summary>
        /// 结束点
        /// </summary>
        public Point EndPoint { get; set; }

        /// <summary>
        /// 路径状态
        /// 0未知，1正常，2异常，3维护
        /// </summary>
        public int PathStatus { get; set; }

    }

}
