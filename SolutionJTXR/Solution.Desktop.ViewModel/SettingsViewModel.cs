using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Solution.Desktop.ViewModel
{
    public class SettingsViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SettingsViewModel()
        {
            int.TryParse(Utility.ConfigHelper.GetAppSetting("ServerPort"), out serverPort);
            int.TryParse(Utility.ConfigHelper.GetAppSetting("PageSize"), out pageSize);
            functionView = System.IO.File.ReadAllText("MenuFunctionViewInfoMap.json");
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
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

        #region ServerIp
        private string serverIp = Utility.ConfigHelper.GetAppSetting("ServerIp");

        /// <summary>
        /// ServerIp
        /// </summary>
        public string ServerIp
        {
            get { return serverIp; }
            set { Set(ref serverIp, value); }
        }
        #endregion

        #region ServerPort
        private int serverPort = 13805;

        /// <summary>
        /// ServerIp
        /// </summary>
        public int ServerPort
        {
            get { return serverPort; }
            set { Set(ref serverPort, value); }
        }
        #endregion


        #region ServerUri
        private string serverUri = Utility.ConfigHelper.GetAppSetting("ServerUri");

        /// <summary>
        /// ServerUri
        /// </summary>
        public string ServerUri
        {
            get { return serverUri; }
            set { Set(ref serverUri, value); }
        }
        #endregion

        #region PageSize
        private int pageSize = 200;

        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { Set(ref pageSize, value); }
        }
        #endregion

        #region PageSizeList
        private List<int> pageSizeList = new List<int>() { 20, 50, 100, 200, 500, 1000, 5000, 10000 };

        /// <summary>
        /// PageSize
        /// </summary>
        public List<int> PageSizeList
        {
            get { return pageSizeList; }
            set { Set(ref pageSizeList, value); }
        }
        #endregion



        #region FunctionView
        private string functionView;

        /// <summary>
        /// FunctionView
        /// </summary>
        public string FunctionView
        {
            get { return functionView; }
            set { Set(ref functionView, value); }
        }
        #endregion

        #region 确定命令相关
        /// <summary>
        /// 确定命令
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        /// <summary>
        /// 确定命令执行函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            try
            {
                Utility.ConfigHelper.AddAppSetting("ServerIp", ServerIp);
                Utility.ConfigHelper.AddAppSetting("ServerPort", ServerPort.ToString());
                Utility.ConfigHelper.AddAppSetting("ServerUri", ServerUri);
                Utility.ConfigHelper.AddAppSetting("PageSize", PageSize.ToString());
                Messenger.Default.Send<Boolean>(false, MessengerToken.ClosePopup);
            }
            catch (Exception ex)
            {
                Message = "修改设置失败！请与管理员联系！" + System.Environment.NewLine + ex.Message;
                Utility.LogHelper.Error(Message, ex);
            }
        }

        #endregion

        #region 取消命令相关
        /// <summary>
        /// 取消命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 退出命令执行函数
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<Boolean>(false, MessengerToken.ClosePopup);
        }
        #endregion
    }
}
