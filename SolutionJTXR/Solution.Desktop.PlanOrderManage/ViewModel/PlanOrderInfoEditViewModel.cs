using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.PlanOrderManage.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace Solution.Desktop.PlanOrderManage.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class PlanOrderInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PlanOrderInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.PlanOrderInfo = parameter as PlanOrderInfoModel;
        }

        #region 订单模型
        private PlanOrderInfoModel planOrderInfo = new PlanOrderInfoModel();
        /// <summary>
        /// 企业模型
        /// </summary>
        public PlanOrderInfoModel PlanOrderInfo
        {
            get { return planOrderInfo; }
            set
            {
                Set(ref planOrderInfo, value);
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
            return PlanOrderInfo == null ? false : PlanOrderInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "PlanOrderInfo/Update",
                Utility.JsonHelper.ToJson(new List<PlanOrderInfoModel> { PlanOrderInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<PlanOrderInfoModel>(PlanOrderInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<PlanOrderInfoModel>();
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
