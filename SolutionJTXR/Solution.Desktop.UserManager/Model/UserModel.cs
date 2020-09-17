using Solution.Desktop.Core;
using Solution.Desktop.RoleManager.Model;
using Solution.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Solution.Desktop.CommSocketServer.Model
{
    /// <summary>
    /// 用户信息模型
    /// </summary>
    public class UserModel : ModelBase, ILockable
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

        #region 用户名
        private string _userName;

        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string UserName
        {
            get { return _userName; }
            set { Set(ref _userName, value); }
        }
        #endregion

        #region 用户昵称
        private string _nickName;

        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string NickName
        {
            get { return _nickName; }
            set { Set(ref _nickName, value); }
        }
        #endregion

        #region 密码
        private string _password;
        [Required(ErrorMessage = "必填项，大于6个字符,必须包含小写字母")]
        [RegularExpression(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$", ErrorMessage = "密码长度必须大于6个字符，必须包含小写字母")]
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        #endregion

        #region 电子邮箱

        private string _email;

        [Required(ErrorMessage = "电子邮箱必填"), MaxLength(50, ErrorMessage = "50")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "无效的电子邮箱")]

        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }
        #endregion

        #region 电子邮箱验证

        private bool _emailConfirmed;

        public bool EmailConfirmed
        {
            get { return _emailConfirmed; }
            set { Set(ref _emailConfirmed, value); }
        }
        #endregion

        #region 电话号码

        private string _phoneNumber;
        [Required(ErrorMessage = "电话号码必填"), MaxLength(11, ErrorMessage = "")]
        [RegularExpression(@"^[1][3,4,5,6,7,8,9][0-9]{9}$", ErrorMessage = "无效的电话号码")]
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { Set(ref _phoneNumber, value); }
        }
        #endregion

        #region 电话号码验证

        private bool _phoneNumberConfirmed;

        //[Required(ErrorMessage = "电话号码必填")]
        public bool PhoneNumberConfirmed
        {
            get { return _phoneNumberConfirmed; }
            set { Set(ref _phoneNumberConfirmed, value); }
        }
        #endregion

        #region 登录锁定UTC时间

        private DateTime? _lockoutEndDateUtc;

        public DateTime? LockoutEndDateUtc
        {
            get { return _lockoutEndDateUtc; }
            set { Set(ref _lockoutEndDateUtc, value); }
        }
        #endregion

        #region 当前登录失败次数

        private int _accessFailedCount;

        public int AccessFailedCount
        {
            get { return _accessFailedCount; }
            set { Set(ref _accessFailedCount, value); }
        }
        #endregion

        #region 角色列表
        private ObservableCollection<RoleModel> _roleInfoList = new ObservableCollection<RoleModel>();

        /// <summary>
        /// 角色信息信息数据
        /// </summary>
        public ObservableCollection<RoleModel> Roles
        {
            get { return _roleInfoList; }
            set { Set(ref _roleInfoList, value); }
        }
        #endregion

        #region 是否锁定
        private bool isLocked;

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked
        {
            get { return isLocked; }
            set { Set(ref isLocked, value); }
        }
        #endregion

        #region 记录创建时间
        private DateTime createdTime;

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { Set(ref createdTime, value); }
        }
        #endregion

        protected override void Disposing()
        {
            UserName = null;
            NickName = null;
            Password = null;
            Email = null;
            PhoneNumber = null;
            LockoutEndDateUtc = null;
            Roles = null;
        }

    }

}
