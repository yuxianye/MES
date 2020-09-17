using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.OAuthClientRefreshTokenManager.Model
{
    /// <summary>
    /// 功能信息模型
    /// </summary>
    public class OAuthClientRefreshTokenModel : ModelBase
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

        #region Token值

        private string _value;

        public string Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }
        #endregion

        #region 保护的Ticket

        private string _protectedTicket;

        public string ProtectedTicket
        {
            get { return _protectedTicket; }
            set { Set(ref _protectedTicket, value); }
        }
        #endregion

        #region 生成时间

        private DateTime _issuedUtc;

        public DateTime IssuedUtc
        {
            get { return _issuedUtc; }
            set { Set(ref _issuedUtc, value); }
        }
        #endregion

        #region 过期时间

        private DateTime? _expiresUtc;

        public DateTime? ExpiresUtc
        {
            get { return _expiresUtc; }
            set { Set(ref _expiresUtc, value); }
        }
        #endregion

        #region 客户端编号

        private int? _client_Id;

        public int? Client_Id
        {
            get { return _client_Id; }
            set { Set(ref _client_Id, value); }
        }
        #endregion

        #region 客户端名称

        private string _client_Name;

        public string Client_Name
        {
            get { return _client_Name; }
            set { Set(ref _client_Name, value); }
        }
        #endregion

        #region 用户编号

        private int? _user_Id;

        public int? User_Id
        {
            get { return _user_Id; }
            set { Set(ref _user_Id, value); }
        }
        #endregion

        #region 用户名称

        private string _user_Name;

        public string User_Name
        {
            get { return _user_Name; }
            set { Set(ref _user_Name, value); }
        }
        #endregion

        protected override void Disposing()
        {
            Value = null;
            ProtectedTicket = null;
            ExpiresUtc = null;
            Client_Id = null;
            Client_Name = null;
            User_Id = null;
            User_Name = null;
        }

    }

}
