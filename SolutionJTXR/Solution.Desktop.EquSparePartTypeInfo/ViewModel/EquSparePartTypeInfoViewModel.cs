using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.EquSparePartTypeInfo.Model;
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

namespace Solution.Desktop.EquSparePartTypeInfo.ViewModel
{
    /// <summary>
    /// 备件类别信息Vm
    /// </summary>
    public class EquSparePartTypeInfoViewModel : PageableViewModelBase//《注意：模块主VM与增加和编辑VM继承的基类不同》
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquSparePartTypeInfoViewModel()
        {
            initCommand();
            registerMessenger();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
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

        ///// <summary>
        ///// 逻辑还原命令
        ///// </summary>
        //public ICommand LogicalRestoreCommand { get; set; }

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
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            AddCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAddCommand, OnCanExecuteAddCommand);
            EditCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteEditCommand, OnCanExecuteEditCommand);
            //RecycleCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRecycleCommand, OnCanExecuteRecycleCommand);
            PhysicalDeleteCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePhysicalDeleteCommand, OnCanExecutePhysicalDeleteCommand);
            //LogicalRestoreCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteLogicalRestoreCommand, OnCanExecuteLogicalRestoreCommand);
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand, OnCanExecuteImportCommand);
            ExportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteExportCommand, OnCanExecuteExportCommand);
            PrintCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePrintCommand, OnCanExecutePrintCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
        }

        #endregion

        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<EquSparePartTypeModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<EquSparePartTypeModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(EquSparePartTypeModel EquSparePartTypeModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = EquSparePartTypeInfoList.FirstOrDefault(a => a.Id == EquSparePartTypeModel.Id);
            this.EquSparePartTypeInfo = EquSparePartTypeInfoList.FirstOrDefault();
        }

        #endregion

        #region 模型
        /// <summary>
        /// 备件类别信息模型
        /// </summary>
        private EquSparePartTypeModel equspareparttypeInfo;

        /// <summary>
        /// 备件类别信息模型
        /// </summary>
        public EquSparePartTypeModel EquSparePartTypeInfo
        {
            get { return equspareparttypeInfo; }
            set { Set(ref equspareparttypeInfo, value); }
        }
        #endregion

        #region 备件类别信息模型,用于列表数据显示
        private ObservableCollection<EquSparePartTypeModel> equspareparttypeInfoList = new ObservableCollection<EquSparePartTypeModel>();

        /// <summary>
        /// 备件类别信息数据
        /// </summary>
        public ObservableCollection<EquSparePartTypeModel> EquSparePartTypeInfoList
        {
            get { return equspareparttypeInfoList; }
            set { Set(ref equspareparttypeInfoList, value); }
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
            //pageRepuestParams.SortField = "LastUpdatedTime";
            //pageRepuestParams.SortOrder = "desc";
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquSparePartTypeModel>>>
                (GlobalData.ServerRootUri + "EquSparePartTypeInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取备件类别信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("备件类别信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                if (result.Data.Data.Any())
                {
                    EquSparePartTypeInfoList = new ObservableCollection<EquSparePartTypeModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EquSparePartTypeInfoList?.Clear();
                    TotalCounts = 0;
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquSparePartTypeInfoList = new ObservableCollection<EquSparePartTypeModel>();
                UiMessage = result?.Message ?? "查询备件类别信息失败，请联系管理员！";
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "EquSparePartTypeInfo" && a.Action == "Add");
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EquSparePartTypeInfo" && a.Action == "Add"))
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "EquSparePartTypeInfo" && a.Action == "Update");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            if (!Equals(viewInfo, null))
            {
                viewInfo.Parameter = this.EquSparePartTypeInfo.Clone();
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EquSparePartTypeInfo" && a.Action == "Update"))
                {
                    if (Equals(EquSparePartTypeInfo, null))
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

        ///// <summary>
        ///// 执行逻辑删除命令
        ///// </summary>
        //private async void OnExecuteRecycleCommand()
        //{
        //    var window = (MetroWindow)Application.Current.MainWindow;
        //    var dialogResult = await window.ShowMessageAsync("数据逻辑删除"
        //        , string.Format("确定要删除【{0}】这条数据么？删除后可在管理界面恢复。"
        //        + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", EquSparePartTypeInfo.EnterpriseName)
        //        , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
        //    if (dialogResult == MessageDialogResult.Affirmative)
        //    {
        //        var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>
        //            (GlobalData.ServerRootUri + "EquSparePartTypeInfo/Recycle", Utility.JsonHelper.ToJson(new List<EquSparePartTypeModel>() { EquSparePartTypeInfo }));
        //        if (!Equals(result, null) && result.Successed)
        //        {
        //            Application.Current.Resources["UiMessage"] = result?.Message;
        //            LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        //            EquSparePartTypeInfo = null;
        //        }
        //        else
        //        {
        //            //操作失败，显示错误信息
        //            Application.Current.Resources["UiMessage"] = result?.Message ?? "数据逻辑删除失败，请联系管理员！";
        //            LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //        }
        //    }

        //}

        ///// <summary>
        ///// 是否可以执行逻辑删除命令
        ///// </summary>
        ///// <returns></returns>
        //private bool OnCanExecuteRecycleCommand()
        //{
        //    if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (base.MenuModule.Functions.Any(a => a.Controller == "EquSparePartTypeInfo" && a.Action == "Recycle"))
        //        {
        //            if (Equals(EquSparePartTypeInfo, null))
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        /// <summary>
        /// 执行物理删除命令
        /// </summary>
        private async void OnExecutePhysicalDeleteCommand()
        {
            var window = (MetroWindow)Application.Current.MainWindow;
            var dialogResult = await window.ShowMessageAsync("数据删除"
                , string.Format("确定要从磁盘中删除【{0}】这条数据么？删除后无法恢复。" + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", EquSparePartTypeInfo.EquSparePartTypeName)
                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            if (dialogResult == MessageDialogResult.Affirmative)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquSparePartTypeInfo/Remove", Utility.JsonHelper.ToJson(new List<Guid>() { EquSparePartTypeInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    UiMessage = result?.Message;
                    LogHelper.Info(UiMessage);
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    EquSparePartTypeInfo = null;
                }
                else
                {
                    //操作失败，显示错误信息
                    UiMessage = result?.Message ?? "数据删除失败，请联系管理员！";
                    LogHelper.Info(UiMessage);
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EquSparePartTypeInfo" && a.Action == "Remove"))
                {
                    if (Equals(EquSparePartTypeInfo, null))
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


        ///// <summary>
        ///// 执行逻辑还原命令
        ///// </summary>
        //private async void OnExecuteLogicalRestoreCommand()
        //{
        //    var window = (MetroWindow)Application.Current.MainWindow;
        //    var dialogResult = await window.ShowMessageAsync("数据逻辑恢复"
        //          , string.Format("确定要恢复【{0}】这条数据么？"
        //          + System.Environment.NewLine + "确认恢复点击【是】，取消恢复点击【否】。", EquSparePartTypeInfo.EnterpriseName)
        //          , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
        //    if (dialogResult == MessageDialogResult.Affirmative)
        //    {
        //        var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>
        //            (GlobalData.ServerRootUri + "EquSparePartTypeInfo/Restore", Utility.JsonHelper.ToJson(new List<EquSparePartTypeModel>() { EquSparePartTypeInfo }));
        //        if (!Equals(result, null) && result.Successed)
        //        {
        //            Application.Current.Resources["UiMessage"] = result?.Message;
        //            LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        //            EquSparePartTypeInfo = null;
        //        }
        //        else
        //        {
        //            //操作失败，显示错误信息
        //            Application.Current.Resources["UiMessage"] = result?.Message ?? "数据恢复失败，请联系管理员！";
        //            LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //        }
        //    }

        //}

        ///// <summary>
        ///// 是否可以执行逻辑还原命令
        ///// </summary>
        ///// <returns></returns>
        //private bool OnCanExecuteLogicalRestoreCommand()
        //{
        //    if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (base.MenuModule.Functions.Any(a => a.Controller == "EquSparePartTypeInfo" && a.Action == "Restore"))
        //        {
        //            if (Equals(EquSparePartTypeInfo, null))
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return IsAdmin;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

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
            FilterRule filterRuleName = new FilterRule("EquSparePartTypeName", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleCode = new FilterRule("EquSparePartTypeCode", txt.Trim(), FilterOperate.Contains);
            filterGroup.Rules.Add(filterRuleName);
            filterGroup.Rules.Add(filterRuleCode);
            pageRepuestParams.FilterGroup = filterGroup;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
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
