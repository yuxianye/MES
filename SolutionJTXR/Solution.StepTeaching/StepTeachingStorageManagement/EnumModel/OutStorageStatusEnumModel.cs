﻿using System.ComponentModel;

namespace Solution.MatWarehouseStorageManagement.Dtos
{
    public static class OutStorageStatusEnumModel
    {
        public enum OutStorageStatus
        {
            /// <summary>
            /// 待组盘
            /// </summary>
            [Description("待组盘")]
            待组盘 = 1,

            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            已完成 = 2,
        }
    }
}
