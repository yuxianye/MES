using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EntDepartmentInfo.Model;
using Solution.Desktop.EquipmentInfo.Model;
using Solution.Desktop.EquMaintenancePlanInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using Solution.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Solution.Desktop.EquMaintenancePlanInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquMaintenancePlanInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquMaintenancePlanInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getDepartmentPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getEquipmentInfoPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        #region 设备模型
        private EquipmentInfoModel equipmentModel = new EquipmentInfoModel();
        /// <summary>
        /// 设备模型
        /// </summary>
        public EquipmentInfoModel EquipmentModel
        {
            get { return equipmentModel; }
            set
            {
                Set(ref equipmentModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 设备维修计划信息
        private EquMaintenancePlanInfoModel equMaintenancePlanInfo = new EquMaintenancePlanInfoModel();

        /// <summary>
        /// 设备维修计划信息
        /// </summary>
        public EquMaintenancePlanInfoModel EquMaintenancePlanInfo
        {
            get { return equMaintenancePlanInfo; }
            set
            {
                Set(ref equMaintenancePlanInfo, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        #endregion

        #region 部门模型
        /// <summary>
        /// 企业信息模型
        /// </summary>
        private EntDepartmentInfoModel entDepartmentInfo = new EntDepartmentInfoModel();

        /// <summary>
        /// 企业信息模型
        /// </summary>
        public EntDepartmentInfoModel EntDepartmentInfo
        {
            get { return entDepartmentInfo; }
            set { Set(ref entDepartmentInfo, value); }
        }
        #endregion

        #region 部门信息模型,用于列表数据显示
        private ObservableCollection<EntDepartmentInfoModel> entDepartmentInfoList = new ObservableCollection<EntDepartmentInfoModel>();

        /// <summary>
        /// 企业信息数据
        /// </summary>
        public ObservableCollection<EntDepartmentInfoModel> EntDepartmentInfoList
        {
            get { return entDepartmentInfoList; }
            set { Set(ref entDepartmentInfoList, value); }
        }
        #endregion

        #region 设备维修计划信息模型,用于列表数据显示
        private ObservableCollection<EquMaintenancePlanInfoModel> equMaintenancePlanList = new ObservableCollection<EquMaintenancePlanInfoModel>();

        /// <summary>
        /// 设备维修计划信息模型,用于列表数据显示
        /// </summary>
        public ObservableCollection<EquMaintenancePlanInfoModel> EquMaintenancePlanList
        {
            get { return equMaintenancePlanList; }
            set { Set(ref equMaintenancePlanList, value); }
        }
        #endregion

        #region 设备信息模型,用于列表数据显示
        private ObservableCollection<EquipmentInfoModel> equipmentInfoList = new ObservableCollection<EquipmentInfoModel>();

        /// <summary>
        /// 设备信息数据
        /// </summary>
        public ObservableCollection<EquipmentInfoModel> EquipmentInfoList
        {
            get { return equipmentInfoList; }
            set { Set(ref equipmentInfoList, value); }
        }
        #endregion

        #region 设备维护计划状态类型

        System.Array maintenancePlanStateEnum = Enum.GetValues(typeof(MaintenancePlanState)).Cast<MaintenancePlanState>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array MaintenancePlanStateEnums
        {
            get { return maintenancePlanStateEnum; }
            set
            {
                Set(ref maintenancePlanStateEnum, value);
            }
        }
        #endregion

        #region 设备维护计划状态类型

        System.Array maintenanceTypeEnum = Enum.GetValues(typeof(MaintenanceType)).Cast<MaintenanceType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array MaintenanceTypeEnums
        {
            get { return maintenanceTypeEnum; }
            set
            {
                Set(ref maintenanceTypeEnum, value);
            }
        }
        #endregion

        #region 读取分页数据

        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        #region 部门信息分页数据查询
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getDepartmentPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EntDepartmentInfoModel>>>
                (GlobalData.ServerRootUri + "EntDepartmentInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取部门信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("部门信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                //Application.Current.Resources["UiMessage"] = result?.Message;
                //LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                //UiMessage = result?.Message;
                //LogHelper.Info(UiMessage);
                if (result.Data.Data.Any())
                {
                    EntDepartmentInfoList = new ObservableCollection<EntDepartmentInfoModel>(result.Data.Data);
                }
                else
                {
                    EntDepartmentInfoList?.Clear();
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EntDepartmentInfoList = new ObservableCollection<EntDepartmentInfoModel>();
                //Application.Current.Resources["UiMessage"] = result?.Message ?? "查询企业信息失败，请联系管理员！";
                UiMessage = result?.Message ?? "查询部门信息失败，请联系管理员！";
            }
        }
        #endregion

        #region 设备信息分页数据查询

        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getEquipmentInfoPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif

            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquipmentInfoModel>>>(GlobalData.ServerRootUri + "EquipmentInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取设备信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("设备信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                //UiMessage = result?.Message;
                //LogHelper.Info(UiMessage);
                if (result.Data.Data.Any())
                {
                    EquipmentInfoList = new ObservableCollection<EquipmentInfoModel>(result.Data.Data);
                }
                else
                {
                    EquipmentInfoList?.Clear();
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquipmentInfoList = new ObservableCollection<EquipmentInfoModel>();
                UiMessage = result?.Message ?? "查询设备信息失败，请联系管理员！";
            }
        }

        #endregion

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
            return EquMaintenancePlanInfo == null ? false : EquMaintenancePlanInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            if (Equals(EquipmentModel, null))
            {
                UiMessage = "请选择设备信息！";
                LogHelper.Info(UiMessage);
                return;
            }
            if (Equals(EntDepartmentInfo, null))
            {
                UiMessage = "请选择设备备件信息！";
                LogHelper.Info(UiMessage);
                return;
            }
            EquMaintenancePlanInfo.EquipmentInfo = EquipmentModel;
            EquMaintenancePlanInfo.CheckDepartment = EntDepartmentInfo;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquMaintenancePlanInfo/Add",
                Utility.JsonHelper.ToJson(new List<EquMaintenancePlanInfoModel> { EquMaintenancePlanInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EquMaintenancePlanInfoModel>(EquMaintenancePlanInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
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
    }
}