﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Utility
{
    /// <summary>
    /// 用于实现<see cref="IDisposable"/>接口，表示当前类型是可释放的
    /// </summary>
    public abstract class Disposable : IDisposable
    {
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
        ~Disposable()
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
    }
}
