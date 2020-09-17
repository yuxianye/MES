using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.EquSparePartsInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Solution.Desktop.EquipmentInfo.Model;
using System.Collections.ObjectModel;
using System.Linq;
using Solution.Desktop.EquSparePartTypeInfo.Model;
using Solution.Utility.Extensions;

namespace Solution.Desktop.EquSparePartsInfo.ViewModel
{
    /// <summary>
    /// 新增
    /// </summary>
    public class EquSparePartsInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquSparePartsInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            getPageData(1, 200);

        }
        public override void OnParamterChanged(object parameter)
        {
            this.EquSparePartsModel = parameter as EquSparePartsModel;
        }
        #region 备件信息模型

        //private EquSparePartsModel EquSparePartsModel = new EquSparePartsModel();
        //private EntProductionLineInfoModel entproductionlineInfoModel = new EntProductionLineInfoModel();

        private EquSparePartsModel equSparePartsModel = new EquSparePartsModel();
        /// <summary>
        /// 备件信息模型
        /// </summary>
        public EquSparePartsModel EquSparePartsModel
        {
            get { return equSparePartsModel; }
            set
            {
                Set(ref equSparePartsModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
        /// <summary>
        /// 处理下来列表枚举(数量单位)
        /// </summary>
        System.Array sparePartUnitEnum = Enum.GetValues(typeof(SparePartUnitEnum.SparePartUnit)).Cast<SparePartUnitEnum.SparePartUnit>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array SparePartUnit
        {
            get { return sparePartUnitEnum; }
            set
            {
                Set(ref sparePartUnitEnum, value);
            }
        }
        #region 备件信息模型,用于类别数据显示
        private ObservableCollection<EquSparePartTypeModel> sparePartTypeInfoList = new ObservableCollection<EquSparePartTypeModel>();

        /// <summary>
        /// 设备类别数据
        /// </summary>
        public ObservableCollection<EquSparePartTypeModel> EquSparePartTypeInfoList
        {
            get { return sparePartTypeInfoList; }
            set { Set(ref sparePartTypeInfoList, value); }
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


            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<EquSparePartTypeModel>>>(GlobalData.ServerRootUri + "EquSparePartTypeInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取备件类别信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("备件类别信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    EquSparePartTypeInfoList = new ObservableCollection<EquSparePartTypeModel>(result.Data.Data);

                    //TotalCounts = result.Data.Total;
                }
                else
                {
                    EquSparePartTypeInfoList?.Clear();
                    //TotalCounts = 0;
                    UiMessage = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                EquSparePartTypeInfoList = new ObservableCollection<EquSparePartTypeModel>();
                UiMessage = result?.Message ?? "查询备件类别信息失败，请联系管理员！";
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
            return EquSparePartsModel == null ? false : EquSparePartsModel.IsValidated;
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "EquSparePartsInfo/Update",
                Utility.JsonHelper.ToJson(new List<EquSparePartsModel> { EquSparePartsModel }));

            if (!Equals(result, null) && result.Successed)
            {
                UiMessage = result?.Message;
                LogHelper.Info(UiMessage);
                Messenger.Default.Send<EquSparePartsModel>(EquSparePartsModel, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
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
