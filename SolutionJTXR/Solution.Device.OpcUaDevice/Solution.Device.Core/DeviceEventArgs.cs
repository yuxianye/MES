using System;

namespace Solution.Device.Core
{
    /// <summary>
    /// 设备事件参数
    /// </summary>
    public class DeviceEventArgs<T> : EventArgs where T : IDeviceParam
    {
        /// <summary>
        /// 设备事件参数的构造函数
        /// </summary>
        /// <param name="data">事件的数据</param>
        public DeviceEventArgs(T data)
        {
            this.Data = data;
        }

        /// <summary>
        /// 事件的数据
        /// </summary>
        public T Data { get; set; }

        public string Message { get; set; }

        public void Dispose()
        {
            this.Data = default(T);
            this.Message = null;
        }
    }
}
