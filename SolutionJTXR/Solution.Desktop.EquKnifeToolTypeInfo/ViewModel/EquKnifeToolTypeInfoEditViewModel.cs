using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquKnifeToolTypeInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.EquKnifeToolTypeInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquKnifeToolTypeInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquKnifeToolTypeInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.KnifeToolTypeModel = parameter as KnifeToolTypeModel;
        }

        #region 刀具类别信息模型
        private KnifeToolTypeModel equipmentModel = new KnifeToolTypeModel();
        /// <summary>
        /// 刀具类别信息模型
        /// </summary>
        public KnifeToolTypeModel KnifeToolTypeModel
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
            return KnifeToolTypeModel == null ? false : KnifeToolTypeModel.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquKnifeToolTypeInfo/Update",//EquKnifeToolTypeInfo
               Utility.JsonHelper.ToJson(new List<KnifeToolTypeModel> { KnifeToolTypeModel }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<KnifeToolTypeModel>(KnifeToolTypeModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EquSparePartTypeModel>();
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
