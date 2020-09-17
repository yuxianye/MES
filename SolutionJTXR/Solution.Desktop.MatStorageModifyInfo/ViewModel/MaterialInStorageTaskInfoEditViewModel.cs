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
            getPageDataRole(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            //
            getPageDataMaterial(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getPageDataMatPallet(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        public override void OnParamterChanged(object parameter)
        {

            this.MaterialInStorageInfo = parameter as MaterialInStorageInfoModel;

            this.UserInfo = parameter as UserModel;

            //系统管理员
            int UserInfo_Id = 1;

            var roleIds = GetRolesByUserId(UserInfo_Id);
            if (roleIds.Any())
            {
                foreach (var id in roleIds)
                {
                    for (int i = 0; i < RoleInfoList.Count; i++)
                    {
                        if (id == RoleInfoList[i].Id)
                        {
                            RoleInfoList[i].IsChecked = true;
                        }
                    }
                }
            }
        }

        #region 用户信息模型
        /// <summary>
        /// 用户信息模型
        /// </summary>
        private UserModel _userInfo = new UserModel();
        /// <summary>
        /// 用户信息模型
        /// </summary>
        public UserModel UserInfo
        {
            get { return _userInfo; }
            set
            {
                Set(ref _userInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 角色信息模型
        /// <summary>
        /// 角色信息模型
        /// </summary>
        private RoleModel _roleInfo;// = new EnterpriseModel();
        /// <summary>
        /// 角色信息模型
        /// </summary>
        public RoleModel RoleInfo
        {
            get { return _roleInfo; }
            set { Set(ref _roleInfo, value); }
        }
        #endregion

        /// ///////////////////////////////////////////////////////////

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
                //CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        /// ///////////////////////////////////////////////////////////

        #region 角色信息信息模型,用于列表数据显示
        private ObservableCollection<RoleModel> _roleInfoList = new ObservableCollection<RoleModel>();

        /// <summary>
        /// 角色信息信息数据
        /// </summary>
        public ObservableCollection<RoleModel> RoleInfoList
        {
            get { return _roleInfoList; }
            set { Set(ref _roleInfoList, value); }
        }
        #endregion

        private ObservableCollection<MaterialBatchInfoModel> _dataSource = new ObservableCollection<MaterialBatchInfoModel>();

        public ObservableCollection<MaterialBatchInfoModel> DataSource
        {
            get { return _dataSource; }
            set { Set(ref _dataSource, value); }
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
            MaterialInStorageInfo.Roles = DataSource;

            MaterialBatchInfoModel aa = new MaterialBatchInfoModel();
            aa.Id = Guid.Parse("2E1AC53A-1F8E-E811-8786-005056C00008");
            DataSource.Add(aa);
            //
            MaterialBatchInfoModel bb = new MaterialBatchInfoModel();
            bb.Id = Guid.Parse("2E1AC53A-1F8E-E811-8786-005056C00008");
            DataSource.Add(bb);
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MaterialInStorageTaskInfo/EditUsers",
                Utility.JsonHelper.ToJson(new List<MaterialInStorageInfoModel> { MaterialInStorageInfo }));

            //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MaterialInStorageInfo/Update",
            //    Utility.JsonHelper.ToJson(new List<MaterialInStorageInfoModel> { MaterialInStorageInfo }));

            //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "UserManager/Update",
            //    Utility.JsonHelper.ToJson(new List<UserModel> { UserInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                //Messenger.Default.Send<MaterialInStorageInfoModel>(MaterialInStorageInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<UserModel>(UserInfo, MessengerToken.DataChanged);
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
        private void getPageDataRole(int pageIndex, int pageSize)
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<RoleModel>>>(GlobalData.ServerRootUri + "RoleManager/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取角色信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("角色信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //RoleInfoList = new ObservableCollection<RoleModel>(result.Data.Data);
                    RoleInfoList = new ObservableCollection<RoleModel>();
                    foreach (var data in result.Data.Data)
                    {
                        RoleModel role = new RoleModel();
                        role = data;
                        role.PropertyChanged += OnPropertyChangedCommand;
                        RoleInfoList.Add(role);
                    }
                    int TotalCounts = result.Data.Total;
                }
                else
                {
                    RoleInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                RoleInfoList = new ObservableCollection<RoleModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询角色信息失败，请联系管理员！";
            }
        }

        public List<int> GetRolesByUserId(int userId)
        {
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<int>>>(GlobalData.ServerRootUri + $"UserManager/GetRolesByUserId/{userId}");
            return result.Data;
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
                    DataSource.Add(selectedObj);
                }
                else if (!selectedObj.IsChecked)
                {
                    DataSource.Remove(selectedObj);
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
