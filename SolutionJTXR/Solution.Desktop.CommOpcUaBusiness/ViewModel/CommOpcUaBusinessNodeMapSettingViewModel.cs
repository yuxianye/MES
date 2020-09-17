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
using Solution.Desktop.EquipmentInfo.Model;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Core.Enum;

namespace Solution.Desktop.CommOpcUaBusiness.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class CommOpcUaBusinessNodeMapSettingViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommOpcUaBusinessNodeMapSettingViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            BusinessSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnBusinessSelectionChangedCommand);
            EquipmentSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnEquipmentSelectionChangedCommand);
        }
        public override void OnParamterChanged(object parameter)
        {
            this.CommOpcUaBusinessNodeMapModel.OpcUaBusiness_Id = (parameter as CommOpcUaBusinessModel).Id;
            getBusinessPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getEquipmentPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getNodePageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            SelectCheckedNodes();
        }

        #region Socket通信业务模型
        /// <summary>
        /// OpcUa业务数据模型
        /// </summary>
        private CommOpcUaBusinessModel commOpcUaBusinessModel;// = new EnterpriseModel();
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
        #region OpcUa业务数据模型
        /// <summary>
        /// OpcUa业务数据模型
        /// </summary>
        private CommOpcUaBusinessNodeMapModel commOpcUaBusinessNodeMapModel = new CommOpcUaBusinessNodeMapModel();
        /// <summary>
        /// OpcUa业务数据模型
        /// </summary>
        public CommOpcUaBusinessNodeMapModel CommOpcUaBusinessNodeMapModel
        {
            get { return commOpcUaBusinessNodeMapModel; }
            set
            {
                Set(ref commOpcUaBusinessNodeMapModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        #region 业务列表
        private ObservableCollection<CommOpcUaBusinessModel> businessInfoList = new ObservableCollection<CommOpcUaBusinessModel>();
        public ObservableCollection<CommOpcUaBusinessModel> BusinessInfoList
        {
            get { return businessInfoList; }
            set { Set(ref businessInfoList, value); }
        }
        #endregion
        #region 数据点数据模型
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

        #region 数据点列表
        private ObservableCollection<CommOpcUaNodeModel> nodeInfoList = new ObservableCollection<CommOpcUaNodeModel>();
        public ObservableCollection<CommOpcUaNodeModel> NodeInfoList
        {
            get { return nodeInfoList; }
            set { Set(ref nodeInfoList, value); }
        }
        #endregion
        #region 已配置数据点列表
        private ObservableCollection<CommOpcUaBusinessNodeMapModel> existNodeInfoList = new ObservableCollection<CommOpcUaBusinessNodeMapModel>();
        public ObservableCollection<CommOpcUaBusinessNodeMapModel> ExistNodeInfoList
        {
            get { return existNodeInfoList; }
            set { Set(ref existNodeInfoList, value); }
        }
        #endregion
        #region 设备列表
        private ObservableCollection<EquipmentModel> equipmentInfoList = new ObservableCollection<EquipmentModel>();

        /// <summary>
        /// 设备数据
        /// </summary>
        public ObservableCollection<EquipmentModel> EquipmentInfoList
        {
            get { return equipmentInfoList; }
            set { Set(ref equipmentInfoList, value); }
        }
        #endregion
        public ObservableCollection<CommOpcUaNodeModel> LocalNodeInfoList { get; set; } = new ObservableCollection<CommOpcUaNodeModel>();
        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }
        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand SearchCommand { get; set; }
        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand BusinessSelectionChangedCommand { get; set; }
        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand EquipmentSelectionChangedCommand { get; set; }
        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return CommOpcUaBusinessNodeMapModel == null ? false : CommOpcUaBusinessNodeMapModel.IsValidated;
        }
        private void OnEquipmentSelectionChangedCommand()
        {
            SelectCheckedNodes();
        }
        private void OnBusinessSelectionChangedCommand()
        {
            SelectCheckedNodes();
        }

        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 取得业务分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getBusinessPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<CommOpcUaBusinessModel>>>(GlobalData.ServerRootUri + "CommOpcUaBusiness/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取Opc Ua业务数据用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("Opc Ua业务数据内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                   BusinessInfoList = new ObservableCollection<CommOpcUaBusinessModel>(result.Data.Data);
                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    BusinessInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                BusinessInfoList = new ObservableCollection<CommOpcUaBusinessModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询Opc Ua业务数据失败，请联系管理员！";
            }
        }
        /// <summary>
        /// 获取设备分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getEquipmentPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquipmentModel>>>(GlobalData.ServerRootUri + "EquipmentInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取设备列表用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("设备列表内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EquipmentInfoList = new ObservableCollection<EquipmentModel>(result.Data.Data);
                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    EquipmentInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquipmentInfoList = new ObservableCollection<EquipmentModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询设备列表失败，请联系管理员！";
            }
        }
        /// <summary>
        /// 获取数据点分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getNodePageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<CommOpcUaNodeModel>>>(GlobalData.ServerRootUri + "CommOpcUaNode/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取数据点列表用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("数据点列表内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    NodeInfoList?.Clear();
                    foreach (var data in result.Data.Data)
                    {
                        CommOpcUaNodeModel commOpcUaNode = new CommOpcUaNodeModel();
                        commOpcUaNode = data;
                        commOpcUaNode.PropertyChanged += OnPropertyChangedCommand;
                        NodeInfoList.Add(commOpcUaNode);
                    }
                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    NodeInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                NodeInfoList = new ObservableCollection<CommOpcUaNodeModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询数据点列表失败，请联系管理员！";
            }
        }

       
        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            CommOpcUaBusinessNodeMapModel.CommOpcUaNodeInfoList = LocalNodeInfoList;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "CommOpcUaBusiness/Setting",
                Utility.JsonHelper.ToJson(new List<CommOpcUaBusinessNodeMapModel> { CommOpcUaBusinessNodeMapModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<CommOpcUaBusinessNodeMapModel>(CommOpcUaBusinessNodeMapModel, MessengerToken.DataChanged);
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
        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchCommand(string txt)
        {
                FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
                FilterRule filterRuleName = new FilterRule("NodeName", txt.Trim(), FilterOperate.Contains);
                FilterRule filterRuleServer = new FilterRule("ServerName", txt.Trim(), FilterOperate.Contains);
                filterGroup.Rules.Add(filterRuleName);
                filterGroup.Rules.Add(filterRuleServer);
                pageRepuestParams.FilterGroup = filterGroup;
                getNodePageData(1, 1000);
                SelectCheckedNodes();
        }

        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                var selectedObj = sender as CommOpcUaNodeModel;
                if (selectedObj == null)
                    return;
                if (selectedObj.IsChecked)
                {
                    LocalNodeInfoList.Add(selectedObj);
                }
                else if (!selectedObj.IsChecked)
                {
                    LocalNodeInfoList.Remove(selectedObj);
                }
            }
        }
        private CommOpcUaBusinessNodeMapModel nodemapmodel = new CommOpcUaBusinessNodeMapModel();
        private void SelectCheckedNodes()
        {
            LocalNodeInfoList.Clear();
            nodemapmodel.EquipmentID = CommOpcUaBusinessNodeMapModel.EquipmentID;
            nodemapmodel.OpcUaBusiness_Id = CommOpcUaBusinessNodeMapModel.OpcUaBusiness_Id;
            for (int i = 0; i < NodeInfoList.Count; i++)
            {
                NodeInfoList[i].IsChecked = false;
            }
            GetNodeInfoByIds(nodemapmodel);
            if (ExistNodeInfoList.Any())
            {
                foreach (var data in ExistNodeInfoList)
                {
                    for (int i = 0; i < NodeInfoList.Count; i++)
                    {
                        if (data.OpcUaNode_Id == NodeInfoList[i].Id)
                        {
                            NodeInfoList[i].IsChecked = true;
                        }
                    }
                }
            }
        }

        private void GetNodeInfoByIds(CommOpcUaBusinessNodeMapModel model)
        {
#if DEBUG
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = 200;
            FilterGroup filterGroup = new FilterGroup(FilterOperate.And);
            FilterRule filterRule1 = new FilterRule("EquipmentID", model.EquipmentID, FilterOperate.Equal);
            FilterRule filterRule2 = new FilterRule("OpcUaBusiness.Id", model.OpcUaBusiness_Id, FilterOperate.Equal);
            if (model.EquipmentID != Guid.Empty)
            {
                filterGroup.Rules.Add(filterRule1);
            }
            filterGroup.Rules.Add(filterRule2);
            pageRepuestParams.FilterGroup = filterGroup;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<CommOpcUaBusinessNodeMapModel>>>(GlobalData.ServerRootUri + "CommOpcUaBusinessNodeMap/GridData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
                stopwatch.Stop();
                Utility.LogHelper.Info("获取已配置数据点信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
                Utility.LogHelper.Info("已配置数据点信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    if (result.Data.Data.Any())
                    {
                        //TotalCounts = result.Data.Total;
                        //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                        ExistNodeInfoList = new ObservableCollection<CommOpcUaBusinessNodeMapModel>(result.Data.Data);
                       // TotalCounts = result.Data.Total;
                    }
                    else
                    {
                    ExistNodeInfoList?.Clear();
                       // TotalCounts = 0;
                        Application.Current.Resources["UiMessage"] = "未找到数据";
                    }
                }
                else
                {
                //操作失败，显示错误信息
                ExistNodeInfoList = new ObservableCollection<CommOpcUaBusinessNodeMapModel>();
                    Application.Current.Resources["UiMessage"] = result?.Message ?? "查询厂区信息失败，请联系管理员！";
                }
            }
        }
}
