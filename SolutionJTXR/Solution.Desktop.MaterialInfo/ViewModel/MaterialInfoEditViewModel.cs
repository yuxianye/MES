using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.MaterialInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using Solution.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.MaterialInfo.ViewModel
{
    /// <summary>
    /// 编辑VM
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MaterialInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
        }
        public override void OnParamterChanged(object parameter)
        {
            this.MaterialInfo = parameter as MaterialInfoModel;
        }

        #region 仓库类型模型
        private MaterialInfoModel materialModel = new MaterialInfoModel();
        /// <summary>
        /// 仓库类型模型
        /// </summary>
        public MaterialInfoModel MaterialInfo
        {
            get { return materialModel; }
            set
            {
                Set(ref materialModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 物料类型模型 
        System.Array materialTypes = Enum.GetValues(typeof(MaterialTypeEnumModel.MaterialType)).Cast<MaterialTypeEnumModel.MaterialType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array MaterialTypes
        {
            get { return materialTypes; }
            set
            {
                Set(ref materialTypes, value);
            }
        }
        #endregion

        #region 物料单位模型 
        System.Array materialUnits = Enum.GetValues(typeof(MaterialUnitEnumModel.MaterialUnit)).Cast<MaterialUnitEnumModel.MaterialUnit>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array MaterialUnits
        {
            get { return materialUnits; }
            set
            {
                Set(ref materialUnits, value);
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
            return MaterialInfo == null ? false : MaterialInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MaterialInfo/Update",
                Utility.JsonHelper.ToJson(new List<MaterialInfoModel> { MaterialInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MaterialInfoModel>(MaterialInfo, MessengerToken.DataChanged);
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
