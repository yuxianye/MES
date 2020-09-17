using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Model;
using Solution.Utility.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solution.Desktop.ViewModel
{
    public class LoginViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginViewModel()
        {
            initConfigData();
            LoginCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteLoginCommand);
            ExitCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteExitCommand);
            SettingCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSettingCommand);
        }
        private void initConfigData()
        {
            bool.TryParse(Utility.ConfigHelper.GetAppSetting("IsRemberUserName"), out bool _isRemberUserName);
            IsRemberUserName = _isRemberUserName;

            bool.TryParse(Utility.ConfigHelper.GetAppSetting("IsRemberUserPassword"), out bool _isRemberUserPassword);
            IsRemberUserPassword = _isRemberUserPassword;

            LoginUser.UserName = Utility.ConfigHelper.GetAppSetting("RemberUserName");

            if (string.IsNullOrWhiteSpace(Utility.ConfigHelper.GetAppSetting("RemberUserPassword")))
            {
                LoginUser.PassWord = null;
            }
            else
            {
                LoginUser.PassWord = Utility.Secutiry.RsaHelper.Decrypt(Utility.ConfigHelper.GetAppSetting("RemberUserPassword"));
            }
        }

        #region 消息
        private string message;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { Set(ref message, value); }
        }
        #endregion

        #region 用户实体
        private LoginUser loginUser = new LoginUser();

        /// <summary>
        /// 用户实体
        /// </summary>
        public LoginUser LoginUser
        {
            get { return loginUser; }
            set { Set(ref loginUser, value); }
        }
        #endregion

        #region 是否记住用户名
        private bool isRemberUserName = true;

        /// <summary>
        /// 是否记住用户名
        /// </summary>
        public bool IsRemberUserName
        {
            get { return isRemberUserName; }
            set
            {
                Set(ref isRemberUserName, value);
                if (!value) IsRemberUserPassword = false;
            }
        }
        #endregion

        #region 是否记住用户密码
        private bool isRemberUserPassword = true;

        /// <summary>
        /// 是否记住用户密码
        /// </summary>
        public bool IsRemberUserPassword
        {
            get { return isRemberUserPassword; }
            set
            {
                Set(ref isRemberUserPassword, value);
                if (value) IsRemberUserName = true;
            }
        }
        #endregion

        #region 登陆命令相关
        /// <summary>
        /// 登陆命令
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// 登陆命令执行函数
        /// </summary>
        private void OnExecuteLoginCommand()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LoginUser.UserName))
                {
                    Message = "请输入用户名";
                    return;
                }
                if (string.IsNullOrWhiteSpace(LoginUser.PassWord))
                {
                    Message = "请输入密码";
                    return;
                }
                Message = "正在核对用户名和密码...";
#if DEBUG
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
#endif
                var result = Task.Factory.StartNew<OperationResult<PageResult<MenuModule>>>
                    (
                    () => (Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MenuModule>>>(GlobalData.ServerRootUri + "Identity/Login", Utility.JsonHelper.ToJson(LoginUser)))
                   ).Result;
#if DEBUG
                stopwatch.Stop();
                Utility.LogHelper.Info("登陆用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
                Utility.LogHelper.Info("登陆内容：" + Utility.JsonHelper.ToJson(result));
#endif

                //Message = result?.Message;
                if (!Equals(result, null) && result.Successed)
                {

                    Message = "登陆成功，正在加载...";
                    GlobalData.CurrentLoginUser = this.LoginUser;
                    GlobalData.CurrentUserModule = result.Data.Data;
                    if (result.Data.Data.Any())
                    {
                        Utility.ConfigHelper.AddAppSetting("IsRemberUserName", IsRemberUserName.ToString());
                        Utility.ConfigHelper.AddAppSetting("IsRemberUserPassword", IsRemberUserPassword.ToString());

                        if (IsRemberUserName)
                        {
                            Utility.ConfigHelper.AddAppSetting("RemberUserName", LoginUser.UserName);
                        }
                        else
                        {
                            Utility.ConfigHelper.AddAppSetting("RemberUserName", "");
                        }
                        if (IsRemberUserPassword)
                        {
                            Utility.ConfigHelper.AddAppSetting("RemberUserPassword", Utility.Secutiry.RsaHelper.Encryption(LoginUser.PassWord));
                        }
                        else
                        {
                            Utility.ConfigHelper.AddAppSetting("RemberUserPassword", "");
                        }


                        Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    }
                    else
                    {
                        Message = "未找到该用户可用的功能权限！请与管理员联系！";
                    }
                    GetToken(result.Message);
                }
                else
                {
                    //登陆失败，显示错误信息
                    Message = result == null ? "请确认与服务器连接是否正常！" : result.Message;
                }

            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Utility.LogHelper.Error("登陆异常！请与管理员联系！", ex);
            }
        }

        public void GetToken(string Message)
        {
            ClientAndSecretData clientAndSecretData = Utility.JsonHelper.FromJson<ClientAndSecretData>(Message);
            try
            {
                var accessToken = HttpClientHelper.GetAccessToken(GlobalData.TokenUri, clientAndSecretData.ClientId, clientAndSecretData.ClientSecret, loginUser.UserName, loginUser.PassWord).Result;
                if (String.IsNullOrEmpty(accessToken))
                {
                    Message = "未能获取到Token！请与管理员联系！";
                }
                else
                {
                    GlobalData.AccessTocken = accessToken;
                }

            }
            catch (Exception ex)
            {
                Utility.LogHelper.Error("获取Token信息异常！", ex);
            }
        }

        #endregion

        #region 退出命令相关
        /// <summary>
        /// 退出命令
        /// </summary>
        public ICommand ExitCommand { get; set; }

        /// <summary>
        /// 退出命令执行函数
        /// </summary>
        private void OnExecuteExitCommand()
        {
            Messenger.Default.Send<Boolean>(false, MessengerToken.LoginExit);
        }
        #endregion

        #region 设置命令相关
        /// <summary>
        /// 登陆命令
        /// </summary>
        public ICommand SettingCommand { get; set; }

        /// <summary>
        /// 登陆命令执行函数
        /// </summary>
        private void OnExecuteSettingCommand()
        {
            try
            {
                ViewInfo viewInfo = new ViewInfo("系统设置", ViewType.Popup, "Solution.Desktop.View", "Solution.Desktop.View.SettingsView", "Solution.Desktop.ViewModel", "Solution.Desktop.ViewModel.SettingsViewModel", "pack://application:,,,/Solution.Desktop.Resource;component/Images/Settings2_32x32.png");
                Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Utility.LogHelper.Error("打开设置页面失败！请与管理员联系！", ex);

            }
        }
        #endregion
    }
}
