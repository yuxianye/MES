using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.MaterialStorageLogInfo.Model;
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

namespace Solution.Desktop.MaterialStorageLogInfo.ViewModel
{
    /// <summary>
    /// 库存流水信息Vm
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MaterialStorageLogInfoViewModel : PageableViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialStorageLogInfoViewModel()
        {
            initCommand();
            registerMessenger();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            //
            //右键快捷菜单
            MenuItems = new List<MenuItem>()
            {
                //new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Add_32x32.png"),Header =ResourceHelper.FindResource ("New"),Command =AddCommand},
                //new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png") ,Header=ResourceHelper.FindResource ("Edit"),Command =EditCommand},
                //new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Del_32x32.png") ,Header=ResourceHelper.FindResource ("PhysicalDelete"),Command =PhysicalDeleteCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Refresh_32x32.png") ,Header=ResourceHelper.FindResource ("Refresh"),Command =RefreshCommand},
                //new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Import_32x32.png" ),Header=ResourceHelper.FindResource ("Import"),Command =ImportCommand},
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
        /// 打印命令
        /// </summary>
        public ICommand PrintCommand { get; set; }

        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand SearchCommand { get; set; }

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            //AddCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAddCommand, OnCanExecuteAddCommand);
            //EditCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteEditCommand, OnCanExecuteEditCommand);
            //PhysicalDeleteCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePhysicalDeleteCommand, OnCanExecutePhysicalDeleteCommand);
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            //ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand, OnCanExecuteImportCommand);
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
            Messenger.Default.Register<MaterialStorageLogInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            //Messenger.Default.Unregister<ViewInfo>(this, Model.MessengerToken.Navigate, Navigate);
            //Messenger.Default.Unregister<object>(this, Model.MessengerToken.ClosePopup, ClosePopup);
            //Messenger.Default.Unregister<MenuInfo>(this, Model.MessengerToken.SetMenuStatus, SetMenuStatus);
            //
            Messenger.Default.Unregister<MaterialStorageLogInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }
        #endregion

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(MaterialStorageLogInfoModel MaterialStorageLogInfoChange)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = MaterialStorageLogInfoList.FirstOrDefault(a => a.Id == MaterialStorageLogInfoChange.Id);
            this.MaterialStorageLogInfo = MaterialStorageLogInfoList.FirstOrDefault();
        }

        #region 模型
        /// <summary>
        /// 库存流水信息模型
        /// </summary>
        private MaterialStorageLogInfoModel matwarehousetypeModel;
        /// <summary>
        /// 库存流水信息模型
        /// </summary>
        public MaterialStorageLogInfoModel MaterialStorageLogInfo
        {
            get { return matwarehousetypeModel; }
            set { Set(ref matwarehousetypeModel, value); }
        }
        #endregion

        #region 库存流水信息模型,用于列表数据显示
        private ObservableCollection<MaterialStorageLogInfoModel> matwarehousetypeModelList = new ObservableCollection<MaterialStorageLogInfoModel>();

        /// <summary>
        /// 库存流水信息数据
        /// </summary>
        public ObservableCollection<MaterialStorageLogInfoModel> MaterialStorageLogInfoList
        {
            get { return matwarehousetypeModelList; }
            set { Set(ref matwarehousetypeModelList, value); }
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
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialStorageLogInfoModel>>>
                (GlobalData.ServerRootUri + "MaterialStorageLogInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取库存流水信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("库存流水信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    MaterialStorageLogInfoList = new ObservableCollection<MaterialStorageLogInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialStorageLogInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialStorageLogInfoList = new ObservableCollection<MaterialStorageLogInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询库存流水信息失败，请联系管理员！";
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialStorageLogInfo" && a.Action == "Add");
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "MaterialStorageLogInfo" && a.Action == "Add"))
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialStorageLogInfo" && a.Action == "Update");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            if (!Equals(viewInfo, null))
            {
                viewInfo.Parameter = this.MaterialStorageLogInfo.Clone();
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "MaterialStorageLogInfo" && a.Action == "Update"))
                {
                    if (Equals(MaterialStorageLogInfo, null))
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
                , string.Format("确定要从磁盘中删除【{0}】这条数据么？删除后无法恢复。" + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", MaterialStorageLogInfo.Id)
                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            if (dialogResult == MessageDialogResult.Affirmative)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>
                    (GlobalData.ServerRootUri + "MaterialStorageLogInfo/Remove", Utility.JsonHelper.ToJson(new List<Guid>() { MaterialStorageLogInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    MaterialStorageLogInfo = null;
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "MaterialStorageLogInfo" && a.Action == "Remove"))
                {
                    if (Equals(MaterialStorageLogInfo, null))
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
            //FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            //FilterRule filterRuleCode = new FilterRule("BatchCode", txt.Trim(), FilterOperate.Contains);
            //filterGroup.Rules.Add(filterRuleCode);
            ////
            //pageRepuestParams.FilterGroup = filterGroup;
            //getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
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
