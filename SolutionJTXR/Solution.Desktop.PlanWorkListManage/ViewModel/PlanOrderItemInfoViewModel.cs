using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.PlanOrderManage.Model;
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

namespace Solution.Desktop.PlanWorkListManage.ViewModel
{
    public class PlanOrderItemInfoViewModel : PageableViewModelBase
    {

        public PlanOrderItemInfoViewModel()
        {
            //this.MenuModule
            initCommand();
            registerMessenger();
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            MenuItems = new List<MenuItem>()
            {
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png"),Header =ResourceHelper.FindResource ("ScheduleList"),Command =ScheduleCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Refresh_32x32.png") ,Header=ResourceHelper.FindResource ("Refresh"),Command =RefreshCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Import_32x32.png" ),Header=ResourceHelper.FindResource ("Import"),Command =ImportCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Export_32x32.png") ,Header=ResourceHelper.FindResource ("Export"),Command =ExportCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Print_32x32.png") ,Header=ResourceHelper.FindResource ("Print"),Command =PrintCommand},
            };
        }

        #region 子订单模型
        /// <summary>
        /// 子订单模型
        /// </summary>
        private PlanOrderItemInfoModel planOrderItemInfo;// = new EnterpriseModel();
        /// <summary>
        /// 子订单模型
        /// </summary>
        public PlanOrderItemInfoModel PlanOrderItemInfo
        {
            get { return planOrderItemInfo; }
            set { Set(ref planOrderItemInfo, value); }
        }
        #endregion

        #region 子订单信息模型,用于列表数据显示
        private ObservableCollection<PlanOrderItemInfoModel> planOrderItemInfoList = new ObservableCollection<PlanOrderItemInfoModel>();

        /// <summary>
        /// 子订单信息数据
        /// </summary>
        public ObservableCollection<PlanOrderItemInfoModel> PlanOrderItemInfoList
        {
            get { return planOrderItemInfoList; }
            set { Set(ref planOrderItemInfoList, value); }
        }
        #endregion

        #region 命令定义和初始化

        /// <summary>
        /// 子订单排产命令
        /// </summary>
        public ICommand ScheduleCommand { get; set; }

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
            ScheduleCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteScheduleCommand, OnCanExecuteScheduleCommand);
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
            Messenger.Default.Register<PlanOrderItemInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(PlanOrderItemInfoModel orderitemInfoModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = PlanOrderItemInfoList.FirstOrDefault(a => a.Id == orderitemInfoModel.Id);
            this.PlanOrderItemInfo = PlanOrderItemInfoList.FirstOrDefault();
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<PlanOrderItemInfoModel>(this, MessengerToken.DataChanged, dataChanged);
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

            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<PlanOrderItemInfoModel>>>(GlobalData.ServerRootUri + "PlanOrderItemInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取子订单信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("子订单信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    PlanOrderItemInfoList = new ObservableCollection<PlanOrderItemInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    PlanOrderItemInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                PlanOrderItemInfoList = new ObservableCollection<PlanOrderItemInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询子订单信息失败，请联系管理员！";
            }
        }

        #endregion

        #region 命令和消息等执行函数

        /// <summary>
        /// 执行新建命令
        /// </summary>
        private void OnExecuteScheduleCommand()
        {

            //功能与页面的对应，Controller和Action 与配置文件对应。
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "PlanOrderItemInfo" && a.Action == "GetScheduleInfoListByOrderItemID");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.PlanOrderItemInfo.Clone();
            Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行新增命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteScheduleCommand()
        {
            if (Equals(base.MenuModule, null) || Equals(base.MenuModule.Functions, null) || base.MenuModule.Functions.Count < 1)
            {
                return false;
            }
            else
            {
                //与服务端对应
                if (base.MenuModule.Functions.Any(a => a.Controller == "PlanOrderItemInfo" && a.Action == "GetScheduleInfoListByOrderItemID"))
                {
                    if (Equals(PlanOrderItemInfo, null))
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
                if (base.MenuModule.Functions.Any(a => a.Controller == "PlanOrderItemInfo" && a.Action == "Add"))
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
            if (Equals(PlanOrderItemInfoList, null) || !PlanOrderItemInfoList.Any())
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
            if (Equals(PlanOrderItemInfoList, null) || !PlanOrderItemInfoList.Any())
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
            FilterRule filterRuleName = new FilterRule("OrderItemName", txt.Trim(), "contains");
            FilterRule filterRuleCode = new FilterRule("OrderItemCode", txt.Trim(), "contains");
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
