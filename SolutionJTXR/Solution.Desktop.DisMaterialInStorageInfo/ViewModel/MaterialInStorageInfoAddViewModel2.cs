using GalaSoft.MvvmLight.Messaging;
using Solution.Desktop.Core;
using Solution.Desktop.MaterialInfo.Model;
using Solution.Desktop.MaterialInStorageInfo.Model;
using Solution.Desktop.MatPalletInfo.Model;
using Solution.Desktop.MatSupplierInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.Model;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static Solution.Desktop.MatWareHouseInfo.Model.InStorageTypeEnumModel;

namespace Solution.Desktop.MaterialInStorageInfo.ViewModel
{
    /// <summary>
    /// 新增VM
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MaterialInStorageInfoAddViewModel2 : VmBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialInStorageInfoAddViewModel2()
        {
            ConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConfirmCommand, OnCanExecuteConfirmCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteCancelCommand);
            //
            MaterialSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteMaterialSelectionChangedCommand);
            InquantityTextChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteInquantityTextChangedCommand);
            //
            getPageDataMaterial(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getPageDataMatPallet(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            getPageDataMatSupplier(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            //
            MaterialInStorageInfo.InStorageBillCode = BillCodeUtility.getInStorageBillCode();
            MaterialInStorageInfo.InStorageType = InStorageType.原料手动入库;
            MaterialInStorageInfo.InStorageStatus = InStorageStatusEnumModel.InStorageStatus.待组盘;
            MaterialInStorageInfo.CreateUser = GlobalData.CurrentLoginUser.UserName;
            MaterialInStorageInfo.AuditStatus = AuditStatusEnumModel.AuditStatus.未审核;
        }

        #region 物料入库模型
        private MaterialInStorageInfoModel materialinstorageModel = new MaterialInStorageInfoModel();
        /// <summary>
        /// 物料入库模型
        /// </summary>
        public MaterialInStorageInfoModel MaterialInStorageInfo
        {
            get { return materialinstorageModel; }
            set
            {
                Set(ref materialinstorageModel, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region 入库类型模型 
        public System.Array instorageTypes = Enum.GetValues(typeof(InStorageTypeEnumModel.InStorageType));

        public System.Array InStorageTypes
        {
            get { return instorageTypes; }
            set
            {
                Set(ref instorageTypes, value);
            }
        }
        #endregion


        #region 入库状态模型 
        public System.Array instorageStatus = Enum.GetValues(typeof(InStorageStatusEnumModel.InStorageStatus));

        public System.Array InStorageStatus
        {
            get { return instorageStatus; }
            set
            {
                Set(ref instorageStatus, value);
            }
        }
        #endregion

        #region 审核状态模型 
        public System.Array auditStatus = Enum.GetValues(typeof(AuditStatusEnumModel.AuditStatus));

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
        /// 入库数量改变命令
        /// </summary>
        public ICommand InquantityTextChangedCommand { get; set; }
        //

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConfirmCommand()
        {
            return MaterialInStorageInfo == null ? false : MaterialInStorageInfo.IsValidated;
        }

        /// <summary>
        /// 执行确认命令函数
        /// </summary>
        private void OnExecuteConfirmCommand()
        {
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MaterialInStorageInfo/Add",
                Utility.JsonHelper.ToJson(new List<MaterialInStorageInfoModel> { MaterialInStorageInfo }));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MaterialInStorageInfoModel>(MaterialInStorageInfo, MessengerToken.DataChanged);
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
            MaterialInfoModel materialInfo = MaterialInfoList.FirstOrDefault(m => m.Id == MaterialInStorageInfo.MaterialID);
            //
            if (materialInfo != null)
            {
                MaterialInStorageInfo.MaterialCode = materialInfo.MaterialCode;
                MaterialInStorageInfo.FullPalletQuantity = materialInfo.FullPalletQuantity;
            }
            else
            {
                MaterialInStorageInfo.MaterialCode = null;
                MaterialInStorageInfo.FullPalletQuantity = null;
            }
            //
            MaterialInStorageInfoSetPalletQuantity.SetPalletQuantity(MaterialInStorageInfo);
        }


        /// <summary>
        /// 入库数量改变命令函数
        /// </summary>
        private void OnExecuteInquantityTextChangedCommand()
        {
            MaterialInStorageInfoSetPalletQuantity.SetPalletQuantity(MaterialInStorageInfo);
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


        #region 供应商信息模型,用于列表数据显示
        private ObservableCollection<MatSupplierInfoModel> matsupplierinfoList = new ObservableCollection<MatSupplierInfoModel>();

        /// <summary>
        /// 供应商信息数据
        /// </summary>
        public ObservableCollection<MatSupplierInfoModel> MatSupplierInfoList
        {
            get { return matsupplierinfoList; }
            set { Set(ref matsupplierinfoList, value); }
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialInfoModel>>>(GlobalData.ServerRootUri + "MaterialInfo/PageData1", Utility.JsonHelper.ToJson(pageRepuestParams));

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
                    //MatPalletInfoList = new ObservableCollection<MatPalletInfoModel>(result.Data.Data);
                    //TotalCounts = result.Data.Total;
                    //
                    MatPalletInfoList = new ObservableCollection<MatPalletInfoModel>();
                    foreach (var data in result.Data.Data)
                    {
                        MatPalletInfoModel matpalletinfo = new MatPalletInfoModel();
                        matpalletinfo = data;
                        //
                        if (matpalletinfo.WareHouseLocationCode == null)
                        {
                            MatPalletInfoList.Add(matpalletinfo);
                        }
                        //
                    }
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


        #region 分页数据查询
        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// 
        /// <summary>
        /// 获取供应商列表
        /// </summary>
        private void getPageDataMatSupplier(int pageIndex, int pageSize)
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatSupplierInfoModel>>>(GlobalData.ServerRootUri + "MatSupplierInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取供应商信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("供应商信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MatSupplierInfoList = new ObservableCollection<MatSupplierInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MatSupplierInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatSupplierInfoList = new ObservableCollection<MatSupplierInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询供应商信息失败，请联系管理员！";
            }
        }

        #endregion
    }
}
