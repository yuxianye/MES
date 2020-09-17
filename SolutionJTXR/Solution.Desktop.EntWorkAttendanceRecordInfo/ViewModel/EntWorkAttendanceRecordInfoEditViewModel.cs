using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EntDepartmentInfo.Model;
using Solution.Desktop.EntEmployeeInfo.Model;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.EntSiteInfo.Model;
using Solution.Desktop.EntWorkAttendanceRecordInfo.EnumType;
using Solution.Desktop.EntWorkAttendanceRecordInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using Solution.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.EntWorkAttendanceRecordInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EntWorkAttendanceRecordInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntWorkAttendanceRecordInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            SelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSelectionChangedCommand);
            getPageData(1, 200);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.EntWorkAttendanceRecordInfoModel = parameter as EntWorkAttendanceRecordInfoModel;
            //getPageData1();
        }

        #region 车间模型
        private EntEmployeeInfoModel entEmployeeInfoModel = new EntEmployeeInfoModel();
        private EntWorkAttendanceRecordInfoModel entWorkAttendanceRecordInfoModel = new EntWorkAttendanceRecordInfoModel();
        /// <summary>
        /// 车间模型
        /// </summary>
        public EntWorkAttendanceRecordInfoModel EntWorkAttendanceRecordInfoModel
        {
            get { return entWorkAttendanceRecordInfoModel; }
            set
            {
                Set(ref entWorkAttendanceRecordInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public EntEmployeeInfoModel EntEmployeeInfoModel
        {
            get { return entEmployeeInfoModel; }
            set
            {
                Set(ref entEmployeeInfoModel, value);
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
            return EntWorkAttendanceRecordInfoModel == null ? false : EntWorkAttendanceRecordInfoModel.IsValidated;
        }
        public ICommand SelectionChangedCommand { get; set; }

        /// <summary>
        /// 执行下拉列表改变事件
        /// </summary>
        private void OnExecuteSelectionChangedCommand()
        {
            //EntSiteInfoList?.Clear();
            //this.EntEmployeeInfoModel.DepartmentCode = Guid.Empty;
            //getPageData1();            
        }
        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EntWorkAttendanceRecordInfo/Update",
                Utility.JsonHelper.ToJson(new List<EntWorkAttendanceRecordInfoModel> { EntWorkAttendanceRecordInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                //Application.Current.Resources["UiMessage"] = result?.Message;
                //LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EntWorkAttendanceRecordInfoModel>(EntWorkAttendanceRecordInfoModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<EntAreaInfoModel>();
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

        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 企业信息模型,用于下拉列表数据显示
        private ObservableCollection<EntEmployeeInfoModel> entEmployeeInfoList = new ObservableCollection<EntEmployeeInfoModel>();

        /// <summary>
        /// 企业信息数据
        /// </summary>
        public ObservableCollection<EntEmployeeInfoModel> EntEmployeeInfoList
        {
            get { return entEmployeeInfoList; }
            set { Set(ref entEmployeeInfoList, value); }
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
            
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntEmployeeInfoModel>>>(GlobalData.ServerRootUri + "EntEmployeeInfo/GetEntEmployeeList", Utility.JsonHelper.ToJson(pageRepuestParams));

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
                    EntEmployeeInfoList = new ObservableCollection<EntEmployeeInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EntEmployeeInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EntEmployeeInfoList = new ObservableCollection<EntEmployeeInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询企业信息失败，请联系管理员！";
            }
        }

        #region 请假类型
        System.Array leaveTypes = Enum.GetValues(typeof(LeaveType)).Cast<LeaveType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array LeaveTypes
        {
            get { return leaveTypes; }
            set
            {
                Set(ref leaveTypes, value);
            }
        }
        #endregion

        #region 班次
        System.Array shiftss = Enum.GetValues(typeof(Shifts)).Cast<Shifts>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array Shiftss
        {
            get { return shiftss; }
            set
            {
                Set(ref shiftss, value);
            }
        }
        #endregion
    }
}