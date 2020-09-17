using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EntDepartmentInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.EntDepartmentInfo.ViewModel
{
    /// <summary>
    /// 编辑VM
    /// </summary>
    public class EntDepartmentInfoEditViewModel : VmBase//《注意：模块主VM与增加和编辑VM继承的基类不同》
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntDepartmentInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }
        public override void OnParamterChanged(object parameter)
        {
            this.EntDepartmentInfoModel = parameter as EntDepartmentInfoModel;
        }

        #region 企业模型
        private EntDepartmentInfoModel entDepartmentInfoModel = new EntDepartmentInfoModel();
        /// <summary>
        /// 企业模型
        /// </summary>
        public EntDepartmentInfoModel EntDepartmentInfoModel
        {
            get { return entDepartmentInfoModel; }
            set
            {
                Set(ref entDepartmentInfoModel, value);
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
            return EntDepartmentInfoModel == null ? false : EntDepartmentInfoModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EntDepartmentInfo/Update",
                Utility.JsonHelper.ToJson(new List<EntDepartmentInfoModel> { EntDepartmentInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                //Application.Current.Resources["UiMessage"] = result?.Message;
                //LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EntDepartmentInfoModel>(EntDepartmentInfoModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EntDepartmentInfoList = new ObservableCollection<EnterpriseModel>();

                //Application.Current.Resources["UiMessage"] = result?.Message ?? "操作失败，请联系管理员！";
                //LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                UiMessage = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(UiMessage);
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
