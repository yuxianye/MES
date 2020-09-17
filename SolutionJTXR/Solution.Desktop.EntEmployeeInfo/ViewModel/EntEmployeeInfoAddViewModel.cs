using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
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
using Solution.Desktop.Core.Model;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.EntEmployeeInfo.Model;
using Solution.Desktop.EntEmployeeInfo.EnumType;
using Solution.Utility.Extensions;
using Solution.Desktop.EntDepartmentInfo.Model;

namespace Solution.Desktop.EntEmployeeInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EntEmployeeInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntEmployeeInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            SelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSelectionChangedCommand);
            getPageData(1, 200);
            
        }

        #region 区域模型
        private EntEmployeeInfoModel entEmployeeInfoModel = new EntEmployeeInfoModel();
        private EntDepartmentInfoModel entDepartmentInfoModel = new EntDepartmentInfoModel();
       
        /// <summary>
        /// 区域模型
        /// </summary>
        public EntEmployeeInfoModel EntEmployeeInfoModel
        {
            get { return entEmployeeInfoModel; }
            set
            {
                Set(ref entEmployeeInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public EntDepartmentInfoModel EntDepartmentInfoModel
        {
            get { return entDepartmentInfoModel; }
            set
            {
                Set(ref entDepartmentInfoModel, value);
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
            return EntEmployeeInfoModel == null ? false : EntEmployeeInfoModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EntEmployeeInfo/Add",
                Utility.JsonHelper.ToJson(new List<EntEmployeeInfoModel> { EntEmployeeInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                //Application.Current.Resources["UiMessage"] = result?.Message;
                //LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EntEmployeeInfoModel>(EntEmployeeInfoModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EntAreaInfoModel>();
                //Application.Current.Resources["UiMessage"] = result?.Message ?? "操作失败，请联系管理员！";
                //LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                UiMessage = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(UiMessage);
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
            //EntDepartmentInfoList?.Clear();
            //this.EntEmployeeInfoModel.DepartmentInfo_Id = Guid.Empty;
            //getPageData1();

            //EntEmployeeInfoModel.DepartmentInfoName = EntDepartmentInfoList
        }
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 企业信息模型,用于下拉列表数据显示
        private ObservableCollection<EntDepartmentInfoModel> entDepartmentInfoList = new ObservableCollection<EntDepartmentInfoModel>();

        /// <summary>
        /// 企业信息数据
        /// </summary>
        public ObservableCollection<EntDepartmentInfoModel> EntDepartmentInfoList
        {
            get { return entDepartmentInfoList; }
            set { Set(ref entDepartmentInfoList, value); }
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
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntDepartmentInfoModel>>>(GlobalData.ServerRootUri + "EntDepartmentInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

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
                    EntDepartmentInfoList = new ObservableCollection<EntDepartmentInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EntDepartmentInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EntDepartmentInfoList = new ObservableCollection<EntDepartmentInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询企业信息失败，请联系管理员！";
            }
        }

//        private void getPageData1()
//        {
//#if DEBUG
//            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//            stopwatch.Start();
//#endif

//            entDepartmentInfoModel.Id= EntEmployeeInfoModel.DepartmentInfo_Id;


            
//            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntDepartmentInfoModel>>>(GlobalData.ServerRootUri + "EntDepartmentInfo/PageData", Utility.JsonHelper.ToJson(entDepartmentInfoModel));

//#if DEBUG
//            stopwatch.Stop();
//            Utility.LogHelper.Info("获取厂区信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
//            Utility.LogHelper.Info("厂区信息内容：" + Utility.JsonHelper.ToJson(result));
//#endif

//            if (!Equals(result, null) && result.Successed)
//            {
//                Application.Current.Resources["UiMessage"] = result?.Message;
//                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
//                if (result.Data.Data.Any())
//                {
//                    //TotalCounts = result.Data.Total;
//                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
//                    EntDepartmentInfoList = new ObservableCollection<EntDepartmentInfoModel>(result.Data.Data);
//                    TotalCounts = result.Data.Total;
//                }
//                else
//                {
//                    EntSiteInfoList?.Clear();
//                    TotalCounts = 0;
//                    Application.Current.Resources["UiMessage"] = "未找到数据";
//                }
//            }
//            else
//            {
//                //操作失败，显示错误信息
//                EntSiteInfoList = new ObservableCollection<EntSiteInfoModel>();
//                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询厂区信息失败，请联系管理员！";
//            }
//        }

        #region 性别
        System.Array sexs = Enum.GetValues(typeof(Sex)).Cast<Sex>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array Sexs
        {
            get { return sexs; }
            set
            {
                Set(ref sexs, value);
            }
        }
        #endregion

        #region 职务
        System.Array workPosts = Enum.GetValues(typeof(Duty)).Cast<Duty>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array WorkPosts
        {
            get { return workPosts; }
            set
            {
                Set(ref workPosts, value);
            }
        }
        #endregion

        #region 工种
        System.Array workBranchs = Enum.GetValues(typeof(WorkType)).Cast<WorkType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array WorkBranchs
        {
            get { return workBranchs; }
            set
            {
                Set(ref workBranchs, value);
            }
        }
        #endregion

        #region 技能
        System.Array skillses = Enum.GetValues(typeof(Skills)).Cast<Skills>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array Skillses
        {
            get { return skillses; }
            set
            {
                Set(ref skillses, value);
            }
        }
        #endregion

        #region 学历
        System.Array educations = Enum.GetValues(typeof(Education)).Cast<Education>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array Educations
        {
            get { return educations; }
            set
            {
                Set(ref educations, value);
            }
        }
        #endregion

        #region 职称
        System.Array professionalTitleses = Enum.GetValues(typeof(PositionalTitles)).Cast<PositionalTitles>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array ProfessionalTitleses
        {
            get { return professionalTitleses; }
            set
            {
                Set(ref professionalTitleses, value);
            }
        }
        #endregion
    }
}
