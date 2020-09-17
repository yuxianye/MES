using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.ProManufacturingBillManage.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using Solution.Utility.Extensions;

namespace Solution.Desktop.ProManufacturingBillManage.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class ProManufacturingBillInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProManufacturingBillInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.ProManufacturingBillInfo = parameter as ProManufacturingBillInfoModel;
        }

        #region 企业模型
        private ProManufacturingBillInfoModel billModel = new ProManufacturingBillInfoModel();
        /// <summary>
        /// 企业模型
        /// </summary>
        public ProManufacturingBillInfoModel ProManufacturingBillInfo
        {
            get { return billModel; }
            set
            {
                Set(ref billModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        #region 制造清单类型
        System.Array billTypes = Enum.GetValues(typeof(ProductEnumModel.BillType)).Cast<ProductEnumModel.BillType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();
        public System.Array BillTypes
        {
            get { return billTypes; }
            set
            {
                Set(ref billTypes, value);
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
            return ProManufacturingBillInfo == null ? false : ProManufacturingBillInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            //ProManufacturingBillInfoModel.Id = CombHelper.NewComb();
            ProManufacturingBillInfo.LastUpdatedTime = DateTime.Now;

            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "ProManufacturingBillInfo/Update",
                Utility.JsonHelper.ToJson(new List<ProManufacturingBillInfoModel> { ProManufacturingBillInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<ProManufacturingBillInfoModel>(ProManufacturingBillInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<ProManufacturingBillInfoModel>();
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
