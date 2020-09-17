using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.ProductionRuleManage.Model;
using Solution.Desktop.ProductInfo.Model;
using Solution.Desktop.ProductionRuleStatusInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Solution.Desktop.ProductionRuleManage.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class ProductionRuleInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionRuleInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getPageData(1, 200);
        }

        #region 配方模型
        private ProductionRuleInfoModel ProductionRuleInfoModel = new ProductionRuleInfoModel();

        /// <summary>
        /// 配方模型
        /// </summary>
        public ProductionRuleInfoModel ProductionRuleInfo
        {
            get { return ProductionRuleInfoModel; }
            set
            {
                Set(ref ProductionRuleInfoModel, value);
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
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ProductionRuleInfo/Add",
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
                //EnterpriseInfoList = new ObservableCollection<ProductionRuleInfoModel>();
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


        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 产品信息模型,用于列表数据显示
        private ObservableCollection<ProductInfoModel> productInfoList = new ObservableCollection<ProductInfoModel>();

        /// <summary>
        /// 配方信息数据
        /// </summary>
        public ObservableCollection<ProductInfoModel> ProductInfoList
        {
            get { return productInfoList; }
            set { Set(ref productInfoList, value); }
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
        private void getPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductInfoModel>>>(GlobalData.ServerRootUri + "ProductInfo/GetProductInfoList", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取产品信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("产品信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    ProductInfoList = new ObservableCollection<ProductInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    ProductInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProductInfoList = new ObservableCollection<ProductInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询产品信息失败，请联系管理员！";
            }
        }

    }
}
