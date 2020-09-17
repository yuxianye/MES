using System;

namespace Solution.Desktop.Core
{
    /// <summary>
    /// ViewModelBase，VM基类
    /// </summary>
    public class VmBase : GalaSoft.MvvmLight.ViewModelBase, IDisposable
    {
        public object parameter;
        /// <summary>
        /// 特殊参数
        /// </summary>
        public object Parameter
        {
            get { return parameter; }
            set
            {
                parameter = value;
                OnParamterChanged(value);
            }
        }

        #region 当前区域
        private System.Globalization.CultureInfo currentCulture = System.Globalization.CultureInfo.CurrentCulture;

        /// <summary>
        /// 当前区域
        /// </summary>
        public System.Globalization.CultureInfo CurrentCulture
        {
            get { return currentCulture; }
            set { Set(ref currentCulture, value); }
        }
        #endregion

        #region 界面信息
        private string uiMessage;

        /// <summary>
        /// 界面信息
        /// </summary>
        public string UiMessage
        {
            get { return uiMessage; }
            set { Set(ref uiMessage, value); }
        }
        #endregion

        /// <summary>
        /// 菜单模块信息
        /// </summary>
        public MenuModule MenuModule { get; set; }

        /// <summary>
        /// 参数变更事件函数
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void OnParamterChanged(object parameter)
        {

        }

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
        ~VmBase()
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

        }

    }
}
