using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Device.Core
{
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
        ~DeviceParamEntityBase()
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
    }
}
