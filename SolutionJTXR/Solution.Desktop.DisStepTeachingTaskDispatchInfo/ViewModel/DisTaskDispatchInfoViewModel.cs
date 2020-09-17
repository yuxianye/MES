using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.DisTaskDispatchInfo.Model;
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

namespace Solution.Desktop.DisTaskDispatchInfo.ViewModel
{
    public class DisTaskDispatchInfoViewModel : PageableViewModelBase
    {

        public DisTaskDispatchInfoViewModel()
        {
            //this.MenuModule
            initCommand();
            registerMessenger();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            MenuItems = new List<MenuItem>()
            {
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png"),Header =ResourceHelper.FindResource ("TaskOrderChecking"),Command =TaskOrderCheckingCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Refresh_32x32.png") ,Header=ResourceHelper.FindResource ("Refresh"),Command =RefreshCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Import_32x32.png" ),Header=ResourceHelper.FindResource ("Import"),Command =ImportCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Export_32x32.png") ,Header=ResourceHelper.FindResource ("Export"),Command =ExportCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Print_32x32.png") ,Header=ResourceHelper.FindResource ("Print"),Command =PrintCommand},
            };
        }

        #region 任务模型
        /// <summary>
        /// 任务模型
        /// </summary>
        private DisTaskDispatchInfoModel disTaskDispatchInfo;// = new EnterpriseModel();
        /// <summary>
        /// 任务模型
        /// </summary>
        public DisTaskDispatchInfoModel DisTaskDispatchInfo
        {
            get { return disTaskDispatchInfo; }
            set { Set(ref disTaskDispatchInfo, value); }
        }
        #endregion

        #region 任务模型,用于列表数据显示
        private ObservableCollection<DisTaskDispatchInfoModel> disTaskDispatchInfoList = new ObservableCollection<DisTaskDispatchInfoModel>();

        /// <summary>
        /// 任务数据
        /// </summary>
        public ObservableCollection<DisTaskDispatchInfoModel> DisTaskDispatchInfoList
        {
            get { return disTaskDispatchInfoList; }
            set { Set(ref disTaskDispatchInfoList, value); }
        }
        #endregion

        #region 命令定义和初始化

        /// <summary>
        /// 子订单排产命令
        /// </summary>
        public ICommand TaskOrderCheckingCommand { get; set; }

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
            TaskOrderCheckingCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteTaskOrderCheckingCommand, OnCanExecuteTaskOrderCheckingCommand);
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
            Messenger.Default.Register<DisTaskDispatchInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(DisTaskDispatchInfoModel orderitemInfoModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = DisTaskDispatchInfoList.FirstOrDefault(a => a.Id == orderitemInfoModel.Id);
            this.DisTaskDispatchInfo = DisTaskDispatchInfoList.FirstOrDefault();
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<DisTaskDispatchInfoModel>(this, MessengerToken.DataChanged, dataChanged);
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


            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<DisTaskDispatchInfoModel>>>(GlobalData.ServerRootUri + "DisTaskDispatchInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取任务信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("任务信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    DisTaskDispatchInfoList = new ObservableCollection<DisTaskDispatchInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    DisTaskDispatchInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                DisTaskDispatchInfoList = new ObservableCollection<DisTaskDispatchInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询任务信息失败，请联系管理员！";
            }
        }

        #endregion

        #region 命令和消息等执行函数

        /// <summary>
        /// 执行新建命令
        /// </summary>
        private void OnExecuteTaskOrderCheckingCommand()
        {

            //功能与页面的对应，Controller和Action 与配置文件对应。
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "DisTaskDispatchInfo" && a.Action == "GetDisTaskDispatchItemInfoListByInventoryID");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.DisTaskDispatchInfo.Clone();
            Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行新增命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteTaskOrderCheckingCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                //与服务端对应
                if (base.MenuModule.Functions.Any(a => a.Controller == "DisTaskDispatchInfo" && a.Action == "GetDisTaskDispatchItemInfoListByInventoryID"))
                {
                    if (Equals(DisTaskDispatchInfo, null))
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "DisTaskDispatchInfo" && a.Action == "Add"))
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
            if (Equals(DisTaskDispatchInfoList, null) || !DisTaskDispatchInfoList.Any())
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
            if (Equals(DisTaskDispatchInfoList, null) || !DisTaskDispatchInfoList.Any())
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
            FilterRule filterRuleName = new FilterRule("DisStepTeachingTaskType.TaskTypeName", txt.Trim(), "contains");
            FilterRule filterRuleCode = new FilterRule("TaskCode", txt.Trim(), "contains");
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
