using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.MaterialBatchInfo.Model;
using Solution.Desktop.MatStorageModifyInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using Solution.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.MatStorageModifyInfo.ViewModel
{
    /// <summary>
    /// 编辑VM
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MatStorageModifyInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatStorageModifyInfoEditViewModel()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //
            materialbatchSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutematerialbatchSelectionChangedCommand);
            CurrentAmountTextChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCurrentAmountTextChangedCommand);
            OriginalAmountTextChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteOriginalAmountTextChangedCommand);
            //
            getPageDataMaterialBatch(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }
        public override void OnParamterChanged(object parameter)
        {
            this.MatStorageModifyInfo = parameter as MatStorageModifyInfoModel;
        }

        #region 库存调整模型
        private MatStorageModifyInfoModel matwarehousetypeModel = new MatStorageModifyInfoModel();
        /// <summary>
        /// 库存调整模型
        /// </summary>
        public MatStorageModifyInfoModel MatStorageModifyInfo
        {
            get { return matwarehousetypeModel; }
            set
            {
                Set(ref matwarehousetypeModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 调整状态模型 
        System.Array storagemodifyStates = Enum.GetValues(typeof(StorageModifyStateEnumModel.StorageModifyState)).Cast<StorageModifyStateEnumModel.StorageModifyState>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array StorageModifyStates
        {
            get { return storagemodifyStates; }
            set
            {
                Set(ref storagemodifyStates, value);
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
        /// 批次编号下拉列表改变
        /// </summary>
        public ICommand materialbatchSelectionChangedCommand { get; set; }

        /// <summary>
        /// 现数量改变命令
        /// </summary>
        public ICommand CurrentAmountTextChangedCommand { get; set; }

        //
        /// <summary>
        /// 原数量改变命令
        /// </summary>
        public ICommand OriginalAmountTextChangedCommand { get; set; }


        private void OnExecutematerialbatchSelectionChangedCommand()
        {
            MaterialBatchInfoModel materialbatchInfo = MaterialBatchInfoList.FirstOrDefault(m => m.Id == MatStorageModifyInfo.MaterialBatch_Id);
            //
            if (materialbatchInfo != null)
            {
                MatStorageModifyInfo.WareHouseLocationName = materialbatchInfo.WareHouseLocationName;
                MatStorageModifyInfo.MaterialCode = materialbatchInfo.MaterialCode;
                MatStorageModifyInfo.MaterialName = materialbatchInfo.MaterialName;
                MatStorageModifyInfo.OriginalAmount = materialbatchInfo.Quantity;
                MatStorageModifyInfo.FullPalletQuantity = materialbatchInfo.FullPalletQuantity;
            }
            else
            {
                MatStorageModifyInfo.WareHouseLocationName = null;
                MatStorageModifyInfo.MaterialCode = null;
                MatStorageModifyInfo.MaterialName = null;
                MatStorageModifyInfo.OriginalAmount = null;
                MatStorageModifyInfo.FullPalletQuantity = null;
            }
        }

        /// <summary>
        /// 现数量改变命令函数
        /// </summary>
        private void OnExecuteCurrentAmountTextChangedCommand()
        {
            MatStorageModifyInfoSetChangedAmount.SetChangedAmount(MatStorageModifyInfo);
        }


        /// <summary>
        /// 原数量改变命令函数
        /// </summary>
        private void OnExecuteOriginalAmountTextChangedCommand()
        {
            MatStorageModifyInfoSetChangedAmount.SetChangedAmount(MatStorageModifyInfo);
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return MatStorageModifyInfo == null ? false : MatStorageModifyInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MatStorageModifyInfo/Update",
                Utility.JsonHelper.ToJson(new List<MatStorageModifyInfoModel> { MatStorageModifyInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MatStorageModifyInfoModel>(MatStorageModifyInfo, MessengerToken.DataChanged);
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

        ////////////////////////////////////////////////////////////

        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

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



        #region 批次信息模型,用于列表数据显示
        private ObservableCollection<MaterialBatchInfoModel> materialbatchInfoList = new ObservableCollection<MaterialBatchInfoModel>();

        /// <summary>
        /// 批次信息数据
        /// </summary>
        public ObservableCollection<MaterialBatchInfoModel> MaterialBatchInfoList
        {
            get { return materialbatchInfoList; }
            set { Set(ref materialbatchInfoList, value); }
        }
        #endregion

        private void getPageDataMaterialBatch(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialBatchInfoModel>>>(GlobalData.ServerRootUri + "MaterialBatchInfo/PageDataBatchCode", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取批次信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("批次信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    MaterialBatchInfoList = new ObservableCollection<MaterialBatchInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialBatchInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialBatchInfoList = new ObservableCollection<MaterialBatchInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询批次信息失败，请联系管理员！";
            }
        }
    }

}
