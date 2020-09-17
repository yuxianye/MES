using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.CommOpcUaServer.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.CommOpcUaServer.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class CommOpcUaServerAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommOpcUaServerAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }

        #region OpcUa通讯服务器模型
        /// <summary>
        /// OpcUa通讯服务器模型
        /// </summary>
        private CommOpcUaServerModel commOpcUaServerModel = new CommOpcUaServerModel();
        /// <summary>
        /// OpcUa通讯服务器模型
        /// </summary>
        public CommOpcUaServerModel CommOpcUaServerModel
        {
            get { return commOpcUaServerModel; }
            set
            {
                Set(ref commOpcUaServerModel, value);
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
            return CommOpcUaServerModel == null ? false : CommOpcUaServerModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "CommOpcUaServer/Add",
                Utility.JsonHelper.ToJson(new List<CommOpcUaServerModel> { CommOpcUaServerModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<CommOpcUaServerModel>(CommOpcUaServerModel, MessengerToken.DataChanged);
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
