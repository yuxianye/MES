using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EntAreaInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.MatWareHouseTypeInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.MatWareHouseInfo.ViewModel
{
    /// <summary>
    /// 编辑VM
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MatWareHouseInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatWareHouseInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //
            getPageDataEntArea(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getPageDataMatWareHouseType(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }
        public override void OnParamterChanged(object parameter)
        {
            this.MatWareHouseInfo = parameter as MatWareHouseInfoModel;
        }

        #region 仓库模型
        private MatWareHouseInfoModel matwarehouseModel = new MatWareHouseInfoModel();
        /// <summary>
        /// 仓库模型
        /// </summary>
        public MatWareHouseInfoModel MatWareHouseInfo
        {
            get { return matwarehouseModel; }
            set
            {
                Set(ref matwarehouseModel, value);
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
            return MatWareHouseInfo == null ? false : MatWareHouseInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MatWareHouseInfo/Update",
                Utility.JsonHelper.ToJson(new List<MatWareHouseInfoModel> { MatWareHouseInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MatWareHouseInfoModel>(MatWareHouseInfo, MessengerToken.DataChanged);
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

        private void getPageDataEntArea(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntAreaInfoModel>>>(GlobalData.ServerRootUri + "EntAreaInfo/GetEntAreaList", Utility.JsonHelper.ToJson(pageRepuestParams));

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


        #region 仓库类型信息模型,用于列表数据显示
        private ObservableCollection<MatWareHouseTypeInfoModel> matwarehousetypeModelList = new ObservableCollection<MatWareHouseTypeInfoModel>();

        /// <summary>
        /// 仓库类型信息数据
        /// </summary>
        public ObservableCollection<MatWareHouseTypeInfoModel> MatWareHouseTypeInfoList
        {
            get { return matwarehousetypeModelList; }
            set { Set(ref matwarehousetypeModelList, value); }
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
        /// 获取仓库类型列表
        /// </summary>
        private void getPageDataMatWareHouseType(int pageIndex, int pageSize)
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseTypeInfoModel>>>(GlobalData.ServerRootUri + "MatWareHouseTypeInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取仓库类型信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("仓库类型信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MatWareHouseTypeInfoList = new ObservableCollection<MatWareHouseTypeInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MatWareHouseTypeInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatWareHouseTypeInfoList = new ObservableCollection<MatWareHouseTypeInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询仓库类型信息失败，请联系管理员！";
            }
        }
        #endregion
    }
}
