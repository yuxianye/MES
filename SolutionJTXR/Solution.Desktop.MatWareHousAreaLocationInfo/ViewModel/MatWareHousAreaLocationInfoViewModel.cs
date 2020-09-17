using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.MatWareHousAreaLocationInfo.Model;
using Solution.Desktop.MatWareHousAreaLocationInfo.View;
using Solution.Desktop.MatWareHouseAreaInfo.Model;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Utility;
using Solution.Utility.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialUnitEnumModel;

namespace Solution.Desktop.MatWareHousAreaLocationInfo.ViewModel
{
    /// <summary>
    /// 仓库类型信息Vm
    /// 注意：模块主VM与增加和编辑VM继承的基类不同
    /// </summary>
    public class MatWareHousAreaLocationInfoViewModel : PageableViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatWareHousAreaLocationInfoViewModel()
        {
            initCommand();
            registerMessenger();
            //
            //右键快捷菜单
            MenuItems = new List<MenuItem>()
            {
                new MenuItem (){Icon =BitmapImageHelper.GetImage(@"pack://application:,,,/Solution.Desktop.Resource;component/Images/Refresh_32x32.png") ,Header=ResourceHelper.FindResource ("Refresh"),Command =RefreshCommand},
            };
            // 
            getMatWareHousAreaLocationType();
            getPageDataMatWareHouse(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
        }

        #region 命令定义和初始化


        /// <summary>
        /// 刷新命令
        /// </summary>
        public ICommand RefreshCommand { get; set; }

        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand SearchCommand { get; set; }

        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand SearchItemCommand { get; set; }
        public ICommand SearchItem2Command { get; set; }


        /// <summary>
        /// 下拉列表改变
        /// </summary>
        public ICommand matwarehouseSelectionChangedCommand { get; set; }

        /// <summary>
        /// 下拉列表改变
        /// </summary>
        public ICommand matwarehouseareaSelectionChangedCommand { get; set; }

        Guid matwarehouseareaSelectionGuid;

        bool bDataGridDetailCreate = false;

        int DataGridDetailColumnCount = 0;

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void initCommand()
        {
            RefreshCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshCommand);
            SearchCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<string>(OnExecuteSearchCommand);
            SearchItemCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<DataGrid>(OnExecuteSearchItemCommand);
            SearchItem2Command = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<DataGrid>(OnExecuteSearchItem2Command);
            //
            matwarehouseSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<Guid>(OnExecutematwarehouseSelectionChangedCommand);
            matwarehouseareaSelectionChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand<Guid>(OnExecutematwarehouseareaSelectionChangedCommand);
            //
        }

        #endregion

        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            Messenger.Default.Register<MatWareHousAreaLocationInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }

        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            //Messenger.Default.Unregister<ViewInfo>(this, Model.MessengerToken.Navigate, Navigate);
            //Messenger.Default.Unregister<object>(this, Model.MessengerToken.ClosePopup, ClosePopup);
            //Messenger.Default.Unregister<MenuInfo>(this, Model.MessengerToken.SetMenuStatus, SetMenuStatus);
            //
            Messenger.Default.Unregister<MatWareHousAreaLocationInfoModel>(this, MessengerToken.DataChanged, dataChanged);
        }
        #endregion

        /// <summary>
        /// 模型数据改变
        /// </summary>
        /// <param name="obj"></param>
        private void dataChanged(MatWareHousAreaLocationInfoModel MatWareHousAreaLocationInfoChange)
        {
            //getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            var tmpModel = MatWareHousAreaLocationInfoList.FirstOrDefault(a => a.Id == MatWareHousAreaLocationInfoChange.Id);
            this.MatWareHousAreaLocationInfo = MatWareHousAreaLocationInfoList.FirstOrDefault();
        }

        #region 模型
        /// <summary>
        /// 仓库类型信息模型
        /// </summary>
        private MatWareHousAreaLocationInfoModel matwarehousetypeModel;
        /// <summary>
        /// 仓库类型信息模型
        /// </summary>
        public MatWareHousAreaLocationInfoModel MatWareHousAreaLocationInfo
        {
            get { return matwarehousetypeModel; }
            set { Set(ref matwarehousetypeModel, value); }
        }
        #endregion

        #region 仓库类型信息模型,用于列表数据显示
        private ObservableCollection<MatWareHousAreaLocationInfoModel> matwarehousetypeModelList = new ObservableCollection<MatWareHousAreaLocationInfoModel>();

        /// <summary>
        /// 仓库类型信息数据
        /// </summary>
        public ObservableCollection<MatWareHousAreaLocationInfoModel> MatWareHousAreaLocationInfoList
        {
            get { return matwarehousetypeModelList; }
            set { Set(ref matwarehousetypeModelList, value); }
        }
        #endregion

        #region 仓库类型信息模型,用于列表数据显示
        private DataTable matwarehousarealocationdatatable = new DataTable();

        /// <summary>
        /// 仓库类型信息数据
        /// </summary>
        public DataTable MatWareHousAreaLocationDataTable
        {
            get { return matwarehousarealocationdatatable; }
            set { Set(ref matwarehousarealocationdatatable, value); }
        }
        #endregion
        

        void getWareHousAreaLocationInfo(Guid matwarehouseareaSelection)
        {
            string smatwarehouseareaSelection = "";
            if (matwarehouseareaSelection != Guid.Empty)
            {
                smatwarehouseareaSelection = matwarehouseareaSelection.ToString();
            }
            //
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<MatWareHousAreaLocationInfoModel>>>
             (GlobalData.ServerRootUri + $"MatWareHousAreaLocationInfo/GetPageData1/{smatwarehouseareaSelection}");
            //
            if (result != null && result.Data != null && result.Data.Any())
            {
                MatWareHousAreaLocationInfoList = new ObservableCollection<MatWareHousAreaLocationInfoModel>(result.Data);
                TotalCounts = result.Data.Count;
                //
                DataGridDetailColumnCount = MatWareHousAreaLocationInfoList.FirstOrDefault().ColumnNumber;
                SetDataTableData();
                //
                bDataGridDetailCreate = true;
                MatWareHousAreaLocationItemInfoList.Clear();
            }
            else
            {
                MatWareHousAreaLocationInfoList?.Clear();
                TotalCounts = 0;
                Application.Current.Resources["UiMessage"] = "未找到数据";
            }
        }

        void DataGridDetailCreate(DataGrid DataGridDetail)
        {
            if (bDataGridDetailCreate)
            {
                for (int ii = DataGridDetail.Columns.Count - 1; ii >= 0; ii--)
                {
                    DataGridColumn datagridColumn = DataGridDetail.Columns[ii];
                    if (datagridColumn.Header.ToString().IndexOf("列") != -1)
                    {
                        DataGridDetail.Columns.Remove(datagridColumn);
                    }
                }
                //
                for (int i = 0; i < DataGridDetailColumnCount; i++)
                {
                    DataGridTextColumn a1 = new DataGridTextColumn();
                    string sColumn = string.Format($"{i + 1:000}列", i + 1);
                    a1.Header = sColumn;
                    //
                    //Style aa = new Style();
                    //aa.TargetType = "DataGridColumnHeader";
                    //aa.Setters = Style..Setters.pr.HorizontalContentAlignment = "DataGridColumnHeader";
                    //a1.HeaderStyle = "{DynamicResource DataGridColumnHeaderStyle}";
                    a1.Width = 72;
                    a1.Binding = new Binding("ColumnNumber" + string.Format($"{i + 1:00}", i + 1));
                    DataGridDetail.Columns.Insert(1 + i, a1);
                }
                //
                bDataGridDetailCreate = false;
            }
        }

        void SetDataTableData()
        {
            var aa = MatWareHousAreaLocationInfoList;
            //
            MatWareHousAreaLocationDataTable.Columns.Clear();
            MatWareHousAreaLocationDataTable.Clear();
            //
            MatWareHousAreaLocationDataTable.Columns.Add(new DataColumn("WareHouseName", typeof(string)));
            MatWareHousAreaLocationDataTable.Columns.Add(new DataColumn("WareHouseAreaName", typeof(string)));
            MatWareHousAreaLocationDataTable.Columns.Add(new DataColumn("LayerNumber", typeof(string)));
            //
            for (int i = 0; i < DataGridDetailColumnCount; i++)
            {
                string sColumn = string.Format($"ColumnNumber{i + 1:00}", i + 1);
                MatWareHousAreaLocationDataTable.Columns.Add(new DataColumn(sColumn, typeof(string)));
            }
            //
            for (int i = 0; i < TotalCounts; i++)
            {
                DataRow dr = MatWareHousAreaLocationDataTable.NewRow();
                dr["WareHouseName"] = MatWareHousAreaLocationInfoList[i].LayerNumber;
                dr["WareHouseAreaName"] = MatWareHousAreaLocationInfoList[i].WareHouseAreaName;
                dr["LayerNumber"] = MatWareHousAreaLocationInfoList[i].LayerNumber;
                //
                for (int k = 0; k < DataGridDetailColumnCount; k++)
                {
                    string sColumn = string.Format($"ColumnNumber{k + 1:00}", k + 1);
                    //
                    dr[sColumn] = MatWareHousAreaLocationInfoList[i].WareHouseColumns[k];
                }
                MatWareHousAreaLocationDataTable.Rows.Add(dr);
            }
            //DataGridDetail.CanUserAddRows = false;
        }

        #region 仓库类型信息模型,用于列表数据显示
        private ObservableCollection<MatWareHousAreaLocationTypeModel> matwarehousarealocationinfolist2 = new ObservableCollection<MatWareHousAreaLocationTypeModel>();

        /// <summary>
        /// 仓库类型信息数据
        /// </summary>
        public ObservableCollection<MatWareHousAreaLocationTypeModel> MatWareHousAreaLocationInfoList2
        {
            get { return matwarehousarealocationinfolist2; }
            set { Set(ref matwarehousarealocationinfolist2, value); }
        }
        #endregion


        void getMatWareHousAreaLocationType()
        {
            MatWareHousAreaLocationTypeModel statusModel1 = new MatWareHousAreaLocationTypeModel();
            statusModel1.MatWareHouseLocationType1 = "";
            statusModel1.MatWareHouseLocationType2 = "";
            statusModel1.MatWareHouseLocationType3 = "";
            statusModel1.MatWareHouseLocationType4 = "";
            statusModel1.MatWareHouseLocationType5 = "";
            MatWareHousAreaLocationInfoList2.Add(statusModel1);
        }

        #region 仓库类型信息模型,用于列表数据显示
        private ObservableCollection<MatWareHousAreaLocationItemInfoModel> matwarehousarealocationiteminfolist = new ObservableCollection<MatWareHousAreaLocationItemInfoModel>();

        /// <summary>
        /// 仓库类型信息数据
        /// </summary>
        public ObservableCollection<MatWareHousAreaLocationItemInfoModel> MatWareHousAreaLocationItemInfoList
        {
            get { return matwarehousarealocationiteminfolist; }
            set { Set(ref matwarehousarealocationiteminfolist, value); }
        }
        #endregion


        #region 分页数据查询
        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 分页执行函数
        /// </summary>
        /// <param name="e"></param>
        public override void OnExecutePageChangedCommand(PageChangedEventArgs e)
        {
            Utility.LogHelper.Info(e.PageIndex.ToString() + " " + e.PageSize);
            //getPageData(e.PageIndex, e.PageSize);
        }
   

        #endregion

        #region 命令和消息等执行函数


        private void OnExecutematwarehouseSelectionChangedCommand(Guid matwarehouseSelection)
        {
            getPageDataMatWareHouseArea(matwarehouseSelection);
        }

        private void OnExecutematwarehouseareaSelectionChangedCommand(Guid matwarehouseareaSelection)
        {
            matwarehouseareaSelectionGuid = matwarehouseareaSelection;
            //
            getWareHousAreaLocationInfo(matwarehouseareaSelectionGuid);
        }

        /// <summary>
        /// 执行刷新命令
        /// </summary>
        private void OnExecuteRefreshCommand()
        {
            //pageRepuestParams.FilterGroup = null;
            //getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            //
            if (!(matwarehouseareaSelectionGuid == Guid.Empty))
            {
                getWareHousAreaLocationInfo(matwarehouseareaSelectionGuid);
            }
        }
        

        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchCommand(string txt)
        {
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRuleName = new FilterRule("WareHouseAreaName", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleCode = new FilterRule("WareHouseAreaCode", txt.Trim(), FilterOperate.Contains);
            FilterRule filterRuleCode2 = new FilterRule("LayerNumber", txt.Trim(), FilterOperate.Contains);
            filterGroup.Rules.Add(filterRuleName);
            filterGroup.Rules.Add(filterRuleCode);
            filterGroup.Rules.Add(filterRuleCode2);
            //
            pageRepuestParams.FilterGroup = filterGroup;
            //getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        }


        /// <summary>
        /// 执行查询命令
        /// </summary>
        private void OnExecuteSearchItemCommand(DataGrid DataGridDetail)
        {
            if (DataGridDetail.CurrentCell != null &&
                DataGridDetail.CurrentCell.Column != null)
            {
                foreach (DataGridColumn datagridColumn in DataGridDetail.Columns)
                {
                    if (datagridColumn.Header.Equals(DataGridDetail.CurrentCell.Column.Header))
                    {
                        TextBlock textBlock = datagridColumn.GetCellContent(DataGridDetail.CurrentItem) as TextBlock;
                        //MatWareHousAreaLocationInfoModel matwarehousarealocationInfo = DataGridDetail.CurrentItem as MatWareHousAreaLocationInfoModel;
                        //
                        if (textBlock != null && textBlock.Tag != null)
                        {
                            string[] asInfo = textBlock.Tag.ToString().Split(' ');
                            string sWareHouseLocationCode = "";
                            //
                            if (asInfo.Length >= 7)
                            {
                                sWareHouseLocationCode = asInfo[2];
                                getWareHousAreaLocationItemInfo(sWareHouseLocationCode);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void OnExecuteSearchItem2Command(DataGrid DataGridDetail)
        {
            DataGridDetailCreate(DataGridDetail);
        }

        void getWareHousAreaLocationItemInfo(string warehousarealocation)
        {
            var result = Utility.Http.HttpClientHelper.GetResponse<OperationResult<List<MatWareHousAreaLocationItemInfoModel>>>
                (GlobalData.ServerRootUri + $"MatWareHousAreaLocationInfo/GetPageData2/{warehousarealocation}");
            //
            if (result != null && result.Data != null && result.Data.Any())
            {
                MatWareHousAreaLocationItemInfoList = new ObservableCollection<MatWareHousAreaLocationItemInfoModel>(result.Data);
                //
                TotalCounts = result.Data.Count;
            }
            else
            {
                MatWareHousAreaLocationInfoList?.Clear();
                TotalCounts = 0;
                Application.Current.Resources["UiMessage"] = "未找到数据";
            }
        }

        #endregion

        /// <summary>
        /// 清理资源
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
            unRegisterMessenger();
        }

        /// ////////////////////////////////////////////////////////////////////////

        #region 仓库信息模型,用于列表数据显示
        private ObservableCollection<MatWareHouseInfoModel> matwarehouseModelList = new ObservableCollection<MatWareHouseInfoModel>();

        /// <summary>
        /// 仓库信息数据
        /// </summary>
        public ObservableCollection<MatWareHouseInfoModel> MatWareHouseInfoList
        {
            get { return matwarehouseModelList; }
            set { Set(ref matwarehouseModelList, value); }
        }

        #endregion

        /// ////////////////////////////////////////////////////////////////////////

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
        private void getPageDataMatWareHouse(int pageIndex, int pageSize)
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseInfoModel>>>(GlobalData.ServerRootUri + "MatWareHouseInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取仓库信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("仓库信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MatWareHouseInfoList = new ObservableCollection<MatWareHouseInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MatWareHouseInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatWareHouseInfoList = new ObservableCollection<MatWareHouseInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询仓库信息失败，请联系管理员！";
            }
        }
        #endregion


        #region 仓库货区信息模型,用于列表数据显示
        private ObservableCollection<MatWareHouseAreaInfoModel> matwarehouseareaModelList = new ObservableCollection<MatWareHouseAreaInfoModel>();

        /// <summary>
        /// 仓库货区信息数据
        /// </summary>
        public ObservableCollection<MatWareHouseAreaInfoModel> MatWareHouseAreaInfoList
        {
            get { return matwarehouseareaModelList; }
            set { Set(ref matwarehouseareaModelList, value); }
        }

        #endregion

        /// //////////////////////////////////////////////////////////////////////// 

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
        private void getPageDataMatWareHouseArea(Guid matwarehouseSelection)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize"));

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/WareHouseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            //
            MatWareHouseAreaInfoModel matwarehouseareainfoModel = new MatWareHouseAreaInfoModel();
            matwarehouseareainfoModel.Id = matwarehouseSelection;
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseAreaInfoModel>>>(GlobalData.ServerRootUri + "MatWareHouseAreaInfo/GetWareHouseAreaListByID", Utility.JsonHelper.ToJson(matwarehouseareainfoModel));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取仓库货区信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("仓库货区信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //TotalCounts = result.Data.Total;
                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
                    MatWareHouseAreaInfoList = new ObservableCollection<MatWareHouseAreaInfoModel>(result.Data.Data);
                    TotalCounts = result.Data.Total;
                }
                else
                {
                    MatWareHouseAreaInfoList?.Clear();
                    TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatWareHouseAreaInfoList = new ObservableCollection<MatWareHouseAreaInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询仓库货区信息失败，请联系管理员！";
            }
        }
        #endregion
    }
}
