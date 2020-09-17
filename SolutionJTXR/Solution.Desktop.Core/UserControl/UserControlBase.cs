using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Solution.Desktop.Core
{
    /// <summary>
    /// 用户控件的基类
    /// </summary>
    public class UserControlBase : UserControl, IDisposable
    {
        public UserControlBase()
        {
#if DEBUG
            //调试时加载资源
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                var uri = new Uri(@"pack://application:,,,/Solution.Desktop.Resource;component/DefaultResources.xaml", UriKind.RelativeOrAbsolute);
                ResourceDictionary res = new ResourceDictionary { Source = uri };
                this.Resources.MergedDictionaries.Add(res);

            }
#endif

        }

        ///// <summary>
        ///// 特殊参数
        ///// </summary>
        //public object Parameter { get; set; }

        ///// <summary>
        ///// 模块模型
        ///// </summary>
        //public MenuModule MenuModule { get; set; }

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
        ~UserControlBase()
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
        protected virtual void Disposing()
        {
            (this.DataContext as VmBase)?.Dispose();
        }

    }
}
