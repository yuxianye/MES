using Solution.Desktop.Core;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Model
{

    /// <summary>
    /// 登陆用户模型
    /// 普通类继承自 <see cref="Core.ModuleBase"/>,不用加Model后缀。带通知接口的类继承自<see cref="GalaSoft.MvvmLight.ObservableObject"/>加Model后缀
    /// </summary>
    public class LoginUser : ModelBase
    {
        private string userName
#if DEBUG
            = "admin";
#else
            ;
#endif
        public string UserName
        {
            get { return userName; }
            set { Set(ref userName, value); }
        }

        private string passWord
#if DEBUG
            = "yuxianye1";
#else
            ;
#endif
        [Required(ErrorMessage = "密码必填")]
        [RegularExpression(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$", ErrorMessage = "密码长度必须大于6个字符，必须包含小写字母")]
        public string PassWord
        {
            get { return passWord; }
            set { Set(ref passWord, value); }
        }



        protected override void Disposing()
        {
            UserName = null;
            PassWord = null;
        }
    }

    /// <summary>
    /// 修改密码模型
    /// </summary>
    public class ModifyPasswordModel : ModelBase
    {

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { Set(ref userName, value); }
        }

        public string passWord;

        [Required(ErrorMessage = "旧密码必填")]
        [RegularExpression(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$", ErrorMessage = "密码长度必须大于6个字符，必须包含字符和数字")]
        public string PassWord
        {
            get { return passWord; }
            set { Set(ref passWord, value); }
        }

        private string newPassword;
        /// <summary>
        /// 用户新密码
        /// </summary>
        [Required(ErrorMessage = "新密码必填")]
        [RegularExpression(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$", ErrorMessage = "密码长度必须大于6个字符，必须包含字符和数字")]
        public string NewPassword
        {
            get { return newPassword; }
            set { Set(ref newPassword, value); }
        }

        protected override void Disposing()
        {
            UserName = null;
            PassWord = null;
            NewPassword = null;
        }
    }


}