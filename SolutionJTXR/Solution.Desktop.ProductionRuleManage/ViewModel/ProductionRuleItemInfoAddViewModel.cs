using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.ProductionRuleManage.Model;
using Solution.Desktop.ProductInfo.Model;
using Solution.Desktop.ProductionProcessInfo.Model;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.EntSiteInfo.Model;
using Solution.Desktop.EntAreaInfo.Model;
using Solution.Desktop.EntProductionLineInfo.Model;
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
   public class ProductionRuleItemInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionRuleItemInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            enterpriseSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteenterpriseSelectionChangedCommand);
            siteSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutesiteSelectionChangedCommand);
            areaSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteareaSelectionChangedCommand);
            productionLineSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutproductionLineSelectionChangedCommand);
            getPageData(1, 200);
        }

        public override void OnParamterChanged(object parameter)
        {
            this.ProductionRuleItemInfo.ProductionRule_Id = new Guid(parameter.ToString());
        }
        #region 配方明细模型
        private ProductionRuleItemInfoModel productionRuleItemInfo = new ProductionRuleItemInfoModel();
        private EnterpriseModel enterpriseModel = new EnterpriseModel();
        private EntSiteInfoModel entsiteModel = new EntSiteInfoModel();
        private EntAreaInfoModel entareaModel = new EntAreaInfoModel();

        /// <summary>
        /// 配方明细模型
        /// </summary>
        public ProductionRuleItemInfoModel ProductionRuleItemInfo
        {
            get { return productionRuleItemInfo; }
            set
            {
                Set(ref productionRuleItemInfo, value);
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

        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand ConfirmCommand { get; set; }

        /// <summary>
        /// 取消（关闭）命令
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// 企业信息下拉列表改变
        /// </summary>
        public ICommand enterpriseSelectionChangedCommand { get; set; }

        /// <summary>
        /// 厂区信息下拉列表改变
        /// </summary>
        public ICommand siteSelectionChangedCommand { get; set; }

        /// <summary>
        /// 区域信息下拉列表改变
        /// </summary>
        public ICommand areaSelectionChangedCommand { get; set; }

        /// <summary>
        /// 生产线信息下拉列表改变
        /// </summary>
        public ICommand productionLineSelectionChangedCommand { get; set; }


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
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ProductionRuleInfo/AddRuleItem",
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
                //EnterpriseInfoList = new ObservableCollection<ProductionRuleItemInfoModel>();
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

        private void OnExecuteenterpriseSelectionChangedCommand()
        {
            EntSiteInfoList?.Clear();
            EntAreaInfoList?.Clear();
            EntProductionLineInfoList?.Clear();
            ProductionProcessInfoList?.Clear();
            this.EntProductionLineInfo.EntSite_Id = Guid.Empty;
            this.EntProductionLineInfo.EntArea_Id = Guid.Empty;
            this.EntProductionLineInfo.Id = Guid.Empty;
            ProductionRuleItemInfo.ProductionProcess_Id = Guid.Empty;
            getPageData1();
        }

        private void OnExecutesiteSelectionChangedCommand()
        {
            EntAreaInfoList?.Clear();
            EntProductionLineInfoList?.Clear();
            ProductionProcessInfoList?.Clear();
            this.EntProductionLineInfo.EntArea_Id = Guid.Empty;
            this.EntProductionLineInfo.Id = Guid.Empty;
            ProductionRuleItemInfo.ProductionProcess_Id = Guid.Empty;
            getPageData2();
        }
        private void OnExecuteareaSelectionChangedCommand()
        {
            EntProductionLineInfoList?.Clear();
            ProductionProcessInfoList?.Clear();
            this.EntProductionLineInfo.Id = Guid.Empty;
            ProductionRuleItemInfo.ProductionProcess_Id = Guid.Empty;
            getPageData3();
        }

        private void OnExecutproductionLineSelectionChangedCommand()
        {
            ProductionProcessInfoList?.Clear();
            ProductionRuleItemInfo.ProductionProcess_Id = Guid.Empty;
            getPageData4();
        }
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 企业信息模型,用于列表数据显示
        private ObservableCollection<EnterpriseModel> enterpriseInfoList = new ObservableCollection<EnterpriseModel>();

        /// <summary>
        /// 企业信息数据
        /// </summary>
        public ObservableCollection<EnterpriseModel> EnterpriseInfoList
        {
            get { return enterpriseInfoList; }
            set { Set(ref enterpriseInfoList, value); }
        }
        #endregion
        #region 厂区信息模型,用于下拉列表数据显示
        private ObservableCollection<EntSiteInfoModel> entsiteInfoList = new ObservableCollection<EntSiteInfoModel>();

        /// <summary>
        /// 厂区信息数据
        /// </summary>
        public ObservableCollection<EntSiteInfoModel> EntSiteInfoList
        {
            get { return entsiteInfoList; }
            set { Set(ref entsiteInfoList, value); }
        }
        #endregion
        #region 厂区信息模型,用于下拉列表数据显示
        private ObservableCollection<EntAreaInfoModel> entareaInfoList = new ObservableCollection<EntAreaInfoModel>();

        /// <summary>
        /// 区域信息数据
        /// </summary>
        public ObservableCollection<EntAreaInfoModel> EntAreaInfoList
        {
            get { return entareaInfoList; }
            set { Set(ref entareaInfoList, value); }
        }
        #endregion

        #region 生产线信息模型,用于下拉列表数据显示
        private ObservableCollection<EntProductionLineInfoModel> entProductionLineInfoList = new ObservableCollection<EntProductionLineInfoModel>();

        /// <summary>
        /// 区域信息数据
        /// </summary>
        public ObservableCollection<EntProductionLineInfoModel> EntProductionLineInfoList
        {
            get { return entProductionLineInfoList; }
            set { Set(ref entProductionLineInfoList, value); }
        }
        #endregion

        #region 工序信息模型,用于下拉列表数据显示
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
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EnterpriseModel>>>(GlobalData.ServerRootUri + "EnterpriseInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取企业信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("企业信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EnterpriseInfoList = new ObservableCollection<EnterpriseModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EnterpriseInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询企业信息失败，请联系管理员！";
            }
        }

        /// <summary>
        /// 获取某一企业信息下的厂区列表
        /// </summary>
        private void getPageData1()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif

            enterpriseModel.Id = EntProductionLineInfo.Enterprise_Id;


            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntSiteInfoModel>>>(GlobalData.ServerRootUri + "EntSiteInfo/GetEntSiteListByEnterpriseID", Utility.JsonHelper.ToJson(enterpriseModel));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取厂区信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("厂区信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EntSiteInfoList = new ObservableCollection<EntSiteInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EntSiteInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EntSiteInfoList = new ObservableCollection<EntSiteInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询厂区信息失败，请联系管理员！";
            }
        }

        /// <summary>
        /// 获取某一厂区下的区域列表
        /// </summary>
        private void getPageData2()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif

            entsiteModel.Id = EntProductionLineInfo.EntSite_Id;


            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntAreaInfoModel>>>(GlobalData.ServerRootUri + "EntAreaInfo/GetEntAreaListByEntSiteID", Utility.JsonHelper.ToJson(entsiteModel));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取区域信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("区域信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EntAreaInfoList = new ObservableCollection<EntAreaInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EntAreaInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EntAreaInfoList = new ObservableCollection<EntAreaInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询区域信息失败，请联系管理员！";
            }
        }

        /// <summary>
        /// 获取某一区域下的生产线列表
        /// </summary>
        private void getPageData3()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif

            entareaModel.Id = EntProductionLineInfo.EntArea_Id;


            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntProductionLineInfoModel>>>(GlobalData.ServerRootUri + "EntProductionLineInfo/GetEntProductionLineListByEntAreaID", Utility.JsonHelper.ToJson(entareaModel));

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
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EntProductionLineInfoList?.Clear();
                    TotalCounts = 0;
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


        /// <summary>
        /// 获取某一生产线下的工序列表
        /// </summary>
        private void getPageData4()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif


            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionProcessInfoModel>>>(GlobalData.ServerRootUri + "ProductionProcessInfo/GetProductionProcessInfoListByLineID", Utility.JsonHelper.ToJson(EntProductionLineInfo));

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
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    ProductionProcessInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProductionProcessInfoList = new ObservableCollection<ProductionProcessInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询工序信息失败，请联系管理员！";
            }
        }
    }
}
