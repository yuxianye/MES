using System;

namespace Solution.Device.Core
{
    /// <summary>
    /// 设备参数接口
    /// </summary>
    public interface IDeviceParam : IDisposable
    {
        string Message { get; set; }
    }

}
