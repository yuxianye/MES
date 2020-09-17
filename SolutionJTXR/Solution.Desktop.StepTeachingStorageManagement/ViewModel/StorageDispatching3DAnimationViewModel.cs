using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.MaterialOutStorageInfo.Model;
using Solution.Desktop.MaterialBatchInfo.Model;
using Solution.Desktop.DisStepActionInfo.Model;
using Solution.Desktop.Core;
using Solution.Desktop.Core.Enum;
using Solution.Desktop.Core.Model;
using Solution.Desktop.Model;
using Solution.Desktop.ViewModel;
using Solution.Utility;
using Solution.Utility.Extensions;
using Solution.Utility.Socket;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Solution.Desktop.MatWareHouseInfo.Model;
using Solution.Desktop.MaterialInStorageInfo.Model;
using Solution.Desktop.MatWareHouseLocationInfo.Model;
using Solution.Desktop.StepTeachingStorageManagement.Model;
using Solution.Desktop.MatStorageMoveInfo.Model;

namespace Solution.Desktop.StepTeachingStorageManagement.ViewModel
{      /// <summary>
       /// agv动画
       /// 小车动态生成，小车状态根据底层数据改变后，动态改变小车颜色
       /// 路线动态生成，两个地标点之间是一段线路，根据地标点俩俩一对生成路线，起点到终点是正方向，小车正向移动。终点到起点是反方向，小车逆向移动。
       /// 小车到地标点后，根据运动的路径，选择动画的path,然后结合小车速度，和地标点距离计算路段的时间
       /// 
       /// 后端试试通讯变化后，根据绑定的点位名称，更新界面车辆的状态、报警等信息。
       /// 界面控制功能部分，根据小车的点表和意义，通过实时通讯接口发送命令到车辆。
       /// 
       /// AGV车辆管理与界面更新采用面向对象的控制方式，通过AgvCar控件综合入口，对agv车辆经行各种管控和界面的动画更新，
       /// 控件里面集成模型数据该数据从后台取出后赋值给控件的属性。
       /// AGV控件对外暴露业务控制点位，具体控制点位有内部处理。
       /// </summary>
    public class StorageDispatching3DAnimationViewModel : VmBase
    {
        private void timercallback(object state)
        {
        }
        #region 绑定AGV和堆垛机位置
        /// <summary>
        /// 绑定AGV和堆垛机位置
        /// </summary>
        private void SetAgvStackerPosition()
        {
            if (ClientDataEntities.Count > 0)
            {
                var v = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_CurrentPosition")).FirstOrDefault();
                if (!Equals(v, null))
                {
                    if (!Equals(v.Value, null))
                    {
                        string agvposition = v.Value.ToString();
                        int agvp = int.Parse(agvposition);
                        AgvPosition = new Point3D(agvp % 100, AgvPosition.Y, AgvPosition.Z);
                        AgvNamePosition = new Point3D(agvp % 100, AgvPosition.Y, AgvPosition.Z + 5.5);
                    }
                }
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("Stacker_CurrentPosition")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        int stackp = int.Parse(v1.Value.ToString());
                        StackerPosition = new Point3D(StackerPosition.X, stackp % 100, StackerPosition.Z);
                    }
                }
            }
        }
        #endregion

        #region 库位信息模型,用于列表数据显示
        private ObservableCollection<MatWareHouseLocationInfoModel> matwarehouselocationinfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();

        /// <summary>
        /// 库位信息数据
        /// </summary>
        public ObservableCollection<MatWareHouseLocationInfoModel> MatWareHouseLocationInfoList
        {
            get { return matwarehouselocationinfoList; }
            set { Set(ref matwarehouselocationinfoList, value); }
        }
        #endregion

        private void InitParaMeters()
        {
            StepActionCode = "StepAction_MaterialInStorage";
            isMaterialInConfirmTip = false;
            isMaterialInFinishTip = false;
            isProductOutFinishTip = false;
            isStorageMoveFinishTip = false;
            isEmptyPalletInFinishTip = false;
        }
        /// <summary>
        /// 原料空托盘库位/空库位列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getPageDataMatWareHouseLocation(int pageIndex, int pageSize, int type)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortField = "CreatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            FilterGroup filterGroup = new FilterGroup(FilterOperate.And);
            FilterRule filterRule1 = new FilterRule("WareHouseLocationType", 1, FilterOperate.Equal);
            FilterRule filterRule2 = new FilterRule("IsUse", 1, FilterOperate.Equal);
            filterGroup.Rules.Add(filterRule1);
            filterGroup.Rules.Add(filterRule2);
            pageRepuestParams.FilterGroup = filterGroup;
            //原料手动入库
            string sType = "";
            if (type == 1)
            {
                sType = "MatWareHouseLocationInfo/PageDataPallet1";
            }
            //空托盘入库
            if (type == 2)
            {
                sType = "MatWareHouseLocationInfo/PageDataPallet2";
            }
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseLocationInfoModel>>>(GlobalData.ServerRootUri + sType, Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取库位信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("库位信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    if (type == 1)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            MatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>(result.Data.Data.Where(x => x.StorageQuantity == null || x.StorageQuantity <= 0).Take(1).ToList());
                        }));
                    }
                    if (type == 2)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            MatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>(result.Data.Data.Take(1).ToList());
                        }));
                    }
                    //foreach (var data in result.Data.Data)
                    //{
                    //    MatWareHouseLocationInfoModel matwarehouselocationinfo = new MatWareHouseLocationInfoModel();
                    //    matwarehouselocationinfo = data;
                    //    //
                    //    //原料手动入库
                    //    if (type == 1)
                    //    {
                    //        //原料库位
                    //        if (matwarehouselocationinfo.StorageQuantity == null || matwarehouselocationinfo.StorageQuantity <= 0)
                    //        {
                    //            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    //            {
                    //                MatWareHouseLocationInfoList.Add(matwarehouselocationinfo);
                    //            }));

                    //        }
                    //    }
                    //    //空托盘入库
                    //    if (type == 2)
                    //    {
                    //        //原料库位                           
                    //        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    //        {
                    //            MatWareHouseLocationInfoList.Add(matwarehouselocationinfo);
                    //        }));
                    //    }
                    //}
                    int TotalCounts = result.Data.Total;
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MatWareHouseLocationInfoList?.Clear();
                    }));
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询库位信息失败，请联系管理员！";
            }
        }

        System.Threading.Timer timer;
        public StorageDispatching3DAnimationViewModel()
        {
            initCommand();
        }
        private Point3D agvPosition = new Point3D(40, 105, 2.5);
        public Point3D AgvPosition
        {
            get
            {
                return agvPosition;
            }

            protected set
            {
                Set(ref agvPosition, value, "AgvPosition");
            }
        }
        private Point3D agvNamePosition = new Point3D(40, 105, 8);
        public Point3D AgvNamePosition
        {
            get
            {
                return agvNamePosition;
            }

            protected set
            {
                Set(ref agvNamePosition, value, "AgvNamePosition");
            }
        }


        private Point3D stackerPosition = new Point3D(30, 60, 10);

        public Point3D StackerPosition
        {
            get
            {
                return stackerPosition;
            }

            protected set
            {
                Set(ref stackerPosition, value, "StackerPosition");
            }
        }
        private string StepActionCode = "StepAction_MaterialInStorage";
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void init()
        {
            InitClientData();
            InitMaterialData();
        }
        private void InitMaterialInfoPageData()
        {
            getMaterialInStorageInfoPageData(1, 1, 1);
            getPageDataMatWareHouseLocation(1, 200, 1);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MaterialBatchInfoList?.Clear();
                MatStorageMoveInfoList?.Clear();
                MaterialOutStorageInfoList?.Clear();
            }));
        }
        private void InitProductOutInfoPageData()
        {
            getMaterialOutStorageInfoPageData(1, 1);
            if (MaterialOutStorageInfoList.Count > 0)
            {
                GetPageDataMaterialBatch(1, 100);
            }
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MatWareHouseLocationInfoList?.Clear();
                MatStorageMoveInfoList?.Clear();
                MaterialInStorageInfoList?.Clear();
            }));
        }
        private void InitPalletInInfoPageData()
        {
            getMaterialInStorageInfoPageData(1, 1, 2);
            getPageDataMatWareHouseLocation(1, 200, 2);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MaterialBatchInfoList?.Clear();
                MaterialOutStorageInfoList?.Clear();
                MatStorageMoveInfoList?.Clear();
            }));
        }
        private void InitStorageMovePageData()
        {
            getStorageMovePageData(1, 1);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MaterialBatchInfoList?.Clear();
                MaterialOutStorageInfoList?.Clear();
                MaterialInStorageInfoList?.Clear();
                MatWareHouseLocationInfoList?.Clear();
            }));
        }
        /// <summary>
        /// 初始化原料入库单、成品出库单、自动分配原料入库库位和成品出库库位
        /// </summary>
        private void InitMaterialData()
        {
            if (ActionProcess == 0)
            {
                getMaterialInStorageInfoPageData(1, 1, 1);
                getPageDataMatWareHouseLocation(1, 200, 1);
                getMaterialOutStorageInfoPageData(1, 1);
                if (MaterialOutStorageInfoList.Count > 0)
                {
                    GetPageDataMaterialBatch(1, 100);
                }
                getStorageMovePageData(1, 1);
            }
            if (ActionProcess == 1)
            {
                InitMaterialInfoPageData();
            }
            if (ActionProcess == 4)
            {
                InitProductOutInfoPageData();
            }
            if (ActionProcess == 8)
            {
                InitPalletInInfoPageData();
            }
            if (ActionProcess == 6)
            {
                InitStorageMovePageData();
            }
        }
        /// <summary>
        /// 初始化数据采集点
        /// </summary>
        private void InitClientData()
        {
            var result = getClientDataEntities(1, 100, StepActionCode);
            if (!Equals(result, null))
            {
                ClientDataEntities = new ObservableCollection<ClientDataEntity>(result);
                this.client.Send(GetMessage(JsonHelper.ToJson(ClientDataEntities)));
            }
        }
        #region 分页数据路段查询


        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();


        #endregion




        #region 分页查询工序工序


        private ObservableCollection<ClientDataEntity> clientDataEntities = new ObservableCollection<ClientDataEntity>();

        public ObservableCollection<ClientDataEntity> ClientDataEntities
        {
            get { return clientDataEntities; }
            set { Set(ref clientDataEntities, value); }
        }

        /// <summary>
        /// 获取工序分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private List<ClientDataEntity> getClientDataEntities(int pageIndex, int pageSize, string StepActionCode)
        {
#if DEBUG
            getProcessData(1, 200, StepActionCode);
            if (ProcessInfoList.Count > 0)
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
#endif
                pageRepuestParams = new PageRepuestParams();
                FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
                foreach (var item in ProcessInfoList)
                {
                    Guid? processid = item.ProductionProcessInfo_ID;
                    FilterRule filterRule = new FilterRule("ProductionProcessInfo_Id", processid, FilterOperate.Equal);
                    filterGroup.Rules.Add(filterRule);
                }
                pageRepuestParams.FilterGroup = filterGroup;
                pageRepuestParams.SortField = GlobalData.SortField;
                pageRepuestParams.SortOrder = GlobalData.SortOrder;
                pageRepuestParams.PageIndex = pageIndex;
                pageRepuestParams.PageSize = pageSize;
                var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionProcessEquipmentBusinessNodeMapModel>>>(GlobalData.ServerRootUri + "ProductionProcessEquipmentBusinessNodeMap/GridData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
                stopwatch.Stop();
                Utility.LogHelper.Info("获取分步工序数据列表用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
                Utility.LogHelper.Info("分步工序数据列表内容：" + Utility.JsonHelper.ToJson(result));
#endif

                if (!Equals(result, null) && result.Successed)
                {
                    List<ClientDataEntity> results = new List<ClientDataEntity>(result.Data.Data.Count());
                    Application.Current.Resources["UiMessage"] = result?.Message;
                    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());

                    if (result.Data.Data.Any())
                    {
                        foreach (var data in result.Data.Data)
                        {
                            ClientDataEntity ClientDataEntities = new ClientDataEntity();
                            if (data.NodeUrl.Contains("Storage_ActionProcess"))
                            {
                                ClientDataEntities.FunctionCode = FuncCode.Read;
                            }
                            else
                            {
                                ClientDataEntities.FunctionCode = FuncCode.SubScription;
                            }
                            ClientDataEntities.ProductionProcessEquipmentBusinessNodeMapId = data.Id;
                            ClientDataEntities.StatusCode = 0;
                            ClientDataEntities.NodeId = data.NodeUrl;
                            ClientDataEntities.BusinessName = data.BusinessName;
                            ClientDataEntities.Description = data.Description;
                            ClientDataEntities.ValueType = Type.GetType("System." + data.DataType);
                            results.Add(ClientDataEntities);
                        }
                        if (StepActionCode == "StepAction_MaterialInStorage")
                        {
                            Application.Current.Resources["UiMessage"] = "准备执行原料入库！";
                        }
                        if (StepActionCode == "StepAction_PalletInStorage")
                        {
                            Application.Current.Resources["UiMessage"] = "正在执行:智能仓储分步-空托盘入库";
                        }
                        if (StepActionCode == "StepAction_MoveStorage")
                        {
                            Application.Current.Resources["UiMessage"] = "正在执行:智能仓储分步-移库";
                        }
                        if (StepActionCode == "StepAction_ProductOutStorage")
                        {
                            Application.Current.Resources["UiMessage"] = "正在执行:智能仓储分步-成品出库";
                        }
                        return results;
                    }
                    else
                    {
                        Application.Current.Resources["UiMessage"] = "未找到数据";
                        return null;

                    }
                }
                else
                {
                    //操作失败，显示错误信息
                    Application.Current.Resources["UiMessage"] = result?.Message ?? "查询数据点列表失败，请联系管理员！";
                    return null;

                }

            }
            else
            {
                Application.Current.Resources["UiMessage"] = "工序配置数据为空！";
                return null;
            }
        }
        #region DisStepActionProcessMap模型,用于列表数据显示
        private ObservableCollection<DisStepActionProcessMapModel> processInfoList = new ObservableCollection<DisStepActionProcessMapModel>();

        /// <summary>
        /// Agv信息数据
        /// </summary>
        public ObservableCollection<DisStepActionProcessMapModel> ProcessInfoList
        {
            get { return processInfoList; }
            set { Set(ref processInfoList, value); }
        }
        #endregion
        private void getProcessData(int pageIndex, int pageSize, string StepActionCode)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams = new PageRepuestParams();
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRule1 = new FilterRule("DisStepActionInfo.StepActionCode", StepActionCode, FilterOperate.Equal);
            filterGroup.Rules.Add(filterRule1);
            pageRepuestParams.FilterGroup = filterGroup;
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<DisStepActionProcessMapModel>>>(GlobalData.ServerRootUri + "DisStepActionProcessMapInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取分步工序列表用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("分步工序列表内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    ProcessInfoList = new ObservableCollection<DisStepActionProcessMapModel>(result.Data.Data);
                }
                else
                {
                    ProcessInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                ProcessInfoList = new ObservableCollection<DisStepActionProcessMapModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询分步工序列表失败，请联系管理员！";
            }
        }
        #endregion

        private double ratioX;
        private double initRatioX()
        {
            var maxX = 10;
            var minX = 2;

            return (mainContent.ActualWidth - mainContent.ActualWidth * 0.1) / (maxX - minX);
        }

        private double ratioY;
        private double minX;
        private double minY;
        private double offsetX;
        private double offsetY;


        private double initRatioY()
        {
            var maxY = 10;
            var minY = 3;

            return (mainContent.ActualHeight - mainContent.ActualHeight * 0.1) / (maxY - minY);

        }

        #region 地图总宽
        public double mapWidth = 0;

        /// <summary>
        /// 地图总宽
        /// </summary>
        public double MapWidth
        {
            get { return mapWidth; }
            set
            {
                Set(ref mapWidth, value);
            }
        }
        #endregion

        #region 地图总高

        /// <summary>
        /// 地图总高
        /// </summary>
        public double mapHeight = 0;
        public double MapHeight
        {
            get { return mapHeight; }
            set
            {
                Set(ref mapHeight, value);
            }
        }
        #endregion

        public Canvas mainContent = new Canvas() { Background = Utility.Windows.ResourceHelper.FindResource("WorkshopBackBrush1") as Brush };
        public Canvas MainContent
        {
            get { return mainContent; }
            set { Set(ref mainContent, value); }
        }


        ~StorageDispatching3DAnimationViewModel()
        {
        }


        #region 命令定义和初始化

        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand ConnectCommand { get; set; }

        public ICommand DisconnectCommand { get; set; }

        public ICommand RefreshAllCommand { get; set; }

        public ICommand MaterialInCommand { get; set; }
        public ICommand MaterialInConfirmCommand { get; set; }
        public ICommand EmptyPalletInCommand { get; set; }
        public ICommand StorageMoveCommand { get; set; }
        public ICommand ProductOutCommand { get; set; }
        public ICommand InitCommand { get; set; }

        private void initCommand()
        {
            ConnectCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConnectCommand, OnCanExecuteConnectCommand);
            DisconnectCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteDisconnectCommand, OnCanExecuteDisconnectCommand);
            InitCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteInitCommand, OnCanExecuteInitCommand);
            RefreshAllCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshAllCommand, OnCanExecuteRefreshAllCommand);
            MaterialInCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteMaterialInCommand, OnCanExecuteMaterialInCommand);
            MaterialInConfirmCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteMaterialInConfirmCommand, OnCanExecuteMaterialInConfirmCommand);
            EmptyPalletInCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteEmptyPalletInCommand, OnCanExecuteEmptyPalletInCommand);
            StorageMoveCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteStorageMoveCommand, OnCanExecuteStorageMoveCommand);
            ProductOutCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteProductOutCommand, OnCanExecuteProductOutCommand);
        }

        #endregion

        #region  MVVMLight消息注册和取消
        /// <summary>
        /// 注册MVVMLight消息
        /// </summary>
        private void registerMessenger()
        {
            //Messenger.Default.Register<Fork>(this, MessengerToken.DataChanged, dataChanged);
        }


        /// <summary>
        /// 取消注册MVVMlight消息
        /// </summary>
        private void unRegisterMessenger()
        {
            //Messenger.Default.Unregister<ViewInfo>(this, Model.MessengerToken.Navigate, Navigate);

            //Messenger.Default.Unregister<object>(this, Model.MessengerToken.ClosePopup, ClosePopup);

            //Messenger.Default.Unregister<MenuInfo>(this, Model.MessengerToken.SetMenuStatus, SetMenuStatus);
        }
        #endregion



        #region 命令和消息等执行函数
        int a = 0;
        /// <summary>
        /// 执行新建命令
        /// </summary>
        private void OnExecuteConnectCommand()
        {
            SocketConnected();
            OnExecuteRefreshAllCommand();
        }

        /// <summary>
        /// 是否可以执行新增命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConnectCommand()
        {
            return !Equals(this.client, null) && !connectStatus;
        }

        private void WriteData(string NodeId, int value)
        {
            if (ClientDataEntities.Count > 0)
            {
                var clientDataEntities = ClientDataEntities.Where(a => a.NodeId.Contains(NodeId)).FirstOrDefault();
                if (!Equals(null, clientDataEntities))
                {
                    List<ClientDataEntity> clientDataEntitiesSend = new List<ClientDataEntity>(1);
                    clientDataEntities.FunctionCode = FuncCode.Write;
                    clientDataEntities.Value = (UInt16)value;
                    clientDataEntitiesSend.Add(clientDataEntities);
                    this.client.Send(GetMessage(JsonHelper.ToJson(clientDataEntitiesSend)));
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
        private void WriteBoolData(string NodeId, Boolean value)
        {
            if (ClientDataEntities.Count > 0)
            {
                var clientDataEntities = ClientDataEntities.Where(a => a.NodeId.Contains(NodeId)).FirstOrDefault();
                if (!Equals(null, clientDataEntities))
                {
                    List<ClientDataEntity> clientDataEntitiesSend = new List<ClientDataEntity>(1);
                    clientDataEntities.FunctionCode = FuncCode.Write;
                    clientDataEntities.Value = (Boolean)value;
                    clientDataEntitiesSend.Add(clientDataEntities);
                    this.client.Send(GetMessage(JsonHelper.ToJson(clientDataEntitiesSend)));
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        private int ActionProcess = 0;
        private bool isMaterialInConfirmTip = false;
        private bool isMaterialInFinishTip = false;
        private bool isProductOutFinishTip = false;
        private bool isStorageMoveFinishTip = false;
        private bool isEmptyPalletInFinishTip = false;
        /// <summary>
        /// 执行原料入库命令
        /// </summary>
        private void OnExecuteMaterialInCommand()
        {
            InitMaterialInfoPageData();
            int count1 = MaterialInStorageInfoList.Count;
            int count2 = MatWareHouseLocationInfoList.Count;
            if (count1 == 0)
            {
                Application.Current.Resources["UiMessage"] = "请先填写原料入库单！";
                return;
            }
            if (count2 == 0)
            {
                Application.Current.Resources["UiMessage"] = "仓库中没有空托盘的原料库位！";
                return;
            }
            if (count1 > 0 && count2 > 0)
            {
                ActionProcess = 1;
                WriteData("Storage_ActionProcess", ActionProcess);
                MaterialInStorageInfo = MaterialInStorageInfoList.FirstOrDefault();
                StepActionCode = "StepAction_MaterialInStorage";
                InitClientData();
                Application.Current.Resources["UiMessage"] = "正在执行:智能仓储分步-原料入库";
            }
            else
            {
                ActionProcess = 0;
                WriteData("Storage_ActionProcess", ActionProcess);
                Application.Current.Resources["UiMessage"] = "请先填写原料入库单或没有空托盘库位！";
                return;
            }
        }
        /// <summary>
        /// 是否可以执行原料入库命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteMaterialInCommand()
        {
            return ActionProcess == 0 && connectStatus;
        }
        /// <summary>
        /// 执行原料入库确认指令
        /// </summary>
        private void OnExecuteMaterialInConfirmCommand()
        {
            ActionProcess = 2;
            WriteData("Storage_ActionProcess", ActionProcess);
        }
        /// <summary>
        /// 是否可以执行原料入库确认指令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteMaterialInConfirmCommand()
        {
            return ActionProcess == 1 && connectStatus;
        }
        /// <summary>
        /// 执行空托盘入库命令
        /// </summary>
        private void OnExecuteEmptyPalletInCommand()
        {
            InitPalletInInfoPageData();
            int count1 = MaterialInStorageInfoList.Count;
            int count2 = MatWareHouseLocationInfoList.Count;
            if (count1 == 0)
            {
                Application.Current.Resources["UiMessage"] = "请先填写空托盘入库单！";
                return;
            }
            if (count2 == 0)
            {
                Application.Current.Resources["UiMessage"] = "仓库中没有空库位的原料库位！";
                return;
            }
            if (count1 > 0 && count2 > 0)
            {
                ActionProcess = 8;
                WriteData("Storage_ActionProcess", ActionProcess);
                MaterialInStorageInfo = MaterialInStorageInfoList.FirstOrDefault();
                StepActionCode = "StepAction_PalletInStorage";
                InitClientData();
                Application.Current.Resources["UiMessage"] = "正在执行:智能仓储分步-空托盘入库";
            }
            else
            {
                ActionProcess = 0;
                WriteData("Storage_ActionProcess", ActionProcess);
                Application.Current.Resources["UiMessage"] = "请先填写空托盘入库单或没有空库位！";
                return;
            }
        }
        /// <summary>
        /// 是否可以执行空托盘入库
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteEmptyPalletInCommand()
        {
            return (ActionProcess == 0 && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行移库命令
        /// </summary>
        private void OnExecuteStorageMoveCommand()
        {
            InitStorageMovePageData();
            int count1 = MatStorageMoveInfoList.Count;
            if (count1 == 0)
            {
                Application.Current.Resources["UiMessage"] = "请先填写移库单！";
                return;
            }
            if (count1 > 0)
            {
                ActionProcess = 6;
                WriteData("Storage_ActionProcess", ActionProcess);
                MatStorageMoveInfo = MatStorageMoveInfoList.FirstOrDefault();
                StepActionCode = "StepAction_MoveStorage";
                InitClientData();
                Application.Current.Resources["UiMessage"] = "正在执行:智能仓储分步-移库";
            }
            else
            {
                ActionProcess = 0;
                WriteData("Storage_ActionProcess", ActionProcess);
                Application.Current.Resources["UiMessage"] = "请先填写移库单！";
                return;
            }
        }
        /// <summary>
        /// 是否可以执行移库命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteStorageMoveCommand()
        {
            return (ActionProcess == 0 && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行成品出库命令
        /// </summary>
        private void OnExecuteProductOutCommand()
        {
            InitProductOutInfoPageData();
            int count1 = MaterialOutStorageInfoList.Count;
            int count2 = MaterialBatchInfoList.Count;
            if (count1 == 0)
            {
                Application.Current.Resources["UiMessage"] = "请先填写成品出库单！";
                return;
            }
            if (count2 == 0)
            {
                Application.Current.Resources["UiMessage"] = "仓库中成品库存不足！";
                return;
            }
            if (count1 > 0 && count2 > 0)
            {
                ActionProcess = 4;
                WriteData("Storage_ActionProcess", ActionProcess);
                MaterialInStorageInfo = MaterialInStorageInfoList.FirstOrDefault();
                StepActionCode = "StepAction_ProductOutStorage";
                InitClientData();
                Application.Current.Resources["UiMessage"] = "正在执行:智能仓储分步-成品出库";
            }
            else
            {
                ActionProcess = 0;
                WriteData("Storage_ActionProcess", ActionProcess);
                Application.Current.Resources["UiMessage"] = "请先填写空成品出库单或成品库存不足！";
                return;
            }
        }
        /// <summary>
        /// 是否可以执行成品出库命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteProductOutCommand()
        {
            return (ActionProcess == 0 && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行断开连接
        /// </summary>
        private void OnExecuteDisconnectCommand()
        {
            client.CloseAsync();
            connectStatus = false;
            client.Closed += OnClose;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CommandManager.InvalidateRequerySuggested();

            }));
        }
        /// <summary>
        /// 是否可用执行断开连接
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteDisconnectCommand()
        {
            return !Equals(this.client, null) && connectStatus;
        }
        /// <summary>
        /// 执行初始化指令
        /// </summary>
        private void OnExecuteInitCommand()
        {
            ActionProcess = 0;
            WriteData("Storage_ActionProcess", ActionProcess);
            InitParaMeters();
            OnExecuteRefreshAllCommand();
        }
        /// <summary>
        /// 是否可用执行初始化指令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteInitCommand()
        {
            return connectStatus;
        }
        private void OnExecuteSizeChangedCommand()
        {
            minX = 2;
            minY = 3;
            ratioX = initRatioX();
            ratioY = initRatioY();
            offsetX = ratioX * minX - mainContent.ActualWidth * 0.1 / 2;
            offsetY = ratioY * minY - mainContent.ActualHeight * 0.1 / 2;
        }

        private void OnExecuteRefreshAllCommand()
        {
            Task.Factory.StartNew(new Action(init));
            System.Threading.Thread.Sleep(1000);
            OnExecuteSizeChangedCommand();
        }

        private bool OnCanExecuteRefreshAllCommand()
        {
            return connectStatus;
        }
        #endregion

        #region Tcp客户端读取OPC数据

        private bool connectStatus = false;
        private SocketClientHelper client = new SocketClientHelper(Utility.ConfigHelper.GetAppSetting("ServerIp"), Convert.ToInt32(Utility.ConfigHelper.GetAppSetting("ServerPort")));

        private void SocketConnected()
        {
            if (connectStatus)
            {
                return;
            }

            connectStatus = client.ConnectAsync().Result;
            client.Connected += OnConnect;
            client.Closed += OnClose;
            client.OnDataReceived += OnReceive;
            client.Error += OnError;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CommandManager.InvalidateRequerySuggested();

            }));
        }


        private String GetMessage(String msgBody)
        {
            return "[STX]" + msgBody + "[ETX]";
        }

        private void OnError(object sender, Exception e)
        {
            Application.Current.Resources["UiMessage"] = e.ToString();
        }

        private void OnConnect(object sender, EventArgs args)
        {
            Application.Current.Resources["UiMessage"] = $"通讯连接结果：{connectStatus}";
        }

        private void OnClose(object sender, EventArgs args)
        {
            Application.Current.Resources["UiMessage"] = "连接已断开！";
        }

        private void OnReceive(object sender, DataEventArgs args)
        {
            var cde = JsonHelper.FromJson<List<ClientDataEntity>>(args.PackageInfo.Data);
            connectStatus = true;
            LogHelper.Info(cde.FirstOrDefault()?.NodeId + "      " + cde.FirstOrDefault()?.Value?.ToString());

            foreach (var v in cde)
            {
                var tmp = ClientDataEntities.FirstOrDefault(a => a.NodeId == v.NodeId);
                if (!Equals(tmp, null))
                {
                    if (!Equals(v.Value, null))
                    {
                        tmp.OldVaule = v.OldVaule?.ToString();
                        tmp.Value = v.Value?.ToString();
                    }

                    tmp.StatusCode = v.StatusCode;
                    LogHelper.Info(tmp.NodeId + " " + tmp.Value?.ToString());
                }
            }
            SetAgvStackerPosition();
            ReadActionProcessData();
        }

        private void ReadActionProcessData()
        {

            if (ClientDataEntities.Count > 0)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("Storage_ActionProcess")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        ActionProcess = int.Parse(v1.Value.ToString());
                        switch (ActionProcess)
                        {
                            case 2:
                                if (!isMaterialInConfirmTip)
                                {
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        CommandManager.InvalidateRequerySuggested();

                                    }));
                                    Application.Current.Resources["UiMessage"] = "原料入库人工已确认！";
                                    isMaterialInConfirmTip = true;
                                }
                                break;
                            case 3:
                                if (!isMaterialInFinishTip)
                                {
                                    ActionProcess = 0;
                                    WriteData("Storage_ActionProcess", ActionProcess);
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        CommandManager.InvalidateRequerySuggested();

                                    }));
                                    Application.Current.Resources["UiMessage"] = "智能仓储分步-原料入库完成";
                                    isMaterialInFinishTip = true;
                                    InitParaMeters();
                                    InitMaterialData();
                                }
                                break;
                            case 5:
                                if (!isProductOutFinishTip)
                                {
                                    ActionProcess = 0;
                                    WriteData("Storage_ActionProcess", ActionProcess);
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        CommandManager.InvalidateRequerySuggested();

                                    }));
                                    Application.Current.Resources["UiMessage"] = "智能仓储分步-成品出库完成";
                                    isProductOutFinishTip = true;
                                    InitParaMeters();
                                    InitMaterialData();
                                }
                                break;
                            case 7:
                                if (!isStorageMoveFinishTip)
                                {
                                    ActionProcess = 0;
                                    WriteData("Storage_ActionProcess", ActionProcess);
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        CommandManager.InvalidateRequerySuggested();

                                    }));
                                    Application.Current.Resources["UiMessage"] = "智能仓储分步-移库完成";
                                    isStorageMoveFinishTip = true;
                                    InitParaMeters();
                                    InitMaterialData();
                                }
                                break;
                            case 9:
                                if (!isEmptyPalletInFinishTip)
                                {
                                    ActionProcess = 0;
                                    WriteData("Storage_ActionProcess", ActionProcess);
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        CommandManager.InvalidateRequerySuggested();

                                    }));
                                    Application.Current.Resources["UiMessage"] = "智能仓储分步-空托盘入库完成";
                                    isEmptyPalletInFinishTip = true;
                                    InitParaMeters();
                                    InitMaterialData();
                                }
                                break;
                            default: break;
                        }
                    }
                }


            }


        }
        #endregion


        #region 物料入库信息模型,用于列表数据显示
        private ObservableCollection<MaterialInStorageInfoModel> materialinstorageModelList = new ObservableCollection<MaterialInStorageInfoModel>();

        /// <summary>
        /// 物料入库信息数据
        /// </summary>
        public ObservableCollection<MaterialInStorageInfoModel> MaterialInStorageInfoList
        {
            get { return materialinstorageModelList; }
            set { Set(ref materialinstorageModelList, value); }
        }

        private MaterialInStorageInfoModel materialinstorage = new MaterialInStorageInfoModel();

        /// <summary>
        /// 物料入库信息数据
        /// </summary>
        public MaterialInStorageInfoModel MaterialInStorageInfo
        {
            get { return materialinstorage; }
            set { Set(ref materialinstorage, value); }
        }
        #endregion
        #region 原料入库列表
        /// <summary>
        /// 原料入库列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getMaterialInStorageInfoPageData(int pageIndex, int pageSize, int type)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortField = "CreatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            FilterGroup filterGroup = new FilterGroup(FilterOperate.And);
            FilterRule filterRule1 = new FilterRule();
            if (type == 1)
            {
                filterRule1 = new FilterRule("InStorageType", 1, FilterOperate.Equal);
            }
            if (type == 2)
            {
                filterRule1 = new FilterRule("InStorageType", 4, FilterOperate.Equal);
            }
            FilterRule filterRule2 = new FilterRule("InStorageStatus", 1, FilterOperate.Equal);
            FilterRule filterRule3 = new FilterRule("PalletQuantity", 1, FilterOperate.Equal);
            filterGroup.Rules.Add(filterRule1);
            filterGroup.Rules.Add(filterRule2);
            if (type == 1)
            {
                filterGroup.Rules.Add(filterRule3);
            }
            //
            pageRepuestParams.FilterGroup = filterGroup;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialInStorageInfoModel>>>
                (GlobalData.ServerRootUri + "MaterialInStorageInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取原料入库信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("原料入库信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    MaterialInStorageInfoList = new ObservableCollection<MaterialInStorageInfoModel>(result.Data.Data);
                    //  TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialInStorageInfoList = new ObservableCollection<MaterialInStorageInfoModel>();
                    //  TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialInStorageInfoList = new ObservableCollection<MaterialInStorageInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询原料入库信息失败，请联系管理员！";
            }
        }
        #endregion

        #region 物料出库信息模型,用于列表数据显示
        private ObservableCollection<MaterialOutStorageInfoModel> materialoutstorageModelList = new ObservableCollection<MaterialOutStorageInfoModel>();

        /// <summary>
        /// 物料出库信息数据
        /// </summary>
        public ObservableCollection<MaterialOutStorageInfoModel> MaterialOutStorageInfoList
        {
            get { return materialoutstorageModelList; }
            set { Set(ref materialoutstorageModelList, value); }
        }

        private MaterialOutStorageInfoModel materialoutstorage = new MaterialOutStorageInfoModel();

        /// <summary>
        /// 物料出库信息数据
        /// </summary>
        public MaterialOutStorageInfoModel MaterialOutStorageInfo
        {
            get { return materialoutstorage; }
            set { Set(ref materialoutstorage, value); }
        }
        #endregion
        #region 成品出库列表
        /// <summary>
        /// 成品出库列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getMaterialOutStorageInfoPageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortField = "CreatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            FilterGroup filterGroup = new FilterGroup(FilterOperate.And);
            FilterRule filterRule1 = new FilterRule("OutStorageType", 2, FilterOperate.Equal);
            FilterRule filterRule2 = new FilterRule("OutStorageStatus", 1, FilterOperate.Equal);
            FilterRule filterRule3 = new FilterRule("PalletQuantity", 1, FilterOperate.Equal);
            filterGroup.Rules.Add(filterRule1);
            filterGroup.Rules.Add(filterRule2);
            filterGroup.Rules.Add(filterRule3);
            //
            pageRepuestParams.FilterGroup = filterGroup;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialOutStorageInfoModel>>>
                (GlobalData.ServerRootUri + "MaterialOutStorageInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取成品出库信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("成品出库信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    MaterialOutStorageInfoList = new ObservableCollection<MaterialOutStorageInfoModel>(result.Data.Data);
                    //  TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialOutStorageInfoList = new ObservableCollection<MaterialOutStorageInfoModel>();
                    //  TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialOutStorageInfoList = new ObservableCollection<MaterialOutStorageInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询物料出库信息失败，请联系管理员！";
            }
        }
        #endregion

        #region 库位批次信息模型,用于列表数据显示
        private ObservableCollection<MaterialBatchInfoModel> materialbatchinfoList = new ObservableCollection<MaterialBatchInfoModel>();

        /// <summary>
        /// 库位信息信息数据
        /// </summary>
        public ObservableCollection<MaterialBatchInfoModel> MaterialBatchInfoList
        {
            get { return materialbatchinfoList; }
            set { Set(ref materialbatchinfoList, value); }
        }
        #endregion
        #region 获取库位物料批次信息
        private void GetPageDataMaterialBatch(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            //pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            pageRepuestParams.SortField = "CreatedTime";
            FilterGroup filterGroup = new FilterGroup(FilterOperate.And);
            MaterialOutStorageInfoModel outinfo = MaterialOutStorageInfoList.FirstOrDefault();
            FilterRule frMaterialCode = new FilterRule("Material.MaterialCode", outinfo.MaterialCode, FilterOperate.Equal);
            FilterRule frtype = new FilterRule("MatWareHouseLocation.WareHouseLocationType", 2, FilterOperate.Equal);
            FilterRule frquantity = new FilterRule("Quantity", outinfo.Quantity, FilterOperate.Equal);
            filterGroup.Rules.Add(frMaterialCode);
            filterGroup.Rules.Add(frtype);
            filterGroup.Rules.Add(frquantity);
            pageRepuestParams.FilterGroup = filterGroup;
            //
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialBatchInfoModel>>>(GlobalData.ServerRootUri + "MaterialBatchInfo/PageDataMaterialBatchInfo1", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取库位批次信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("库位批次信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MaterialBatchInfoList = new ObservableCollection<MaterialBatchInfoModel>(result.Data.Data.Where(x => x.Pallet_Id != Guid.Empty && x.Pallet_Id != null).Take(1).ToList());
                    }));
                    int TotalCounts = result.Data.Total;
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MaterialBatchInfoList?.Clear();
                    }));
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MaterialBatchInfoList = new ObservableCollection<MaterialBatchInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询库位批次信息失败，请联系管理员！";
            }
        }

        #endregion
        #region 移库模型
        /// <summary>
        /// 移库信息模型
        /// </summary>
        private MatStorageMoveInfoModel matwarehousetypeModel;
        /// <summary>
        /// 移库信息模型
        /// </summary>
        public MatStorageMoveInfoModel MatStorageMoveInfo
        {
            get { return matwarehousetypeModel; }
            set { Set(ref matwarehousetypeModel, value); }
        }
        #endregion

        #region 移库信息模型,用于列表数据显示
        private ObservableCollection<MatStorageMoveInfoModel> matwarehousetypeModelList = new ObservableCollection<MatStorageMoveInfoModel>();

        /// <summary>
        /// 移库信息数据
        /// </summary>
        public ObservableCollection<MatStorageMoveInfoModel> MatStorageMoveInfoList
        {
            get { return matwarehousetypeModelList; }
            set { Set(ref matwarehousetypeModelList, value); }
        }
        #endregion
        #region 获取移库单列表
        /// <summary>
        /// 获取移库单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getStorageMovePageData(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams = new PageRepuestParams();
            pageRepuestParams.SortField = "LastUpdatedTime";
            pageRepuestParams.SortOrder = "desc";

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            FilterRule filterRule1 = new FilterRule("StorageMoveState", 1, FilterOperate.Equal);
            filterGroup.Rules.Add(filterRule1);
            pageRepuestParams.FilterGroup = filterGroup;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatStorageMoveInfoModel>>>
                (GlobalData.ServerRootUri + "MatStorageMoveInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取移库信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("移库信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    MatStorageMoveInfoList = new ObservableCollection<MatStorageMoveInfoModel>(result.Data.Data);
                    int TotalCounts = result.Data.Total;
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MatStorageMoveInfoList?.Clear();
                    }));
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatStorageMoveInfoList = new ObservableCollection<MatStorageMoveInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询移库信息失败，请联系管理员！";
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
        protected override void Disposing()
        {
            base.Disposing();
            unRegisterMessenger();

        }

    }
}

