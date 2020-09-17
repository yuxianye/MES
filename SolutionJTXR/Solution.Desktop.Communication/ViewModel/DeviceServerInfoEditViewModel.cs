using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Communication.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.Communication.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class DeviceServerInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeviceServerInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }
        public override void OnParamterChanged(object parameter)
        {
            this.DeviceServerInfoModel = parameter as DeviceServerInfoModel;
        }

        #region Socket通信模型
        /// <summary>
        /// Socket通信模型
        /// </summary>
        private DeviceServerInfoModel deviceServerInfoModel;// = new EnterpriseModel();
        /// <summary>
        /// Socket通信模型
        /// </summary>
        public DeviceServerInfoModel DeviceServerInfoModel
        {
            get { return deviceServerInfoModel; }
            set
            {
                Set(ref deviceServerInfoModel, value);
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
            return DeviceServerInfoModel == null ? false : DeviceServerInfoModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "DeviceServerInfo/Update",
                Utility.JsonHelper.ToJson(new List<DeviceServerInfoModel> { DeviceServerInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<DeviceServerInfoModel>(DeviceServerInfoModel, MessengerToken.DataChanged);
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
