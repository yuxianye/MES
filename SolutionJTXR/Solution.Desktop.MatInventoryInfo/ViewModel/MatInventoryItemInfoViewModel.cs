using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.MatInventoryInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Utility;
using Solution.Utility.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Solution.Desktop.MatInventoryInfo.ViewModel
{
    public class MatInventoryItemInfoViewModel : PageableViewModelBase
    {
        public MatInventoryItemInfoViewModel()
        {
            //this.MenuModule
            initCommand();
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

        #region 订单模型
        /// <summary>
        /// 订单信息模型
        /// </summary>
        private MatInventoryItemInfoModel planOrderInfo;// = new EnterpriseModel();
        /// <summary>
        /// 订单信息模型
        /// </summary>
        public MatInventoryItemInfoModel MatInventoryItemInfo
        {
            get { return planOrderInfo; }
            set { Set(ref planOrderInfo, value); }
        }
        #endregion
        #region 订单模型
        private MatInventoryInfoModel orderInfoModel = new MatInventoryInfoModel();
        /// <summary>
        /// 企业模型
        /// </summary>
        public MatInventoryInfoModel MatInventoryInfo
        {
            get { return orderInfoModel; }
            set
            {
                Set(ref orderInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        #region 盘点明细信息信息模型,用于列表数据显示
        private ObservableCollection<MatInventoryItemInfoModel> planOrderItemInfoList = new ObservableCollection<MatInventoryItemInfoModel>();

        /// <summary>
        /// 订单信息数据
        /// </summary>
        public ObservableCollection<MatInventoryItemInfoModel> MatInventoryItemInfoList
        {
            get { return planOrderItemInfoList; }
            set { Set(ref planOrderItemInfoList, value); }
        }
        #endregion
        public override void OnParamterChanged(object parameter)
        {
            this.MatInventoryInfo = parameter as MatInventoryInfoModel;
            //getPageData();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }
        #region 命令定义和初始化

        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public ICommand EditCommand { get; set; }

        ///// <summary>
        ///// 可恢复的删除命令
        ///// </summary>
        //public ICommand RecycleCommand { get; set; }

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
        /// 作业命令
        /// </summary>
        public ICommand DistributeCommand { get; set; }

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
            // RecycleCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRecycleCommand, OnCanExecuteRecycleCommand);
            PhysicalDeleteCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePhysicalDeleteCommand, OnCanExecutePhysicalDeleteCommand);
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand, OnCanExecuteImportCommand);
            ExportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteExportCommand, OnCanExecuteExportCommand);
            PrintCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePrintCommand, OnCanExecutePrintCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            DistributeCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteDistributeCommand, OnCanExecuteDistributeCommand);
        }

        #endregion

        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<MatInventoryItemInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(MatInventoryItemInfoModel planorderiteminfo)
        {
            //getPageData();
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = MatInventoryItemInfoList.FirstOrDefault(a => a.Id == planorderiteminfo.Id);
            this.MatInventoryItemInfo = MatInventoryItemInfoList.FirstOrDefault();
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<MatInventoryItemInfoModel>(this, MessengerToken.DataChanged, dataChanged);
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
            //getPageData();
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
            FilterRule filterRuleName = new FilterRule("MatInventory.Id", MatInventoryInfo.Id, "equal");
            filterGroup.Rules.Add(filterRuleName);

            pageRepuestParams.FilterGroup = filterGroup;

            //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatInventoryItemInfoModel>>>(GlobalData.ServerRootUri + "MatInventoryInfo/GetMatInventoryItemInfoListByInventoryID", Utility.JsonHelper.ToJson(MatInventoryInfo));
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatInventoryItemInfoModel>>>(GlobalData.ServerRootUri + "MatInventoryItemInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取盘点明细信息信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("盘点明细信息信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    MatInventoryItemInfoList = new ObservableCollection<MatInventoryItemInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MatInventoryItemInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatInventoryItemInfoList = new ObservableCollection<MatInventoryItemInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询盘点明细信息信息失败，请联系管理员！";
            }
        }

        #endregion

        #region 命令和消息等执行函数

        /// <summary>
        /// 执行新建命令
        /// </summary>
        private void OnExecuteAddCommand()
        {
            //if (MatInventoryInfo.OrderState != PlanEnumModel.OrderState.未开始)
            //{
            //    Application.Current.Resources["UiMessage"] = "订单状态为‘未开始’的记录才能增加盘点明细信息，请联系管理员！";
            //}
            //else
            //{
            //
            //功能与页面的对应，Controller和Action 与配置文件对应。
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MatInventoryInfo" && a.Action == "AddItem");
                var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
                viewInfo.Parameter = this.MatInventoryInfo.Id.ToString();
                Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
            //}
        }

        /// <summary>
        /// 是否可以执行新增命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteAddCommand()
        {
            return true;

        }

        /// <summary>
        /// 执行编辑命令
        /// </summary>
        private void OnExecuteEditCommand()
        {
            //if (MatInventoryInfo.OrderState != PlanEnumModel.OrderState.未开始)
            //{
            //    Application.Current.Resources["UiMessage"] = "订单状态为‘未开始’的记录才能被修改，请联系管理员！";
            //}
            //else
            //{
                //
                //Controller和Action 与配置文件对应
                var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MatInventoryInfo" && a.Action == "UpdateItem");
                var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
                viewInfo.Parameter = this.MatInventoryItemInfo.Clone();
                Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
            //}
        }

        /// <summary>
        /// 是否可以执行编辑命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteEditCommand()
        {
            //盘点结束
            if (Equals(MatInventoryItemInfo, null) || MatInventoryItemInfo.InventoryState == InventoryStateEnumModel.InventoryState.InventoryFinishState)
            //if (Equals(MatInventoryItemInfo, null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        /// <summary>
        /// 执行物理删除命令
        /// </summary>
        private void OnExecutePhysicalDeleteCommand()
        {
            if (MessageBox.Show("确定要从磁盘中删除这条数据么？删除后无法恢复。", "数据删除", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MatInventoryItemInfo/Remove", Utility.JsonHelper.ToJson(new List<Guid>() { MatInventoryItemInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    //getPageData();
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    MatInventoryItemInfo = null;
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
            //if (Equals(MatStorageMoveInfo, null))
            //盘点结束
            if (Equals(MatInventoryItemInfo, null) || MatInventoryItemInfo.InventoryState == InventoryStateEnumModel.InventoryState.InventoryFinishState)
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
            //getPageData();
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }

        /// <summary>
        /// 执行作业命令
        /// </summary>
        private void OnExecuteDistributeCommand()
        {
            if (MessageBox.Show("确定要操作这条数据么？", "数据处理", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MatInventoryItemInfo/Distribute", Utility.JsonHelper.ToJson(new List<MatInventoryItemInfoModel>() { MatInventoryItemInfo }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    //getPageData();
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    MatInventoryItemInfo = null;
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
        /// 是否可以执行作业命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteDistributeCommand()
        {
            //盘点结束
            if (Equals(MatInventoryItemInfo, null) || MatInventoryItemInfo.InventoryState == InventoryStateEnumModel.InventoryState.InventoryFinishState)
            //if (Equals(MatInventoryItemInfo, null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行导入命令
        /// </summary>
        private void OnExecuteImportCommand()
        {


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
                if (base.MenuModule.Functions.Any(a => a.Controller == "MatInventoryItemInfo" && a.Action == "Add"))
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
        }

        /// <summary>
        /// 是否可以执行导出命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteExportCommand()
        {
            if (Equals(MatInventoryItemInfoList, null) || !MatInventoryItemInfoList.Any())
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

        }

        /// <summary>
        /// 是否可以执行打印命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecutePrintCommand()
        {
            if (Equals(MatInventoryItemInfoList, null) || !MatInventoryItemInfoList.Any())
            {
                return false;
            }
            else
            {
                return true;
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
            FilterRule filterRuleName = new FilterRule("OrderItemName", txt.Trim(), "contains");
            FilterRule filterRuleCode = new FilterRule("OrderItemCode", txt.Trim(), "contains");
            filterGroup.Rules.Add(filterRuleName);
            filterGroup.Rules.Add(filterRuleCode);

            pageRepuestParams.FilterGroup = filterGroup;
            //getPageData();
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }

        #endregion

        ///// <summary>
        ///// 清理资源
        ///// </summary>
        //public override void Cleanup()
        //{
        //    base.Cleanup();

        //    unRegisterMessenger();
        //}
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
