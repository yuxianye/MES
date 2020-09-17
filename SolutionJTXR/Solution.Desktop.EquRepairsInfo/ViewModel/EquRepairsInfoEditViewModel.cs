using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EntDepartmentInfo.Model;
using Solution.Desktop.EquFactoryInfo.Model;
using Solution.Desktop.EquipmentInfo.Model;
using Solution.Desktop.EquipmentTypeInfo.Model;
using Solution.Desktop.EquRepairsInfo.Model;
using Solution.Desktop.EquSparePartsInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using Solution.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Solution.Desktop.EquRepairsInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquRepairsInfoEditViewModel : VmBase
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquRepairsInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getEquipmentInfoPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getEquSparePartsInfoPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));

        }

        public override void OnParamterChanged(object parameter)
        {
            this.EquRepairsInfo = parameter as EquRepairsInfoModel;
            this.EquipmentModel = EquipmentInfoList.Where(x=>x.Id == Guid.Parse(EquRepairsInfo.EquipmentInfo_Id)).FirstOrDefault();
            this.EquSparePartsInfo = EquSparePartsInfoList.Where(x=>x.Id == Guid.Parse(EquRepairsInfo.SparePartsInfo_Id)).FirstOrDefault();
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

        #region 设备维修信息模型
        /// <summary>
        /// 设备维修信息模型
        /// </summary>
        private EquRepairsInfoModel equRepairsInfo = new EquRepairsInfoModel();
        /// <summary>
        /// 设备维修信息模型
        /// </summary>
        public EquRepairsInfoModel EquRepairsInfo
        {
            get { return equRepairsInfo; }
            set { Set(ref equRepairsInfo, value); }
        }
        #endregion

        #region 设备备件模型
        /// <summary>
        /// 备件信息模型
        /// </summary>
        private EquSparePartsModel equSparePartsInfo = new EquSparePartsModel();
        /// <summary>
        /// 备件信息模型
        /// </summary>
        public EquSparePartsModel EquSparePartsInfo
        {
            get { return equSparePartsInfo; }
            set { Set(ref equSparePartsInfo, value); }
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

        #region 设备信息模型,用于列表数据显示
        private ObservableCollection<EquRepairsInfoModel> equRepairsInfoList = new ObservableCollection<EquRepairsInfoModel>();

        /// <summary>
        /// 设备维修信息数据
        /// </summary>
        public ObservableCollection<EquRepairsInfoModel> EquRepairsInfoList
        {
            get { return equRepairsInfoList; }
            set { Set(ref equRepairsInfoList, value); }
        }
        #endregion

        #region 设备备件模型,用于列表数据显示
        private ObservableCollection<EquSparePartsModel> equSparePartsInfoList = new ObservableCollection<EquSparePartsModel>();

        /// <summary>
        /// 设备备件信息数据
        /// </summary>
        public ObservableCollection<EquSparePartsModel> EquSparePartsInfoList
        {
            get { return equSparePartsInfoList; }
            set { Set(ref equSparePartsInfoList, value); }
        }
        #endregion

        #region 故障类别

        System.Array FaultTypeEnum = Enum.GetValues(typeof(FaultType)).Cast<FaultType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array FaultTypes
        {
            get { return FaultTypeEnum; }
            set
            {
                Set(ref FaultTypeEnum, value);
            }
        }
        #endregion

        #region 读取分页数据

        #region 设备信息分页数据查询
        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

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

        #region 设备备件分页数据查询
        /// <summary>
        /// 取得设备备件分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getEquSparePartsInfoPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            //pageRepuestParams.SortField = "LastUpdatedTime";
            //pageRepuestParams.SortOrder = "desc";
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquSparePartsModel>>>(GlobalData.ServerRootUri + "EquSparePartsInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取设备备件信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("设备备件信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EquSparePartsInfoList = new ObservableCollection<EquSparePartsModel>(result.Data.Data);
                }
                else
                {
                    EquSparePartsInfoList?.Clear();
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquSparePartsInfoList = new ObservableCollection<EquSparePartsModel>();
                UiMessage = result?.Message ?? "查询设备备件信息失败，请联系管理员！";
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
            return EquRepairsInfo == null ? false : EquRepairsInfo.IsValidated;
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
            if (Equals(EquipmentModel, null))
            {
                UiMessage = "请选择设备信息！";
                LogHelper.Info(UiMessage);
                return;
            }
            if (Equals(EquSparePartsInfo, null))
            {
                UiMessage = "请选择设备备件信息！";
                LogHelper.Info(UiMessage);
                return;
            }

            EquRepairsInfo.EquipmentInfo = EquipmentModel;
            EquRepairsInfo.SparePartsInfo = EquSparePartsInfo;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquRepairsInfo/Update",
                Utility.JsonHelper.ToJson(new List<EquRepairsInfoModel> { EquRepairsInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EquRepairsInfoModel>(EquRepairsInfo, MessengerToken.DataChanged);
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
