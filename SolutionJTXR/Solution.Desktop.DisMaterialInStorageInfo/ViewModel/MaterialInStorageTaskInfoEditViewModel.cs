using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.CommSocketServer.Model;
using Solution.Desktop.Core;
using Solution.Desktop.MaterialBatchInfo.Model;
using Solution.Desktop.MaterialInfo.Model;
using Solution.Desktop.MaterialInStorageInfo.Model;
using Solution.Desktop.MatPalletInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.MatWareHouseLocationInfo.Model;
using Solution.Desktop.Model;
using Solution.Desktop.RoleManager.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.MaterialInStorageInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class MaterialInStorageTaskInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialInStorageTaskInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //
            getPageDataMaterial(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getPageDataMatPallet(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        public override void OnParamterChanged(object parameter)
        {
            this.MaterialInStorageInfo = parameter as MaterialInStorageInfoModel;
            //
            if (MaterialInStorageInfo.InStorageType == InStorageTypeEnumModel.InStorageType.空托盘入库)
            {
                getPageDataMatWareHouseLocation(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")), 2);
            }
            else if (MaterialInStorageInfo.InStorageType == InStorageTypeEnumModel.InStorageType.原料手动入库)
            {
                getPageDataMatWareHouseLocation(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")), 1);
            }
            //
            ////系统管理员
            //int UserInfo_Id = 1;
            ////var roleIds = GetRolesByUserId(UserInfo_Id);
            //Guid[] roleIds = new Guid[5];

            //if (roleIds.Any())
            //{
            //    foreach (var id in roleIds)
            //    {
            //        for (int i = 0; i < MatWareHouseLocationInfoList.Count; i++)
            //        {
            //            if (id == MatWareHouseLocationInfoList[i].Id)
            //            {
            //                MatWareHouseLocationInfoList[i].IsChecked = true;
            //            }
            //        }
            //    }
            //}
        }

        //public List<int> GetRolesByUserId(int userId)
        //{
        //    var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<int>>>(GlobalData.ServerRootUri + $"UserManager/GetRolesByUserId/{userId}");
        //    return result.Data;
        //}

        /// ///////////////////////////////////////////////////////////

        #region 库位信息模型,用于列表数据显示
        private ObservableCollection<MatWareHouseLocationInfoModel> matwarehouselocationinfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();

        /// <summary>
        /// 库位信息数据
        /// </summary>
        public ObservableCollection<MatWareHouseLocationInfoModel> MatWareHouseLocationInfoList
        {
            get { return matwarehouselocationinfoList; }
            set { Set(ref matwarehouselocationinfoList, value); }
        }
        #endregion
    
        private ObservableCollection<MatWareHouseLocationInfoModel> matwarehouselocationinfoDataSource = new ObservableCollection<MatWareHouseLocationInfoModel>();

        public ObservableCollection<MatWareHouseLocationInfoModel> MatWareHouseLocationInfoDataSource
        {
            get { return matwarehouselocationinfoDataSource; }
            set { Set(ref matwarehouselocationinfoDataSource, value); }
        }

        /// ///////////////////////////////////////////////////////////

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
            return MaterialInStorageInfo == null ? false : MaterialInStorageInfo.IsValidated;
        }

        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            MaterialInStorageInfo.MatWareHouseLocations = MatWareHouseLocationInfoDataSource;
            //
            if ( MaterialInStorageInfo.InStorageType == InStorageTypeEnumModel.InStorageType.空托盘入库)
            {
                if (MatWareHouseLocationInfoDataSource.Count != 1 )
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = "请选择且仅选一个库位！";
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    //
                    return;
                }
            }
            else if (MaterialInStorageInfo.InStorageType == InStorageTypeEnumModel.InStorageType.原料手动入库)
            {
                if (MatWareHouseLocationInfoDataSource.Count != MaterialInStorageInfo.PalletQuantity)
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = "请按配盘数量选择库位，请联系管理员！";
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    //
                    return;
                }
            }
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MaterialInStorageInfo/Distribute",
                Utility.JsonHelper.ToJson(new List<MaterialInStorageInfoModel> { MaterialInStorageInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                //Messenger.Default.Send<MaterialInStorageInfoModel>(MaterialInStorageInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<MaterialInStorageInfoModel>(MaterialInStorageInfo, MessengerToken.DataChanged);
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
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getPageDataMatWareHouseLocation(int pageIndex, int pageSize, int iType)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            //pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            //
            string sType = "";
            //原料手动入库
            if ( iType == 1 )
            {
                sType = "MatWareHouseLocationInfo/PageDataPallet1";
            }
            //空托盘入库
            else if (iType == 2)
            {
                sType = "MatWareHouseLocationInfo/PageDataPallet2";
            }
            //
            //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseLocationInfoModel>>>(GlobalData.ServerRootUri + "MatWareHouseLocationInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseLocationInfoModel>>>(GlobalData.ServerRootUri + sType, Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取库位信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("库位信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //RoleInfoList = new ObservableCollection<RoleModel>(result.Data.Data);
                    MatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();
                    foreach (var data in result.Data.Data)
                    {
                        MatWareHouseLocationInfoModel matwarehouselocationinfo = new MatWareHouseLocationInfoModel();
                        matwarehouselocationinfo = data;
                        //
                        //原料手动入库
                        if (iType == 1)
                        {
                            if ((matwarehouselocationinfo.StorageQuantity == null || matwarehouselocationinfo.StorageQuantity <= 0) &&
                                 matwarehouselocationinfo.WareHouseLocationType == WareHouseLocationTypeEnumModel.WareHouseLocationType.原料库位 )
                            {
                                matwarehouselocationinfo.PropertyChanged += OnPropertyChangedCommand;
                                MatWareHouseLocationInfoList.Add(matwarehouselocationinfo);
                            }
                        }
                        //空托盘入库
                        else if (iType == 2)
                        {
                            if (matwarehouselocationinfo.IsUse &&
                                matwarehouselocationinfo.WareHouseLocationType == WareHouseLocationTypeEnumModel.WareHouseLocationType.原料库位)
                            {
                                matwarehouselocationinfo.PropertyChanged += OnPropertyChangedCommand;
                                MatWareHouseLocationInfoList.Add(matwarehouselocationinfo);
                            }
                        }
                        //
                    }
                    int TotalCounts = result.Data.Total;
                }
                else
                {
                    MatWareHouseLocationInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询库位信息失败，请联系管理员！";
            }
        }

        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                var selectedObj = sender as MatWareHouseLocationInfoModel;
                if (selectedObj == null)
                    return;
                if (selectedObj.IsChecked)
                {
                    if (MaterialInStorageInfo.InStorageType == InStorageTypeEnumModel.InStorageType.空托盘入库)
                    {
                        MatWareHouseLocationInfoDataSource.Clear();
                        var tmplist = MatWareHouseLocationInfoList.Where(a => a.IsChecked == true && a.Id != selectedObj.Id).ToList();
                        for (int i = 0; i < tmplist?.Count(); i++)
                        {
                            tmplist[i].IsChecked = false;
                        }
                    }
                    MatWareHouseLocationInfoDataSource.Add(selectedObj);
                }
                else if (!selectedObj.IsChecked)
                {
                    MatWareHouseLocationInfoDataSource.Remove(selectedObj);
                }
            }
        }

        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }

        ///////////////////////////////////////////////////////////////////////

        #region 物料入库模型
        //即物料入库任务模型
        private MaterialInStorageInfoModel matwarehousetypeModel = new MaterialInStorageInfoModel();
        /// <summary>
        /// 物料入库模型
        /// </summary>
        public MaterialInStorageInfoModel MaterialInStorageInfo
        {
            get { return matwarehousetypeModel; }
            set
            {
                Set(ref matwarehousetypeModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 入库类型模型 
        public System.Array instorageTypes = Enum.GetValues(typeof(InStorageTypeEnumModel.InStorageType));

        public System.Array InStorageTypes
        {
            get { return instorageTypes; }
            set
            {
                Set(ref instorageTypes, value);
            }
        }
        #endregion

        #region 入库状态模型 
        public System.Array instorageStatus = Enum.GetValues(typeof(InStorageStatusEnumModel.InStorageStatus));

        public System.Array InStorageStatus
        {
            get { return instorageStatus; }
            set
            {
                Set(ref instorageStatus, value);
            }
        }
        #endregion

        #region 审核状态模型 
        public System.Array auditStatus = Enum.GetValues(typeof(AuditStatusEnumModel.AuditStatus));

        public System.Array AuditStatus
        {
            get { return auditStatus; }
            set
            {
                Set(ref auditStatus, value);
            }
        }
        #endregion

        ////////////////////////////////////////////////////////////

        #region 物料信息模型,用于列表数据显示
        private ObservableCollection<MaterialInfoModel> materialinfoList = new ObservableCollection<MaterialInfoModel>();

        /// <summary>
        /// 物料信息数据
        /// </summary>
        public ObservableCollection<MaterialInfoModel> MaterialInfoList
        {
            get { return materialinfoList; }
            set { Set(ref materialinfoList, value); }
        }

        #endregion


        #region 托盘信息模型,用于列表数据显示
        private ObservableCollection<MatPalletInfoModel> matpalletinfoList = new ObservableCollection<MatPalletInfoModel>();

        /// <summary>
        /// 托盘信息数据
        /// </summary>
        public ObservableCollection<MatPalletInfoModel> MatPalletInfoList
        {
            get { return matpalletinfoList; }
            set { Set(ref matpalletinfoList, value); }
        }

        #endregion

        /// ////////////////////////////////////////////////////////////////////////


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
        private void getPageDataMaterial(int pageIndex, int pageSize)
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialInfoModel>>>(GlobalData.ServerRootUri + "MaterialInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取物料信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("物料信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MaterialInfoList = new ObservableCollection<MaterialInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialInfoList = new ObservableCollection<MaterialInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询物料信息失败，请联系管理员！";
            }
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
        /// 获取托盘列表
        /// </summary>
        private void getPageDataMatPallet(int pageIndex, int pageSize)
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatPalletInfoModel>>>(GlobalData.ServerRootUri + "MatPalletInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取托盘信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("托盘信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MatPalletInfoList = new ObservableCollection<MatPalletInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MatPalletInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatPalletInfoList = new ObservableCollection<MatPalletInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询托盘信息失败，请联系管理员！";
            }
        }
        #endregion
        ///////////////////////////////////////////////////////////////////////
    }
}
