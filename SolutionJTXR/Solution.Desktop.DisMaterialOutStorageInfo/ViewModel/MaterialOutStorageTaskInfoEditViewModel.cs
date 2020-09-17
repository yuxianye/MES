using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.CommSocketServer.Model;
using Solution.Desktop.Core;
using Solution.Desktop.MaterialBatchInfo.Model;
using Solution.Desktop.MaterialInfo.Model;
using Solution.Desktop.MaterialOutStorageInfo.Model;
using Solution.Desktop.MatPalletInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.Model;
using Solution.Desktop.RoleManager.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.MaterialOutStorageInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class MaterialOutStorageTaskInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialOutStorageTaskInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //
            getPageDataMaterial(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getPageDataMatPallet(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        public override void OnParamterChanged(object parameter)
        {
            this.MaterialOutStorageInfo = parameter as MaterialOutStorageInfoModel;
            //
            if (MaterialOutStorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.空托盘出库)
            {
                getPageDataMaterialBatch(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")), 2);
            }
            else if (MaterialOutStorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.成品手动出库)
            {
                getPageDataMaterialBatch(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")), 1);
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

        #region 库位批次信息模型,用于列表数据显示
        private ObservableCollection<MaterialBatchInfoModel> materialbatchinfoList = new ObservableCollection<MaterialBatchInfoModel>();

        /// <summary>
        /// 库位信息信息数据
        /// </summary>
        public ObservableCollection<MaterialBatchInfoModel> MaterialBatchInfoList
        {
            get { return materialbatchinfoList; }
            set { Set(ref materialbatchinfoList, value); }
        }
        #endregion

        private ObservableCollection<MaterialBatchInfoModel> materialbatchinfoDataSource = new ObservableCollection<MaterialBatchInfoModel>();

        public ObservableCollection<MaterialBatchInfoModel> MaterialBatchInfoDataSource
        {
            get { return materialbatchinfoDataSource; }
            set { Set(ref materialbatchinfoDataSource, value); }
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
            return MaterialOutStorageInfo == null ? false : MaterialOutStorageInfo.IsValidated;
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
            MaterialOutStorageInfo.MaterialBatchs = MaterialBatchInfoDataSource;
            //
            if (MaterialOutStorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.空托盘出库)
            {
                if (MaterialBatchInfoDataSource.Count != 1)
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = "请选择且仅选一个库位！";
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    //
                    return;
                }
            }
            else if (MaterialOutStorageInfo.OutStorageType == OutStorageTypeEnumModel.OutStorageType.成品手动出库)
            {
                decimal dMaterialBatchsQuantity = MaterialOutStorageInfo.MaterialBatchs.Sum(m => m.Quantity).Value;
                if ( dMaterialBatchsQuantity < MaterialOutStorageInfo.Quantity)
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = "请按出库数量选择库位，请联系管理员！";
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                    //
                    return;
                }
            }
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MaterialOutStorageInfo/Distribute",
                Utility.JsonHelper.ToJson(new List<MaterialOutStorageInfoModel> { MaterialOutStorageInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MaterialOutStorageInfoModel>(MaterialOutStorageInfo, MessengerToken.DataChanged);
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
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getPageDataMaterialBatch(int pageIndex, int pageSize, int iType)
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
            //成品手动出库
            if (iType == 1)
            {
                sType = "MaterialBatchInfo/PageDataMaterialBatchInfo1";
            }
            //空托盘出库
            else if (iType == 2)
            {
                sType = "MaterialBatchInfo/PageDataMaterialBatchInfo2";
            }
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialBatchInfoModel>>>(GlobalData.ServerRootUri + sType, Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取库位批次信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("库位批次信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //RoleInfoList = new ObservableCollection<RoleModel>(result.Data.Data);
                    MaterialBatchInfoList = new ObservableCollection<MaterialBatchInfoModel>();
                    foreach (var data in result.Data.Data)
                    {
                        MaterialBatchInfoModel materialbatchinfo = new MaterialBatchInfoModel();
                        materialbatchinfo = data;
                        //
                        //成品手动出库
                        if (iType == 1)
                        {
                            //为测试需要 调整为成品库位，应为原料库位
                            //
                            if (materialbatchinfo.MaterialCode == MaterialOutStorageInfo.MaterialCode &&
                                materialbatchinfo.PalletCode != null &&
                                materialbatchinfo.WareHouseLocationType == WareHouseLocationTypeEnumModel.WareHouseLocationType.原料库位 )
                            {
                                materialbatchinfo.PropertyChanged += OnPropertyChangedCommand;
                                MaterialBatchInfoList.Add(materialbatchinfo);
                            }
                        }
                        //空托盘出库
                        else if (iType == 2)
                        {
                            //为测试需要 调整为成品库位，应为原料库位
                            //
                            if ( materialbatchinfo.PalletCode == MaterialOutStorageInfo.PalletCode &&
                                (materialbatchinfo.Quantity == null || materialbatchinfo.Quantity <= 0) &&
                                (materialbatchinfo.WareHouseLocationType == WareHouseLocationTypeEnumModel.WareHouseLocationType.原料库位) )
                            {
                                materialbatchinfo.IsChecked = true;
                                //
                                materialbatchinfo.PropertyChanged += OnPropertyChangedCommand;
                                MaterialBatchInfoList.Add(materialbatchinfo);
                                //
                                MaterialBatchInfoDataSource.Add(materialbatchinfo);
                            }
                        }
                    }
                    int TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialBatchInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialBatchInfoList = new ObservableCollection<MaterialBatchInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询库位批次信息失败，请联系管理员！";
            }
        }

        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                var selectedObj = sender as MaterialBatchInfoModel;
                if (selectedObj == null)
                    return;
                if (selectedObj.IsChecked)
                {
                    MaterialBatchInfoDataSource.Add(selectedObj);
                }
                else if (!selectedObj.IsChecked)
                {
                    MaterialBatchInfoDataSource.Remove(selectedObj);
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

        #region 物料出库模型
        //即物料出库任务模型
        private MaterialOutStorageInfoModel matwarehousetypeModel = new MaterialOutStorageInfoModel();
        /// <summary>
        /// 物料出库模型
        /// </summary>
        public MaterialOutStorageInfoModel MaterialOutStorageInfo
        {
            get { return matwarehousetypeModel; }
            set
            {
                Set(ref matwarehousetypeModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion


        #region 出库类型模型 
        public System.Array outstorageTypes = Enum.GetValues(typeof(OutStorageTypeEnumModel.OutStorageType));

        public System.Array OutStorageTypes
        {
            get { return outstorageTypes; }
            set
            {
                Set(ref outstorageTypes, value);
            }
        }
        #endregion

        #region 出库状态模型 
        public System.Array outstorageStatus = Enum.GetValues(typeof(OutStorageStatusEnumModel.OutStorageStatus));

        public System.Array OutStorageStatus
        {
            get { return outstorageStatus; }
            set
            {
                Set(ref outstorageStatus, value);
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

        /// ////////////////////////////////////////////////////////////////////////

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
