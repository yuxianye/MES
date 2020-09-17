using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Solution.Desktop.Core
{

    /// <summary>
    /// 模型的基类，所有模型都需要继承自此基类，通知接口除外。通知接口的模型继承 GalaSoft.MvvmLight.ObservableObject
    /// </summary>
    public abstract class ModelBase : GalaSoft.MvvmLight.ObservableObject, IDataErrorInfo, IDisposable, ICloneable
    {
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
        ~ModelBase()
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

        #region IDataErrorInfo
        public string this[string columnName]
        {
            get
            {
                var vc = new ValidationContext(this, null, null);
                vc.MemberName = columnName;
                var res = new List<ValidationResult>();
                var result = Validator.TryValidateProperty(this.GetType().GetProperty(columnName).GetValue(this, null), vc, res);
                if (res.Count > 0)
                {
                    foreach (var vr in res)
                    {

                        if (dicError.ContainsKey(columnName))
                        {
                            dicError[columnName] = (vr == ValidationResult.Success) ? false : true;
                        }
                        else
                        {
                            dicError.Add(columnName, (vr == ValidationResult.Success) ? false : true);
                        }
                    }
                    return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                }
                if (dicError.ContainsKey(columnName))
                {
                    dicError[columnName] = false;
                }
                else
                {
                    dicError.Add(columnName, false);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// 错误字典
        /// </summary>
        private Dictionary<string, bool> dicError = new Dictionary<string, bool>();

        /// <summary>
        /// 是否通过验证，false验证失败 true验证通过
        /// </summary>
        public bool IsValidated
        {
            get
            {
                foreach (var v in dicError)
                {
                    //有错误那么验证失败，返回false
                    if (v.Value)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #endregion

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
