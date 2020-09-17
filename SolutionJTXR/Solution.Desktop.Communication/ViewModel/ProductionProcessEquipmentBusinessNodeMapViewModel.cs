using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Communication.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Core.Enum;

namespace Solution.Desktop.Communication.ViewModel
{
    /// <summary>
    /// 设置
    /// </summary>
    public class ProductionProcessEquipmentBusinessNodeMapViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionProcessEquipmentBusinessNodeMapViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            SaveCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSaveCommand, OnCanExecuteSaveCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            BusinessSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnBusinessSelectionChangedCommand);
            EquipmentSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnEquipmentSelectionChangedCommand);
            ProductionProcessChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnProductionProcessChangedCommand);
        }
        public override void OnParamterChanged(object parameter)
        {
            var tmpBusinessNodeModel = parameter as BusinessNodeModel;
            if (Equals(tmpBusinessNodeModel, null))
            {



            }
            else
            {
                this.EquipmentBusinessNodeMapModel.BusinessNode_Id = tmpBusinessNodeModel.Id;
                getBusinessPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
                getEquipmentPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
                getNodePageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")), false, "");
                getProcessPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
                SelectCheckedNodes();
            }

        }

        #region Socket通信业务模型
        /// <summary>
        /// 业务点名模型
        /// </summary>
        private BusinessNodeModel businessNodeModel;
        /// <summary>
        /// 业务点名模型
        /// </summary>
        public BusinessNodeModel BusinessNodeModel
        {
            get { return businessNodeModel; }
            set
            {
                Set(ref businessNodeModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region OpcUa业务数据模型
        /// <summary>
        /// OpcUa业务数据模型
        /// </summary>
        private ProductionProcessEquipmentBusinessNodeMapModel equipmentBusinessNodeMapModel = new ProductionProcessEquipmentBusinessNodeMapModel();
        /// <summary>
        /// OpcUa业务数据模型
        /// </summary>
        public ProductionProcessEquipmentBusinessNodeMapModel EquipmentBusinessNodeMapModel
        {
            get { return equipmentBusinessNodeMapModel; }
            set
            {
                Set(ref equipmentBusinessNodeMapModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 业务列表
        private ObservableCollection<BusinessNodeModel> businessInfoList = new ObservableCollection<BusinessNodeModel>();
        public ObservableCollection<BusinessNodeModel> BusinessInfoList
        {
            get { return businessInfoList; }
            set { Set(ref businessInfoList, value); }
        }
        #endregion

        #region 数据点数据模型
        /// <summary>
        /// Opc Ua数据点数据模型
        /// </summary>
        private DeviceNodeModel deviceNodeModel;// = new EnterpriseModel();
        /// <summary>
        /// Opc Ua数据点数据模型
        /// </summary>
        public DeviceNodeModel DeviceNodeModel
        {
            get { return deviceNodeModel; }
            set { Set(ref deviceNodeModel, value); }
        }
        #endregion

        #region 设备数据点列表
        private ObservableCollection<DeviceNodeModel> nodeInfoList = new ObservableCollection<DeviceNodeModel>();
        public ObservableCollection<DeviceNodeModel> NodeInfoList
        {
            get { return nodeInfoList; }
            set { Set(ref nodeInfoList, value); }
        }
        #endregion
        #region 工序列表
        private ObservableCollection<ProductionProcessInfoModel> productionProcessInfoList = new ObservableCollection<ProductionProcessInfoModel>();
        public ObservableCollection<ProductionProcessInfoModel> ProductionProcessInfoList
        {
            get { return productionProcessInfoList; }
            set { Set(ref productionProcessInfoList, value); }
        }
        #endregion
        #region 已配置数据点列表
        private ObservableCollection<ProductionProcessEquipmentBusinessNodeMapModel> existNodeInfoList = new ObservableCollection<ProductionProcessEquipmentBusinessNodeMapModel>();
        public ObservableCollection<ProductionProcessEquipmentBusinessNodeMapModel> ExistNodeInfoList
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
        public ObservableCollection<DeviceNodeModel> LocalNodeInfoList { get; set; } = new ObservableCollection<DeviceNodeModel>();
        /// <summary>
        /// 确认命令(关闭弹窗)
        /// </summary>
        public ICommand ConfirmCommand { get; set; }
        /// <summary>
        /// 保存命令（不关闭弹窗）
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand SearchCommand { get; set; }

        /// <summary>
        /// 工序选择改变命令
        /// </summary>
        public ICommand ProductionProcessChangedCommand { get; set; }

        /// <summary>
        /// 命令
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
            return EquipmentBusinessNodeMapModel == null ? false : EquipmentBusinessNodeMapModel.IsValidated;
        }
        private bool OnCanExecuteSaveCommand()
        {
            return EquipmentBusinessNodeMapModel == null ? false : EquipmentBusinessNodeMapModel.IsValidated;
        }
        private void OnEquipmentSelectionChangedCommand()
        {
            SelectCheckedNodes();
        }
        private void OnBusinessSelectionChangedCommand()
        {
            SelectCheckedNodes();
        }
        private void OnProductionProcessChangedCommand()
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
            pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<BusinessNodeModel>>>(GlobalData.ServerRootUri + "BusinessNode/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

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
                    BusinessInfoList = new ObservableCollection<BusinessNodeModel>(result.Data.Data);
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
                BusinessInfoList = new ObservableCollection<BusinessNodeModel>();
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
            pageRepuestParams = new PageRepuestParams();
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
                    EquipmentInfoList = new ObservableCollection<EquipmentModel>(result.Data.Data);
                    EquipmentModel equ = new EquipmentModel();
                    EquipmentInfoList.Insert(0, equ);
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
        private void getNodePageData(int pageIndex, int pageSize, bool isSearch, string searchInfo)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams = new PageRepuestParams();
            if (isSearch)
            {
                FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
                FilterRule filterRuleName = new FilterRule("NodeName", searchInfo, FilterOperate.Contains);
                FilterRule filterRuleServer = new FilterRule("DeviceServerInfo.DeviceServerName", searchInfo, FilterOperate.Contains);
                FilterRule filterRuleUrl = new FilterRule("NodeUrl", searchInfo, FilterOperate.Contains);
                filterGroup.Rules.Add(filterRuleName);
                filterGroup.Rules.Add(filterRuleServer);
                filterGroup.Rules.Add(filterRuleUrl);
                pageRepuestParams.FilterGroup = filterGroup;
            }
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<DeviceNodeModel>>>(GlobalData.ServerRootUri + "DeviceNode/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

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
                        DeviceNodeModel commOpcUaNode = new DeviceNodeModel();
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
                NodeInfoList = new ObservableCollection<DeviceNodeModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询数据点列表失败，请联系管理员！";
            }
        }

        /// <summary>
        /// 获取工序分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getProcessPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortField = "ProductionProcessName";
            pageRepuestParams.SortOrder = "asc";
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionProcessInfoModel>>>(GlobalData.ServerRootUri + "ProductionProcessInfo/GetProductionProcessInfoList", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取工序列表用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("工序列表内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    ProductionProcessInfoList = new ObservableCollection<ProductionProcessInfoModel>(result.Data.Data);
                    ProductionProcessInfoModel pro = new ProductionProcessInfoModel();
                    ProductionProcessInfoList.Insert(0, pro);
                }
                else
                {
                    ProductionProcessInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProductionProcessInfoList = new ObservableCollection<ProductionProcessInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询工序列表失败，请联系管理员！";
            }
        }
        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            EquipmentBusinessNodeMapModel.DeviceNodeList = LocalNodeInfoList;
            if (EquipmentBusinessNodeMapModel.Equipment_Id == null)
            {
                EquipmentBusinessNodeMapModel.Equipment_Id = Guid.Empty;
            }
            if (EquipmentBusinessNodeMapModel.ProductionProcessInfo_Id == null)
            {
                EquipmentBusinessNodeMapModel.ProductionProcessInfo_Id = Guid.Empty;
            }
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "BusinessNode/Setting",
                Utility.JsonHelper.ToJson(new List<ProductionProcessEquipmentBusinessNodeMapModel> { EquipmentBusinessNodeMapModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<ProductionProcessEquipmentBusinessNodeMapModel>(EquipmentBusinessNodeMapModel, MessengerToken.DataChanged);
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
        /// 执行保存命令函数
        /// </summary>
        private void OnExecuteSaveCommand()
        {
            EquipmentBusinessNodeMapModel.DeviceNodeList = LocalNodeInfoList;
            if (EquipmentBusinessNodeMapModel.Equipment_Id == null)
            {
                EquipmentBusinessNodeMapModel.Equipment_Id = Guid.Empty;
            }
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "BusinessNode/Setting",
                Utility.JsonHelper.ToJson(new List<ProductionProcessEquipmentBusinessNodeMapModel> { EquipmentBusinessNodeMapModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<ProductionProcessEquipmentBusinessNodeMapModel>(EquipmentBusinessNodeMapModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.DataChanged);
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
            getNodePageData(1, 1000, true, txt.Trim());
            SelectCheckedNodes();
        }

        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                var selectedObj = sender as DeviceNodeModel;
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
        private ProductionProcessEquipmentBusinessNodeMapModel nodemapmodel = new ProductionProcessEquipmentBusinessNodeMapModel();
        private void SelectCheckedNodes()
        {
            LocalNodeInfoList.Clear();
            if (EquipmentBusinessNodeMapModel.Equipment_Id == null)
            {
                EquipmentBusinessNodeMapModel.Equipment_Id = Guid.Empty;
            }
            if (EquipmentBusinessNodeMapModel.ProductionProcessInfo_Id == null)
            {
                EquipmentBusinessNodeMapModel.ProductionProcessInfo_Id = Guid.Empty;
            }
            nodemapmodel.Equipment_Id = EquipmentBusinessNodeMapModel.Equipment_Id;
            nodemapmodel.BusinessNode_Id = EquipmentBusinessNodeMapModel.BusinessNode_Id;
            nodemapmodel.ProductionProcessInfo_Id = equipmentBusinessNodeMapModel.ProductionProcessInfo_Id;
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
                        if (data.DeviceNode_Id == NodeInfoList[i].Id)
                        {
                            NodeInfoList[i].IsChecked = true;
                        }
                    }
                }
            }
        }

        private void GetNodeInfoByIds(ProductionProcessEquipmentBusinessNodeMapModel model)
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
            FilterRule filterRule1 = new FilterRule("Equipment_Id", model.Equipment_Id, FilterOperate.Equal);
            FilterRule filterRule2 = new FilterRule("BusinessNode.Id", model.BusinessNode_Id, FilterOperate.Equal);
            FilterRule filterRule3 = new FilterRule("ProductionProcessInfo_Id", model.ProductionProcessInfo_Id, FilterOperate.Equal);
            filterGroup.Rules.Add(filterRule1);
            filterGroup.Rules.Add(filterRule2);
            filterGroup.Rules.Add(filterRule3);
            pageRepuestParams.FilterGroup = filterGroup;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionProcessEquipmentBusinessNodeMapModel>>>(GlobalData.ServerRootUri + "ProductionProcessEquipmentBusinessNodeMap/GridData", Utility.JsonHelper.ToJson(pageRepuestParams));

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
                    ExistNodeInfoList = new ObservableCollection<ProductionProcessEquipmentBusinessNodeMapModel>(result.Data.Data);
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
                ExistNodeInfoList = new ObservableCollection<ProductionProcessEquipmentBusinessNodeMapModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询厂区信息失败，请联系管理员！";
            }
        }
    }
}
