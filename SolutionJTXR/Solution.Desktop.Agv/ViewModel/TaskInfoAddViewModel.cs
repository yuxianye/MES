using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.Agv.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using Solution.Utility.Extensions;

namespace Solution.Desktop.Agv.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class TaskInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);


            getMarkPointInfoPageData(1, 200);
            getAgvInfoPageData(1, 200);
            getRouteInfoPageData(1, 200);

        }

        #region 模型
        private TaskInfoModel taskInfoModel = new TaskInfoModel();
        /// <summary>
        /// 模型
        /// </summary>
        public TaskInfoModel TaskInfoModel
        {
            get { return taskInfoModel; }
            set
            {
                Set(ref taskInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region MarkPointInfo模型,用于列表数据显示
        private ObservableCollection<MarkPointInfoModel> markPointInfoList = new ObservableCollection<MarkPointInfoModel>();

        /// <summary>
        /// MarkPointInfo信息数据
        /// </summary>
        public ObservableCollection<MarkPointInfoModel> MarkPointInfoList
        {
            get { return markPointInfoList; }
            set { Set(ref markPointInfoList, value); }
        }
        #endregion

        #region AgvInfo模型,用于列表数据显示
        private ObservableCollection<AgvInfoModel> agvInfoList = new ObservableCollection<AgvInfoModel>();

        /// <summary>
        /// AgvInfo信息数据
        /// </summary>
        public ObservableCollection<AgvInfoModel> AgvInfoList
        {
            get { return agvInfoList; }
            set { Set(ref agvInfoList, value); }
        }
        #endregion

        #region RouteInfo模型,用于列表数据显示
        private ObservableCollection<RouteInfoModel> routeInfoList = new ObservableCollection<RouteInfoModel>();

        /// <summary>
        /// RouteInfo信息数据
        /// </summary>
        public ObservableCollection<RouteInfoModel> RouteInfoList
        {
            get { return routeInfoList; }
            set { Set(ref routeInfoList, value); }
        }
        #endregion


        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 取得地标点分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getMarkPointInfoPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MarkPointInfoModel>>>(GlobalData.ServerRootUri + "MarkPointInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取地标信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("地标信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MarkPointInfoList = new ObservableCollection<MarkPointInfoModel>(result.Data.Data);
                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    MarkPointInfoList?.Clear();
                    //TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MarkPointInfoList = new ObservableCollection<MarkPointInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询地标信息失败，请联系管理员！";
            }
        }

        /// <summary>
        /// 取得Agv分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getAgvInfoPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<AgvInfoModel>>>(GlobalData.ServerRootUri + "AgvInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取地标信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("地标信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    AgvInfoList = new ObservableCollection<AgvInfoModel>(result.Data.Data);
                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    AgvInfoList?.Clear();
                    //TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                AgvInfoList = new ObservableCollection<AgvInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询地标信息失败，请联系管理员！";
            }
        }

        /// <summary>
        /// 取得Route分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getRouteInfoPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<RouteInfoModel>>>(GlobalData.ServerRootUri + "RouteInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取地标信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("地标信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    RouteInfoList = new ObservableCollection<RouteInfoModel>(result.Data.Data);
                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    RouteInfoList?.Clear();
                    //TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                RouteInfoList = new ObservableCollection<RouteInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询地标信息失败，请联系管理员！";
            }
        }
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
            return TaskInfoModel == null ? false : TaskInfoModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {

            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "TaskInfo/Add",
                Utility.JsonHelper.ToJson(new List<TaskInfoModel> { TaskInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<TaskInfoModel>(TaskInfoModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
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

        #region 任务类型
        System.Array taskTypes = Enum.GetValues(typeof(TaskType)).Cast<TaskType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array TaskTypes
        {
            get { return taskTypes; }
            set
            {
                Set(ref taskTypes, value);
            }
        }
        #endregion

        #region 任务状态
        System.Array taskStatusTypes = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array TaskStatusTypes
        {
            get { return taskStatusTypes; }
            set
            {
                Set(ref taskStatusTypes, value);
            }
        }
        #endregion
    }
}
