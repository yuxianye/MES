using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.Agv.Model
{
    public enum TaskType : int
    {
        /// <summary>
        ///  任务类型
        /// </summary>
        /// 
        [Description("订单任务")]
        OrderType = 1,

        [Description("小车任务")]
        AgvType = 2,

    }

    public enum TaskStatus : int
    {
        /// <summary>
        ///  任务状态
        /// </summary>
        /// 
        [Description("准备")]
        Ready = 1,

        [Description("车辆未分配")]
        NoDistribution = 2,

        [Description("车辆已分配")]
        Distribution = 3,

        [Description("执行中")]
        Implement = 4,

        [Description("车辆故障")]
        Breakdown = 5,

        [Description("到达")]
        Arrive = 6,

        [Description("完成")]
        Complete = 7,

        [Description("取消")]
        Cancel = 8,

    }
}
