using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.EnterpriseInfo.ViewModel
{
    /// <summary>
    /// 编辑VM
    /// </summary>
    public class EnterpriseInfoEditViewModel : VmBase//《注意：模块主VM与增加和编辑VM继承的基类不同》
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EnterpriseInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }
        public override void OnParamterChanged(object parameter)
        {
            this.EnterpriseModel = parameter as EnterpriseModel;
        }

        #region 企业模型
        private EnterpriseModel enterpriseModel = new EnterpriseModel();
        /// <summary>
        /// 企业模型
        /// </summary>
        public EnterpriseModel EnterpriseModel
        {
            get { return enterpriseModel; }
            set
            {
                Set(ref enterpriseModel, value);
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
            return EnterpriseModel == null ? false : EnterpriseModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Update",
                Utility.JsonHelper.ToJson(new List<EnterpriseModel> { EnterpriseModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<EnterpriseModel>(EnterpriseModel, MessengerToken.DataChanged);
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
