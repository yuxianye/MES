﻿using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class InStorageStatusEnumModel
    {
        public enum InStorageStatus
        {
            /// <summary>
            /// 待组盘
            /// </summary>
            [Description("待组盘")]
            InStorageUnFinishStatus = 1,

            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            InStorageFinishStatus = 2,
        }
    }
}
