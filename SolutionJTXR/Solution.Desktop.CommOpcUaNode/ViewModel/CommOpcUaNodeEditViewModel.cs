using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.CommOpcUaNode.Model;
using Solution.Desktop.CommOpcUaServer.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.CommOpcUaNode.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class CommOpcUaNodeEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommOpcUaNodeEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getServerPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }
        public override void OnParamterChanged(object parameter)
        {
            this.CommOpcUaNodeModel = parameter as CommOpcUaNodeModel;
            this.CommOpcUaServerInfo = CommOpcUaServerInfoList.Where(x => x.ServerName.Equals(CommOpcUaNodeModel.ServerName)).FirstOrDefault();
        }

        #region Opc Ua数据点数据模型
        /// <summary>
        /// Opc Ua数据点数据模型
        /// </summary>
        private CommOpcUaNodeModel commOpcUaNodeModel;// = new EnterpriseModel();
        /// <summary>
        /// Opc Ua数据点数据模型
        /// </summary>
        public CommOpcUaNodeModel CommOpcUaNodeModel
        {
            get { return commOpcUaNodeModel; }
            set
            {
                Set(ref commOpcUaNodeModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region Opc Ua服务器数据模型
        private CommOpcUaServerModel commOpcUaServerInfo;

        public CommOpcUaServerModel CommOpcUaServerInfo
        {
            get { return commOpcUaServerInfo; }
            set { Set(ref commOpcUaServerInfo, value); }
        }
        #endregion


        #region Opc Ua服务器数据模型,用于列表数据显示
        private ObservableCollection<CommOpcUaServerModel> commOpcUaServerInfoList = new ObservableCollection<CommOpcUaServerModel>();

        /// <summary>
        /// Opc Ua数据点数据
        /// </summary>
        public ObservableCollection<CommOpcUaServerModel> CommOpcUaServerInfoList
        {
            get { return commOpcUaServerInfoList; }
            set { Set(ref commOpcUaServerInfoList, value); }
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
            return CommOpcUaNodeModel == null ? false : CommOpcUaNodeModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            if (CommOpcUaServerInfo != null)
            {
                CommOpcUaNodeModel.ServerId = CommOpcUaServerInfo.Id;
                CommOpcUaNodeModel.ServerName = CommOpcUaServerInfo.ServerName;
            }
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "CommOpcUaNode/Update",
                Utility.JsonHelper.ToJson(new List<CommOpcUaNodeModel> { CommOpcUaNodeModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<CommOpcUaNodeModel>(CommOpcUaNodeModel, MessengerToken.DataChanged);
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
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getServerPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<CommOpcUaServerModel>>>(GlobalData.ServerRootUri + "CommOpcUaServer/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取Opc Ua服务器信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("Opc Ua服务器信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    CommOpcUaServerInfoList = new ObservableCollection<CommOpcUaServerModel>(result.Data.Data);
                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    CommOpcUaServerInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                CommOpcUaServerInfoList = new ObservableCollection<CommOpcUaServerModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询Opc Ua服务器信息失败，请联系管理员！";
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
