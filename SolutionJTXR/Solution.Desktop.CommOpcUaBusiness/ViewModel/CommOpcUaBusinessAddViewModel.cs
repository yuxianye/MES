using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.CommOpcUaBusiness.Model;
using Solution.Desktop.CommOpcUaNode.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.CommOpcUaBusiness.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class CommOpcUaBusinessAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommOpcUaBusinessAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
           // getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        #region OpcUa业务数据模型
        /// <summary>
        /// OpcUa业务数据模型
        /// </summary>
        private CommOpcUaBusinessModel commOpcUaBusinessModel = new CommOpcUaBusinessModel();
        /// <summary>
        /// OpcUa业务数据模型
        /// </summary>
        public CommOpcUaBusinessModel CommOpcUaBusinessModel
        {
            get { return commOpcUaBusinessModel; }
            set
            {
                Set(ref commOpcUaBusinessModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region Opc Ua数据点数据模型
        /// <summary>
        /// Opc Ua数据点数据模型
        /// </summary>
        private CommOpcUaNodeModel commOpcUaNodeInfo;// = new EnterpriseModel();
        /// <summary>
        /// Opc Ua数据点数据模型
        /// </summary>
        public CommOpcUaNodeModel CommOpcUaNodeInfo
        {
            get { return commOpcUaNodeInfo; }
            set { Set(ref commOpcUaNodeInfo, value); }
        }
        #endregion

        #region Opc Ua数据点数据模型,用于列表数据显示
        private ObservableCollection<CommOpcUaNodeModel> commOpcUaNodeInfoList = new ObservableCollection<CommOpcUaNodeModel>();

        /// <summary>
        /// Opc Ua数据点数据
        /// </summary>
        public ObservableCollection<CommOpcUaNodeModel> CommOpcUaNodeInfoList
        {
            get { return commOpcUaNodeInfoList; }
            set { Set(ref commOpcUaNodeInfoList, value); }
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
            return CommOpcUaBusinessModel == null ? false : CommOpcUaBusinessModel.IsValidated;
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
        private void getPageData(int pageIndex, int pageSize)
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
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<CommOpcUaNodeModel>>>(GlobalData.ServerRootUri + "CommOpcUaNode/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取Opc Ua数据点数据用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("Opc Ua数据点数据内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    CommOpcUaNodeInfoList = new ObservableCollection<CommOpcUaNodeModel>(result.Data.Data);
                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    CommOpcUaNodeInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                CommOpcUaNodeInfoList = new ObservableCollection<CommOpcUaNodeModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询Opc Ua数据点数据失败，请联系管理员！";
            }
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            //if (CommOpcUaNodeInfo != null)
            //{
            //    CommOpcUaBusinessModel.NodeId = CommOpcUaNodeInfo.Id;
            //    CommOpcUaBusinessModel.NodeName = CommOpcUaNodeInfo.NodeName;
            //}
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "CommOpcUaBusiness/Add",
                Utility.JsonHelper.ToJson(new List<CommOpcUaBusinessModel> { CommOpcUaBusinessModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<CommOpcUaBusinessModel>(CommOpcUaBusinessModel, MessengerToken.DataChanged);
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
