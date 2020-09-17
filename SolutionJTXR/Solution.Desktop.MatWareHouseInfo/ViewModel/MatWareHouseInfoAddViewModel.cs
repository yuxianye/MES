using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EntAreaInfo.Model;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.EntProductionLineInfo.Model;
using Solution.Desktop.EntSiteInfo.Model;
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
    /// 新增VM
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MatWareHouseInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatWareHouseInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //
            enterpriseSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteenterpriseSelectionChangedCommand);
            siteSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutesiteSelectionChangedCommand);
            //
            getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getPageDataMatWareHouseType(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        private EnterpriseModel enterpriseModel = new EnterpriseModel();
        private EntSiteInfoModel entsiteModel = new EntSiteInfoModel();

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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MatWareHouseInfo/Add",
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


        private void OnExecuteenterpriseSelectionChangedCommand()
        {
            getPageData1();
            //
            if (MatWareHouseInfo != null)
            {
                EntProductionLineInfo.EntSite_Id = Guid.Empty;
                //
                MatWareHouseInfo.EntArea_Id = Guid.Empty;
                EntAreaInfoList?.Clear();
            };
        }

        private void OnExecutesiteSelectionChangedCommand()
        {
            getPageData2();
        }

        /// ///////////////////////////////////////////

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
            //
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
