using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.OAuthClientSecretManager.Model
{
    /// <summary>
    /// 客户端密钥信息模型
    /// </summary>
    public class OAuthClientSecretModel : ModelBase
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

        #region 密钥值

        private string _value;

        public string Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }
        #endregion

        #region 密钥类型

        private string _type;

        public string Type
        {
            get { return _type; }
            set { Set(ref _type, value); }
        }
        #endregion

        #region 描述

        private string _remark;

        public string Remark
        {
            get { return _remark; }
            set { Set(ref _remark, value); }
        }
        #endregion

        #region 是否锁定

        private string _isLocked;

        public string IsLocked
        {
            get { return _isLocked; }
            set { Set(ref _isLocked, value); }
        }
        #endregion

        #region 开始时间

        private DateTime? _beginTime;

        public DateTime? BeginTime
        {
            get { return _beginTime; }
            set { Set(ref _beginTime, value); }
        }
        #endregion

        #region 结束时间

        private DateTime? _endTime;

        public DateTime? EndTime
        {
            get { return _endTime; }
            set { Set(ref _endTime, value); }
        }
        #endregion

        #region 客户端ID

        private int _client_Id;

        public int Client_Id
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

        protected override void Disposing()
        {
            Value = null;
            this.Type = null;
            Remark = null;
            IsLocked = null;
            BeginTime = null;
            EndTime = null;
            Client_Name = null;
        }

    }

}
