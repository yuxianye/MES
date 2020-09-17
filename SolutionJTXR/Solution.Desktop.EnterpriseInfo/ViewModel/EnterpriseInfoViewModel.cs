﻿using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Solution.Utility.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Solution.Utility.Extensions;
using System.Data;
using Solution.Utility.Npoi;
using System.Threading.Tasks;
using System.IO;
//using System.Windows.Forms;

namespace Solution.Desktop.EnterpriseInfo.ViewModel
{
    /// <summary>
    /// 企业信息Vm
    /// </summary>
    public class EnterpriseInfoViewModel : PageableViewModelBase//《注意：模块主VM与增加和编辑VM继承的基类不同》
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EnterpriseInfoViewModel()
        {
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
            ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand);
            ExportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteExportCommand);
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
            Messenger.Default.Register<EnterpriseModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<EnterpriseModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(EnterpriseModel enterpriseModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = EnterpriseInfoList.FirstOrDefault(a => a.Id == enterpriseModel.Id);
            this.EnterpriseInfo = EnterpriseInfoList.FirstOrDefault();
        }

        #endregion

        #region 模型
        /// <summary>
        /// 企业信息模型
        /// </summary>
        private EnterpriseModel enterpriseInfo;

        /// <summary>
        /// 企业信息模型
        /// </summary>
        public EnterpriseModel EnterpriseInfo
        {
            get { return enterpriseInfo; }
            set { Set(ref enterpriseInfo, value); }
        }
        #endregion

        #region 企业信息模型,用于列表数据显示
        private ObservableCollection<EnterpriseModel> enterpriseInfoList = new ObservableCollection<EnterpriseModel>();

        /// <summary>
        /// 企业信息数据
        /// </summary>
        public ObservableCollection<EnterpriseModel> EnterpriseInfoList
        {
            get { return enterpriseInfoList; }
            set { Set(ref enterpriseInfoList, value); }
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

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EnterpriseModel>>>
                (GlobalData.ServerRootUri + "EnterpriseInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取企业信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("企业信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    EnterpriseInfoList = new ObservableCollection<EnterpriseModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EnterpriseInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询企业信息失败，请联系管理员！";
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "EnterpriseInfo" && a.Action == "AddEnterprises");
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EnterpriseInfo" && a.Action == "Add"))
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "EnterpriseInfo" && a.Action == "EditEnterprises");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            if (!Equals(viewInfo, null))
            {
                viewInfo.Parameter = this.EnterpriseInfo.Clone();
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EnterpriseInfo" && a.Action == "Update"))
                {
                    if (Equals(EnterpriseInfo, null))
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
        //        + System.Environment.NewLine + "确认删除点击【是】，取消删除点击【否】。", EnterpriseInfo.EnterpriseName)
        //        , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
        //    if (dialogResult == MessageDialogResult.Affirmative)
        //    {
        //        var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>
        //            (GlobalData.ServerRootUri + "EnterpriseInfo/Recycle", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
        //        if (!Equals(result, null) && result.Successed)
        //        {
        //            Application.Current.Resources["UiMessage"] = result?.Message;
        //            LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        //            EnterpriseInfo = null;
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
        //        if (base.MenuModule.Functions.Any(a => a.Controller == "EnterpriseInfo" && a.Action == "Recycle"))
        //        {
        //            if (Equals(EnterpriseInfo, null))
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
        private void OnExecutePhysicalDeleteCommand()
        {
            if (MessageBox.Show("是否删除该企业数据？", "数据删除", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>
                    (GlobalData.ServerRootUri + "EnterpriseInfo/Remove", Utility.JsonHelper.ToJson(new List<Guid>() { EnterpriseInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    EnterpriseInfo = null;
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "EnterpriseInfo" && a.Action == "Remove"))
                {
                    if (Equals(EnterpriseInfo, null))
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
        //          + System.Environment.NewLine + "确认恢复点击【是】，取消恢复点击【否】。", EnterpriseInfo.EnterpriseName)
        //          , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
        //    if (dialogResult == MessageDialogResult.Affirmative)
        //    {
        //        var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>
        //            (GlobalData.ServerRootUri + "EnterpriseInfo/Restore", Utility.JsonHelper.ToJson(new List<EnterpriseModel>() { EnterpriseInfo }));
        //        if (!Equals(result, null) && result.Successed)
        //        {
        //            Application.Current.Resources["UiMessage"] = result?.Message;
        //            LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        //            EnterpriseInfo = null;
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
        //        if (base.MenuModule.Functions.Any(a => a.Controller == "EnterpriseInfo" && a.Action == "Restore"))
        //        {
        //            if (Equals(EnterpriseInfo, null))
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
        private async void OnExecuteImportCommand()
        {
            var window = (MetroWindow)Application.Current.MainWindow;
            var dialogResult = await window.ShowMessageAsync("数据导入"
                , "确定要导入企业信息数据到磁盘中吗？将会删除磁盘中原有数据"
                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            if (dialogResult == MessageDialogResult.Affirmative)
            {
                string filePath = "";
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
                {
                    filePath = openFileDialog.FileName;
                }
                if (string.IsNullOrEmpty(filePath))
                {
                    Application.Current.Resources["UiMessage"] = "数据导入失败，选择存储路径为空！";
                    return;
                }
                if (EnterpriseInfoList.Any())
                {
                    try
                    {
                        EnterpriseModel model = new EnterpriseModel();
                        Dictionary<String, Type> filedNameDc = model.GetFiledNameAndType();
                        DataTable dt = NPOIHelper.Import(filePath);
                        DataTable dtNew = new DataTable();
                        foreach (var kvp in filedNameDc)
                        {
                            dtNew.Columns.Add(kvp.Key, kvp.Value);
                        }
                        object[] values = new object[filedNameDc.Count];
                        foreach (DataRow row in dt.Rows)
                        {
                            for (int i = 0; i < dtNew.Columns.Count; i++)
                            {
                                values[i] = row[i];
                            }
                            dtNew.Rows.Add(values);
                        }
                        List<EnterpriseModel> enterpriseModels = dtNew.DataTableToList<EnterpriseModel>();
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Resources["UiMessage"] = "数据导入失败！" + ex.ToString();
                    }

                    if (File.Exists(filePath))
                    {
                        Application.Current.Resources["UiMessage"] = "数据导入已完成！";
                    }
                    else
                    {
                        Application.Current.Resources["UiMessage"] = "数据导入失败！";
                    }
                }
                else
                {
                    Application.Current.Resources["UiMessage"] = "数据导入失败,无数据！";
                }
            }
        }


        /// <summary>
        /// 执行导出命令
        /// </summary>
        private async void OnExecuteExportCommand()
        {
            var window = (MetroWindow)Application.Current.MainWindow;
            var dialogResult = await window.ShowMessageAsync("数据导出"
                , "确定要从磁盘中导出企业信息数据吗？"
                , MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "是", NegativeButtonText = "否" });
            if (dialogResult == MessageDialogResult.Affirmative)
            {
                string filePath = "";
                System.Windows.Forms.SaveFileDialog openFileDialog = new System.Windows.Forms.SaveFileDialog();
                openFileDialog.FileName = "企业信息" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".xls";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
                {
                    filePath = openFileDialog.FileName;
                }
                if (string.IsNullOrEmpty(filePath))
                {
                    Application.Current.Resources["UiMessage"] = "数据导出失败，选择存储路径为空！";
                    return;
                }
                if (EnterpriseInfoList.Any())
                {
                    try
                    {
                        EnterpriseModel model = new EnterpriseModel();
                        List<string> displayNameList = model.GetDisplayName();
                        DataTable dataTable = EnterpriseInfoList.ToDataTable<EnterpriseModel>(displayNameList);
                        //filePath += @"\企业信息" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".xls";
                        NPOIHelper.Export(dataTable, "企业信息", filePath);
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Resources["UiMessage"] = "数据导出失败！" + ex.ToString();
                    }

                    if (File.Exists(filePath))
                    {
                        Application.Current.Resources["UiMessage"] = "数据导出已完成！";
                    }
                    else
                    {
                        Application.Current.Resources["UiMessage"] = "数据导出失败！";
                    }
                }
                else
                {
                    Application.Current.Resources["UiMessage"] = "数据导出失败,无数据！";
                }
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
            return false;
        }

        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchCommand(string txt)
        {
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRuleName = new FilterRule("EnterpriseName", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleCode = new FilterRule("EnterpriseCode", txt.Trim(), FilterOperate.Contains);
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
