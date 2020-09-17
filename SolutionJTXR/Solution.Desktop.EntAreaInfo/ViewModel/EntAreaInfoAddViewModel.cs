using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.EntAreaInfo.Model;
using Solution.Desktop.EntSiteInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Core.Enum;

namespace Solution.Desktop.EntAreaInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EntAreaInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntAreaInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            SelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSelectionChangedCommand);
            getPageData(1, 200);
            
        }

        #region 区域模型
        private EntAreaInfoModel entAreaInfoModel = new EntAreaInfoModel();
        private EnterpriseModel enterpriseModel = new EnterpriseModel();
       
        /// <summary>
        /// 区域模型
        /// </summary>
        public EntAreaInfoModel EntAreaInfoModel
        {
            get { return entAreaInfoModel; }
            set
            {
                Set(ref entAreaInfoModel, value);
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

        public ICommand SelectionChangedCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return EntAreaInfoModel == null ? false : EntAreaInfoModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EntAreaInfo/Add",
                Utility.JsonHelper.ToJson(new List<EntAreaInfoModel> { EntAreaInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<EntAreaInfoModel>(EntAreaInfoModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EntAreaInfoModel>();
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
        /// 执行下拉列表改变事件
        /// </summary>
        private void OnExecuteSelectionChangedCommand()
        {
            EntSiteInfoList?.Clear();
            this.EntAreaInfoModel.EntSite_Id = Guid.Empty;
            getPageData1();
        }
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 企业信息模型,用于下拉列表数据显示
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


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            
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

        private void getPageData1()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            
            enterpriseModel.Id= EntAreaInfoModel.Enterprise_Id;


            
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
    }
}
