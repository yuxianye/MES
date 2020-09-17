using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.MatStorageMoveInfo.Model;
using Solution.Desktop.MatWareHouseLocationInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.MatStorageMoveInfo.ViewModel
{
    /// <summary>
    /// 编辑VM
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MatStorageMoveInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatStorageMoveInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //
            getStorageMoveInfoFromWareHouseLocationID();
            getStorageMoveInfoToWareHouseLocationID();            
        }
        public override void OnParamterChanged(object parameter)
        {
            this.MatStorageMoveInfo = parameter as MatStorageMoveInfoModel;
            //
            getPageDataWareHouseLocation(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        #region 物料入库模型
        private MatStorageMoveInfoModel matwarehousetypeModel = new MatStorageMoveInfoModel();
        /// <summary>
        /// 物料入库模型
        /// </summary>
        public MatStorageMoveInfoModel MatStorageMoveInfo
        {
            get { return matwarehousetypeModel; }
            set
            {
                Set(ref matwarehousetypeModel, value);
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
            return MatStorageMoveInfo == null ? false : MatStorageMoveInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MatStorageMoveInfo/Update",
                Utility.JsonHelper.ToJson(new List<MatStorageMoveInfoModel> { MatStorageMoveInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MatStorageMoveInfoModel>(MatStorageMoveInfo, MessengerToken.DataChanged);
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

        ////////////////////////////////////////////////////////////

        #region 仓库库位信息模型,用于列表数据显示
        private ObservableCollection<MatWareHouseLocationInfoModel> frommatwarehouselocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();

        /// <summary>
        /// 仓库库位信息数据
        /// </summary>
        public ObservableCollection<MatWareHouseLocationInfoModel> FromMatWareHouseLocationInfoList
        {
            get { return frommatwarehouselocationInfoList; }
            set { Set(ref frommatwarehouselocationInfoList, value); }
        }
        #endregion


        #region 仓库库位信息模型,用于列表数据显示
        private ObservableCollection<MatWareHouseLocationInfoModel> tomatwarehouselocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();

        /// <summary>
        /// 仓库库位信息数据
        /// </summary>
        public ObservableCollection<MatWareHouseLocationInfoModel> ToMatWareHouseLocationInfoList
        {
            get { return tomatwarehouselocationInfoList; }
            set { Set(ref tomatwarehouselocationInfoList, value); }
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

        public List<string> StorageMoveInfoFromWareHouseLocationIDList;

        void getStorageMoveInfoFromWareHouseLocationID()
        {
            var storagemoveinfofromwarehouselocationid = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<string>>>
                         (GlobalData.ServerRootUri + "MatStorageMoveInfo/GetFromWareHouseLocationID");
            //
            StorageMoveInfoFromWareHouseLocationIDList = new List<string>();
            if (storagemoveinfofromwarehouselocationid != null)
            {
                StorageMoveInfoFromWareHouseLocationIDList = storagemoveinfofromwarehouselocationid.Data;
            }
        }


        public List<string> StorageMoveInfoToWareHouseLocationIDList;

        void getStorageMoveInfoToWareHouseLocationID()
        {
            var storagemoveinfotowarehouselocationid = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<string>>>
                         (GlobalData.ServerRootUri + "MatStorageMoveInfo/GetToWareHouseLocationID");
            //
            StorageMoveInfoToWareHouseLocationIDList = new List<string>();
            if (storagemoveinfotowarehouselocationid != null)
            {
                StorageMoveInfoToWareHouseLocationIDList = storagemoveinfotowarehouselocationid.Data;
            }
        }

        #region 分页数据查询
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// 
        /// <summary>
        /// 获取库位列表
        /// </summary>
        private void getPageDataWareHouseLocation(int pageIndex, int pageSize)
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseLocationInfoModel>>>
                  (GlobalData.ServerRootUri + "MatWareHouseLocationInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取仓库库位用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("仓库库位内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    var MatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>(result.Data.Data);
                    foreach (MatWareHouseLocationInfoModel matwarehouselocationInfo in MatWareHouseLocationInfoList)
                    {
                        if (matwarehouselocationInfo.PalletCode != null)
                        {
                            int i = StorageMoveInfoFromWareHouseLocationIDList.Count(m => m.Contains(matwarehouselocationInfo.Id.ToString()));
                            if (i == 0 || matwarehouselocationInfo.Id == MatStorageMoveInfo.FromLocationID)
                            {
                                FromMatWareHouseLocationInfoList.Add(matwarehouselocationInfo);
                            }
                        }
                        else
                        {
                            int i = StorageMoveInfoToWareHouseLocationIDList.Count(m => m.Contains(matwarehouselocationInfo.Id.ToString()));
                            if (i == 0 || matwarehouselocationInfo.Id == MatStorageMoveInfo.ToLocationID)
                            {
                                ToMatWareHouseLocationInfoList.Add(matwarehouselocationInfo);
                            }
                        }
                    }
                    //
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    FromMatWareHouseLocationInfoList?.Clear();
                    ToMatWareHouseLocationInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                FromMatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();
                ToMatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询仓库库位失败，请联系管理员！";
            }
        }
        #endregion

    }
}
