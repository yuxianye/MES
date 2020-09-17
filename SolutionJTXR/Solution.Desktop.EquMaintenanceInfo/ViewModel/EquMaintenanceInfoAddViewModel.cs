using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquipmentInfo.Model;
using Solution.Desktop.EquMaintenanceInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Solution.Desktop.EquMaintenanceInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquMaintenanceInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquMaintenanceInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);


            getPageData(1, 200);

        }

        #region 设备维护信息模型

        //private EquMaintenanceModel EquMaintenanceModel = new EquMaintenanceModel();
        //private EntProductionLineInfoModel entproductionlineInfoModel = new EntProductionLineInfoModel();

        private EquMaintenanceModel equMaintenanceModel = new EquMaintenanceModel();
        /// <summary>
        /// 设备维护信息模型
        /// </summary>
        public EquMaintenanceModel EquMaintenanceModel
        {
            get { return equMaintenanceModel; }
            set
            {
                Set(ref equMaintenanceModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

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
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
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
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquipmentInfoList = new ObservableCollection<EquipmentInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询地标信息失败，请联系管理员！";
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
            if (Equals(EquMaintenanceModel.ActualStartTime, null) || Equals(EquMaintenanceModel.ActualFinishTime, null))
            {
                return false;
            }
            else
            {
                return EquMaintenanceModel == null ? false : EquMaintenanceModel.IsValidated;
            }
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
           
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquMaintenanceInfo/Add",
                Utility.JsonHelper.ToJson(new List<EquMaintenanceModel> { EquMaintenanceModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<EquMaintenanceModel>(EquMaintenanceModel, MessengerToken.DataChanged);
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
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }


    }
}
