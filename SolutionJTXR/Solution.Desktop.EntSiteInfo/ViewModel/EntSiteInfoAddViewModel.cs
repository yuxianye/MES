using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.EntSiteInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Solution.Desktop.EntSiteInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EntSiteInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntSiteInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            PageCountChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnPageCountChangedCommand, OnCanPageCountChangedCommand);
            PageSizeChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnPageSizeChangedCommand, OnCanPageSizeChangedCommand);
            TotalPageCountChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnTotalPageCountChangedCommand, OnCanTotalPageCountChangedCommand);
            ComboxSearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnSearchChangedCommand, OnCanSearchChangedCommand);
            EnterpriseNameChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnEnterpriseNameChangedCommand, OnCanEnterpriseNameChangedCommand);

            PageSizeList = new List<int>() { 5,10,20,50};
            getPageData(1, 10000);
            TotalPageCount = (int)Math.Ceiling((decimal)EnterpriseInfoList.Count() / pageSize);
            ShowEnterpriseInfoList = EnterpriseInfoList.Skip((PageCount - 1) * pageSize).Take(pageSize).ToObservableCollection<EnterpriseModel>();

        }

        #region 厂区模型
        private EntSiteInfoModel entSiteInfoModel = new EntSiteInfoModel();
       
        /// <summary>
        /// 厂区模型
        /// </summary>
        public EntSiteInfoModel EntSiteInfoModel
        {
            get { return entSiteInfoModel; }
            set
            {
                Set(ref entSiteInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 分页ComboBox

        private ObservableCollection<EnterpriseModel> pageEnterpriseInfoList = new ObservableCollection<EnterpriseModel>();

        /// <summary>
        /// 企业信息数据
        /// </summary>
        public ObservableCollection<EnterpriseModel> PageEnterpriseInfoList
        {
            get { return pageEnterpriseInfoList; }
            set { Set(ref pageEnterpriseInfoList, value); }
        }

        private int pageCount = 1;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageCount
        {
            get { return pageCount; }
            set { Set(ref pageCount, value); }
        }

        private int pageSize = 5;
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { Set(ref pageSize, value); }
        }

        private int totalPageCount = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get { return totalPageCount; }
            set { Set(ref totalPageCount, value); }
        }

        private string comboboxContent = "";
        /// <summary>
        /// combobox内容
        /// </summary>
        public string ComboboxContent
        {
            get { return comboboxContent; }
            set { Set(ref comboboxContent, value); }
        }

        private List<int> pageSizeList = new List<int>();
        /// <summary>
        /// 页码list
        /// </summary>
        public List<int> PageSizeList
        {
            get { return pageSizeList; }
            set { Set(ref pageSizeList, value); }
        }

        private ObservableCollection<EnterpriseModel> showEnterpriseInfoList = new ObservableCollection<EnterpriseModel>();

        /// <summary>
        /// 企业信息数据
        /// </summary>
        public ObservableCollection<EnterpriseModel> ShowEnterpriseInfoList
        {
            get { return showEnterpriseInfoList; }
            set { Set(ref showEnterpriseInfoList, value); }
        }

        /// <summary>
        /// 页号改变命令
        /// </summary>
        public ICommand PageCountChangedCommand { get; set; }

        /// <summary>
        /// 页面容量改变命令
        /// </summary>
        public ICommand PageSizeChangedCommand { get; set; }

        /// <summary>
        /// 总页数改变命令
        /// </summary>
        public ICommand TotalPageCountChangedCommand { get; set; }

        /// <summary>
        /// ComboBox搜索命令
        /// </summary>
        public ICommand ComboxSearchCommand { get; set; }

        /// <summary>
        /// 企业名称改变命令
        /// </summary>
        public ICommand EnterpriseNameChangedCommand { get; set; }

        private void OnPageCountChangedCommand()
        {
            if (EnterpriseInfoList.Any())
            {
                ShowEnterpriseInfoList = EnterpriseInfoList.Skip((PageCount - 1) * pageSize).Take(pageSize).ToObservableCollection<EnterpriseModel>();
            }
        }

        private bool OnCanPageCountChangedCommand()
        {
            return true;
        }

        private void OnPageSizeChangedCommand()
        {
            if (EnterpriseInfoList.Any())
            {
                TotalPageCount = (int)Math.Ceiling((decimal)EnterpriseInfoList.Count() / pageSize);
                ShowEnterpriseInfoList = EnterpriseInfoList.Skip((PageCount - 1) * pageSize).Take(pageSize).ToObservableCollection<EnterpriseModel>();
            }
        }

        private bool OnCanPageSizeChangedCommand()
        {
            return true;
        }

        private void OnTotalPageCountChangedCommand()
        {

        }

        private bool OnCanTotalPageCountChangedCommand()
        {
            return true;
        }

        private void OnSearchChangedCommand()
        {
            if (String.IsNullOrEmpty(ComboboxContent))
            {
                pageRepuestParams.FilterGroup = null;
                getPageData(1, 10000);
                TotalPageCount = (int)Math.Ceiling((decimal)EnterpriseInfoList.Count() / pageSize);
                PageCount = 1;
                ShowEnterpriseInfoList = EnterpriseInfoList.Skip((PageCount - 1) * pageSize).Take(pageSize).ToObservableCollection<EnterpriseModel>();
            }
            else
            {
                FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
                FilterRule filterRuleName = new FilterRule("EnterpriseName", ComboboxContent.Trim(), "contains");
                filterGroup.Rules.Add(filterRuleName);

                pageRepuestParams.FilterGroup = filterGroup;
                getPageData(1, 10000);
                TotalPageCount = (int)Math.Ceiling((decimal)EnterpriseInfoList.Count() / pageSize);
                PageCount = 1;
                ShowEnterpriseInfoList = EnterpriseInfoList.Skip((PageCount - 1) * pageSize).Take(pageSize).ToObservableCollection<EnterpriseModel>();
            }
        }

        private bool OnCanSearchChangedCommand()
        {
            return true;
        }

        public void OnEnterpriseNameChangedCommand()
        {
            ComboboxContent = EnterpriseInfo.EnterpriseName;
        }

        private bool OnCanEnterpriseNameChangedCommand()
        {
            return true;
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
            return EntSiteInfoModel == null ? false : EntSiteInfoModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EntSiteInfo/Add",
                Utility.JsonHelper.ToJson(new List<EntSiteInfoModel> { EntSiteInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<EntSiteInfoModel>(EntSiteInfoModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EntSiteInfoModel>();
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
        #region 企业信息模型,用于列表数据显示
        private EnterpriseModel enterpriseInfo = new EnterpriseModel();
        public EnterpriseModel EnterpriseInfo
        {
            get { return enterpriseInfo; }
            set { Set(ref enterpriseInfo, value); }
        }


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
    }
}
