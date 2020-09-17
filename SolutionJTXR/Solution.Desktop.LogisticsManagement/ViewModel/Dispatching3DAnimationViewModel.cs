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
using Solution.Desktop.LogisticsManagement.Model;

namespace Solution.Desktop.LogisticsManagement.ViewModel
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
    public class Dispatching3DAnimationViewModel : VmBase
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
            isClickToWorkIsland1Command = false;
            isClickIsland1OnCommand = false;
            isClickIsland1WorkingCommand = false;
            isClickIsland1OffCommand = false;
            isClickToWorkIsland2Command = false;
            isClickIsland2OnCommand = false;
            isClickIsland2WorkingCommand = false;
            isClickIsland2OffCommand = false;
            isClickProductInCommand = false;
            isClickInitCommand = false;
            StepActionCode = "StepAction_MaterialOutStorageShow";
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                //MaterialOutStorageInfoList?.Clear();
                MaterialBatchInfoList?.Clear();
                MatWareHouseLocationInfoList?.Clear();
            }));
        }
        private void getPageDataMatWareHouseLocation(int pageIndex, int pageSize)
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
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MatWareHouseLocationInfoModel>>>(GlobalData.ServerRootUri + "MatWareHouseLocationInfo/PageDataProductEmptyLocation", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取成品空库位信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("成品空库位信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    MatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>(result.Data.Data);
                    //  TotalCounts = result.Data.Total;
                }
                else
                {
                    MatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();
                    //  TotalCounts = 0;
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                MatWareHouseLocationInfoList = new ObservableCollection<MatWareHouseLocationInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询成品空库位信息失败，请联系管理员！";
            }
        }

        System.Threading.Timer timer;
        public Dispatching3DAnimationViewModel()
        {
            initCommand();
        }
        #region 全程
        private bool isAllAction = false;
        private bool isClickToWorkIsland1Command = false;
        private bool isClickIsland1OnCommand = false;
        private bool isClickIsland1WorkingCommand = false;
        private bool isClickIsland1OffCommand = false;
        private bool isClickToWorkIsland2Command = false;
        private bool isClickIsland2OnCommand = false;
        private bool isClickIsland2WorkingCommand = false;
        private bool isClickIsland2OffCommand = false;
        private bool isClickProductInCommand = false;
        private bool isClickInitCommand = false;
        #endregion
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
        private string StepActionCode = "StepAction_MaterialOutStorageShow";
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void init()
        {
            InitMaterialData();
            InitClientData();
        }
        /// <summary>
        /// 初始化原料自动出库单、自动分配原料库位和成品库位
        /// </summary>
        private void InitMaterialData()
        {
            if (ActionProcess == 0)
            {
                getMaterialOutStorageInfoPageData(1, 1);
                if (MaterialOutStorageInfoList.Count > 0)
                {
                    GetPageDataMaterialBatch(1, 100);
                }
                if (MaterialOutStorageInfoList.Count == 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MaterialBatchInfoList?.Clear();
                    }));
                }
                getPageDataMatWareHouseLocation(1, 1);
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
                            if (data.NodeUrl.Contains("StepAction_IsWholeCourse") || data.NodeUrl.Contains("StepAction_ActionProcess"))
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
                        if (StepActionCode == "StepAction_MaterialOutStorageShow")
                        {
                            Application.Current.Resources["UiMessage"] = "准备执行原料出库！";
                        }
                        if (StepActionCode == "StepAction_ToWorkIsland1")
                        {

                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "转运至加工岛1";
                        }
                        if (StepActionCode == "StepAction_Island1On")
                        {

                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛1上料";
                        }
                        if (StepActionCode == "StepAction_Island1Working")
                        {

                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛1生产";
                        }
                        if (StepActionCode == "StepAction_Island1Off")
                        {
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛1下料";
                        }
                        if (StepActionCode == "StepAction_ToWorkIsland2")
                        {
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "转运至加工岛2";
                        }
                        if (StepActionCode == "StepAction_Island2On")
                        {
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛2上料";
                        }
                        if (StepActionCode == "StepAction_Island2Working")
                        {
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛2生产";
                        }
                        if (StepActionCode == "StepAction_Island2Off")
                        {
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛2下料";
                        }
                        if (StepActionCode == "StepAction_ProductInStorage")
                        {
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "成品回库";
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


        ~Dispatching3DAnimationViewModel()
        {
        }


        #region 命令定义和初始化

        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand ConnectCommand { get; set; }

        public ICommand DisconnectCommand { get; set; }

        public ICommand RefreshAllCommand { get; set; }

        public ICommand MaterialOutCommand { get; set; }
        public ICommand ToWorkIsland1Command { get; set; }
        public ICommand Island1OnCommand { get; set; }
        public ICommand Island1WorkingCommand { get; set; }
        public ICommand Island1OffCommand { get; set; }
        public ICommand ToWorkIsland2Command { get; set; }
        public ICommand Island2OnCommand { get; set; }
        public ICommand Island2WorkingCommand { get; set; }
        public ICommand Island2OffCommand { get; set; }
        public ICommand ProductInCommand { get; set; }

        public ICommand AllActionCommand { get; set; }
        public ICommand InitCommand { get; set; }

        private void initCommand()
        {
            ConnectCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConnectCommand, OnCanExecuteConnectCommand);
            DisconnectCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteDisconnectCommand, OnCanExecuteDisconnectCommand);
            InitCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteInitCommand, OnCanExecuteInitCommand);
            //SizeChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSizeChangedCommand);
            RefreshAllCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshAllCommand, OnCanExecuteRefreshAllCommand);
            MaterialOutCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteMaterialOutCommand, OnCanExecuteMaterialOutCommand);
            ToWorkIsland1Command = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteToWorkIsland1Command, OnCanExecuteToWorkIsland1Command);
            Island1OnCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteIsland1OnCommand, OnCanExecuteIsland1OnCommand);
            Island1WorkingCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteIsland1WorkingCommand, OnCanExecuteIsland1WorkingCommand);
            Island1OffCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteIsland1OffCommand, OnCanExecuteIsland1OffCommand);
            ToWorkIsland2Command = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteToWorkIsland2Command, OnCanExecuteToWorkIsland2Command);
            Island2OnCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteIsland2OnCommand, OnCanExecuteIsland2OnCommand);
            Island2WorkingCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteIsland2WorkingCommand, OnCanExecuteIsland2WorkingCommand);
            Island2OffCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteIsland2OffCommand, OnCanExecuteIsland2OffCommand);
            ProductInCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteProductInCommand, OnCanExecuteProductInCommand);
            AllActionCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteAllActionCommand, OnCanExecuteAllActionCommand);
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

        ///// <summary>
        ///// 模型数据改变
        ///// </summary>
        ///// <param name="obj"></param>
        //private void dataChanged(Fork CommOpcUaBusinessModel)
        //{
        //getPageData(pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
        //var tmpModel = CommOpcUaBusinessInfoList.FirstOrDefault(a => a.Id == CommOpcUaBusinessModel.Id);
        //this.CommOpcUaBusinessInfo = CommOpcUaBusinessInfoList.FirstOrDefault();
        ////新增、不存在的数据插入到第一行便于查看
        //if (Equals(tmpModel, null))
        //{
        //    this.EnterpriseInfoList.Insert(0, enterpriseModel);
        //    //this.EnterpriseInfoList.Insert(0, enterpriseModel);
        //    EnterpriseInfoList.RemoveAt(this.EnterpriseInfoList.Count - 1);
        //}
        //else
        //{
        //    //修改的更新后置于第一行，便于查看
        //    tmpModel = enterpriseModel;
        //    EnterpriseInfoList.Move(EnterpriseInfoList.IndexOf(tmpModel), 0);
        //    tmpModel = enterpriseModel;
        //}
        //}

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

        #region 分页数据查询
        /// <summary>
        /// 分页请求参数
        /// </summary>
        //private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        ///// <summary>
        ///// 分页执行函数
        ///// </summary>
        ///// <param name="e"></param>
        //public override void OnExecutePageChangedCommand(PageChangedEventArgs e)
        //{
        //    Utility.LogHelper.Info(e.PageIndex.ToString() + " " + e.PageSize);
        //    getPageData(e.PageIndex, e.PageSize);
        //}
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
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;


            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////Console.WriteLine(await (await _httpClient.GetAsync("/api/service/EnterpriseInfo/Get?id='1'")).Content.ReadAsStringAsync());
            //Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            //var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<Fork>>>(GlobalData.ServerRootUri + "CommOpcUaBusiness/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取OpcUa业务数据用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            //Utility.LogHelper.Info("OpcUa业务数据内容：" + Utility.JsonHelper.ToJson(result));
#endif

            //if (!Equals(result, null) && result.Successed)
            //{
            //    Application.Current.Resources["UiMessage"] = result?.Message;
            //    LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            //    if (result.Data.Data.Any())
            //    {
            //        //TotalCounts = result.Data.Total;
            //        //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
            //        CommOpcUaBusinessInfoList = new ObservableCollection<Fork>(result.Data.Data);
            //        TotalCounts = result.Data.Total;
            //    }
            //    else
            //    {
            //        CommOpcUaBusinessInfoList?.Clear();
            //        Application.Current.Resources["UiMessage"] = "未找到数据";
            //    }
            //}
            //else
            //{
            //    //操作失败，显示错误信息
            //    CommOpcUaBusinessInfoList = new ObservableCollection<Fork>();
            //    Application.Current.Resources["UiMessage"] = result?.Message ?? "查询OpcUa业务数据失败，请联系管理员！";
            //}
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

            //if (a > 6)
            //{
            //    a = 0;
            //}
            //if (agvCars.Count() < 1)
            //{
            //    return;
            //}
            //a = a + 1;
            //agvCars[0].Run(mainContent, mainContent.FindChild<Path>("路段" + a), 10);
            //System.Diagnostics.Debug.Print(mainContent.ActualHeight.ToString());
            //System.Diagnostics.Debug.Print(mainContent.ActualWidth.ToString());
        }


        private void Test_Matrix(Path path)
        {

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


        /// <summary>
        /// 执行原料出库命令
        /// </summary>
        private void OnExecuteMaterialOutCommand()
        {
            OnExecuteRefreshAllCommand();
            if (MatWareHouseLocationInfoList.Count == 0)
            {
                Application.Current.Resources["UiMessage"] = "仓库中没有空库位的成品库位！";
                return;
            }
            int count1 = MaterialOutStorageInfoList.Count;
            int count2 = MaterialBatchInfoList.Count;
            if (count1 > 0 && count2 > 0)
            {
                MaterialOutStorageInfo = MaterialOutStorageInfoList.FirstOrDefault();
                ActionProcess = 1;
                WriteData("StepAction_ActionProcess", ActionProcess);
                isAllAction = false;
                WriteBoolData("StepAction_IsWholeCourse", false);
                Application.Current.Resources["UiMessage"] = "正在执行:分步-原料出库";
            }
            else
            {
                isAllAction = false;
                WriteBoolData("StepAction_IsWholeCourse", false);
                ActionProcess = 0;
                WriteData("StepAction_ActionProcess", ActionProcess);
                Application.Current.Resources["UiMessage"] = "请先填写原料出库单或原料不足！";
                return;
            }
        }
        /// <summary>
        /// 是否可以执行原料出库命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteMaterialOutCommand()
        {
            return ActionProcess == 0 && (MaterialBatchInfoList.Count > 0 ? true : false) && connectStatus;
        }
        /// <summary>
        /// 执行去加工岛1命令
        /// </summary>
        private void OnExecuteToWorkIsland1Command()
        {
            StepActionCode = "StepAction_ToWorkIsland1";
            ActionProcess = 3;
            WriteData("StepAction_ActionProcess", ActionProcess);
            OnExecuteRefreshAllCommand();


        }
        /// <summary>
        /// 是否可以执行去加工岛1命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteToWorkIsland1Command()
        {
            return (ActionProcess == 2 && !isAllAction && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行加工岛1上料命令
        /// </summary>
        private void OnExecuteIsland1OnCommand()
        {
            StepActionCode = "StepAction_Island1On";
            ActionProcess = 5;
            WriteData("StepAction_ActionProcess", ActionProcess);
            OnExecuteRefreshAllCommand();
        }
        /// <summary>
        /// 是否可以执行加工岛1上料命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland1OnCommand()
        {
            return (ActionProcess == 4 && !isAllAction && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行加工岛1生产命令
        /// </summary>
        private void OnExecuteIsland1WorkingCommand()
        {
            StepActionCode = "StepAction_Island1Working";
            ActionProcess = 7;
            WriteData("StepAction_ActionProcess", ActionProcess);
            OnExecuteRefreshAllCommand();
            // 给Robot1发送TransLineD1_Arrived = 1信号(加工岛1开始加工)
        }
        /// <summary>
        /// 是否可以执行加工岛1生产命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland1WorkingCommand()
        {
            return (ActionProcess == 6 && !isAllAction && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行加工岛1下料命令
        /// </summary>
        private void OnExecuteIsland1OffCommand()
        {

            StepActionCode = "StepAction_Island1Off";
            ActionProcess = 9;
            WriteData("StepAction_ActionProcess", ActionProcess);
            OnExecuteRefreshAllCommand();
        }
        /// <summary>
        /// 是否可以执行加工岛1下料命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland1OffCommand()
        {
            return (ActionProcess == 8 && !isAllAction && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行转运到加工岛2命令
        /// </summary>
        private void OnExecuteToWorkIsland2Command()
        {
            StepActionCode = "StepAction_ToWorkIsland2";
            ActionProcess = 11;
            WriteData("StepAction_ActionProcess", ActionProcess);
            OnExecuteRefreshAllCommand();

        }
        /// <summary>
        /// 是否可以执行转运到加工岛2命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteToWorkIsland2Command()
        {
            return (ActionProcess == 10 && !isAllAction && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行加工岛2上料命令
        /// </summary>
        private void OnExecuteIsland2OnCommand()
        {
            StepActionCode = "StepAction_Island2On";
            ActionProcess = 13;
            WriteData("StepAction_ActionProcess", ActionProcess);
            OnExecuteRefreshAllCommand();
        }
        /// <summary>
        /// 是否可以执行加工岛2上料命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland2OnCommand()
        {
            return (ActionProcess == 12 && !isAllAction && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行加工岛2生产命令
        /// </summary>
        private void OnExecuteIsland2WorkingCommand()
        {
            StepActionCode = "StepAction_Island2Working";
            ActionProcess = 15;
            WriteData("StepAction_ActionProcess", ActionProcess);
            OnExecuteRefreshAllCommand();
        }
        /// <summary>
        /// 是否可以执行加工岛2生产命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland2WorkingCommand()
        {
            return (ActionProcess == 14 && !isAllAction && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行加工岛2下料命令
        /// </summary>
        private void OnExecuteIsland2OffCommand()
        {
            StepActionCode = "StepAction_Island2Off";
            ActionProcess = 17;
            WriteData("StepAction_ActionProcess", ActionProcess);
            OnExecuteRefreshAllCommand();
        }
        /// <summary>
        /// 是否可以执行加工岛2下料命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland2OffCommand()
        {
            return (ActionProcess == 16 && !isAllAction && connectStatus) ? true : false;
        }
        /// <summary>
        /// 执行成品入库命令
        /// </summary>
        private void OnExecuteProductInCommand()
        {
            StepActionCode = "StepAction_ProductInStorage";
            ActionProcess = 19;
            WriteData("StepAction_ActionProcess", ActionProcess);
            OnExecuteRefreshAllCommand();
        }
        /// <summary>
        /// 是否可以执行成品入库命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteProductInCommand()
        {
            return (ActionProcess == 18 && !isAllAction && connectStatus) ? true : false;
        }

        /// <summary>
        /// 执行全程命令
        /// </summary>
        private void OnExecuteAllActionCommand()
        {
            OnExecuteRefreshAllCommand();
            if (MatWareHouseLocationInfoList.Count == 0)
            {
                Application.Current.Resources["UiMessage"] = "仓库中没有空库位的成品库位！";
                return;
            }
            int count1 = MaterialOutStorageInfoList.Count;
            int count2 = MaterialBatchInfoList.Count;
            if (count1 > 0 && count2 > 0)
            {
                MaterialOutStorageInfo = MaterialOutStorageInfoList.FirstOrDefault();
                isAllAction = true;
                WriteBoolData("StepAction_IsWholeCourse", true);
                ActionProcess = 1;
                WriteData("StepAction_ActionProcess", ActionProcess);
                Application.Current.Resources["UiMessage"] = "正在执行:全程-原料出库";
            }
            else
            {
                isAllAction = false;
                WriteBoolData("StepAction_IsWholeCourse", false);
                ActionProcess = 0;
                WriteData("StepAction_ActionProcess", ActionProcess);
                Application.Current.Resources["UiMessage"] = "请先填写原料出库单或原料不足！";
                return;
            }

        }
        /// <summary>
        /// 是否可以执行全程命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteAllActionCommand()
        {
            return (ActionProcess == 0 && MaterialBatchInfoList.Count > 0 && connectStatus) ? true : false;
        }
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

        private bool OnCanExecuteDisconnectCommand()
        {
            return !Equals(this.client, null) && connectStatus;
        }
        private void OnExecuteInitCommand()
        {
            ActionProcess = 0;
            isAllAction = false;
            WriteData("StepAction_ActionProcess", ActionProcess);
            WriteBoolData("StepAction_IsWholeCourse", isAllAction);
            isClickInitCommand = true;
            InitParaMeters();
            OnExecuteRefreshAllCommand();
        }

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
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("StepAction_IsWholeCourse")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        isAllAction = bool.Parse(v1.Value.ToString());
                        //if (isAllAction)
                        //{
                        switch (ActionProcess)
                        {
                            case 2:
                                if (isAllAction)
                                {
                                    if (!isClickToWorkIsland1Command)
                                    {
                                        OnExecuteToWorkIsland1Command();
                                        isClickToWorkIsland1Command = true;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    if (!isClickToWorkIsland1Command)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            CommandManager.InvalidateRequerySuggested();

                                        }));
                                        string tip = (isAllAction ? "全程-" : "分步-");
                                        Application.Current.Resources["UiMessage"] = tip + "原料出库完成";
                                        isClickToWorkIsland1Command = true;
                                    }
                                }
                                break;
                            case 4:
                                if (isAllAction)
                                {
                                    if (!isClickIsland1OnCommand)
                                    {
                                        OnExecuteIsland1OnCommand();
                                        isClickIsland1OnCommand = true;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    if (!isClickIsland1OnCommand)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            CommandManager.InvalidateRequerySuggested();

                                        }));
                                        string tip = (isAllAction ? "全程-" : "分步-");
                                        Application.Current.Resources["UiMessage"] = tip + "转运到加工岛1完成";
                                        isClickIsland1OnCommand = true;
                                    }
                                }
                                break;
                            case 6:
                                if (isAllAction)
                                {
                                    if (!isClickIsland1WorkingCommand)
                                    {
                                        OnExecuteIsland1WorkingCommand();
                                        isClickIsland1WorkingCommand = true;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    if (!isClickIsland1WorkingCommand)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            CommandManager.InvalidateRequerySuggested();

                                        }));
                                        isClickIsland1WorkingCommand = true;
                                        string tip = (isAllAction ? "全程-" : "分步-");
                                        Application.Current.Resources["UiMessage"] = tip + "加工岛1上料完成";
                                    }
                                }
                                break;
                            case 8:
                                if (isAllAction)
                                {
                                    if (!isClickIsland1OffCommand)
                                    {
                                        OnExecuteIsland1OffCommand();
                                        isClickIsland1OffCommand = true;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    if (!isClickIsland1OffCommand)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            CommandManager.InvalidateRequerySuggested();

                                        }));
                                        string tip = (isAllAction ? "全程-" : "分步-");
                                        Application.Current.Resources["UiMessage"] = tip + "加工岛1生产完成";
                                        isClickIsland1OffCommand = true;
                                    }
                                }
                                break;
                            case 10:
                                if (isAllAction)
                                {
                                    if (!isClickToWorkIsland2Command)
                                    {
                                        OnExecuteToWorkIsland2Command();
                                        isClickToWorkIsland2Command = true;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    if (!isClickToWorkIsland2Command)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            CommandManager.InvalidateRequerySuggested();

                                        }));
                                        string tip = (isAllAction ? "全程-" : "分步-");
                                        Application.Current.Resources["UiMessage"] = tip + "加工岛1下料完成";
                                        isClickToWorkIsland2Command = true;
                                    }
                                }
                                break;
                            case 12:
                                if (isAllAction)
                                {
                                    if (!isClickIsland2OnCommand)
                                    {
                                        OnExecuteIsland2OnCommand();
                                        isClickIsland2OnCommand = true;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    if (!isClickIsland2OnCommand)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            CommandManager.InvalidateRequerySuggested();

                                        }));
                                        string tip = (isAllAction ? "全程-" : "分步-");
                                        Application.Current.Resources["UiMessage"] = tip + "转运到加工岛2完成";
                                        isClickIsland2OnCommand = true;
                                    }
                                }
                                break;
                            case 14:
                                if (isAllAction)
                                {
                                    if (!isClickIsland2WorkingCommand)
                                    {
                                        OnExecuteIsland2WorkingCommand();
                                        isClickIsland2WorkingCommand = true;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    if (!isClickIsland2WorkingCommand)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            CommandManager.InvalidateRequerySuggested();

                                        }));
                                        string tip = (isAllAction ? "全程-" : "分步-");
                                        Application.Current.Resources["UiMessage"] = tip + "加工岛2上料完成";
                                        isClickIsland2WorkingCommand = true;
                                    }
                                }
                                break;
                            case 16:
                                if (isAllAction)
                                {
                                    if (!isClickIsland2OffCommand)
                                    {
                                        OnExecuteIsland2OffCommand();
                                        isClickIsland2OffCommand = true;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    if (!isClickIsland2OffCommand)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            CommandManager.InvalidateRequerySuggested();

                                        }));
                                        string tip = (isAllAction ? "全程-" : "分步-");
                                        Application.Current.Resources["UiMessage"] = tip + "加工岛2生产完成";
                                        isClickIsland2OffCommand = true;
                                    }
                                }
                                break;
                            case 18:
                                if (isAllAction)
                                {
                                    if (!isClickProductInCommand)
                                    {
                                        OnExecuteProductInCommand();
                                        isClickProductInCommand = true;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    if (!isClickProductInCommand)
                                    {
                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            CommandManager.InvalidateRequerySuggested();

                                        }));
                                        string tip = (isAllAction ? "全程-" : "分步-");
                                        Application.Current.Resources["UiMessage"] = tip + "加工岛2下料完成";
                                        isClickProductInCommand = true;
                                    }
                                }
                                break;
                            case 20:
                                if (!isClickInitCommand)
                                {
                                    OnExecuteInitCommand();

                                }
                                break;
                            default: break;
                        }

                        //}
                    }
                }
                var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("StepAction_ActionProcess")).FirstOrDefault();
                if (!Equals(v2, null))
                {
                    if (!Equals(v2.Value, null))
                    {
                        ActionProcess = int.Parse(v2.Value.ToString());
                    }
                }

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
        #region

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
            FilterRule filterRule1 = new FilterRule("OutStorageType", 4, FilterOperate.Equal);
            FilterRule filterRule2 = new FilterRule("OutStorageStatus", 1, FilterOperate.Equal);
            filterGroup.Rules.Add(filterRule1);
            filterGroup.Rules.Add(filterRule2);
            //
            pageRepuestParams.FilterGroup = filterGroup;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MaterialOutStorageInfoModel>>>
                (GlobalData.ServerRootUri + "MaterialOutStorageInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取物料出库信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("物料出库信息内容：" + Utility.JsonHelper.ToJson(result));
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
            FilterRule frtype = new FilterRule("MatWareHouseLocation.WareHouseLocationType", 1, FilterOperate.Equal);
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
        private ObservableCollection<MaterialBatchInfoModel> materialbatchinfoDataSource = new ObservableCollection<MaterialBatchInfoModel>();

        public ObservableCollection<MaterialBatchInfoModel> MaterialBatchInfoDataSource
        {
            get { return materialbatchinfoDataSource; }
            set { Set(ref materialbatchinfoDataSource, value); }
        }
        private MaterialBatchInfoModel batchinfo = new MaterialBatchInfoModel();
        void OnPropertyChangedCommand(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsChecked"))
            {
                var selectedObj = sender as MaterialBatchInfoModel;
                if (selectedObj == null)
                    return;
                if (selectedObj.IsChecked)
                {
                    MaterialBatchInfoDataSource.Clear();
                    var tmplist = MaterialBatchInfoList.Where(a => a.IsChecked == true && a.Id != selectedObj.Id).ToList();
                    for (int i = 0; i < tmplist?.Count(); i++)
                    {
                        tmplist[i].IsChecked = false;
                    }
                    MaterialBatchInfoDataSource.Add(selectedObj);
                }
                else if (!selectedObj.IsChecked)
                {
                    MaterialBatchInfoDataSource.Remove(selectedObj);
                }
            }
        }


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

