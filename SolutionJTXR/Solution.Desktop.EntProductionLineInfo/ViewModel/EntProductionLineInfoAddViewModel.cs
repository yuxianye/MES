using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.EntProductionLineInfo.Model;
using Solution.Desktop.EntSiteInfo.Model;
using Solution.Desktop.EntAreaInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Solution.Desktop.EntProductionLineInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EntProductionLineInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntProductionLineInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            enterpriseSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteenterpriseSelectionChangedCommand);
            siteSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutesiteSelectionChangedCommand);
            getPageData(1, 200);
        }

        #region 生产线模型
        private EntProductionLineInfoModel entProductionLineInfoModel = new EntProductionLineInfoModel();
        private EnterpriseModel enterpriseModel = new EnterpriseModel();
        private EntSiteInfoModel entsiteModel = new EntSiteInfoModel();

        /// <summary>
        /// 生产线模型
        /// </summary>
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
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return EntProductionLineInfo == null ? false : EntProductionLineInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            EntProductionLineInfo.DurationUnit = 3;
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EntProductionLineInfo/Add",
                Utility.JsonHelper.ToJson(new List<EntProductionLineInfoModel> { EntProductionLineInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<EntProductionLineInfoModel>(EntProductionLineInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EntProductionLineInfoModel>();
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
            this.EntProductionLineInfo.EntSite_Id = Guid.Empty;
            this.EntProductionLineInfo.EntArea_Id = Guid.Empty;
            getPageData1();
        }

        private void OnExecutesiteSelectionChangedCommand()
        {
            EntAreaInfoList?.Clear();
            this.EntProductionLineInfo.EntArea_Id = Guid.Empty;
            getPageData2();
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
    }
}
