using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquipmentInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Solution.Desktop.EntProductionLineInfo.Model;
using Solution.Desktop.EquipmentTypeInfo.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Solution.Desktop.EquSparePartTypeInfo.Model;
using Solution.Desktop.EntDepartmentInfo.Model;
using Solution.Desktop.EquFactoryInfo.Model;
using Solution.Utility.Extensions;

namespace Solution.Desktop.EquipmentInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquipmentInfoEditViewModel : VmBase
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getEquTypePageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getDepartmentPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getEquFactoryPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));

        }

        public override void OnParamterChanged(object parameter)
        {
            this.EquipmentModel = parameter as EquipmentInfoModel;
            this.EntDepartmentInfo = EntDepartmentInfoList.Where(x=>x.Id == Guid.Parse(EquipmentModel.DepartmentInfo_Id)).FirstOrDefault();
            this.EquFactoryInfo = EquFactoryInfoList.Where(x=>x.Id == Guid.Parse(EquipmentModel.EquFactoryInfo_Id)).FirstOrDefault();
            this.EquipmentTypeModel = EquipmentTypeInfoList.Where(x=>x.Id == EquipmentModel.EquipmentType_Id).FirstOrDefault();
        }

        #region 设备模型
        private EquipmentInfoModel equipmentModel = new EquipmentInfoModel();
        /// <summary>
        /// 企业模型
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

        #region 设备类别模型
        /// <summary>
        ///设备类别信息模型
        /// </summary>
        private EquipmentTypeModel equipmenttypemode = new EquipmentTypeModel();
        public EquipmentTypeModel EquipmentTypeModel
        {
            get { return equipmenttypemode; }
            set
            {
                Set(ref equipmenttypemode, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 部门模型
        /// <summary>
        /// 企业信息模型
        /// </summary>
        private EntDepartmentInfoModel entDepartmentInfo;

        /// <summary>
        /// 企业信息模型
        /// </summary>
        public EntDepartmentInfoModel EntDepartmentInfo
        {
            get { return entDepartmentInfo; }
            set { Set(ref entDepartmentInfo, value); }
        }
        #endregion

        #region 设备厂家模型
        /// <summary>
        /// 设备厂家模型
        /// </summary>
        private EquFactoryModel equFactoryInfo;// = new EquipmentActionInfoModel();
        /// <summary>
        /// 设备厂家模型
        /// </summary>
        public EquFactoryModel EquFactoryInfo
        {
            get { return equFactoryInfo; }
            set { Set(ref equFactoryInfo, value); }
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

        #region 设备类别信息模型,用于列表数据显示
        private ObservableCollection<EquipmentTypeModel> equipmentTypeInfoList = new ObservableCollection<EquipmentTypeModel>();

        /// <summary>
        /// 设备类别信息数据
        /// </summary>
        public ObservableCollection<EquipmentTypeModel> EquipmentTypeInfoList
        {
            get { return equipmentTypeInfoList; }
            set { Set(ref equipmentTypeInfoList, value); }
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

        #region 设备厂家模型,用于列表数据显示
        private ObservableCollection<EquFactoryModel> equFactoryInfoList = new ObservableCollection<EquFactoryModel>();

        /// <summary>
        /// 设备厂家信息数据
        /// </summary>
        public ObservableCollection<EquFactoryModel> EquFactoryInfoList
        {
            get { return equFactoryInfoList; }
            set { Set(ref equFactoryInfoList, value); }
        }
        #endregion

        #region 设备状态类型

        System.Array equEquipmentStateEnum = Enum.GetValues(typeof(EquEquipmentState)).Cast<EquEquipmentState>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array EquEquipmentStates
        {
            get { return equEquipmentStateEnum; }
            set
            {
                Set(ref equEquipmentStateEnum, value);
            }
        }
        #endregion

        #region ABC数据类型      

        System.Array abcType = Enum.GetValues(typeof(ABCType)).Cast<ABCType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array ABCType
        {
            get { return abcType; }
            set
            {
                Set(ref abcType, value);
            }
        }

        #endregion

        #region 读取分页数据

        #region 设备类型分页数据查询
        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getEquTypePageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif

            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquipmentTypeModel>>>(GlobalData.ServerRootUri + "EquipmentTypeInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取设备信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("设备信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                //UiMessage = result?.Message;
                //LogHelper.Info(UiMessage.ToString());
                if (result.Data.Data.Any())
                {
                    EquipmentTypeInfoList = new ObservableCollection<EquipmentTypeModel>(result.Data.Data);
                }
                else
                {
                    EquipmentTypeInfoList?.Clear();
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquipmentTypeInfoList = new ObservableCollection<EquipmentTypeModel>();
                UiMessage = result?.Message ?? "查询设备信息失败，请联系管理员！";
            }
        }

        #endregion

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

        #region 设备厂家分页数据查询
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getEquFactoryPageData(int pageIndex, int pageSize)
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquFactoryModel>>>(GlobalData.ServerRootUri + "EquFactory/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取设备厂家信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("设备厂家信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                //UiMessage = result?.Message;
                //LogHelper.Info(UiMessage);
                if (result.Data.Data.Any())
                {
                    EquFactoryInfoList = new ObservableCollection<EquFactoryModel>(result.Data.Data);
                }
                else
                {
                    EquFactoryInfoList?.Clear();
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquFactoryInfoList = new ObservableCollection<EquFactoryModel>();
                UiMessage = result?.Message ?? "查询设备厂家信息失败，请联系管理员！";
            }
        }
        #endregion

        #endregion

        #region 命令及执行
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
            return EquFactoryInfo == null ? false : EquFactoryInfo.IsValidated;
        }

        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            if (Equals(EquipmentTypeModel, null))
            {
                UiMessage = "请选择设备类型！";
                LogHelper.Info(UiMessage);
                return;
            }
            if (Equals(EntDepartmentInfo, null))
            {
                UiMessage = "请选择部门！";
                LogHelper.Info(UiMessage);
                return;
            }
            if (Equals(EquFactoryInfo, null))
            {
                UiMessage = "请选择设备厂家！";
                LogHelper.Info(UiMessage);
                return;
            }
            EquipmentModel.Equipmenttype = EquipmentTypeModel;
            EquipmentModel.DepartmentInfo = EntDepartmentInfo;
            EquipmentModel.EquFactoryInfo = EquFactoryInfo;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquipmentInfo/Update",
                Utility.JsonHelper.ToJson(new List<EquipmentInfoModel> { EquipmentModel }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EquipmentInfoModel>(EquipmentModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                UiMessage = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(UiMessage);
            }
        }
        #endregion
    }
}
