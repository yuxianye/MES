using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.ProManufacturingBillManage.Model;
using Solution.Desktop.ProductInfo.Model;
using Solution.Desktop.ProductionRuleManage.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Solution.Utility.Extensions;

namespace Solution.Desktop.ProManufacturingBillManage.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class ProManufacturingBillInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProManufacturingBillInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //  productSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteproductSelectionChangedCommand);
            getPageData(1, 200);
            //  getPageData0();
        }
        #region 配方模型
        /// <summary>
        /// 配方信息模型
        /// </summary>
        private ProductionRuleInfoModel productionRuleInfo;// = new EnterpriseModel();
        /// <summary>
        /// 配方信息模型
        /// </summary>
        public ProductionRuleInfoModel ProductionRuleInfo
        {
            get { return productionRuleInfo; }
            set { Set(ref productionRuleInfo, value); }
        }
        #endregion
        public override void OnParamterChanged(object parameter)
        {
            this.ProductionRuleInfo = parameter as ProductionRuleInfoModel;
        }
        #region 制造清单模型
        private ProManufacturingBillInfoModel proManufacturingBillInfoModel = new ProManufacturingBillInfoModel();
        private ProductInfoModel productInfo = new ProductInfoModel();

        /// <summary>
        /// 制造清单模型
        /// </summary>
        public ProManufacturingBillInfoModel ProManufacturingBillInfo
        {
            get { return proManufacturingBillInfoModel; }
            set
            {
                Set(ref proManufacturingBillInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        #region 制造清单类型
        System.Array billTypes = Enum.GetValues(typeof(ProductEnumModel.BillType)).Cast<ProductEnumModel.BillType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();
        public System.Array BillTypes
        {
            get { return billTypes; }
            set
            {
                Set(ref billTypes, value);
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
        /// 产品信息下拉列表改变
        /// </summary>
        public ICommand productSelectionChangedCommand { get; set; }


        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return ProManufacturingBillInfo == null ? false : ProManufacturingBillInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            ProManufacturingBillInfo.Id = CombHelper.NewComb();
            ProManufacturingBillInfo.Product_Id = this.ProductionRuleInfo.Product_Id;
            ProManufacturingBillInfo.ProductionRule_Id = this.ProductionRuleInfo.Id;
            ProManufacturingBillInfo.LastUpdatedTime = DateTime.Now;
            ProManufacturingBillInfo.CreatedTime = DateTime.Now;


            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ProManufacturingBillInfo/Add",
                Utility.JsonHelper.ToJson(new List<ProManufacturingBillInfoModel> { ProManufacturingBillInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<ProManufacturingBillInfoModel>(ProManufacturingBillInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<ProManufacturingBillInfoModel>();
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

        private void OnExecuteproductSelectionChangedCommand()
        {
            //  getPageData1();
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
        #region 配方信息模型,用于列表数据显示
        private ObservableCollection<ProductionRuleInfoModel> productionRuleInfoList = new ObservableCollection<ProductionRuleInfoModel>();

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

        private List<BillTypeInfo> billTypeList = new List<BillTypeInfo>();
        public List<BillTypeInfo> BillTypeList
        {
            get { return billTypeList; }
            set { Set(ref billTypeList, value); }
        }
        //private void getPageData0()
        //{
        //    foreach (ProductEnumModel.BillType item in Enum.GetValues(typeof(ProductEnumModel.BillType)))
        //    {
        //        BillTypeInfo info = new BillTypeInfo();
        //        info.Id = item.GetHashCode();
        //        info.BillTypeName = item.ToDescription();
        //        BillTypeList.Add(info);
        //    }
        //}
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

        private void getPageData1()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            productInfo.Id = ProManufacturingBillInfo.Product_Id;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionRuleInfoModel>>>(GlobalData.ServerRootUri + "ProductionRuleInfo/GetProductionRuleInfoListByProductID", Utility.JsonHelper.ToJson(productInfo));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取配方信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("配方信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
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
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询配方信息失败，请联系管理员！";
            }
        }

        public class BillTypeInfo
        {
            public int Id { get; set; }
            public string BillTypeName { get; set; }
        }

    }
}
