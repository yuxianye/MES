using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.ProManufacturingBillManage.Model;
using Solution.Desktop.ProductionProcessInfo.Model;
using Solution.Desktop.ProductionRuleManage.Model;
using Solution.Desktop.MaterialInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.ProManufacturingBillManage.ViewModel
{
   public  class ProManufacturingBOMBillItemInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProManufacturingBOMBillItemInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getMaterialPageData(1, 200);
        }

        public override void OnParamterChanged(object parameter)
        {
            this.ProManufacturingBillInfo = parameter as ProManufacturingBillInfoModel;
            getPageData();
        }
        #region 制造清单模型
        private ProManufacturingBillInfoModel proManufacturingBillInfo = new ProManufacturingBillInfoModel();
        private ProductionRuleInfoModel ruleInfo = new ProductionRuleInfoModel();
        /// <summary>
        /// 制造清单模型
        /// </summary>
        public ProManufacturingBillInfoModel ProManufacturingBillInfo
        {
            get { return proManufacturingBillInfo; }
            set
            {
                Set(ref proManufacturingBillInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        #region BOM明细模型
        private ProManufacturingBOMBillItemInfoModel proManufacturingBOMBillItemInfo = new ProManufacturingBOMBillItemInfoModel();

        /// <summary>
        /// 配方明细模型
        /// </summary>
        public ProManufacturingBOMBillItemInfoModel ProManufacturingBOMBillItemInfo
        {
            get { return proManufacturingBOMBillItemInfo; }
            set
            {
                Set(ref proManufacturingBOMBillItemInfo, value);
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
            return ProManufacturingBOMBillItemInfo == null ? false : ProManufacturingBOMBillItemInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            ProManufacturingBOMBillItemInfo.Id = CombHelper.NewComb();
            ProManufacturingBOMBillItemInfo.LastUpdatedTime = DateTime.Now;
            ProManufacturingBOMBillItemInfo.CreatedTime = DateTime.Now;
            ProManufacturingBOMBillItemInfo.ProManufacturingBill_Id = ProManufacturingBillInfo.Id;
            ProManufacturingBOMBillItemInfo.ProductionRule_Id = ProManufacturingBillInfo.ProductionRule_Id;

            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ProManufacturingBillInfo/AddBOMItem",
                Utility.JsonHelper.ToJson(new List<ProManufacturingBOMBillItemInfoModel> { ProManufacturingBOMBillItemInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<ProManufacturingBOMBillItemInfoModel>(ProManufacturingBOMBillItemInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<ProManufacturingBOMBillItemInfoModel>();
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

  
        #region 配方明细信息模型,用于列表数据显示
        private ObservableCollection<ProductionRuleItemInfoModel> productionRuleItemInfoList = new ObservableCollection<ProductionRuleItemInfoModel>();

        /// <summary>
        /// 企业信息数据
        /// </summary>
        public ObservableCollection<ProductionRuleItemInfoModel> ProductionRuleItemInfoList
        {
            get { return productionRuleItemInfoList; }
            set { Set(ref productionRuleItemInfoList, value); }
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
            ruleInfo.Id = ProManufacturingBillInfo.ProductionRule_Id;
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionRuleItemInfoModel>>>(GlobalData.ServerRootUri + "ProductionRuleItemInfo/GetProductionRuleItemInfoListByRuleID", Utility.JsonHelper.ToJson(ruleInfo));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取配方明细信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("配方明细信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    ProductionRuleItemInfoList = new ObservableCollection<ProductionRuleItemInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    ProductionRuleItemInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProductionRuleItemInfoList = new ObservableCollection<ProductionRuleItemInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询配方明细信息失败，请联系管理员！";
            }
        }

        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 物料信息模型,用于列表数据显示
        private ObservableCollection<MaterialInfoModel> materialInfoList = new ObservableCollection<MaterialInfoModel>();

        /// <summary>
        /// 物料信息数据
        /// </summary>
        public ObservableCollection<MaterialInfoModel> MaterialInfoList
        {
            get { return materialInfoList; }
            set { Set(ref materialInfoList, value); }
        }
        #endregion
      /// <summary>
      /// 获取物料信息列表
      /// </summary>
      /// <param name="pageIndex"></param>
      /// <param name="pageSize"></param>
        private void getMaterialPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialInfoModel>>>(GlobalData.ServerRootUri + "MaterialInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取物料信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("物料信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MaterialInfoList = new ObservableCollection<MaterialInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialInfoList = new ObservableCollection<MaterialInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询物料信息失败，请联系管理员！";
            }
        }
    }
}
