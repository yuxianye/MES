using GalaSoft.MvvmLight.Messaging;
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
using Solution.Desktop.DisStepActionInfo.Model;
using Solution.Desktop.ProductionProcessInfo.Model;
using Solution.StepTeachingDispatchManagement.Models;

namespace Solution.Desktop.DisStepActionInfo.ViewModel
{
    /// <summary>
    /// 设置
    /// </summary>
    public class DisStepActionProcessMapViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DisStepActionProcessMapViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            SaveCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSaveCommand, OnCanExecuteSaveCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            DisStepActionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnDisStepActionChangedCommand);
            ProductionProcessChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnProductionProcessChangedCommand);
        }
        public override void OnParamterChanged(object parameter)
        {
            this.DisStepActionInfo = parameter as DisStepActionInfoModel;
            if (Equals(DisStepActionInfo, null))
            {
            }
            else
            {
                getDisStepActionInfoPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
                getProcessPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
                SelectCheckedNodes();
            }

        }

        #region 分步操作列表
        private ObservableCollection<DisStepActionInfoModel> disStepActionInfoList = new ObservableCollection<DisStepActionInfoModel>();
        public ObservableCollection<DisStepActionInfoModel> DisStepActionInfoList
        {
            get { return disStepActionInfoList; }
            set { Set(ref disStepActionInfoList, value); }
        }
        #endregion

        #region 工序列表
        private ObservableCollection<ProductionProcessInfoModel> productionProcessInfoListAll = new ObservableCollection<ProductionProcessInfoModel>();
        public ObservableCollection<ProductionProcessInfoModel> ProductionProcessInfoListAll
        {
            get { return productionProcessInfoListAll; }
            set { Set(ref productionProcessInfoListAll, value); }
        }
        #endregion

        #region 分步操作模型
        /// <summary>
        /// 分步操作模型
        /// </summary>
        private DisStepActionInfoModel disStepActionInfo = new DisStepActionInfoModel();
        /// <summary>
        /// 分步操作模型
        /// </summary>
        public DisStepActionInfoModel DisStepActionInfo
        {
            get { return disStepActionInfo; }
            set
            {
                Set(ref disStepActionInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 工序模型
        /// <summary>
        /// 工序模型
        /// </summary>
        private ProductionProcessInfoModel productionProcessInfoModel = new ProductionProcessInfoModel();
        /// <summary>
        /// 工序模型
        /// </summary>
        public ProductionProcessInfoModel ProductionProcessInfoModel
        {
            get { return productionProcessInfoModel; }
            set
            {
                Set(ref productionProcessInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion       

        #region 分步操作-工序配置模型
        /// <summary>
        /// 分步操作-工序配置模型
        /// </summary>
        private DisStepActionProcessMapModel disStepActionProcessMapModel = new DisStepActionProcessMapModel();
        /// <summary>
        /// 分步操作-工序配置模型
        /// </summary>
        public DisStepActionProcessMapModel DisStepActionProcessMapModel
        {
            get { return disStepActionProcessMapModel; }
            set
            {
                Set(ref disStepActionProcessMapModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 分步操作已配置工序数据列表
        private ObservableCollection<DisStepActionProcessMapModel> existDisStepActionProcessMapInfoList = new ObservableCollection<DisStepActionProcessMapModel>();
        public ObservableCollection<DisStepActionProcessMapModel> ExistDisStepActionProcessMapInfoList
        {
            get { return existDisStepActionProcessMapInfoList; }
            set { Set(ref existDisStepActionProcessMapInfoList, value); }
        }
        #endregion

        public ObservableCollection<ProductionProcessInfoModel> SelectedProcessInfoList { get; set; } = new ObservableCollection<ProductionProcessInfoModel>();

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
        /// 分步操作改变命令
        /// </summary>
        public ICommand DisStepActionChangedCommand { get; set; }

        /// <summary>
        /// 工序选择改变命令
        /// </summary>
        public ICommand ProductionProcessChangedCommand { get; set; }
        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return DisStepActionInfo == null ? false : DisStepActionInfo.IsValidated;
        }
        private bool OnCanExecuteSaveCommand()
        {
            return DisStepActionInfo == null ? false : DisStepActionInfo.IsValidated;
        }
        private void OnDisStepActionChangedCommand()
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
        /// 取得分步操作分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getDisStepActionInfoPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<DisStepActionInfoModel>>>(GlobalData.ServerRootUri + "DisStepActionInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取分步操作数据用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("分步操作数据内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    DisStepActionInfoList = new ObservableCollection<DisStepActionInfoModel>(result.Data.Data);
                }
                else
                {
                    DisStepActionInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                DisStepActionInfoList = new ObservableCollection<DisStepActionInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询分步操作数据失败，请联系管理员！";
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
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
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
                    ProductionProcessInfoListAll?.Clear();
                    foreach (var data in result.Data.Data)
                    {
                        ProductionProcessInfoModel pro = new ProductionProcessInfoModel();
                        pro = data;
                        pro.PropertyChanged += OnPropertyChangedCommand;
                        ProductionProcessInfoListAll.Add(pro);
                    }
                }
                else
                {
                    ProductionProcessInfoListAll?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProductionProcessInfoListAll = new ObservableCollection<ProductionProcessInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询工序列表失败，请联系管理员！";
            }
        }
        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            DisStepActionProcessMapModel.ProductionProcessInfoList = SelectedProcessInfoList;
            DisStepActionProcessMapModel.DisStepActionInfo = DisStepActionInfo;
            DisStepActionProcessMapModel.DisStepActionInfo_ID = DisStepActionInfo.Id;

            /*  if (DisStepActionProcessMapModel.ProductionProcessInfo_ID == null)
              {
                  DisStepActionProcessMapModel.ProductionProcessInfo_ID = Guid.Empty;
              }*/
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "DisStepActionInfo/Setting",
                 Utility.JsonHelper.ToJson(new List<DisStepActionProcessMapModel> { DisStepActionProcessMapModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<DisStepActionProcessMapModel>(DisStepActionProcessMapModel, MessengerToken.DataChanged);
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
            DisStepActionProcessMapModel.ProductionProcessInfoList = SelectedProcessInfoList;
            DisStepActionProcessMapModel.DisStepActionInfo = DisStepActionInfo;
            DisStepActionProcessMapModel.DisStepActionInfo_ID = DisStepActionInfo.Id;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "DisStepActionInfo/Setting",
                Utility.JsonHelper.ToJson(new List<DisStepActionProcessMapModel> { DisStepActionProcessMapModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<DisStepActionProcessMapModel>(DisStepActionProcessMapModel, MessengerToken.DataChanged);
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
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRuleName = new FilterRule("ProductionProcessName", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleCode = new FilterRule("ProductionProcessCode", txt.Trim(), FilterOperate.Contains);
            filterGroup.Rules.Add(filterRuleName);
            filterGroup.Rules.Add(filterRuleCode);
            pageRepuestParams.FilterGroup = filterGroup;
            getProcessPageData(1, 1000);
            SelectCheckedNodes();
        }

        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                var selectedObj = sender as ProductionProcessInfoModel;
                if (selectedObj == null)
                    return;
                if (selectedObj.IsChecked)
                {
                    SelectedProcessInfoList.Add(selectedObj);
                }
                else if (!selectedObj.IsChecked)
                {
                    SelectedProcessInfoList.Remove(selectedObj);
                }
            }
        }
        private DisStepActionProcessMapModel processmapmodel = new DisStepActionProcessMapModel();
        private void SelectCheckedNodes()
        {
            SelectedProcessInfoList.Clear();
            //  processmapmodel.DisStepActionInfo_ID = DisStepActionInfo.Id;
            processmapmodel.DisStepActionInfo = DisStepActionInfo;
            for (int i = 0; i < ProductionProcessInfoListAll.Count; i++)
            {
                ProductionProcessInfoListAll[i].IsChecked = false;
            }

            GetNodeInfoByIds(processmapmodel);

            if (ExistDisStepActionProcessMapInfoList.Any())
            {
                foreach (var data in ExistDisStepActionProcessMapInfoList)
                {
                    for (int i = 0; i < ProductionProcessInfoListAll.Count; i++)
                    {
                        if (!Equals(data.ProductionProcessInfo, null))
                        {
                            if (data.ProductionProcessInfo.Id == ProductionProcessInfoListAll[i].Id)
                            {
                                ProductionProcessInfoListAll[i].IsChecked = true;
                            }
                        }
                    }
                }
            }
        }

        private void GetNodeInfoByIds(DisStepActionProcessMapModel model)
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
            FilterRule filterRule = new FilterRule("DisStepActionInfo.Id", model.DisStepActionInfo.Id, FilterOperate.Equal);
            filterGroup.Rules.Add(filterRule);
            pageRepuestParams.FilterGroup = filterGroup;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<DisStepActionProcessMapModel>>>(GlobalData.ServerRootUri + "DisStepActionProcessMapInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

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
                    ExistDisStepActionProcessMapInfoList = new ObservableCollection<DisStepActionProcessMapModel>(result.Data.Data);
                }
                else
                {
                    ExistDisStepActionProcessMapInfoList?.Clear();
                    // TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ExistDisStepActionProcessMapInfoList = new ObservableCollection<DisStepActionProcessMapModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询信息失败，请联系管理员！";
            }
        }
    }
}
