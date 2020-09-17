using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquFactoryInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.EquFactoryInfo.ViewModel
{
    /// <summary>
    /// 编辑
    /// </summary>
    public class EquFactoryEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquFactoryEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }
        public override void OnParamterChanged(object parameter)
        {
            this.EquFactoryInfo = parameter as EquFactoryModel;
        }

        #region 设备厂家模型
        /// <summary>
        /// 设备厂家模型
        /// </summary>
        private EquFactoryModel equFactoryInfo = new EquFactoryModel();
        /// <summary>
        /// 设备厂家模型
        /// </summary>
        public EquFactoryModel EquFactoryInfo
        {
            get { return equFactoryInfo; }
            set { Set(ref equFactoryInfo, value); }
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
            return EquFactoryInfo == null ? false : EquFactoryInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquFactory/Update",
                Utility.JsonHelper.ToJson(new List<EquFactoryModel> { EquFactoryInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EquFactoryModel>(EquFactoryInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
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
