using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Agv.Model;
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
using System.Threading.Tasks;

namespace Solution.Desktop.Agv.ViewModel
{
    public class RouteInfoViewModel : PageableViewModelBase
    {
        public RouteInfoViewModel()
        {
            //this.MenuModule
            initCommand();
            registerMessenger();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            //右键快捷菜单
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

        #region RouteInfo模型
        /// <summary>
        /// RouteInfo模型
        /// </summary>
        private RouteInfoModel routeInfo;
        /// <summary>
        /// RouteInfoModel模型
        /// </summary>
        public RouteInfoModel RouteInfo
        {
            get { return routeInfo; }
            set { Set(ref routeInfo, value); }
        }
        #endregion

        #region RouteInfo模型,用于列表数据显示
        private ObservableCollection<RouteInfoModel> routeInfoList = new ObservableCollection<RouteInfoModel>();

        /// <summary>
        /// Route信息数据
        /// </summary>
        public ObservableCollection<RouteInfoModel> RouteInfoList
        {
            get { return routeInfoList; }
            set { Set(ref routeInfoList, value); }
        }
        #endregion

        #region 按钮E/D
        private bool _generatBtnEnable = true;
        /// <summary>
        /// 开始按钮是否能用
        /// </summary>
        public bool GeneratBtnEnable
        {
            get { return _generatBtnEnable; }
            set { Set(ref _generatBtnEnable, value); }
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
        /// 可恢复的删除命令
        /// </summary>
        public ICommand RecycleCommand { get; set; }

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
        /// 自动生成命令
        /// </summary>
        public ICommand GenerateRouteCommand { get; set; }

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            AddCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAddCommand, OnCanExecuteAddCommand);
            EditCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteEditCommand, OnCanExecuteEditCommand);
            RecycleCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRecycleCommand, OnCanExecuteRecycleCommand);
            PhysicalDeleteCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePhysicalDeleteCommand, OnCanExecutePhysicalDeleteCommand);
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand, OnCanExecuteImportCommand);
            ExportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteExportCommand, OnCanExecuteExportCommand);
            PrintCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePrintCommand, OnCanExecutePrintCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            GenerateRouteCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnGenerateRouteCommand);
        }

        #endregion

        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<RouteInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(RouteInfoModel RouteInfoModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = RouteInfoList.FirstOrDefault(a => a.Id == RouteInfoModel.Id);
            this.RouteInfo = RouteInfoList.FirstOrDefault();

        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            //Messenger.Default.Unregister<ViewInfo>(this, Model.MessengerToken.Navigate, Navigate);

            //Messenger.Default.Unregister<object>(this, Model.MessengerToken.ClosePopup, ClosePopup);

            //Messenger.Default.Unregister<MenuInfo>(this, Model.MessengerToken.SetMenuStatus, SetMenuStatus);
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

            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<RouteInfoModel>>>(GlobalData.ServerRootUri + "RouteInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取路径信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("路径信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    RouteInfoList = new ObservableCollection<RouteInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    RouteInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                RouteInfoList = new ObservableCollection<RouteInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询路径信息失败，请联系管理员！";
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "RouteInfo" && a.Action == "Add");
            Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行新增命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteAddCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                //与服务端对应
                if (base.MenuModule.Functions.Any(a => a.Controller == "RouteInfo" && a.Action == "Add"))
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
        /// 执行编辑命令
        /// </summary>
        private void OnExecuteEditCommand()
        {
            //Controller和Action 与配置文件对应
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "RouteInfo" && a.Action == "Update");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            if (!Equals(viewInfo, null))
            {
                viewInfo.Parameter = this.RouteInfo.Clone();
            }
            Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行编辑命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteEditCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                if (base.MenuModule.Functions.Any(a => a.Controller == "RouteInfo" && a.Action == "Update"))
                {
                    if (Equals(RouteInfo, null))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行逻辑删除命令
        /// </summary>
        private async void OnExecuteRecycleCommand()
        {
            var window = (MetroWindow)Application.Current.MainWindow;
            var dialogResult = await window.ShowMessageAsync("数据删除"
                , string.Format("确定要删除【{0}】这条数据么？删除后可在管理界面恢复。" + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", RouteInfo.RouteName)
                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            if (dialogResult == MessageDialogResult.Affirmative)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "RouteInfo/Recycle", Utility.JsonHelper.ToJson(new List<RouteInfoModel>() { RouteInfo }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    RouteInfo = null;
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
        /// 是否可以执行逻辑删除命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteRecycleCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                if (base.MenuModule.Functions.Any(a => a.Controller == "RouteInfo" && a.Action == "Recycle"))
                {
                    if (Equals(RouteInfo, null))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 执行物理删除命令
        /// </summary>
        private async void OnExecutePhysicalDeleteCommand()
        {
            var window = (MetroWindow)Application.Current.MainWindow;
            var dialogResult = await window.ShowMessageAsync("数据删除"
                , string.Format("确定要从磁盘中删除【{0}】这条数据么？删除后无法恢复。" + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", RouteInfo.RouteName)
                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            if (dialogResult == MessageDialogResult.Affirmative)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "RouteInfo/Remove", Utility.JsonHelper.ToJson(new List<Guid>() { RouteInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    RouteInfo = null;
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
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                if (base.MenuModule.Functions.Any(a => a.Controller == "RouteInfo" && a.Action == "Remove"))
                {
                    if (Equals(RouteInfo, null))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
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


        }

        /// <summary>
        /// 是否可以执行导入命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteImportCommand()
        {
            return false;
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
            return false;
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
            return false;
        }

        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchCommand(string txt)
        {
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRuleName = new FilterRule("RouteName", txt.Trim());
            FilterRule filterRuleCode = new FilterRule("RouteNo", txt.Trim());
            filterGroup.Rules.Add(filterRuleName);
            filterGroup.Rules.Add(filterRuleCode);

            pageRepuestParams.FilterGroup = filterGroup;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }

        /// <summary>
        /// 自动生成路径库
        /// </summary>
        private void OnGenerateRouteCommand()
        {
            pageRepuestParams.FilterGroup = null;
            AddGenerat();
            //GenerategetPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }

        #region MarkPointInfo模型,用于列表数据显示
        private ObservableCollection<MarkPointInfoModel> markPointInfoList = new ObservableCollection<MarkPointInfoModel>();

        /// <summary>
        /// MarkPointInfo信息数据
        /// </summary>
        public ObservableCollection<MarkPointInfoModel> MarkPointInfoList
        {
            get { return markPointInfoList; }
            set { Set(ref markPointInfoList, value); }
        }
        #endregion

        /// <summary>
        /// 添加路径信息数据
        /// </summary>
        private void AddGenerat()
        {
            GeneratBtnEnable = false;
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            Task<OperationResult> result = Utility.Http.HttpClientHelper.AsyncPostResponse<OperationResult>(GlobalData.ServerRootUri + "RouteInfo/Generate",
               Utility.JsonHelper.ToJson(new List<RouteInfoModel> { RouteInfo }));
            Application.Current.Resources["UiMessage"] = "路径数据生成中,请等待.......................";
            result.GetAwaiter().UnsafeOnCompleted(() =>
            {
                if (!Equals(result, null) && result.Result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Result.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    if (!Equals(RouteInfo, null))
                    {
                        Messenger.Default.Send<RouteInfoModel>(RouteInfo, MessengerToken.DataChanged);
                    }
                    Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
                    OnExecuteRefreshCommand();
                    GeneratBtnEnable = true;
                }
                else
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = result?.Result.Message ?? "操作失败，请联系管理员！";
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    GeneratBtnEnable = true;
                }
            });

        }

        /// <summary>
        /// 取得自动生成路径库分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void GenerategetPageData(int pageIndex, int pageSize)
        {

            //AddGenerat();
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<RouteInfoModel>>>(GlobalData.ServerRootUri + "RouteInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));
            //Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "RouteInfo/Add",Utility.JsonHelper.ToJson(new List<RouteInfoModel> { RouteInfoModel }));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取路径信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("路径信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    RouteInfoList = new ObservableCollection<RouteInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    RouteInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                RouteInfoList = new ObservableCollection<RouteInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询路径信息失败，请联系管理员！";
            }
        }

        #endregion

        /// <summary>
        /// 清理资源
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();

            unRegisterMessenger();
        }
    }
}
