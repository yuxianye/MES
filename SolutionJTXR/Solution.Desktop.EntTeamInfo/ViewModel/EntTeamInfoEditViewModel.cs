using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EntTeamInfo.Model;
using Solution.Desktop.EnterpriseInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.EntTeamInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EntTeamInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntTeamInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.EntTeamInfoModel = parameter as EntTeamInfoModel;
        }

        #region 车间模型
        private EntTeamInfoModel areaModel = new EntTeamInfoModel();
        private EnterpriseModel enterpriseModel = new EnterpriseModel();
        /// <summary>
        /// 车间模型
        /// </summary>
        public EntTeamInfoModel EntTeamInfoModel
        {
            get { return areaModel; }
            set
            {
                Set(ref areaModel, value);
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
            return EntTeamInfoModel == null ? false : EntTeamInfoModel.IsValidated;
        }
        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EntTeamInfo/Update",
                Utility.JsonHelper.ToJson(new List<EntTeamInfoModel> { EntTeamInfoModel }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EntTeamInfoModel>(EntTeamInfoModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<EntTeamInfoModel>();
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
