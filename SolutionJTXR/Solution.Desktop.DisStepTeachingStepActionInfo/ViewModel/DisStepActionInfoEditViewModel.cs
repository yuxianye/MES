using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.DisStepActionInfo.Model;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Solution.Utility.Windows;

namespace Solution.Desktop.DisStepActionInfo.ViewModel
{
    public class DisStepActionInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DisStepActionInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            // getPageData(1, 200);
        }

        public override void OnParamterChanged(object parameter)
        {
            this.DisStepActionInfoModel = parameter as DisStepActionInfoModel;
        }

        #region 分步操作信息模型

        private DisStepActionInfoModel disStepActionInfoModel = new DisStepActionInfoModel();
        /// <summary>
        /// 分步操作信息模型
        /// </summary>
        public DisStepActionInfoModel DisStepActionInfoModel
        {
            get { return disStepActionInfoModel; }
            set
            {
                Set(ref disStepActionInfoModel, value);
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
            if (Equals(DisStepActionInfoModel.StepActionCode, null) || Equals(DisStepActionInfoModel.StepActionName, null))
            {
                return false;
            }
            else
            {
                return DisStepActionInfoModel == null ? false : DisStepActionInfoModel.IsValidated;
            }
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "DisStepActionInfo/Update",
                Utility.JsonHelper.ToJson(new List<DisStepActionInfoModel> { DisStepActionInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<DisStepActionInfoModel>(DisStepActionInfoModel, MessengerToken.DataChanged);
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
