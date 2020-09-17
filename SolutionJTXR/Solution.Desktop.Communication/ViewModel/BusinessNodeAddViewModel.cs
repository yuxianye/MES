using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Communication.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.Communication.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class BusinessNodeAddViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessNodeAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }

        #region 业务点表数据模型
        /// <summary>
        /// 业务点表模型
        /// </summary>
        private BusinessNodeModel businessNodeModel = new BusinessNodeModel();
        /// <summary>
        /// 业务点表模型
        /// </summary>
        public BusinessNodeModel BusinessNodeModel
        {
            get { return businessNodeModel; }
            set
            {
                Set(ref businessNodeModel, value);
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
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return BusinessNodeModel == null ? false : BusinessNodeModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "BusinessNode/Add",
                Utility.JsonHelper.ToJson(new List<BusinessNodeModel> { BusinessNodeModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<BusinessNodeModel>(BusinessNodeModel, MessengerToken.DataChanged);
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
    }
}
