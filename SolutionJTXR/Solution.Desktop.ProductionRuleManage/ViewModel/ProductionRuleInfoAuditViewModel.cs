using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.ProductionRuleManage.Model;
using Solution.Desktop.ProductionRuleStatusInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using Solution.Desktop.ProductionProcessInfo.Model;
using System.Diagnostics;

namespace Solution.Desktop.ProductionRuleManage.ViewModel
{
   public class ProductionRuleInfoAuditViewModel : VmBase
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionRuleInfoAuditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.ProductionRuleInfo = parameter as ProductionRuleInfoModel;
            InitStatusData();
        }

        #region 配方明细模型
        private ProductionRuleInfoModel ruleModel = new ProductionRuleInfoModel();
        /// <summary>
        /// 配方模型
        /// </summary>
        public ProductionRuleInfoModel ProductionRuleInfo
        {
            get { return ruleModel; }
            set
            {
                Set(ref ruleModel, value);
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
            return ProductionRuleInfo == null ? false : ProductionRuleInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            
            ProductionRuleInfo.DurationUnit = 3;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ProductionRuleInfo/Audit",
                Utility.JsonHelper.ToJson(new List<ProductionRuleInfoModel> { ProductionRuleInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<ProductionRuleInfoModel>(ProductionRuleInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<ProductionRuleInfoModel>();
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


        #region 配方状态信息列表模型
        private ObservableCollection<ProductionRuleStatusInfoModel> productionRuleStatusInfoList = new ObservableCollection<ProductionRuleStatusInfoModel>();

        /// <summary>
        /// 工序信息数据
        /// </summary>
        public ObservableCollection<ProductionRuleStatusInfoModel> ProductionRuleStatusInfoList
        {
            get { return productionRuleStatusInfoList; }
            set { Set(ref productionRuleStatusInfoList, value); }
        }
        #endregion
        private void InitStatusData()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
           
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionRuleStatusInfoModel>>>(GlobalData.ServerRootUri + "ProductionRuleStatusInfo/GetProductionRuleStatusAuditInfoList", Utility.JsonHelper.ToJson(null));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取配方审核结果信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("配方审核结果信息内容：" + Utility.JsonHelper.ToJson(result));
#endif


            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    ProductionRuleStatusInfoList = new ObservableCollection<ProductionRuleStatusInfoModel>(result.Data.Data);
                }
                else
                {
                    ProductionRuleStatusInfoList?.Clear();
                    //  TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProductionRuleStatusInfoList = new ObservableCollection<ProductionRuleStatusInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询配方审核结果信息失败，请联系管理员！";
            }
        }
        
    }
}
