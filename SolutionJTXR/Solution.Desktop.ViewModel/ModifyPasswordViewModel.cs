using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.ViewModel
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class ModifyPasswordViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ModifyPasswordViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            this.ModifyPasswordModel = new ModifyPasswordModel()
            {
                UserName = GlobalData.CurrentLoginUser.UserName,
                //PassWord = GlobalData.CurrentLoginUser.PassWord,
            };
            ModifyPasswordModel.PropertyChanged += ModifyPasswordModel_PropertyChanged;
        }

        private void ModifyPasswordModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PassWord")
            {
                if (ModifyPasswordModel.PassWord != GlobalData.CurrentLoginUser.PassWord)
                {
                    Application.Current.Resources["UiMessage"] = "旧密码错误！";
                }
                else
                {
                    Application.Current.Resources["UiMessage"] = "旧密码正确！";
                }
            }
            if (e.PropertyName == "NewPassword")
            {
                if (ModifyPasswordModel.NewPassword == GlobalData.CurrentLoginUser.PassWord)
                {
                    Application.Current.Resources["UiMessage"] = "新密码不能与旧密码相同，请使用其他密码！";
                }
            }
        }

        #region 修改密码模型
        /// <summary>
        /// 修改密码模型
        /// </summary>
        private ModifyPasswordModel modifyPasswordModel;
        /// <summary>
        /// 修改密码模型
        /// </summary>
        public ModifyPasswordModel ModifyPasswordModel
        {
            get { return modifyPasswordModel; }
            set
            {
                Set(ref modifyPasswordModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return ModifyPasswordModel == null ? false : (ModifyPasswordModel.IsValidated && ModifyPasswordModel.NewPassword != GlobalData.CurrentLoginUser.PassWord);
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {

            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "Identity/ModifyPassword",
                Utility.JsonHelper.ToJson(new List<ModifyPasswordModel> { ModifyPasswordModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                GlobalData.CurrentLoginUser.PassWord = ModifyPasswordModel.NewPassword;
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            }
        }

        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }
    }
}
