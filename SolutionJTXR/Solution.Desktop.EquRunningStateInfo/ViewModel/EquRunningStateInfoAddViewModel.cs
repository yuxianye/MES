using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquipmentInfo.Model;
using Solution.Desktop.EquRunningStateInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using Solution.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Solution.Desktop.EquRunningStateInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquRunningStateInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquRunningStateInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);


            getPageData(1, 200);

        }

        #region 设备运行状态模型

        private EquRunningStateInfoModel equRunningStateInfoModel = new EquRunningStateInfoModel();
        /// <summary>
        /// 设备运行状态模型
        /// </summary>
        public EquRunningStateInfoModel EquRunningStateInfoModel
        {
            get { return equRunningStateInfoModel; }
            set
            {
                Set(ref equRunningStateInfoModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        System.Array equRunningStateTypes = Enum.GetValues(typeof(RunningStateTypeEnum.RunningStateType)).Cast<RunningStateTypeEnum.RunningStateType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array EquRunningStateTypes
        {
            get { return equRunningStateTypes; }
            set
            {
                Set(ref equRunningStateTypes, value);
            }
        }

        #region 设备信息模型,用于数据显示
        private ObservableCollection<EquipmentInfoModel> equipmentInfoList = new ObservableCollection<EquipmentInfoModel>();

        /// <summary>
        /// 设备信息数据
        /// </summary>
        public ObservableCollection<EquipmentInfoModel> EquipmentInfoList
        {
            get { return equipmentInfoList; }
            set { Set(ref equipmentInfoList, value); }
        }




        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
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
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EquipmentInfoList = new ObservableCollection<EquipmentInfoModel>(result.Data.Data);

                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    EquipmentInfoList?.Clear();
                    //TotalCounts = 0;
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

        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>


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
            return EquRunningStateInfoModel == null ? false : EquRunningStateInfoModel.IsValidated;
        }

        /// <summary>
        /// 设备类别信息下拉列表改变
        /// </summary>
        public ICommand equipmentTypeSelectionChangedCommand { get; set; }

        /// <summary>
        /// 生产线信息下拉列表改变
        /// </summary>
        public ICommand productionLineSelectionChangedCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquRunningStateInfo/Add",
                Utility.JsonHelper.ToJson(new List<EquRunningStateInfoModel> { EquRunningStateInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EquRunningStateInfoModel>(EquRunningStateInfoModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
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
