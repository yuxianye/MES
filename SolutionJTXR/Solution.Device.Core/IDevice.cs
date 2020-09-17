using System;
using System.Threading.Tasks;

namespace Solution.Device.Core
{
    /// <summary>
    /// 用于设备通知的代理
    /// </summary>
    /// <param name="device"></param>
    /// <param name="deviceEventArgs"></param>
    public delegate void DeviceNotificationEventHandler(IDevice device, DeviceEventArgs<IDeviceParam> deviceEventArgs);

    /// <summary>
    /// 设备接口
    /// </summary>
    public interface IDevice : IDisposable
    {
        /// <summary>
        /// 名称，OpcUa/Opc/COM1
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 通知事件,设备状态、数据、异常等都通过此事件对外发布
        /// </summary>
        event DeviceNotificationEventHandler Notification;

        /// <summary>
        /// 连接/打开设备
        /// </summary>
        /// <typeparam name="Tin">连接/打开设备的参数类型</typeparam>
        /// <typeparam name="Tout">连接/打开设备返回的结果类型</typeparam>
        /// <param name="connectParam">连接/打开参数</param>
        /// <returns>连接/打开设备的结果</returns>
        Task<Tout> Connect<Tin, Tout>(Tin connectParam) where Tin : DeviceConnectParamEntityBase where Tout : DeviceConnectParamEntityBase;

        /// <summary>
        /// 断开/关闭设备
        /// </summary>
        /// <typeparam name="Tin">断开/关闭设备的参数类型</typeparam>
        /// <typeparam name="Tout">断开/关闭设备返回的结果类型</typeparam>
        /// <param name="disConnectParam">断开/关闭连接参数</param>
        /// <returns>连接/打开设备的结果</returns>
        Task<Tout> DisConnect<Tin, Tout>(Tin disConnectParam) where Tin : IDeviceParam where Tout : IDeviceParam;

        /// <summary>
        /// 写入
        /// </summary>
        /// <typeparam name="Tin">写入的参数类型</typeparam>
        /// <typeparam name="Tout">返回结果的类型</typeparam>
        /// <param name="writeParam">写入的参数</param>
        /// <returns>写入的结果</returns>
        Task<Tout> Write<Tin, Tout>(Tin writeParam) where Tin : IDeviceParam where Tout : IDeviceParam;

        /// <summary>
        /// 读
        /// </summary>
        /// <typeparam name="Tin">读取参数的类型</typeparam>
        /// <typeparam name="Tout">返回结果的类型</typeparam>
        /// <param name="readParam">要读取的参数</param>
        /// <returns>读取的结果</returns>
        Task<Tout> Read<Tin, Tout>(Tin readParam) where Tin : IDeviceParam where Tout : IDeviceParam;

        /// <summary>
        /// 批量写入
        /// </summary>
        /// <typeparam name="Tin">写入的参数类型</typeparam>
        /// <typeparam name="Tout">返回结果的类型</typeparam>
        /// <param name="writeParam">写入的参数</param>
        /// <returns>写入的结果</returns>
        Task<Tout[]> Writes<Tin, Tout>(Tin[] writeParams) where Tin : IDeviceParam where Tout : IDeviceParam;

        /// <summary>
        /// 批量读
        /// </summary>
        /// <typeparam name="Tin">读取参数的类型</typeparam>
        /// <typeparam name="Tout">返回结果的类型</typeparam>
        /// <param name="readParam">要读取的参数</param>
        /// <returns>读取的结果</returns>
        Task<Tout[]> Reads<Tin, Tout>(Tin[] readParams) where Tin : IDeviceParam where Tout : IDeviceParam;

        /// <summary>
        /// 注册通讯点
        /// </summary>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="nodes"></param>
        /// <returns></returns>
        Task<Tout> RegisterNodes<Tout>(dynamic[] nodes) where Tout : DeviceOutputParamEntityBase;

    }

}
