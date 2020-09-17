using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.ProductInfo.Model;
using Solution.Desktop.ProductTypeInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace Solution.Desktop.ProductInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class ProductInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getPageData(1, 200);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.ProductInfoModel = parameter as ProductInfoModel;
        }

        #region 产品模型
        private ProductInfoModel productInfoModel = new ProductInfoModel();
        /// <summary>
        /// 企业模型
        /// </summary>
        public ProductInfoModel ProductInfoModel
        {
            get { return productInfoModel; }
            set
            {
                Set(ref productInfoModel, value);
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
            return ProductInfoModel == null ? false : ProductInfoModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ProductInfo/Update",
                Utility.JsonHelper.ToJson(new List<ProductInfoModel> { ProductInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<ProductInfoModel>(ProductInfoModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<ProductInfoModel>();
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
        #region 产品类别信息模型,用于列表数据显示
        private ObservableCollection<ProductTypeInfoModel> productTypeInfoList = new ObservableCollection<ProductTypeInfoModel>();

        /// <summary>
        /// 产品类别信息数据
        /// </summary>
        public ObservableCollection<ProductTypeInfoModel> ProductTypeInfoList
        {
            get { return productTypeInfoList; }
            set { Set(ref productTypeInfoList, value); }
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
           
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductTypeInfoModel>>>(GlobalData.ServerRootUri + "ProductTypeInfo/GetProductTypeList", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取产品类别信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("产品类别信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    ProductTypeInfoList = new ObservableCollection<ProductTypeInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    ProductTypeInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProductTypeInfoList = new ObservableCollection<ProductTypeInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询产品类别信息失败，请联系管理员！";
            }
        }
    }
}
