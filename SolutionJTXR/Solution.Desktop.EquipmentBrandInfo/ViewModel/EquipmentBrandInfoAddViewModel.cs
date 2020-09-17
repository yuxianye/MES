using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquipmentBrandInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Solution.Desktop.EquipmentBrandInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquipmentBrandInfoAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentBrandInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //getPageData(1, 50);
        }

        #region 设备品牌模型
        private EquipmentBrandModel equipmentBrandModel = new EquipmentBrandModel();
        /// <summary>
        /// 企业模型
        /// </summary>
        public EquipmentBrandModel EquipmentBrandModel
        {
            get { return equipmentBrandModel; }
            set
            {
                Set(ref equipmentBrandModel, value);
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
            return EquipmentBrandModel == null ? false : EquipmentBrandModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquipmentBrandInfo/Add",
                Utility.JsonHelper.ToJson(new List<EquipmentBrandModel> { EquipmentBrandModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<EquipmentBrandModel>(EquipmentBrandModel, MessengerToken.DataChanged);
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
        /// <summary>
        /// 为了使用下拉列表，借鉴新增的代码段
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();
        #region 设备品牌模型,用于列表数据显示
        private ObservableCollection<EquipmentBrandModel> equipmentBrandInfoList = new ObservableCollection<EquipmentBrandModel>();

        /// <summary>
        /// 设备品牌数据
        /// </summary>
        public ObservableCollection<EquipmentBrandModel> EquipmentBrandInfoList
        {
            get { return equipmentBrandInfoList; }
            set { Set(ref equipmentBrandInfoList, value); }
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquipmentBrandModel>>>(GlobalData.ServerRootUri + "EquipmentBrandInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取设备品牌用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("设备品牌内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {

                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EquipmentBrandInfoList = new ObservableCollection<EquipmentBrandModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    EquipmentBrandInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquipmentBrandInfoList = new ObservableCollection<EquipmentBrandModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询设备品牌失败，请联系管理员！";
            }
        }
    }
}
