using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.MaterialOutStorageInfo.Model;
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

namespace Solution.Desktop.MaterialOutStorageInfo.ViewModel
{
    /// <summary>
    /// 物料出库信息Vm
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MaterialOutStorageInfoViewModel : PageableViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialOutStorageInfoViewModel()
        {
            initCommand();
            registerMessenger();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            //
            //右键快捷菜单
            MenuItems = new List<MenuItem>()
            {
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Add_32x32.png"),Header =ResourceHelper.FindResource ("MaterialOutStorageNew1"),Command =Add1Command},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Add_32x32.png"),Header =ResourceHelper.FindResource ("MaterialOutStorageNew2"),Command =Add2Command},
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
        //public ICommand AddCommand { get; set; }
        //
        public ICommand Add1Command { get; set; }
        public ICommand Add2Command { get; set; }

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
        /// 审核命令
        /// </summary>
        public ICommand AuditCommand { get; set; }

        /// <summary>
        /// 作业命令
        /// </summary>
        public ICommand DistributeCommand { get; set; }

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            //AddCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAddCommand, OnCanExecuteAddCommand);
            Add1Command = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAdd1Command, OnCanExecuteAddCommand);
            Add2Command = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAdd2Command, OnCanExecuteAddCommand);
            //
            EditCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteEditCommand, OnCanExecuteEditCommand);
            //
            PhysicalDeleteCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePhysicalDeleteCommand, OnCanExecutePhysicalDeleteCommand);
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand, OnCanExecuteImportCommand);
            ExportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteExportCommand, OnCanExecuteExportCommand);
            PrintCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePrintCommand, OnCanExecutePrintCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            AuditCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAuditCommand, OnCanExecuteAuditCommand);
            DistributeCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteDistributeCommand, OnCanExecuteDistributeCommand);
        }

        #endregion

        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<MaterialOutStorageInfoModel>(this, MessengerToken.DataChanged, dataChanged);
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
            Messenger.Default.Unregister<MaterialOutStorageInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }
        #endregion

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(MaterialOutStorageInfoModel MaterialOutStorageInfoChange)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = MaterialOutStorageInfoList.FirstOrDefault(a => a.Id == MaterialOutStorageInfoChange.Id);
            this.MaterialOutStorageInfo = MaterialOutStorageInfoList.FirstOrDefault();           
        }

        #region 模型
        /// <summary>
        /// 物料出库信息模型
        /// </summary>
        private MaterialOutStorageInfoModel matwarehousetypeModel;
        /// <summary>
        /// 物料出库信息模型
        /// </summary>
        public MaterialOutStorageInfoModel MaterialOutStorageInfo
        {
            get { return matwarehousetypeModel; }
            set { Set(ref matwarehousetypeModel, value); }
        }
        #endregion

        #region 物料出库信息模型,用于列表数据显示
        private ObservableCollection<MaterialOutStorageInfoModel> materialoutstorageModelList = new ObservableCollection<MaterialOutStorageInfoModel>();

        /// <summary>
        /// 物料出库信息数据
        /// </summary>
        public ObservableCollection<MaterialOutStorageInfoModel> MaterialOutStorageInfoList
        {
            get { return materialoutstorageModelList; }
            set { Set(ref materialoutstorageModelList, value); }
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

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialOutStorageInfoModel>>>
                (GlobalData.ServerRootUri + "MaterialOutStorageInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取物料出库信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("物料出库信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    MaterialOutStorageInfoList = new ObservableCollection<MaterialOutStorageInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialOutStorageInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialOutStorageInfoList = new ObservableCollection<MaterialOutStorageInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询物料出库信息失败，请联系管理员！";
            }
        }

        #endregion

        #region 命令和消息等执行函数

        /// <summary>
        /// 执行新建命令
        /// </summary>
        //private void OnExecuteAddCommand()
        //{
        //    //功能与页面的对应，Controller和Action 与配置文件对应。
        //    var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Add");
        //    Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
        //}

        private void OnExecuteAdd1Command()
        {
            //功能与页面的对应，Controller和Action 与配置文件对应。
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Add1");
            Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
        }

        private void OnExecuteAdd2Command()
        {
            //功能与页面的对应，Controller和Action 与配置文件对应。
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Add2");
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "MaterialOutStorageInfo" && (a.Action == "Add" ||
                                                                                                    a.Action == "Add1" ||
                                                                                                    a.Action == "Add2")))
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
            if (MaterialOutStorageInfo.OutStorageStatus != OutStorageStatusEnumModel.OutStorageStatus.待组盘)
            {
                Application.Current.Resources["UiMessage"] = "出库单状态为‘待组盘’的记录才能被修改，请联系管理员！";
            }
            else if (MaterialOutStorageInfo.AuditStatus == AuditStatusEnumModel.AuditStatus.审核通过)
            {
                Application.Current.Resources["UiMessage"] = "通过审核的记录不能被修改，请联系管理员！";
            }
            else
            {
                //Controller和Action 与配置文件对应
                MenuFunctionViewInfoMap menuFunctionViewInfoMap = new MenuFunctionViewInfoMap();
                //
                if (MaterialOutStorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.空托盘出库)
                {
                    menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Update1");
                }
                else if (MaterialOutStorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.成品手动出库)
                {
                    menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Update2");
                }
                //                
                var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
                if (!Equals(viewInfo, null))
                {
                    viewInfo.Parameter = this.MaterialOutStorageInfo.Clone();
                }
                Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
            }
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "MaterialOutStorageInfo" && (a.Action == "Update" ||
                                                                                                    a.Action == "Update1" ||
                                                                                                    a.Action == "Update2")))
                {
                    //if (Equals(MaterialOutStorageInfo, null))
                    if (Equals(MaterialOutStorageInfo, null) || MaterialOutStorageInfo.AuditStatus == AuditStatusEnumModel.AuditStatus.审核通过)
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
        /// 执行审核命令
        /// </summary>
        private void OnExecuteAuditCommand()
        {
            //Controller和Action 与配置文件对应
            MenuFunctionViewInfoMap menuFunctionViewInfoMap = new MenuFunctionViewInfoMap();
            //
            if (MaterialOutStorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.空托盘出库)
            {
                menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Audit1");
            }
            else if (MaterialOutStorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.成品手动出库)
            {
                menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Audit2");
            }
            //
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            if (!Equals(viewInfo, null))
            {
                viewInfo.Parameter = this.MaterialOutStorageInfo.Clone();
            }
            Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行审核命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteAuditCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                // 与服务端对应
                if (base.MenuModule.Functions.Any(a => a.Controller == "MaterialOutStorageInfo" && (a.Action == "Audit" ||
                                                                                                    a.Action == "Audit1" ||
                                                                                                    a.Action == "Audit2")))
                {
                    if (Equals(MaterialOutStorageInfo, null) || MaterialOutStorageInfo.AuditStatus == AuditStatusEnumModel.AuditStatus.审核通过 )
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
                , string.Format("确定要从磁盘中删除【{0}】这条数据么？删除后无法恢复。" + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", MaterialOutStorageInfo.OutStorageBillCode)
                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            if (dialogResult == MessageDialogResult.Affirmative)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>
                    (GlobalData.ServerRootUri + "MaterialOutStorageInfo/Remove", Utility.JsonHelper.ToJson(new List<Guid>() { MaterialOutStorageInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    MaterialOutStorageInfo = null;
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Remove"))
                {
                    //if (Equals(MaterialOutStorageInfo, null))
                    if (Equals(MaterialOutStorageInfo, null) || MaterialOutStorageInfo.AuditStatus == AuditStatusEnumModel.AuditStatus.审核通过)
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

        #region 命令和消息等执行函数

        /// <summary>
        /// 执行作业命令
        /// </summary>
        private void OnExecuteDistributeCommand()
        {
            if (MaterialOutStorageInfo.OutStorageStatus != OutStorageStatusEnumModel.OutStorageStatus.待组盘)
            {
                Application.Current.Resources["UiMessage"] = "出库单状态为‘待组盘’的记录才能被配盘下达，请联系管理员！";
            }
            else if (MaterialOutStorageInfo.AuditStatus != AuditStatusEnumModel.AuditStatus.审核通过)
            {
                Application.Current.Resources["UiMessage"] = "通过审核的记录才能被配盘下达，请联系管理员！";
            }
            else
            {
                //Controller和Action 与配置文件对应
                var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Distribute");
                var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
                if (!Equals(viewInfo, null))
                {
                    viewInfo.Parameter = this.MaterialOutStorageInfo.Clone();
                }
                Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
            }
        }

        /// <summary>
        /// 是否可以执行作业命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteDistributeCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                if (base.MenuModule.Functions.Any(a => a.Controller == "MaterialOutStorageInfo" && a.Action == "Distribute"))
                {
                    //if (Equals(MaterialOutStorageInfo, null))
                    if (Equals(MaterialOutStorageInfo, null) || MaterialOutStorageInfo.AuditStatus != AuditStatusEnumModel.AuditStatus.审核通过
                                                             || MaterialOutStorageInfo.OutStorageStatus != OutStorageStatusEnumModel.OutStorageStatus.待组盘)
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

        #endregion

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
            FilterRule filterRuleCode = new FilterRule("OutStorageBillCode", txt.Trim(), FilterOperate.Contains);
            filterGroup.Rules.Add(filterRuleCode);
            //
            pageRepuestParams.FilterGroup = filterGroup;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
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