using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Agv.ControlService.Models
{

    /// <summary>
    /// 数据类型，与KepServer一致
    /// </summary>
    public enum DataType : int
    {
        //Empty = TypeCode.Empty,

        //Object = TypeCode.Object,

        //DBNull = TypeCode.DBNull,

        /// <summary>
        ///  布尔型：true 或 false 的二进制值
        /// </summary>
        [Description("布尔型：true 或 false 的二进制值")]
        Boolean = TypeCode.Boolean,

        /// <summary>
        ///  字符：有符号的 8 位整数数据
        /// </summary>
        [Description("字符：有符号的 8 位整数数据")]
        Char = TypeCode.Char,

        SByte = TypeCode.SByte,

        /// <summary>
        ///  字节：无符号的 8 位整数数据
        /// </summary>
        [Description("字节：无符号的 8 位整数数据")]
        Byte = TypeCode.Byte,

        /// <summary>
        /// 短整型：有符号的 16 位整数数据
        /// </summary>
        [Description("短整型：有符号的 16 位整数数据")]
        Int16 = TypeCode.Int16,

        /// <summary>
        /// 字：无符号的 16 位整数数据
        /// </summary>
        [Description("字：无符号的 16 位整数数据")]
        UInt16 = TypeCode.UInt16,

        /// <summary>
        /// 长整型:有符号的 32 位整数数据
        /// </summary>
        [Description("长整型:有符号的 32 位整数数据")]
        Int32 = TypeCode.Int32,

        /// <summary>
        /// 双字型：无符号的 32 位整数数据
        /// </summary>
        [Description("双字型：无符号的 32 位整数数据")]
        UInt32 = TypeCode.UInt32,

        /// <summary>
        /// 64位有符号整型
        /// </summary>
        [Description("双长整型：有符号的 64 位整数数据")]
        Int64 = TypeCode.Int64,

        /// <summary>
        /// 四字型：无符号的 64 位整数数据
        /// </summary>
        [Description("四字型：无符号的 64 位整数数据")]
        UInt64 = TypeCode.UInt64,

        /// <summary>
        /// 浮点型：32 位实数值 IEEE-754 标准定义
        /// </summary>
        [Description("浮点型：32 位实数值 IEEE-754 标准定义")]
        Single = TypeCode.Single,

        /// <summary>
        /// 16位有符号整型
        /// </summary>
        [Description("双精度：64 位实数值 IEEE-754 标准定义")]
        Double = TypeCode.Double,

        Decimal = TypeCode.Decimal,

        DateTime = TypeCode.DateTime,

        /// <summary>
        /// 字符串：空终止 Unicode 字符串
        /// </summary>
        [Description("字符串：空终止 Unicode 字符串")]
        String = TypeCode.String,

        ///// <summary>
        ///// BCD：两个字节封装的 BCD 值的范围是 0-9999
        ///// </summary>
        //[Description("BCD：两个字节封装的 BCD 值的范围是 0-9999")]
        //BCD = 19,

        ///// <summary>
        ///// LBCD：压缩为四个字节的 BCD 值的范围是 0-99999999
        ///// </summary>
        //[Description("LBCD：压缩为四个字节的 BCD 值的范围是 0-99999999")]
        //LBCD = 12,
    }
}
