using System;

namespace Solution.Device.Core
{
    /// <summary>
    /// 设备参数接口
    /// </summary>
    public interface IDeviceParam
    {

    }

    /// <summary>
    /// 设备操作连接参数实体基类
    /// </summary>
    public abstract class DeviceConnectParamEntityBase : IDeviceParam
    {
        /// <summary>
        /// 设备连接地址、连接字符串
        /// </summary>
        public string DeviceUrl { get; set; }
    }

    /// <summary>
    /// 设备参数实体抽闲基类
    /// </summary>
    public abstract class DeviceParamEntityBase : IDeviceParam
    {
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 数值的类型
        /// </summary>
        public Type ValueType { get; set; }
    }

    /// <summary>
    /// 设备操作输入参数实体基类
    /// </summary>
    public abstract class DeviceInputParamEntityBase : DeviceParamEntityBase
    {
        /// <summary>
        /// 节点名称、编号
        /// </summary>
        public string NodeId { get; set; }
    }

    /// <summary>
    /// 设备操作输出(返回)参数实体基类
    /// </summary>
    public abstract class DeviceOutputParamEntityBase : DeviceInputParamEntityBase
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public uint StatusCode { get; set; }

    }
}
