using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.MaterialBatchInfo.Model;
using Solution.Desktop.MatInventoryInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Solution.Desktop.MatInventoryInfo.ViewModel
{
    public class MatInventoryItemInfoEditViewModel : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatInventoryItemInfoEditViewModel()
        {

            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            materialbatchSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecutematerialbatchSelectionChangedCommand);
            //            
            AccuntAmountTextChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAccuntAmountTextChangedCommand);
            ActualAmountTextChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteActualAmounTextChangedCommand);
            //
            getInventoryMaterialBatchID();
        }

        public List<string> MaterialBatchInfoList1;

        void getInventoryMaterialBatchID()
        {
            var MaterialBatchInfoList0 = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<string>>>
                         (GlobalData.ServerRootUri + "MatInventoryItemInfo/GetMaterialBatchID");
            //
            MaterialBatchInfoList1 = new List<string>();
            if (MaterialBatchInfoList0 != null)
            {
                MaterialBatchInfoList1 = MaterialBatchInfoList0.Data;
            }
        }

        public override void OnParamterChanged(object parameter)
        {
            this.MatInventoryItemInfo = parameter as MatInventoryItemInfoModel;
            //
            getPageDataMaterialBatch(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        #region 订单模型
        private MatInventoryItemInfoModel planOrderItemInfo = new MatInventoryItemInfoModel();
        /// <summary>
        /// 
        /// </summary>
        public MatInventoryItemInfoModel MatInventoryItemInfo
        {
            get { return planOrderItemInfo; }
            set
            {
                Set(ref planOrderItemInfo, value);
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
        /// 账面数量改变命令
        /// </summary>
        public ICommand AccuntAmountTextChangedCommand { get; set; }


        /// <summary>
        /// 实盘数量改变命令
        /// </summary>
        public ICommand ActualAmountTextChangedCommand { get; set; }
        //

        /// <summary>
        /// 选择产品下拉列表命令
        /// </summary>
        public ICommand materialbatchSelectionChangedCommand { get; set; }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return MatInventoryItemInfo == null ? false : MatInventoryItemInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            //MatInventoryItemInfo.RemainQuantity = MatInventoryItemInfo.OrderQuantity;
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MatInventoryInfo/UpdateItem",
                Utility.JsonHelper.ToJson(new List<MatInventoryItemInfoModel> { MatInventoryItemInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MatInventoryItemInfoModel>(MatInventoryItemInfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
            }
            else
            {
                //操作失败，显示错误信息
                //siteInfoList = new ObservableCollection<MatInventoryItemInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            }
        }

        /// <summary>
        /// 账面数量改变命令函数
        /// </summary>
        private void OnExecuteAccuntAmountTextChangedCommand()
        {
            MatInventoryItemInfoSetDifferenceAmount.SetDifferenceAmount(MatInventoryItemInfo);
        }


        /// <summary>
        /// 实盘数量改变命令函数
        /// </summary>
        private void OnExecuteActualAmounTextChangedCommand()
        {
            MatInventoryItemInfoSetDifferenceAmount.SetDifferenceAmount(MatInventoryItemInfo);
        }

        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }

        /// <summary>
        /// 选中产品执行命令
        /// </summary>
        private void OnExecutematerialbatchSelectionChangedCommand()
        {
            MaterialBatchInfoModel materialbatchInfo = MaterialBatchInfoList.FirstOrDefault(m => m.Id == MatInventoryItemInfo.MaterialBatch_Id);
            //
            if (materialbatchInfo != null)
            {
                //MatStorageModifyInfo.WareHouseLocationName = materialbatchInfo.WareHouseLocationName;
                //MatStorageModifyInfo.MaterialCode = materialbatchInfo.MaterialCode;
                //MatStorageModifyInfo.MaterialName = materialbatchInfo.MaterialName;
                MatInventoryItemInfo.AccuntAmount = materialbatchInfo.Quantity;
                MatInventoryItemInfo.FullPalletQuantity = materialbatchInfo.FullPalletQuantity;
            }
            else
            {
                //MatStorageModifyInfo.WareHouseLocationName = null;
                //MatStorageModifyInfo.MaterialCode = null;
                //MatStorageModifyInfo.MaterialName = null;
                MatInventoryItemInfo.AccuntAmount = null;
                MatInventoryItemInfo.FullPalletQuantity = null;
            }
        }

        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

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
                    //
                    int i = 0;
                    foreach (string temp in MaterialBatchInfoList1)
                    {
                        MaterialBatchInfoModel materialbatchInfo = MaterialBatchInfoList.Where(m => m.Id.ToString().Contains(temp)).FirstOrDefault();
                        if (materialbatchInfo != null && materialbatchInfo.Id != MatInventoryItemInfo.MaterialBatch_Id)
                        {
                            MaterialBatchInfoList.Remove(materialbatchInfo);
                            i++;
                        }
                    }
                    //
                    TotalCounts = result.Data.Total - i;
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
