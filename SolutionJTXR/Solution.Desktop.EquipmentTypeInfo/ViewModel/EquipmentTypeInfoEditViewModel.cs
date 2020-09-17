using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquipmentTypeInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.EquipmentTypeInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquipmentTypeInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentTypeInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.EquipmentTypeModel = parameter as EquipmentTypeModel;
        }

        #region 设备类别模型
        private EquipmentTypeModel equipmentModel = new EquipmentTypeModel();
        /// <summary>
        /// 设备类别模型
        /// </summary>
        public EquipmentTypeModel EquipmentTypeModel
        {
            get { return equipmentModel; }
            set
            {
                Set(ref equipmentModel, value);
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
            return EquipmentTypeModel == null ? false : EquipmentTypeModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquipmentTypeInfo/Update",
               Utility.JsonHelper.ToJson(new List<EquipmentTypeModel> { EquipmentTypeModel }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EquipmentTypeModel>(EquipmentTypeModel, MessengerToken.DataChanged);
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
