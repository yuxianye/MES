using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquSparePartTypeInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.EquSparePartTypeInfo.ViewModel
{
    /// <summary>
    /// 新增VM
    /// </summary>
    public class EquSparePartTypeInfoAddViewModel : VmBase//《注意：模块主VM与增加和编辑VM继承的基类不同》
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquSparePartTypeInfoAddViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }

        #region 备件类别模型
        private EquSparePartTypeModel equspareparttypeModel = new EquSparePartTypeModel();
        /// <summary>
        ///备件类别模型
        /// </summary>
        public EquSparePartTypeModel EquSparePartTypeModel
        {
            get { return equspareparttypeModel; }
            set
            {
                Set(ref equspareparttypeModel, value);
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
            return EquSparePartTypeModel == null ? false : EquSparePartTypeModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquSparePartTypeInfo/Add",
                Utility.JsonHelper.ToJson(new List<EquSparePartTypeModel> { EquSparePartTypeModel }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EquSparePartTypeModel>(EquSparePartTypeModel, MessengerToken.DataChanged);
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
