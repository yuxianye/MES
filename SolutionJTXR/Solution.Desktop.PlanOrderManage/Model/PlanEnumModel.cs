using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.PlanOrderManage.Model
{
    public static class PlanEnumModel
    {
        public enum OrderState
        {
            /// <summary>
            /// 未开始
            /// </summary>
            [Description("未开始")]
            UnStart = 1,
            /// <summary>
            /// 已排产
            /// </summary>
            [Description("已排产")]
            Schueduled = 2,
            /// <summary>
            /// 正在生产
            /// </summary>
            [Description("正在生产")]
            WIP = 3,
            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            Finished = 4
        }

        public enum ProductUnit
        {
            /// <summary>
            /// 个
            /// </summary>
            [Description("个")]
            Number = 1,

            /// <summary>
            /// 箱
            /// </summary>
            [Description("箱")]
            Box = 2,
            /// <summary>
            /// 件
            /// </summary>
            [Description("件")]
            Piece = 3,
            /// <summary>
            /// 把
            /// </summary>
            [Description("把")]
            Handle = 4
        }

        public enum ScheduleStatus
        {
            /// <summary>
            /// 未开始
            /// </summary>
            [Description("未开始")]
            UnStart = 1,
            /// <summary>
            /// 已生成工单
            /// </summary>
            [Description("已生成工单")]
            Generated = 2,
            /// <summary>
            /// 已下达
            /// </summary>
            [Description("已下达")]
            Published = 3,
            /// <summary>
            /// 正在生产
            /// </summary>
            [Description("正在生产")]
            WIP = 4,
            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            Finished = 5
        }

        public enum Priority
        {
            /// <summary>
            /// 第一优先
            /// </summary>
            [Description("第一优先")]
            First = 1,

            /// <summary>
            /// 第二优先
            /// </summary>
            [Description("第二优先")]
            Second = 2,
            /// <summary>
            /// 第三优先
            /// </summary>
            [Description("第三优先")]
            Third = 3,
            /// <summary>
            /// 第四优先
            /// </summary>
            [Description("第四优先")]
            Fourth = 4
        }

        public enum DurationUnit
        {
            /// <summary>
            /// 小时
            /// </summary>
            [Description("小时")]
            Hour = 1,

            /// <summary>
            /// 分钟
            /// </summary>
            [Description("分钟")]
            Minute = 2,
            /// <summary>
            /// 秒
            /// </summary>
            [Description("秒")]
            Second = 3
        }
    }
}
