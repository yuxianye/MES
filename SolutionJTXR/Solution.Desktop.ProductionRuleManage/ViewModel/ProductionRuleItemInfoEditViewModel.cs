using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.ProductionRuleManage.Model;
using Solution.Desktop.ProductInfo.Model;
using Solution.Desktop.EntProductionLineInfo.Model;
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
    public class ProductionRuleItemInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionRuleItemInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.ProductionRuleItemInfo = parameter as ProductionRuleItemInfoModel;
            InitProcessData();
            InitLineData();
        }

        #region 配方明细模型
        private ProductionRuleItemInfoModel ruleItemModel = new ProductionRuleItemInfoModel();
        private ProductionProcessInfoModel processInfo = new ProductionProcessInfoModel();
        private EntProductionLineInfoModel lineinfo = new EntProductionLineInfoModel();
        /// <summary>
        /// 配方明细模型
        /// </summary>
        public ProductionRuleItemInfoModel ProductionRuleItemInfo
        {
            get { return ruleItemModel; }
            set
            {
                Set(ref ruleItemModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        #region 生产线模型
        private EntProductionLineInfoModel entProductionLineInfoModel = new EntProductionLineInfoModel();
        public EntProductionLineInfoModel EntProductionLineInfo
        {
            get { return entProductionLineInfoModel; }
            set
            {
                Set(ref entProductionLineInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 工序模型
        private ProductionProcessInfoModel productionProcessInfoModel = new ProductionProcessInfoModel();
        public ProductionProcessInfoModel ProductionProcessInfo
        {
            get { return productionProcessInfoModel; }
            set
            {
                Set(ref productionProcessInfoModel, value);
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
            return ProductionRuleItemInfo == null ? false : ProductionRuleItemInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            ProductionRuleItemInfo.DurationUnit = 3;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ProductionRuleItemInfo/Update",
                Utility.JsonHelper.ToJson(new List<ProductionRuleItemInfoModel> { ProductionRuleItemInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<ProductionRuleItemInfoModel>(ProductionRuleItemInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<ProductionRuleItemInfoModel>();
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

        #region 工序信息列表模型
        private ObservableCollection<ProductionProcessInfoModel> productionProcessInfoList = new ObservableCollection<ProductionProcessInfoModel>();

        /// <summary>
        /// 工序信息数据
        /// </summary>
        public ObservableCollection<ProductionProcessInfoModel> ProductionProcessInfoList
        {
            get { return productionProcessInfoList; }
            set { Set(ref productionProcessInfoList, value); }
        }
        #endregion

        #region 生产线信息列表模型
        private ObservableCollection<EntProductionLineInfoModel> entProductionLineInfoList = new ObservableCollection<EntProductionLineInfoModel>();

        /// <summary>
        /// 工序信息数据
        /// </summary>
        public ObservableCollection<EntProductionLineInfoModel> EntProductionLineInfoList
        {
            get { return entProductionLineInfoList; }
            set { Set(ref entProductionLineInfoList, value); }
        }
        #endregion
        private void InitProcessData()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif

            processInfo.Id = ProductionRuleItemInfo.ProductionProcess_Id;
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionProcessInfoModel>>>(GlobalData.ServerRootUri + "ProductionProcessInfo/GetProductionProcessInfo1", Utility.JsonHelper.ToJson(processInfo));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取工序信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("工序信息内容：" + Utility.JsonHelper.ToJson(result));
#endif


            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    ProductionProcessInfoList = new ObservableCollection<ProductionProcessInfoModel>(result.Data.Data);
                    ProductionProcessInfo = ProductionProcessInfoList.FirstOrDefault();
                }
                else
                {
                    ProductionProcessInfoList?.Clear();
                    //  TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProductionProcessInfoList = new ObservableCollection<ProductionProcessInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询配方明细信息失败，请联系管理员！";
            }
        }
        private void InitLineData()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif

            lineinfo.Id = ProductionProcessInfo.EntProductionLine_Id;
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntProductionLineInfoModel>>>(GlobalData.ServerRootUri + "EntProductionLineInfo/GetEntProductionLineInfo1", Utility.JsonHelper.ToJson(lineinfo));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取生产线信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("生产线信息内容：" + Utility.JsonHelper.ToJson(result));
#endif


            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EntProductionLineInfoList = new ObservableCollection<EntProductionLineInfoModel>(result.Data.Data);
                    EntProductionLineInfo = EntProductionLineInfoList.FirstOrDefault();
                }
                else
                {
                    ProductionProcessInfoList?.Clear();
                    //  TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EntProductionLineInfoList = new ObservableCollection<EntProductionLineInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询生产线信息失败，请联系管理员！";
            }
        }
    }
}
