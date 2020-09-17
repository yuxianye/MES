using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Communication.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using Solution.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.Communication.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class DeviceNodeEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeviceNodeEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getServerPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }
        public override void OnParamterChanged(object parameter)
        {
            this.DeviceNodeModel = parameter as DeviceNodeModel;
            this.DeviceServerInfo = DeviceServerInfoList.Where(x => x.DeviceServerName.Equals(DeviceNodeModel.DeviceServerInfoName)).FirstOrDefault();
        }

        #region 设备数据点数据模型
        /// <summary>
        /// 设备数据点数据模型
        /// </summary>
        private DeviceNodeModel deviceNodeModel;// = new EnterpriseModel();
        /// <summary>
        /// 设备数据点数据模型
        /// </summary>
        public DeviceNodeModel DeviceNodeModel
        {
            get { return deviceNodeModel; }
            set
            {
                Set(ref deviceNodeModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 数据类型      
        System.Array dataTypes = Enum.GetValues(typeof(DataType)).Cast<DataType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array DataTypes
        {
            get { return dataTypes; }
            set
            {
                Set(ref dataTypes, value);
            }
        }
        #endregion

        #region 设备服务器数据模型
        private DeviceServerInfoModel deviceServerInfo;

        public DeviceServerInfoModel DeviceServerInfo
        {
            get { return deviceServerInfo; }
            set { Set(ref deviceServerInfo, value); }
        }
        #endregion


        #region 设备服务器数据模型,用于列表数据显示
        private ObservableCollection<DeviceServerInfoModel> deviceServerInfoList = new ObservableCollection<DeviceServerInfoModel>();

        /// <summary>
        /// 设备数据点数据
        /// </summary>
        public ObservableCollection<DeviceServerInfoModel> DeviceServerInfoList
        {
            get { return deviceServerInfoList; }
            set { Set(ref deviceServerInfoList, value); }
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
            return DeviceNodeModel == null ? false : DeviceNodeModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            if (Equals(DeviceServerInfo, null))
            {
                Application.Current.Resources["UiMessage"] = "请选择设备服务器！";
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            }
            else if (DeviceServerInfo != null)
            {
                DeviceNodeModel.DeviceServerInfo_Id = DeviceServerInfo.Id;
                DeviceNodeModel.DeviceServerInfoName = DeviceServerInfo.DeviceServerName;
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "DeviceNode/Update",
                    Utility.JsonHelper.ToJson(new List<DeviceNodeModel> { DeviceNodeModel }));

                if (!Equals(result, null) && result.Successed)
                {
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    Messenger.Default.Send<DeviceNodeModel>(DeviceNodeModel, MessengerToken.DataChanged);
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
        }

        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getServerPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<DeviceServerInfoModel>>>(GlobalData.ServerRootUri + "DeviceServerInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取设备服务器信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("设备服务器信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    DeviceServerInfoList = new ObservableCollection<DeviceServerInfoModel>(result.Data.Data);
                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    DeviceServerInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                DeviceServerInfoList = new ObservableCollection<DeviceServerInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询设备服务器信息失败，请联系管理员！";
            }
        }

        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }
    }
}
