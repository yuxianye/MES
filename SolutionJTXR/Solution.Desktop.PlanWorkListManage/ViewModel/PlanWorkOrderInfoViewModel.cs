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
    public class PlanWorkOrderInfoViewModel : PageableViewModelBase
    {
        public PlanWorkOrderInfoViewModel()
        {
            initCommand();
            registerMessenger();
        }
        #region 命令定义和初始化


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
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            ImportCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteImportCommand, OnCanExecuteImportCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }

        #endregion
        #region 工序需求模型
        /// <summary>
        /// 工序需求信息模型
        /// </summary>
        private PlanProductionProcessRequirementInfoModel planProductionProcessRequirementInfo;
        /// <summary>
        /// 工序需求信息模型
        /// </summary>
        public PlanProductionProcessRequirementInfoModel PlanProductionProcessRequirementInfo
        {
            get { return planProductionProcessRequirementInfo; }
            set { Set(ref planProductionProcessRequirementInfo, value); }
        }
        #endregion

        #region 工序需求信息模型,用于列表数据显示
        private ObservableCollection<PlanProductionProcessRequirementInfoModel> processInfoList = new ObservableCollection<PlanProductionProcessRequirementInfoModel>();

        /// <summary>
        /// 工序需求信息数据
        /// </summary>
        public ObservableCollection<PlanProductionProcessRequirementInfoModel> PlanProductionProcessRequirementInfoList
        {
            get { return processInfoList; }
            set { Set(ref processInfoList, value); }
        }
        #endregion
        #region 物料需求信息模型,用于列表数据显示
        private ObservableCollection<PlanMaterialRequirementInfoModel> materialInfoList = new ObservableCollection<PlanMaterialRequirementInfoModel>();

        /// <summary>
        /// 物料需求信息数据
        /// </summary>
        public ObservableCollection<PlanMaterialRequirementInfoModel> PlanMaterialRequirementInfoList
        {
            get { return materialInfoList; }
            set { Set(ref materialInfoList, value); }
        }
        #endregion   
        #region 设备需求信息模型,用于列表数据显示
        private ObservableCollection<PlanEquipmentRequirementInfoModel> equipmentInfoList = new ObservableCollection<PlanEquipmentRequirementInfoModel>();

        /// <summary>
        /// 工序需求信息数据
        /// </summary>
        public ObservableCollection<PlanEquipmentRequirementInfoModel> PlanEquipmentRequirementInfoList
        {
            get { return equipmentInfoList; }
            set { Set(ref equipmentInfoList, value); }
        }
        #endregion
        #region 生产计划模型
        /// <summary>
        /// 生产计划模型
        /// </summary>
        private PlanProductionScheduleInfoModel planProductionScheduleInfo;// = new EnterpriseModel();
        /// <summary>
        /// 生产计划模型
        /// </summary>
        public PlanProductionScheduleInfoModel PlanProductionScheduleInfo
        {
            get { return planProductionScheduleInfo; }
            set { Set(ref planProductionScheduleInfo, value); }
        }
        #endregion
        public override void OnParamterChanged(object parameter)
        {
            this.PlanProductionScheduleInfo = parameter as PlanProductionScheduleInfoModel;
            getPageData();
            getPageData1();
            getPageData2();
        }
        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<PlanProductionProcessRequirementInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }
        public override void OnExecutePageChangedCommand(PageChangedEventArgs e)
        {
            Utility.LogHelper.Info(e.PageIndex.ToString() + " " + e.PageSize);
            getPageData();
        }
        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(PlanProductionProcessRequirementInfoModel productInfoModel)
        {
            getPageData();
            getPageData1();
            getPageData2();
            this.PlanProductionProcessRequirementInfo = PlanProductionProcessRequirementInfoList.FirstOrDefault();
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            Messenger.Default.Unregister<PlanProductionProcessRequirementInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }
        #endregion

        #region 工序需求数据查询

        /// <summary>
        /// 取得工序需求数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getPageData()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif


            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<PlanProductionProcessRequirementInfoModel>>>(GlobalData.ServerRootUri + "PlanOrderItemInfo/GetProcessRequireInfoListByScheduleID", Utility.JsonHelper.ToJson(PlanProductionScheduleInfo));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取工序需求信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("工序需求信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    PlanProductionProcessRequirementInfoList = new ObservableCollection<PlanProductionProcessRequirementInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    PlanProductionProcessRequirementInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                PlanProductionProcessRequirementInfoList = new ObservableCollection<PlanProductionProcessRequirementInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询工序需求信息失败，请联系管理员！";
            }
        }

        #endregion
        #region 物料需求数据查询

        /// <summary>
        /// 取得物料需求数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getPageData1()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif


            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<PlanMaterialRequirementInfoModel>>>(GlobalData.ServerRootUri + "PlanOrderItemInfo/GetMaterialRequireInfoListByScheduleID", Utility.JsonHelper.ToJson(PlanProductionScheduleInfo));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取物料需求信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("物料需求信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    PlanMaterialRequirementInfoList = new ObservableCollection<PlanMaterialRequirementInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    PlanMaterialRequirementInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                PlanMaterialRequirementInfoList = new ObservableCollection<PlanMaterialRequirementInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询物料需求信息失败，请联系管理员！";
            }
        }

        #endregion
        #region 设备需求数据查询

        /// <summary>
        /// 取得设备需求数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getPageData2()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif


            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<PlanEquipmentRequirementInfoModel>>>(GlobalData.ServerRootUri + "PlanOrderItemInfo/GetEquipmentRequireInfoListByScheduleID", Utility.JsonHelper.ToJson(PlanProductionScheduleInfo));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取设备需求信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("设备需求信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    PlanEquipmentRequirementInfoList = new ObservableCollection<PlanEquipmentRequirementInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    PlanEquipmentRequirementInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                PlanEquipmentRequirementInfoList = new ObservableCollection<PlanEquipmentRequirementInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询设备需求信息失败，请联系管理员！";
            }
        }

        #endregion
        #region 命令和消息等执行函数
        /// <summary>
        /// 执行刷新命令
        /// </summary>
        private void OnExecuteRefreshCommand()
        {
            getPageData();
            getPageData1();
            getPageData2();
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
