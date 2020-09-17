using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.MatWareHouseLocationInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Solution.Desktop.MatWareHouseAreaInfo.Model;
using Solution.Utility.Extensions;

namespace Solution.Desktop.MatWareHouseLocationInfo.ViewModel
{
    /// <summary>
    /// 编辑VM
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MatWareHouseLocationInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatWareHouseLocationInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //
            getPageDataMatWareHouse(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getPageDataMatWareHouseArea(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }
        public override void OnParamterChanged(object parameter)
        {
            this.MatWareHouseLocationInfo = parameter as MatWareHouseLocationInfoModel;
        }

        #region 仓库库位模型
        private MatWareHouseLocationInfoModel WareHouseLocationModel = new MatWareHouseLocationInfoModel();
        /// <summary>
        /// 仓库库位模型
        /// </summary>
        public MatWareHouseLocationInfoModel MatWareHouseLocationInfo
        {
            get { return WareHouseLocationModel; }
            set
            {
                Set(ref WareHouseLocationModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 库位类型模型 
        System.Array warehouselocationTypes = Enum.GetValues(typeof(WareHouseLocationTypeEnumModel.WareHouseLocationType)).Cast<WareHouseLocationTypeEnumModel.WareHouseLocationType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array WareHouseLocationTypes
        {
            get { return warehouselocationTypes; }
            set
            {
                Set(ref warehouselocationTypes, value);
            }
        }
        #endregion

        #region 库位状态模型 
        System.Array warehouselocationStatus = Enum.GetValues(typeof(WareHouseLocationStatusEnumModel.WareHouseLocationStatus)).Cast<WareHouseLocationStatusEnumModel.WareHouseLocationStatus>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array WareHouseLocationStatus
        {
            get { return warehouselocationStatus; }
            set
            {
                Set(ref warehouselocationStatus, value);
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
            return MatWareHouseLocationInfo == null ? false : MatWareHouseLocationInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MatWareHouseLocationInfo/Update",
                Utility.JsonHelper.ToJson(new List<MatWareHouseLocationInfoModel> { MatWareHouseLocationInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MatWareHouseLocationInfoModel>(MatWareHouseLocationInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
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

        #region 库位信息模型,用于列表数据显示
        private ObservableCollection<MatWareHouseInfoModel> matwarehouseModelList = new ObservableCollection<MatWareHouseInfoModel>();

        /// <summary>
        /// 库位信息数据
        /// </summary>
        public ObservableCollection<MatWareHouseInfoModel> MatWareHouseInfoList
        {
            get { return matwarehouseModelList; }
            set { Set(ref matwarehouseModelList, value); }
        }

        #endregion

        /// ////////////////////////////////////////////////////////////////////////

        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

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


        #region 分页数据查询
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// 
        /// <summary>
        /// 获取仓库列表
        /// </summary>
        private void getPageDataMatWareHouse(int pageIndex, int pageSize)
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
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/WareHouseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseInfoModel>>>(GlobalData.ServerRootUri + "MatWareHouseInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取仓库信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("仓库信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MatWareHouseInfoList = new ObservableCollection<MatWareHouseInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MatWareHouseInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatWareHouseInfoList = new ObservableCollection<MatWareHouseInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询仓库信息失败，请联系管理员！";
            }
        }
        #endregion

        #region 仓库货区信息模型,用于列表数据显示
        private ObservableCollection<MatWareHouseAreaInfoModel> matwarehouseareaModelList = new ObservableCollection<MatWareHouseAreaInfoModel>();

        /// <summary>
        /// 仓库货区信息数据
        /// </summary>
        public ObservableCollection<MatWareHouseAreaInfoModel> MatWareHouseAreaInfoList
        {
            get { return matwarehouseareaModelList; }
            set { Set(ref matwarehouseareaModelList, value); }
        }

        #endregion

        /// //////////////////////////////////////////////////////////////////////// 

        #region 分页数据查询
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// 
        /// <summary>
        /// 获取仓库列表
        /// </summary>
        private void getPageDataMatWareHouseArea(int pageIndex, int pageSize)
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
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/WareHouseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseAreaInfoModel>>>(GlobalData.ServerRootUri + "MatWareHouseAreaInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取仓库货区信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("仓库货区信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MatWareHouseAreaInfoList = new ObservableCollection<MatWareHouseAreaInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MatWareHouseAreaInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatWareHouseAreaInfoList = new ObservableCollection<MatWareHouseAreaInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询仓库货区信息失败，请联系管理员！";
            }
        }
        #endregion
    }
}
