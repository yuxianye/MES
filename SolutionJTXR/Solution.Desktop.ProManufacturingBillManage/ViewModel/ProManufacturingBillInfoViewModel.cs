using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.ProManufacturingBillManage.Model;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Solution.Utility.Windows;
using Solution.Desktop.ProductionRuleManage.Model;

namespace Solution.Desktop.ProManufacturingBillManage.ViewModel
{
    public class ProManufacturingBillInfoViewModel : PageableViewModelBase
    {
        public ProManufacturingBillInfoViewModel()
        {
            //this.MenuModule
            // initCommand();
            registerMessenger();
            MenuItems = new List<MenuItem>()
            {
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Add_32x32.png"),Header =ResourceHelper.FindResource ("New"),Command =AddCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png") ,Header=ResourceHelper.FindResource ("Edit"),Command =EditCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Del_32x32.png") ,Header=ResourceHelper.FindResource ("PhysicalDelete"),Command =PhysicalDeleteCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Refresh_32x32.png") ,Header=ResourceHelper.FindResource ("Refresh"),Command =RefreshCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Import_32x32.png" ),Header=ResourceHelper.FindResource ("Import"),Command =ImportCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Export_32x32.png") ,Header=ResourceHelper.FindResource ("Export"),Command =ExportCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Print_32x32.png") ,Header=ResourceHelper.FindResource ("Print"),Command =PrintCommand},
            };
        }

        #region 制造清单模型
        /// <summary>
        /// 制造清单信息模型
        /// </summary>
        private ProManufacturingBillInfoModel proManufacturingBillInfo;// = new EnterpriseModel();
        /// <summary>
        /// 制造清单信息模型
        /// </summary>
        public ProManufacturingBillInfoModel ProManufacturingBillInfo
        {
            get { return proManufacturingBillInfo; }
            set { Set(ref proManufacturingBillInfo, value); }
        }
        #endregion
        #region 配方模型
        /// <summary>
        /// 配方信息模型
        /// </summary>
        private ProductionRuleInfoModel productionRuleInfo;// = new EnterpriseModel();
        /// <summary>
        /// 配方信息模型
        /// </summary>
        public ProductionRuleInfoModel ProductionRuleInfo
        {
            get { return productionRuleInfo; }
            set { Set(ref productionRuleInfo, value); }
        }
        #endregion
        #region 制造清单信息模型,用于列表数据显示
        private ObservableCollection<ProManufacturingBillInfoModel> proManufacturingBillInfoList = new ObservableCollection<ProManufacturingBillInfoModel>();

        /// <summary>
        /// 制造清单信息数据
        /// </summary>
        public ObservableCollection<ProManufacturingBillInfoModel> ProManufacturingBillInfoList
        {
            get { return proManufacturingBillInfoList; }
            set { Set(ref proManufacturingBillInfoList, value); }
        }
        #endregion

        #region 命令定义和初始化

        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public ICommand EditCommand { get; set; }
        /// <summary>
        /// 清单明细命令
        /// </summary>
        public ICommand BillDetailCommand { get; set; }


        /// <summary>
        /// 物理删除命令
        /// </summary>
        public ICommand PhysicalDeleteCommand { get; set; }

        /// <summary>
        /// 刷新命令
        /// </summary>
        public ICommand RefreshCommand { get; set; }

        /// <summary>
        /// 导入命令
        /// </summary>
        public ICommand ImportCommand { get; set; }

        /// <summary>
        /// 导出命令
        /// </summary>
        public ICommand ExportCommand { get; set; }

        /// <summary>
        /// 导出命令
        /// </summary>
        public ICommand PrintCommand { get; set; }

        /// <summary>
        /// 导出命令
        /// </summary>
        public ICommand SearchCommand { get; set; }
        /// <summary>
        /// 取消命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            AddCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAddCommand, OnCanExecuteAddCommand);
            EditCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteEditCommand, OnCanExecuteEditCommand);
            PhysicalDeleteCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePhysicalDeleteCommand, OnCanExecutePhysicalDeleteCommand);
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand, OnCanExecuteImportCommand);
            ExportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteExportCommand, OnCanExecuteExportCommand);
            PrintCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePrintCommand, OnCanExecutePrintCommand);
            // SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            BillDetailCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteBillDetailCommand, OnCanExecuteBillDetailCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }

        #endregion
        public override void OnParamterChanged(object parameter)
        {
            this.ProductionRuleInfo = parameter as ProductionRuleInfoModel;
            initCommand();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }
        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<ProManufacturingBillInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(ProManufacturingBillInfoModel productionLineModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = ProManufacturingBillInfoList.FirstOrDefault(a => a.Id == productionLineModel.Id);
            this.ProManufacturingBillInfo = ProManufacturingBillInfoList.FirstOrDefault();
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<ProManufacturingBillInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }
        #endregion

        #region 分页数据查询
        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 分页执行函数
        /// </summary>
        /// <param name="e"></param>
        public override void OnExecutePageChangedCommand(PageChangedEventArgs e)
        {
            Utility.LogHelper.Info(e.PageIndex.ToString() + " " + e.PageSize);
            getPageData(e.PageIndex, e.PageSize);
        }
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
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRule1 = new FilterRule("ProductionRule.Id", this.ProductionRuleInfo.Id, "equal");
            filterGroup.Rules.Add(filterRule1);

            pageRepuestParams.FilterGroup = filterGroup;

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProManufacturingBillInfoModel>>>(GlobalData.ServerRootUri + "ProductionRuleInfo/GetBillInfoListByRuleID", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取制造清单信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("制造清单信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    ProManufacturingBillInfoList = new ObservableCollection<ProManufacturingBillInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    ProManufacturingBillInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProManufacturingBillInfoList = new ObservableCollection<ProManufacturingBillInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询制造清单信息失败，请联系管理员！";
            }
        }

        #endregion

        #region 命令和消息等执行函数

        /// <summary>
        /// 执行新建命令
        /// </summary>
        private void OnExecuteAddCommand()
        {
            //功能与页面的对应，Controller和Action 与配置文件对应。
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "ProductionRuleInfo" && a.Action == "AddBill");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.ProductionRuleInfo.Clone();
            Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行新增命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteAddCommand()
        {
            //与服务端对应
            if (this.ProductionRuleInfo.ProductionRuleStatusCode != "UnCommit")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 执行编辑命令
        /// </summary>
        private void OnExecuteEditCommand()
        {
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "ProductionRuleInfo" && a.Action == "UpdateBill");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.ProManufacturingBillInfo.Clone();
            Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行编辑命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteEditCommand()
        {
            if (Equals(ProManufacturingBillInfo, null) || ProManufacturingBillInfo.ProductionRuleStatusCode != "UnCommit")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行清单明细命令
        /// </summary>
        private void OnExecuteBillDetailCommand()
        {
            //功能与页面的对应，Controller和Action 与配置文件对应。
            MenuFunctionViewInfoMap menuFunctionViewInfoMap = new MenuFunctionViewInfoMap();
            if (ProManufacturingBillInfo.BillType.ToString().Equals("BOM"))
            {
                menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "ProductionRuleInfo" && a.Action == "GetProManufacturingBOMBillItemInfoListByBillID");
            }
            else
            {
                menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "ProductionRuleInfo" && a.Action == "GetProManufacturingBORBillItemInfoListByBillID");
            }
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.ProManufacturingBillInfo.Clone();
            Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行清单明细命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteBillDetailCommand()
        {
            //if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            //{
            //    return false;
            //}
            //else
            //{

            // 与服务端对应
            //if (base.MenuModule.Functions.Any(a => a.Controller == "ProductionRuleInfo" && (a.Action == "GetProManufacturingBOMBillItemInfoListByBillID" || a.Action == "GetProManufacturingBORBillItemInfoListByBillID")))
            //{
            if (Equals(ProManufacturingBillInfo, null))
            {
                return false;
            }
            else
            {
                return true;
            }
            //}
            //else
            //{
            //    return false;
            //}
            // }

        }


        /// <summary>
        /// 执行物理删除命令
        /// </summary>
        private void OnExecutePhysicalDeleteCommand()
        {
            if (MessageBox.Show("是否删除该制造清单数据？", "数据删除", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ProManufacturingBillInfo/Delete", Utility.JsonHelper.ToJson(new List<Guid>() { ProManufacturingBillInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    ProManufacturingBillInfo = null;
                }
                else
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = result?.Message ?? "数据删除失败，请联系管理员！";
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                }
            }
        }

        /// <summary>
        /// 是否可以执行物理删除命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecutePhysicalDeleteCommand()
        {

            if (Equals(ProManufacturingBillInfo, null) || ProManufacturingBillInfo.ProductionRuleStatusCode != "UnCommit")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行刷新命令
        /// </summary>
        private void OnExecuteRefreshCommand()
        {
            pageRepuestParams.FilterGroup = null;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }

        /// <summary>
        /// 执行导入命令
        /// </summary>
        private void OnExecuteImportCommand()
        {
            //if (MessageBox.Show(string.Format("真的要要删除【{0}】么？"
            //    + System.Environment.NewLine
            //    + "确认删除点击【是】，取消删除点击【否】。"
            //    , EnterpriseInfo.EnterpriseName), "数据删除", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<Guid>() { EnterpriseInfo.Id }));
            //    var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Delete", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    if (!Equals(result, null) && result.Successed)
            //    {
            //        Application.Current.Resources["UiMessage"] = result?.Message;
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //        getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //        EnterpriseInfo = null;
            //    }
            //    else
            //    {
            //        //操作失败，显示错误信息
            //        Application.Current.Resources["UiMessage"] = result?.Message ?? "数据删除失败，请联系管理员！";
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //    }
            //}

        }

        /// <summary>
        /// 是否可以执行导入命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteImportCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                if (base.MenuModule.Functions.Any(a => a.Controller == "ProManufacturingBillInfo" && a.Action == "Add"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行导出命令
        /// </summary>
        private void OnExecuteExportCommand()
        {
            //if (MessageBox.Show(string.Format("真的要要删除【{0}】么？"
            //    + System.Environment.NewLine
            //    + "确认删除点击【是】，取消删除点击【否】。"
            //    , EnterpriseInfo.EnterpriseName), "数据删除", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<Guid>() { EnterpriseInfo.Id }));
            //    var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Delete", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    if (!Equals(result, null) && result.Successed)
            //    {
            //        Application.Current.Resources["UiMessage"] = result?.Message;
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //        getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //        EnterpriseInfo = null;
            //    }
            //    else
            //    {
            //        //操作失败，显示错误信息
            //        Application.Current.Resources["UiMessage"] = result?.Message ?? "数据删除失败，请联系管理员！";
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //    }
            //}

        }

        /// <summary>
        /// 是否可以执行导出命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteExportCommand()
        {
            if (Equals(ProManufacturingBillInfoList, null) || !ProManufacturingBillInfoList.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行打印命令
        /// </summary>
        private void OnExecutePrintCommand()
        {
            //if (MessageBox.Show(string.Format("真的要要删除【{0}】么？"
            //    + System.Environment.NewLine
            //    + "确认删除点击【是】，取消删除点击【否】。"
            //    , EnterpriseInfo.EnterpriseName), "数据删除", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<Guid>() { EnterpriseInfo.Id }));
            //    var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EnterpriseInfo/Delete", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
            //    if (!Equals(result, null) && result.Successed)
            //    {
            //        Application.Current.Resources["UiMessage"] = result?.Message;
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //        getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //        EnterpriseInfo = null;
            //    }
            //    else
            //    {
            //        //操作失败，显示错误信息
            //        Application.Current.Resources["UiMessage"] = result?.Message ?? "数据删除失败，请联系管理员！";
            //        LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //    }
            //}

        }

        /// <summary>
        /// 是否可以执行打印命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecutePrintCommand()
        {
            if (Equals(ProManufacturingBillInfoList, null) || !ProManufacturingBillInfoList.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchCommand(string txt)
        {
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRuleName = new FilterRule("BillName", txt.Trim(), "contains");
            FilterRule filterRuleCode = new FilterRule("BillCode", txt.Trim(), "contains");
            filterGroup.Rules.Add(filterRuleName);
            filterGroup.Rules.Add(filterRuleCode);

            pageRepuestParams.FilterGroup = filterGroup;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }
        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }
        #endregion
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            base.Disposing();
            unRegisterMessenger();
        }
    }
}
