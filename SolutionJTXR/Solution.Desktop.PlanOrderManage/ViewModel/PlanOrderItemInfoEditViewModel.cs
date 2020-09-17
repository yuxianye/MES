using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.PlanOrderManage.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Solution.Desktop.ProductInfo.Model;
using System.Linq;
using System;
using Solution.Utility.Extensions;

namespace Solution.Desktop.PlanOrderManage.ViewModel
{
    public class PlanOrderItemInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PlanOrderItemInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            productSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteproductSelectionChangedCommand);
            getPageData(1, 200);
        }
        public override void OnParamterChanged(object parameter)
        {
            this.PlanOrderItemInfo = parameter as PlanOrderItemInfoModel;
        }

        #region 订单模型
        private PlanOrderItemInfoModel planOrderItemInfo = new PlanOrderItemInfoModel();
        /// <summary>
        /// 
        /// </summary>
        public PlanOrderItemInfoModel PlanOrderItemInfo
        {
            get { return planOrderItemInfo; }
            set
            {
                Set(ref planOrderItemInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        #region 产品单位类型
        System.Array productUnits = Enum.GetValues(typeof(PlanEnumModel.ProductUnit)).Cast<PlanEnumModel.ProductUnit>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array ProductUnits
        {
            get { return productUnits; }
            set
            {
                Set(ref productUnits, value);
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
        /// 选择产品下拉列表命令
        /// </summary>
        public ICommand productSelectionChangedCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return PlanOrderItemInfo == null ? false : PlanOrderItemInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            PlanOrderItemInfo.RemainQuantity = PlanOrderItemInfo.OrderQuantity;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "PlanOrderItemInfo/Update",
                Utility.JsonHelper.ToJson(new List<PlanOrderItemInfoModel> { PlanOrderItemInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<PlanOrderItemInfoModel>(PlanOrderItemInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<PlanOrderItemInfoModel>();
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

        /// <summary>
        /// 选中产品执行命令
        /// </summary>
        private void OnExecuteproductSelectionChangedCommand()
        {
            var tmp = ProductInfoList.FirstOrDefault(a => a.Id == PlanOrderItemInfo.Product_Id);
            this.PlanOrderItemInfo.ProductCode = tmp.ProductCode;
        }

        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 产品信息模型,用于列表数据显示
        private ObservableCollection<ProductInfoModel> productInfoList = new ObservableCollection<ProductInfoModel>();

        /// <summary>
        /// 产品信息数据
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
