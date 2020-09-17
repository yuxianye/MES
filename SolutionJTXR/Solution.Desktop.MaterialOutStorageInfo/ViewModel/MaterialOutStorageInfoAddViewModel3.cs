using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.MaterialInfo.Model;
using Solution.Desktop.MaterialOutStorageInfo.Model;
using Solution.Desktop.MatPalletInfo.Model;
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

namespace Solution.Desktop.MaterialOutStorageInfo.ViewModel
{
    /// <summary>
    /// 新增VM
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MaterialOutStorageInfoAddViewModel3 : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialOutStorageInfoAddViewModel3()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //
            MaterialSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteMaterialSelectionChangedCommand);
            OutquantityTextChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteOutquantityTextChangedCommand);
            //
            getPageDataMaterial(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getPageDataMatPallet(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            //
            MaterialOutStorageInfo.OutStorageBillCode = BillCodeUtility.getOutStorageBillCode();
            //原料自动出库演示
            MaterialOutStorageInfo.OutStorageType = OutStorageTypeEnumModel.OutStorageType.MaterialAutoShowOutStorageType;
            //待组盘
            MaterialOutStorageInfo.OutStorageStatus = OutStorageStatusEnumModel.OutStorageStatus.OutStorageUnFinishStatus;
            MaterialOutStorageInfo.CreateUser = GlobalData.CurrentLoginUser.UserName;
            //未审核
            MaterialOutStorageInfo.AuditStatus = AuditStatusEnumModel.AuditStatus.UnAuditStatus;
            //配盘数量限制为1
            MaterialOutStorageInfo.PalletQuantity = 1;
            // MaterialOutStorageInfo.FullPalletQuantity = 6;
        }



        #region 物料出库模型
        private MaterialOutStorageInfoModel materialoutstorageModel = new MaterialOutStorageInfoModel();
        /// <summary>
        /// 物料出库模型
        /// </summary>
        public MaterialOutStorageInfoModel MaterialOutStorageInfo
        {
            get { return materialoutstorageModel; }
            set
            {
                Set(ref materialoutstorageModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 出库类型模型 
        System.Array outstorageTypes = Enum.GetValues(typeof(OutStorageTypeEnumModel.OutStorageType)).Cast<OutStorageTypeEnumModel.OutStorageType>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array OutStorageTypes
        {
            get { return outstorageTypes; }
            set
            {
                Set(ref outstorageTypes, value);
            }
        }
        #endregion

        #region 出库状态模型 
        System.Array outstorageStatus = Enum.GetValues(typeof(OutStorageStatusEnumModel.OutStorageStatus)).Cast<OutStorageStatusEnumModel.OutStorageStatus>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array OutStorageStatus
        {
            get { return outstorageStatus; }
            set
            {
                Set(ref outstorageStatus, value);
            }
        }
        #endregion

        #region 审核状态模型 
        System.Array auditStatus = Enum.GetValues(typeof(AuditStatusEnumModel.AuditStatus)).Cast<AuditStatusEnumModel.AuditStatus>().Select(value => new { Key = value, Value = value.ToDescription() }).ToArray();

        public System.Array AuditStatus
        {
            get { return auditStatus; }
            set
            {
                Set(ref auditStatus, value);
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
        /// 物料选择项改变命令
        /// </summary>
        public ICommand MaterialSelectionChangedCommand { get; set; }


        /// <summary>
        /// 出库数量改变命令
        /// </summary>
        public ICommand OutquantityTextChangedCommand { get; set; }
        //

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return MaterialOutStorageInfo == null ? false : MaterialOutStorageInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MaterialOutStorageInfo/Add",
                Utility.JsonHelper.ToJson(new List<MaterialOutStorageInfoModel> { MaterialOutStorageInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MaterialOutStorageInfoModel>(MaterialOutStorageInfo, MessengerToken.DataChanged);
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
        /// 物料选择项改变命令函数
        /// </summary>
        private void OnExecuteMaterialSelectionChangedCommand()
        {
            MaterialInfoModel materialInfo = MaterialInfoList.FirstOrDefault(m => m.Id == MaterialOutStorageInfo.MaterialID);
            //
            if (materialInfo != null)
            {
                MaterialOutStorageInfo.MaterialCode = materialInfo.MaterialCode;
                MaterialOutStorageInfo.FullPalletQuantity = materialInfo.FullPalletQuantity;
            }
            else
            {
                MaterialOutStorageInfo.MaterialCode = null;
                MaterialOutStorageInfo.FullPalletQuantity = null;
            }
            //
            MaterialOutStorageInfoSetPalletQuantity.SetPalletQuantity(MaterialOutStorageInfo);
        }

        /// <summary>
        /// 出库数量改变命令函数
        /// </summary>
        private void OnExecuteOutquantityTextChangedCommand()
        {
            MaterialOutStorageInfoSetPalletQuantity.SetPalletQuantity(MaterialOutStorageInfo);
        }


        /// <summary>
        /// 执行取消命令
        /// </summary>
        private void OnExecuteCancelCommand()
        {
            Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
        }
        ////////////////////////////////////////////////////////////

        #region 物料信息模型,用于列表数据显示
        private ObservableCollection<MaterialInfoModel> materialinfoList = new ObservableCollection<MaterialInfoModel>();

        /// <summary>
        /// 物料信息数据
        /// </summary>
        public ObservableCollection<MaterialInfoModel> MaterialInfoList
        {
            get { return materialinfoList; }
            set { Set(ref materialinfoList, value); }
        }

        #endregion


        #region 托盘信息模型,用于列表数据显示
        private ObservableCollection<MatPalletInfoModel> matpalletinfoList = new ObservableCollection<MatPalletInfoModel>();

        /// <summary>
        /// 托盘信息数据
        /// </summary>
        public ObservableCollection<MatPalletInfoModel> MatPalletInfoList
        {
            get { return matpalletinfoList; }
            set { Set(ref matpalletinfoList, value); }
        }

        #endregion

        /// ////////////////////////////////////////////////////////////////////////

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


        #region 分页数据查询
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// 
        /// <summary>
        /// 获取仓库列表
        /// </summary>
        private void getPageDataMaterial(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/WareHouseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            //
            //为方便测试，物料使用原料类型
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialInfoModel>>>(GlobalData.ServerRootUri + "MaterialInfo/PageData1", Utility.JsonHelper.ToJson(pageRepuestParams));
            //
            //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialInfoModel>>>(GlobalData.ServerRootUri + "MaterialInfo/PageData2", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取物料信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("物料信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MaterialInfoList = new ObservableCollection<MaterialInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialInfoList = new ObservableCollection<MaterialInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询物料信息失败，请联系管理员！";
            }
        }
        #endregion

        #region 分页数据查询
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// 
        /// <summary>
        /// 获取托盘列表
        /// </summary>
        private void getPageDataMatPallet(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/WareHouseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatPalletInfoModel>>>(GlobalData.ServerRootUri + "MatPalletInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取托盘信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("托盘信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MatPalletInfoList = new ObservableCollection<MatPalletInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MatPalletInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatPalletInfoList = new ObservableCollection<MatPalletInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询托盘信息失败，请联系管理员！";
            }
        }
        #endregion
    }
}
