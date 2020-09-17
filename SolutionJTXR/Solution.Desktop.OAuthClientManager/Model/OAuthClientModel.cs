using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.OAuthClientManager.Model
{
    /// <summary>
    /// 客户端信息模型
    /// </summary>
    public class OAuthClientModel : ModelBase
    {
        #region Id
        private int id;

        /// <summary>
        /// Id
        /// </summary>

        public int Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 应用名称

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
        #endregion

        #region 客户端类型

        private OAuthClientType _oAuthClientType;

        public OAuthClientType OAuthClientType
        {
            get { return _oAuthClientType; }
            set { Set(ref _oAuthClientType, value); }
        }
        #endregion

        #region 客户端编号

        private string _clientId;

        public string ClientId
        {
            get { return _clientId; }
            set { Set(ref _clientId, value); }
        }
        #endregion

        #region 应用地址

        private string _url;

        public string Url
        {
            get { return _url; }
            set { Set(ref _url, value); }
        }
        #endregion

        #region 应用Logo地址

        private string _logoUrl;

        public string LogoUrl
        {
            get { return _logoUrl; }
            set { Set(ref _logoUrl, value); }
        }
        #endregion

        #region 重定向地址

        private string _redirectUrl;

        public string RedirectUrl
        {
            get { return _redirectUrl; }
            set { Set(ref _redirectUrl, value); }
        }
        #endregion

        #region 需要授权

        private bool _requireConsent;

        public bool RequireConsent
        {
            get { return _requireConsent; }
            set { Set(ref _requireConsent, value); }
        }
        #endregion

        #region 允许记住授权

        private bool _allowRememberConsent;

        public bool AllowRememberConsent
        {
            get { return _allowRememberConsent; }
            set { Set(ref _allowRememberConsent, value); }
        }
        #endregion

        #region 只允许ClientCrdentials

        private bool _allowClientCredentialsOnly;

        public bool AllowClientCredentialsOnly
        {
            get { return _allowClientCredentialsOnly; }
            set { Set(ref _allowClientCredentialsOnly, value); }
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

        #region 信息创建时间

        private DateTime _createdTime;

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { Set(ref _createdTime, value); }
        }
        #endregion

        protected override void Disposing()
        {
            Name = null;
            ClientId = null;
            Url = null;
            LogoUrl = null;
            RedirectUrl = null;
        }

    }

}
