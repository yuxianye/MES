using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.PlanWorkListManage.Model;
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
using Solution.Desktop.PlanOrderManage.Model;

namespace Solution.Desktop.PlanWorkListManage.ViewModel
{
    public class PlanProductionScheduleInfoViewModel : PageableViewModelBase
    {
        public PlanProductionScheduleInfoViewModel()
        {
            //this.MenuModule
            initCommand();
            registerMessenger();
            MenuItems = new List<MenuItem>()
            {
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Add_32x32.png"),Header =ResourceHelper.FindResource ("New"),Command =AddCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png") ,Header=ResourceHelper.FindResource ("Edit"),Command =EditCommand},
                 new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png") ,Header=ResourceHelper.FindResource ("WorkOrderGenerate"),Command =WorkOrderGenerateCommand},
                  new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png") ,Header=ResourceHelper.FindResource ("WorkOrderChecking"),Command =WorkOrderCheckingCommand},
                   new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Edit_32x32.png") ,Header=ResourceHelper.FindResource ("WorkOrderDistribute"),Command =WorkOrderDistributeCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Del_32x32.png") ,Header=ResourceHelper.FindResource ("PhysicalDelete"),Command =PhysicalDeleteCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Refresh_32x32.png") ,Header=ResourceHelper.FindResource ("Refresh"),Command =RefreshCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Import_32x32.png" ),Header=ResourceHelper.FindResource ("Import"),Command =ImportCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Export_32x32.png") ,Header=ResourceHelper.FindResource ("Export"),Command =ExportCommand},
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Print_32x32.png") ,Header=ResourceHelper.FindResource ("Print"),Command =PrintCommand},
            };
        }

        #region 生产计划模型
        /// <summary>
        /// 生产计划信息模型
        /// </summary>
        private PlanProductionScheduleInfoModel planProductionScheduleInfo;// = new EnterpriseModel();
        /// <summary>
        /// 生产计划信息模型
        /// </summary>
        public PlanProductionScheduleInfoModel PlanProductionScheduleInfo
        {
            get { return planProductionScheduleInfo; }
            set { Set(ref planProductionScheduleInfo, value); }
        }
        #endregion

        #region 生产计划信息模型,用于列表数据显示
        private ObservableCollection<PlanProductionScheduleInfoModel> scheduleInfoList = new ObservableCollection<PlanProductionScheduleInfoModel>();

        /// <summary>
        /// 生产计划信息数据
        /// </summary>
        public ObservableCollection<PlanProductionScheduleInfoModel> PlanProductionScheduleInfoList
        {
            get { return scheduleInfoList; }
            set { Set(ref scheduleInfoList, value); }
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
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }
        /// <summary>
        /// 工单生成命令
        /// </summary>
        public ICommand WorkOrderGenerateCommand { get; set; }
        /// <summary>
        /// 工单查看命令
        /// </summary>
        public ICommand WorkOrderCheckingCommand { get; set; }
        /// <summary>
        /// 工单下达命令
        /// </summary>
        public ICommand WorkOrderDistributeCommand { get; set; }

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            AddCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAddCommand, OnCanExecuteAddCommand);
            EditCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteEditCommand, OnCanExecuteEditCommand);
            PhysicalDeleteCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutePhysicalDeleteCommand, OnCanExecutePhysicalDeleteCommand);
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            WorkOrderGenerateCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteWorkOrderGenerateCommand, OnCanExecuteWorkOrderGenerateCommand);
            WorkOrderCheckingCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteWorkOrderCheckingCommand, OnCanExecuteWorkOrderCheckingCommand);
            WorkOrderDistributeCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteWorkOrderDistributeCommand, OnCanExecuteWorkOrderDistributeCommand);
            ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand, OnCanExecuteImportCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }

        #endregion
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
        public override void OnParamterChanged(object parameter)
        {
            this.PlanOrderItemInfo = parameter as PlanOrderItemInfoModel;
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }
        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<PlanProductionScheduleInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(PlanProductionScheduleInfoModel productInfoModel)
        {
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            this.PlanProductionScheduleInfo = PlanProductionScheduleInfoList.FirstOrDefault();
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<PlanProductionScheduleInfoModel>(this, MessengerToken.DataChanged, dataChanged);
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
            FilterRule filterRuleName = new FilterRule("OrderItem.Id", PlanOrderItemInfo.Id, "equal");
            filterGroup.Rules.Add(filterRuleName);
            pageRepuestParams.FilterGroup = filterGroup;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<PlanProductionScheduleInfoModel>>>(GlobalData.ServerRootUri + "PlanProductionScheduleInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取生产计划信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("生产计划信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    PlanProductionScheduleInfoList = new ObservableCollection<PlanProductionScheduleInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    PlanProductionScheduleInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                PlanProductionScheduleInfoList = new ObservableCollection<PlanProductionScheduleInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询生产计划信息失败，请联系管理员！";
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
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "PlanOrderItemInfo" && a.Action == "AddScheduleInfo");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.PlanOrderItemInfo.Clone();
            Messenger.Default.Send<ViewInfo>(menuFunctionViewInfoMap?.ViewInfo, MessengerToken.Navigate);
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
            //Controller和Action 与配置文件对应
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "PlanOrderItemInfo" && a.Action == "UpdateScheduleInfo");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.PlanProductionScheduleInfo.Clone();
            Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行编辑命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteEditCommand()
        {
            if (PlanProductionScheduleInfo != null && PlanProductionScheduleInfo.ScheduleStatus == PlanEnumModel.ScheduleStatus.UnStart)
            {
                return true;
            }
            else
            { return false; }
        }


        /// <summary>
        /// 执行物理删除命令
        /// </summary>
        private void OnExecutePhysicalDeleteCommand()
        {
            if (MessageBox.Show("是否删除该排产数据？", "数据删除", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "PlanProductionScheduleInfo/Delete", Utility.JsonHelper.ToJson(new List<Guid>() { PlanProductionScheduleInfo.Id }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    PlanProductionScheduleInfo = null;
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
            if (PlanProductionScheduleInfo != null && PlanProductionScheduleInfo.ScheduleStatus == PlanEnumModel.ScheduleStatus.UnStart)
            {
                return true;
            }
            else
            { return false; }
        }

        /// <summary>
        /// 执行刷新命令
        /// </summary>
        private void OnExecuteRefreshCommand()
        {
            pageRepuestParams.FilterGroup = null;
            getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }
        /// <summary>
        /// 执行工单生成命令
        /// </summary>
        private void OnExecuteWorkOrderGenerateCommand()
        {
            if (MessageBox.Show("确定要生成工单么？生成工单后无法修改计划。", "生成工单", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "PlanProductionScheduleInfo/AddWorkList", Utility.JsonHelper.ToJson(new List<PlanProductionScheduleInfoModel> { PlanProductionScheduleInfo }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    PlanProductionScheduleInfo = null;
                }
                else
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = result?.Message ?? "生成工单失败，请联系管理员！";
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                }
            }
        }

        /// <summary>
        /// 是否可以执行工单生成命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteWorkOrderGenerateCommand()
        {
            if (PlanProductionScheduleInfo != null && PlanProductionScheduleInfo.ScheduleStatus == PlanEnumModel.ScheduleStatus.UnStart)
            {
                return true;
            }
            else
            { return false; }
        }
        /// <summary>
        /// 执行工单查看命令
        /// </summary>
        private void OnExecuteWorkOrderCheckingCommand()
        {
            //Controller和Action 与配置文件对应
            var menuFunctionViewInfoMap = GlobalData.MenuFunctionViewInfoMap.FirstOrDefault(a => a.Controller == "PlanOrderItemInfo" && a.Action == "GetProcessRequireInfoListByScheduleID");
            var viewInfo = menuFunctionViewInfoMap?.ViewInfo;
            viewInfo.Parameter = this.PlanProductionScheduleInfo.Clone();
            Messenger.Default.Send<ViewInfo>(viewInfo, MessengerToken.Navigate);
        }

        /// <summary>
        /// 是否可以执行工单查看命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteWorkOrderCheckingCommand()
        {
            if (PlanProductionScheduleInfo != null && PlanProductionScheduleInfo.ScheduleStatus != PlanEnumModel.ScheduleStatus.UnStart)
            {
                return true;
            }
            else
            { return false; }
        }
        /// <summary>
        /// 执行工单下达命令
        /// </summary>
        private void OnExecuteWorkOrderDistributeCommand()
        {
            if (MessageBox.Show("确定要下达工单么？", "下达工单", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "PlanProductionScheduleInfo/DistributeWorkOrder", Utility.JsonHelper.ToJson(new List<PlanProductionScheduleInfoModel> { PlanProductionScheduleInfo }));
                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
                    PlanProductionScheduleInfo = null;
                }
                else
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = result?.Message ?? "下达工单失败，请联系管理员！";
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                }
            }
        }

        /// <summary>
        /// 是否可以执行工单下达命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteWorkOrderDistributeCommand()
        {
            if (PlanProductionScheduleInfo != null && PlanProductionScheduleInfo.ScheduleStatus == PlanEnumModel.ScheduleStatus.Generated)
            {
                return true;
            }
            else
            { return false; }
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
            return true;
        }

        /// <summary>
        /// 执行导出命令
        /// </summary>
        private void OnExecuteExportCommand()
        {


        }


        /// <summary>
        /// 执行打印命令
        /// </summary>
        private void OnExecutePrintCommand()
        {


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
