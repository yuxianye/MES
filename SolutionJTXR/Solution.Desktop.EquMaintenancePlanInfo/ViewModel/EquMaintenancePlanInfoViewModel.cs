using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.EquMaintenancePlanInfo.Model;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Utility;
using Solution.Utility.Extensions;
using Solution.Utility.Npoi;
using Solution.Utility.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using MenuItem = System.Windows.Controls.MenuItem;

namespace Solution.Desktop.EquMaintenancePlanInfo.ViewModel
{
    public class EquMaintenancePlanInfoViewModel : PageableViewModelBase
    {
        public EquMaintenancePlanInfoViewModel()
        {
            //this.MenuModule
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

        #region 设备维修计划信息
        private EquMaintenancePlanInfoModel equMaintenancePlanInfo;

        /// <summary>
        /// 设备维修计划信息
        /// </summary>
        public EquMaintenancePlanInfoModel EquMaintenancePlanInfo
        {
            get { return equMaintenancePlanInfo; }
            set
            {
                Set(ref equMaintenancePlanInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        #endregion

        #region 设备维修计划信息模型,用于列表数据显示
        private ObservableCollection<EquMaintenancePlanInfoModel> equMaintenancePlanList = new ObservableCollection<EquMaintenancePlanInfoModel>();

        /// <summary>
        /// 设备维修计划信息模型,用于列表数据显示
        /// </summary>
        public ObservableCollection<EquMaintenancePlanInfoModel> EquMaintenancePlanList
        {
            get { return equMaintenancePlanList; }
            set { Set(ref equMaintenancePlanList, value); }
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
        /// 打印命令
        /// </summary>
        public ICommand PrintCommand { get; set; }

        /// <summary>
        /// 搜索命令
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
            Messenger.Default.Register<EquMaintenancePlanInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(EquMaintenancePlanInfoModel equMaintenancePlanInfoModelModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = EquMaintenancePlanList.FirstOrDefault(a => a.Id == equMaintenancePlanInfoModelModel.Id);
            this.equMaintenancePlanInfo = EquMaintenancePlanList.FirstOrDefault();
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<EquMaintenancePlanInfoModel>(MessengerToken.DataChanged, dataChanged);
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

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquMaintenancePlanInfoModel>>>(GlobalData.ServerRootUri + "EquMaintenancePlanInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取设备维护计划信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("设备维护计划信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage.ToString());
                if (result.Data.Data.Any())
                {
                    EquMaintenancePlanList = new ObservableCollection<EquMaintenancePlanInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EquMaintenancePlanList?.Clear();
                    TotalCounts = 0;
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquMaintenancePlanList = new ObservableCollection<EquMaintenancePlanInfoModel>();
                UiMessage = result?.Message ?? "查询设备信息失败，请联系管理员！"; 
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "EquMaintenancePlanInfo" && a.Action == "Add");
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EquMaintenancePlanInfo" && a.Action == "Add"))
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "EquMaintenancePlanInfo" && a.Action == "Update");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.EquMaintenancePlanInfo.Clone();
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EquMaintenancePlanInfo" && a.Action == "Update"))
                {
                    if (Equals(EquMaintenancePlanInfo, null))
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
        //private async void OnExecuteRecycleCommand()
        //{
        //    var window = (MetroWindow)Application.Current.MainWindow;
        //    var dialogResult = await window.ShowMessageAsync("数据删除"
        //        , string.Format("确定要删除【{0}】这条数据么？删除后可在管理界面恢复。" + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", EquipmentTypeInfo.EquipmentTypeName)
        //        , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
        //    if (dialogResult == MessageDialogResult.Affirmative)
        //    {
        //        var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquipmentTypeInfo/Recycle", Utility.JsonHelper.ToJson(new List<EquipmentTypeModel>() { EquipmentTypeInfo }));
        //        if (!Equals(result, null) && result.Successed)
        //        {
        //            UiMessage = result?.Message;
        //            LogHelper.Info(UiMessage.ToString());
        //            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        //            EquipmentTypeInfo = null;
        //        }
        //        else
        //        {
        //            //操作失败，显示错误信息
        //            UiMessage = result?.Message ?? "数据删除失败，请联系管理员！";
        //            LogHelper.Info(UiMessage.ToString());
        //        }
        //    }

        //}

        /// <summary>
        /// 是否可以执行逻辑删除命令
        /// </summary>
        /// <returns></returns>
        //private bool OnCanExecuteRecycleCommand()
        //{
        //    if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (base.MenuModule.Functions.Any(a => a.Controller == "EquipmentTypeInfo" && a.Action == "Recycle"))
        //        {
        //            if (Equals(EquipmentTypeInfo, null))
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
                , string.Format("确定要从磁盘中删除【{0}】这条数据么？删除后无法恢复。" + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", EquMaintenancePlanInfo.MaintenanceCode)
                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            if (dialogResult == MessageDialogResult.Affirmative)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquMaintenancePlanInfo/Remove", Utility.JsonHelper.ToJson(new List<Guid>() { EquMaintenancePlanInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    //Application.Current.Resources["UiMessage"] = result?.Message;
                    //LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    UiMessage = result?.Message;
                    LogHelper.Info(UiMessage);
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    EquMaintenancePlanInfo = null;
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EquMaintenancePlanInfo" && a.Action == "Remove"))
                {
                    if (Equals(EquMaintenancePlanInfo, null))
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
        private /*async*/ void OnExecuteImportCommand()
        {
            //var window = (MetroWindow)System.Windows.Application.Current.MainWindow;
            //var dialogResult = await window.ShowMessageAsync("数据导入"
            //    , "确定要导入企业信息数据到磁盘中吗？将会删除磁盘中原有数据"
            //    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            //if (dialogResult == MessageDialogResult.Affirmative)
            //{
            //    string filePath = "";
            //    OpenFileDialog openFileDialog = new OpenFileDialog();
            //    if (openFileDialog.ShowDialog() == DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            //    {
            //        filePath = openFileDialog.FileName;
            //    }
            //    if (string.IsNullOrEmpty(filePath))
            //    {
            //        UiMessage = "数据导入失败，选择存储路径为空！";
            //        return;
            //    }
            //    if (EquMaintenancePlanList.Any())
            //    {
            //        try
            //        {
            //            EquipmentTypeModel model = new EquipmentTypeModel();
            //            Dictionary<String, Type> filedNameDc = model.GetFiledNameAndType();
            //            DataTable dt = NPOIHelper.Import(filePath);
            //            DataTable dtNew = new DataTable();
            //            foreach (var kvp in filedNameDc)
            //            {
            //                dtNew.Columns.Add(kvp.Key, kvp.Value);
            //            }
            //            object[] values = new object[filedNameDc.Count];
            //            foreach (DataRow row in dt.Rows)
            //            {
            //                for (int i = 0; i < dtNew.Columns.Count; i++)
            //                {
            //                    values[i] = row[i];
            //                }
            //                dtNew.Rows.Add(values);
            //            }
            //            List<EquipmentTypeModel> enterpriseModels = dtNew.DataTableToList<EquipmentTypeModel>();
            //            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquipmentTypeInfo/Remove", Utility.JsonHelper.ToJson(EquipmentTypeInfoList.Select(x => x.Id).ToList()));
            //            if (!Equals(result, null) && result.Successed)
            //            {
            //                var addResult = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquipmentTypeInfo/Add",Utility.JsonHelper.ToJson(enterpriseModels));
            //                await window.ShowMessageAsync("数据导入"
            //                    , "数据导入已完成！"
            //                    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //                getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //                EquipmentTypeInfo = null;
            //            }
            //            else
            //            {
            //                //操作失败，显示错误信息
            //                UiMessage = "数据删除失败，请联系管理员！";
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            await window.ShowMessageAsync("数据导入"
            //                    , "数据导入失败！"
            //                    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //        }

            //        //if (File.Exists(filePath))
            //        //{
            //        //    await window.ShowMessageAsync("数据导入"
            //        //        , "数据导入已完成！"
            //        //        , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //        //}
            //        //else
            //        //{
            //        //    await window.ShowMessageAsync("数据导入"
            //        //            , "数据导入失败！"
            //        //            , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //        //}
            //    }
            //    else
            //    {
            //        await window.ShowMessageAsync("数据导入"
            //                    , "数据导入失败,无数据！"
            //                    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EquMaintenancePlanInfo" && a.Action == "Add"))
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
        private /*async*/ void OnExecuteExportCommand()
        {
            //var window = (MetroWindow)Application.Current.MainWindow;
            //var dialogResult = await window.ShowMessageAsync("数据导出"
            //    , "确定要从磁盘中导出企业信息数据吗？"
            //    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            //if (dialogResult == MessageDialogResult.Affirmative)
            //{
            //    string filePath = "";
            //    System.Windows.Forms.SaveFileDialog openFileDialog = new System.Windows.Forms.SaveFileDialog();
            //    openFileDialog.FileName = "设备类型信息" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".xls";
            //    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            //    {
            //        filePath = openFileDialog.FileName;
            //    }
            //    if (string.IsNullOrEmpty(filePath))
            //    {
            //        await window.ShowMessageAsync("数据导出"
            //                    , "数据导出失败，选择存储路径为空！"
            //                    , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //        return;
            //    }
            //    if (EquipmentTypeInfoList.Any())
            //    {
            //        try
            //        {
            //            EquipmentTypeModel model = new EquipmentTypeModel();
            //            List<string> displayNameList = model.GetDisplayName();
            //            DataTable dataTable = EquipmentTypeInfoList.ToDataTable<EquipmentTypeModel>(displayNameList);
            //            NPOIHelper.Export(dataTable, "企业信息", filePath);
            //        }
            //        catch (Exception ex)
            //        {
            //            await window.ShowMessageAsync("数据导出"
            //                , "数据导出失败！"
            //                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //        }

            //        if (File.Exists(filePath))
            //        {
            //            await window.ShowMessageAsync("数据导出"
            //                , "数据导出已完成！"
            //                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //        }
            //        else
            //        {
            //            await window.ShowMessageAsync("数据导出"
            //                , "数据导出失败！"
            //                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //        }
            //    }
            //    else
            //    {
            //        await window.ShowMessageAsync("数据导出"
            //                , "数据导出失败,无数据！"
            //                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //    }
            //}
        }

        /// <summary>
        /// 是否可以执行导出命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteExportCommand()
        {
            if (Equals(EquMaintenancePlanList, null) || !EquMaintenancePlanList.Any())
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
            //        UiMessage = result?.Message;
            //        LogHelper.Info(UiMessage.ToString());
            //        getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //        EnterpriseInfo = null;
            //    }
            //    else
            //    {
            //        //操作失败，显示错误信息
            //        UiMessage = result?.Message ?? "数据删除失败，请联系管理员！";
            //        LogHelper.Info(UiMessage.ToString());
            //    }
            //}

        }

        /// <summary>
        /// 是否可以执行打印命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecutePrintCommand()
        {
            if (Equals(EquMaintenancePlanList, null) || !EquMaintenancePlanList.Any())
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
            FilterRule filterRuleName = new FilterRule("MaintenanceContent", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleCode = new FilterRule("MaintenanceCode", txt.Trim(), FilterOperate.Contains);
            filterGroup.Rules.Add(filterRuleName);
            filterGroup.Rules.Add(filterRuleCode);

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
