using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.FunctionManager.Model
{
    /// <summary>
    /// 功能信息模型
    /// </summary>
    public class FunctionModel : ModelBase
    {
        #region Id
        private Guid id = CombHelper.NewComb();

        /// <summary>
        /// Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 名称

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
        #endregion

        #region 区域

        private string _area;

        public string Area
        {
            get { return _area; }
            set { Set(ref _area, value); }
        }
        #endregion

        #region 控制器

        private string _controler;

        public string Controller
        {
            get { return _controler; }
            set { Set(ref _controler, value); }
        }
        #endregion

        #region 功能

        private string _action;

        public string Action
        {
            get { return _action; }
            set { Set(ref _action, value); }
        }
        #endregion

        #region 功能地址

        private string _url;

        public string Url
        {
            get { return _url; }
            set { Set(ref _url, value); }
        }
        #endregion

        #region 功能类型

        private int _functionType;

        public int FunctionType
        {
            get { return _functionType; }
            set { Set(ref _functionType, value); }
        }
        #endregion

        #region 是否启用操作日志

        private bool _operateLogEnabled;

        public bool OperateLogEnabled
        {
            get { return _operateLogEnabled; }
            set { Set(ref _operateLogEnabled, value); }
        }
        #endregion

        #region 是否启用数据日志

        private bool _dataLogEnabled;

        public bool DataLogEnabled
        {
            get { return _dataLogEnabled; }
            set { Set(ref _dataLogEnabled, value); }
        }
        #endregion

        #region 获取或设置数据缓存时间（秒）

        private int _cacheExpirationSeconds;

        public int CacheExpirationSeconds
        {
            get { return _cacheExpirationSeconds; }
            set { Set(ref _cacheExpirationSeconds, value); }
        }
        #endregion

        #region 获取或设置 是否相对过期时间，否则为绝对过期

        private bool _isCacheSliding;

        public bool IsCacheSliding
        {
            get { return _isCacheSliding; }
            set { Set(ref _isCacheSliding, value); }
        }
        #endregion

        #region 获取或设置 是否控制器，如果为false，则此记录为action的记录

        private bool _isController;

        public bool IsController
        {
            get { return _isController; }
            set { Set(ref _isController, value); }
        }
        #endregion

        #region 是否Ajax记录

        private bool _isAjax;

        public bool IsAjax
        {
            get { return _isAjax; }
            set { Set(ref _isAjax, value); }
        }
        #endregion

        #region 是否子功能

        private bool _isChild;

        public bool IsChild
        {
            get { return _isChild; }
            set { Set(ref _isChild, value); }
        }
        #endregion

        #region 是否锁定

        private bool _isLocked;

        public bool IsLocked
        {
            get { return _isLocked; }
            set { Set(ref _isLocked, value); }
        }
        #endregion

        #region 功能类型是否更改过，如为true，刷新功能时将忽略功能类型

        private bool _isTypeChanged;

        public bool IsTypeChanged
        {
            get { return _isTypeChanged; }
            set { Set(ref _isTypeChanged, value); }
        }
        #endregion

        #region 是否自定义功能

        private bool _isCustom;

        public bool IsCustom
        {
            get { return _isCustom; }
            set { Set(ref _isCustom, value); }
        }
        #endregion

        #region 是否被选中
        private bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                //Set(ref _isChecked, value);
                _isChecked = value;
                if (PropertyChangedHandler != null)
                    PropertyChangedHandler(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }
        #endregion

        protected override void Disposing()
        {
            Name = null;
            Area = null;
            Controller = null;
            Action = null;
            Url = null;

        }

    }

}
