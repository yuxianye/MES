using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using System;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.ViewModel
{
    /// <summary>
    /// 主窗体控制器（主窗体）
    /// </summary>
    public class MainWindowViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainWindowViewModel()
        {
            initCommand();
        }

        /// <summary>
        /// 初始化主页面使用的Command
        /// </summary>
        private void initCommand()
        {
            ModifyPasswordCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteModifyPasswordCommand);
            LogoutCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteLogoutCommand);
            CloseCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCloseCommand);
            SettingCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSettingCommand);
        }

        #region 修改密码、注销，关闭、系统设置 命令

        public ICommand ModifyPasswordCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }

        private void OnExecuteModifyPasswordCommand()
        {
            try
            {
                ViewInfo viewInfo = new ViewInfo("修改密码", ViewType.Popup, "Solution.Desktop.View", "Solution.Desktop.View.ModifyPasswordView", "Solution.Desktop.ViewModel", "Solution.Desktop.ViewModel.ModifyPasswordViewModel", "pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png");
                Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "打开设置页面失败！请与管理员联系！" + ex.Message;
                Utility.LogHelper.Error("打开设置页面失败！请与管理员联系！", ex);
            }
        }

        private async void OnExecuteLogoutCommand()
        {
            try
            {
                var window = (MetroWindow)Application.Current.MainWindow;
                var dialogResult = await window.ShowMessageAsync("注销用户"
                    , "确定要注销用户么？" + System.Environment.NewLine + "确认注销点击【是】，继续使用点击【否】。"
                    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
                if (dialogResult == MessageDialogResult.Affirmative)
                {
                    Application.Current.Shutdown(1);
                }
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "注销用户失败！请与管理员联系！" + ex.Message;
                Utility.LogHelper.Error("注销用户失败！请与管理员联系！", ex);
            }
        }

        private async void OnExecuteCloseCommand()
        {
            try
            {

                var window = (MetroWindow)Application.Current.MainWindow;
                var dialogResult = await window.ShowMessageAsync("关闭系统"
                    , "确定要关闭么？" + System.Environment.NewLine + "确认关闭点击【是】，继续使用点击【否】。"
                    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
                if (dialogResult == MessageDialogResult.Affirmative)
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "关闭系统失败！请与管理员联系！" + ex.Message;
                Utility.LogHelper.Error("关闭系统失败！请与管理员联系！", ex);
            }
        }

        private void OnExecuteSettingCommand()
        {
            try
            {
                ViewInfo viewInfo = new ViewInfo("系统设置", ViewType.Popup, "Solution.Desktop.View", "Solution.Desktop.View.SettingsView", "Solution.Desktop.ViewModel", "Solution.Desktop.ViewModel.SettingsViewModel", "pack://application:,,,/Solution.Desktop.Resource;component/Images/Settings2_32x32.png");
                Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
            }
            catch (Exception ex)
            {
                Application.Current.Resources["UiMessage"] = "打开设置页面失败！请与管理员联系！" + ex.Message;
                Utility.LogHelper.Error("打开设置页面失败！请与管理员联系！", ex);
            }
        }

        #endregion

        public override void Cleanup()
        {
            base.Cleanup();
        }

        protected override void Disposing()
        {
            base.Disposing();
        }
    }
}
