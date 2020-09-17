using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Device.Core
{

    /// <summary>
    /// 设备操作连接参数实体基类
    /// </summary>
    public abstract class DeviceConnectParamEntityBase : IDeviceParam
    {
        /// <summary>
        /// 设备连接地址、连接字符串例如"opc.tcp://127.0.0.1:49320/" "COM1" "192.168.1.235?;
        /// </summary>
        public string DeviceUrl { get; set; }

        /// <summary>
        /// 状态码，连接结果
        /// </summary>
        public uint StatusCode { get; set; }


        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        #region IDisposable
        private bool _disposed;

        /// <summary>
        /// 释放对象，用于外部调用
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放当前对象时释放资源
        /// </summary>
        ~DeviceConnectParamEntityBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// 重写以实现释放对象的逻辑
        /// </summary>
        /// <param name="disposing">是否要释放对象</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                Disposing();
            }
            _disposed = true;
        }

        /// <summary>
        /// 重写以实现释放派生类资源的逻辑
        /// </summary>
        protected abstract void Disposing();


        #endregion


        public override string ToString()
        {
            return $"DeviceUrl;{this.DeviceUrl} StatusCode:{this.StatusCode} Message:{this.Message}";

        }
    }
}
