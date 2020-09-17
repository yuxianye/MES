using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.ProductInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using Solution.Desktop.PlanWorkListManage.Model;
using Solution.Desktop.ProductionRuleManage.Model;
using Solution.Desktop.PlanOrderManage.Model;
using Solution.Utility.Extensions;

namespace Solution.Desktop.PlanWorkListManage.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class PlanProductionScheduleInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PlanProductionScheduleInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getPageData();

        }
        public override void OnParamterChanged(object parameter)
        {
            this.PlanProductionScheduleInfo = parameter as PlanProductionScheduleInfoModel;
        }

        #region 生产计划模型
        private PlanProductionScheduleInfoModel scheduleInfoModel = new PlanProductionScheduleInfoModel();
        /// <summary>
        /// 企业模型
        /// </summary>
        public PlanProductionScheduleInfoModel PlanProductionScheduleInfo
        {
            get { return scheduleInfoModel; }
            set
            {
                Set(ref scheduleInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return PlanProductionScheduleInfo == null ? false : PlanProductionScheduleInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "PlanProductionScheduleInfo/Update",
                Utility.JsonHelper.ToJson(new List<PlanProductionScheduleInfoModel> { PlanProductionScheduleInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<PlanProductionScheduleInfoModel>(PlanProductionScheduleInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<PlanProductionScheduleInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            }
        }

        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }
        #region 优先级
        System.Array prioritys = Enum.GetValues(typeof(PlanEnumModel.ProductUnit)).Cast<PlanEnumModel.Priority>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array Prioritys
        {
            get { return prioritys; }
            set
            {
                Set(ref prioritys, value);
            }
        }
        #endregion


        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 配方信息模型,用于列表数据显示
        private ObservableCollection<ProductionRuleInfoModel> productionRuleInfoList = new ObservableCollection<ProductionRuleInfoModel>();
        private ProductInfoModel productinfo = new ProductInfoModel();
        /// <summary>
        /// 配方信息数据
        /// </summary>
        public ObservableCollection<ProductionRuleInfoModel> ProductionRuleInfoList
        {
            get { return productionRuleInfoList; }
            set { Set(ref productionRuleInfoList, value); }
        }
        #endregion
        #region 当前浏览页面的页码

        private int totalCounts = 0;
        /// <summary>
        /// 所有记录的个数
        /// </summary>
        public int TotalCounts
        {
            get { return totalCounts; }
            set { Set(ref totalCounts, value); }
        }
        #endregion
        private void getPageData()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            productinfo.Id = this.PlanProductionScheduleInfo.Product_Id;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionRuleInfoModel>>>(GlobalData.ServerRootUri + "ProductionRuleInfo/GetProductionRuleInfoList_WIPByProductID", Utility.JsonHelper.ToJson(productinfo));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取某产品的审核通过的配方信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("某产品的审核通过的配方信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    ProductionRuleInfoList = new ObservableCollection<ProductionRuleInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    ProductionRuleInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProductionRuleInfoList = new ObservableCollection<ProductionRuleInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询某产品的审核通过的配方信息失败，请联系管理员！";
            }
        }
    }
}
