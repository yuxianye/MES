﻿using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Solution.Desktop.Agv.Model;
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
using System.Windows.Shapes;

namespace Solution.Desktop.Agv.ViewModel
{
    /// <summary>
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
    public class AgvAnimationViewModel : PageableViewModelBase
    {
        public AgvAnimationViewModel()
        {

            Task.Factory.StartNew(new Action(init));

            //this.MenuModule
            //initCommand();
            //registerMessenger();
            //getPageData(1, int.Parse(Utility.ConfigHelper.GetAppSetting("PageSize")));
            //SocketInit();
        }

        private void init()
        {
            //mainContent.Background = new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = 255, R = 255, G = 45, B = 86 });


            //pathData = MarkPoints.ToPathDataString();


            //Path path = new Path();
            //path.Name = "path1";
            ////path.Data = PathGeometry.CreateFromGeometry(Geometry.Parse("M1,1 L230.67997,1 230.67997,70.67997 140.67998,70.67997 140.67998,135.68002 300.68,85.67999 C300.68,85.67999 300.68,140.68005 300.68,80.68002 300.68,20.679984 340.68002,40.679985 340.68002,40.679985 L383.18005,83.18003 383.18005,115.68004 325.68006,115.68")); ;
            //path.Data = PathGeometry.CreateFromGeometry(Geometry.Parse(pathData)); ;
            //path.Stroke = Utility.Windows.ResourceHelper.FindResource("AgvCarColorBrush") as Brush;



            //path.Stretch = Stretch.None;
            //path.StrokeLineJoin = PenLineJoin.Round;

            //path.StrokeThickness = 2;

            //mainContent.Children.Add(path);
            //for (int i = 0; i < agvCars.Count; i++)
            //{
            //    Canvas.SetLeft(agvCars[i], 100 + 100 * i);
            //    Canvas.SetTop(agvCars[i], 100 + 100 * i);
            //    agvCars[i].Background = Brushes.Green;// new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = 255, R = 0, G = 45, B = 86 });
            //    mainContent.Children.Add(agvCars[i]);
            //}
            //initMarkPoint();

            initCommand();
            roadInfoList = initRoadInfo();
            markPointList = initMarkPoint();
            minX = markPointList.Min(a => a.X);
            minY = markPointList.Min(a => a.Y);

            SocketInit();

            getAgvInfoPageData();
            ClientDataEntities = new ObservableCollection<ClientDataEntity>(getClientDataEntities(1, 100));
            this.client.Send(GetMessage(JsonHelper.ToJson(ClientDataEntities)));

        }

        #region 分页数据路段查询

        private List<RoadInfoModel> roadInfoList;

        /// <summary>
        /// 分页请求参数
        /// </summary>
        private PageRepuestParams pageRepuestParams = new PageRepuestParams();

        /// <summary>
        /// 初始化路段信息
        /// </summary>
        private List<RoadInfoModel> initRoadInfo()
        {

            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = 200;
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<RoadInfoModel>>>(GlobalData.ServerRootUri + "RoadInfo/PageData",
                Utility.JsonHelper.ToJson(pageRepuestParams));
            if (!Equals(result, null) && result.Successed)
            {
                if (result.Data.Data.Any())
                {
                    return result.Data.Data.ToList();
                }
                else
                {
                    return new List<RoadInfoModel>();
                }
            }
            else
            {
                //操作失败，显示错误信息
                LogHelper.Info(result?.Message ?? "查询路段信息失败，请联系管理员！");
                return new List<RoadInfoModel>();
            }
        }

        #endregion

        #region 分页查询地标点

        private List<MarkPointInfoModel> markPointList;

        /// <summary>
        /// 初始化地标点
        /// </summary>
        private List<MarkPointInfoModel> initMarkPoint()
        {
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = 200;

            //过滤未在路段中使用的地标点
            FilterGroup filterGroup = new FilterGroup(FilterOperate.Or);
            foreach (var v in roadInfoList)
            {
                FilterRule filterRuleStartId = new FilterRule("Id", v.StartMarkPointInfo_Id);
                FilterRule filterRuleEndId = new FilterRule("Id", v.EndMarkPointInfo_Id);
                filterGroup.Rules.Add(filterRuleStartId);
                filterGroup.Rules.Add(filterRuleEndId);
            }
            pageRepuestParams.FilterGroup = filterGroup;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MarkPointInfoModel>>>(GlobalData.ServerRootUri + "MarkPointInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    return result.Data.Data.ToList();
                }
                else
                {
                    return new List<MarkPointInfoModel>();
                }
            }
            else
            {
                //操作失败，显示错误信息
                LogHelper.Info(result?.Message ?? "查询地标信息失败，请联系管理员！");
                return new List<MarkPointInfoModel>();
            }
        }

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
        private List<ClientDataEntity> getClientDataEntities(int pageIndex, int pageSize)
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;
            pageRepuestParams.PageIndex = pageIndex;
            pageRepuestParams.PageSize = pageSize;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<ProductionProcessEquipmentBusinessNodeMapModel>>>(GlobalData.ServerRootUri + "ProductionProcessEquipmentBusinessNodeMap/GridData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取工序列表用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("工序列表内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                List<ClientDataEntity> results = new List<ClientDataEntity>(result.Data.Data.Count());
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    //ClientDataEntities?.Clear();
                    foreach (var data in result.Data.Data)
                    {
                        ClientDataEntity clientDataEntity = new ClientDataEntity();
                        clientDataEntity.FunctionCode = FuncCode.SubScription;
                        clientDataEntity.ProductionProcessEquipmentBusinessNodeMapId = data.Id;
                        clientDataEntity.StatusCode = 0;
                        clientDataEntity.NodeId = data.NodeUrl;
                        clientDataEntity.ValueType = Type.GetType("System." + data.DataType);
                        results.Add(clientDataEntity);
                        //ProductionProcessInfoModel commOpcUaNode = new ProductionProcessInfoModel();
                        //commOpcUaNode = data;
                        //commOpcUaNode.PropertyChanged += OnPropertyChangedCommand;
                        //ProductionProcessInfoList.Add(commOpcUaNode);


                    }
                    //TotalCounts = result.Data.Total;
                    return results;

                    //this.client.Send(GetMessage(JsonHelper.ToJson(results)));
                }
                else
                {
                    //ClientDataEntities?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                    return null;

                }
            }
            else
            {
                //操作失败，显示错误信息
                //ClientDataEntities = new ObservableCollection<ClientDataEntity>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询数据点列表失败，请联系管理员！";
                return null;

            }


        }
        #endregion

        private double ratioX;
        private double initRatioX()
        {
            if (Equals(markPointList, null) || !markPointList.Any())
            {
                return 0;
            }
            var maxX = markPointList.Max(x => x.X);
            var minX = markPointList.Min(x => x.X);

            return (mainContent.ActualWidth - mainContent.ActualWidth * 0.1) / (maxX - minX);
        }

        private double ratioY;
        private double minX;
        private double minY;
        private double offsetX;
        private double offsetY;


        private double initRatioY()
        {
            if (Equals(markPointList, null) || !markPointList.Any())
            {
                return 0;
            }
            var maxY = markPointList.Max(x => x.Y);
            var minY = markPointList.Min(x => x.Y);

            return (mainContent.ActualHeight - mainContent.ActualHeight * 0.1) / (maxY - minY);

        }


        #region 根据路段和地标点初始化界面线路

        /// <summary>
        /// 初始化线路
        /// </summary>
        private void initPath()
        {
            System.Diagnostics.Debug.Print($"比率X{ratioX},比率Y{ratioY},控件宽{mainContent.ActualWidth },控件高{MainContent.ActualHeight}，子控件数{mainContent.Children.Count }");
            mainContent.Children.Clear();

            if (Equals(roadInfoList, null) || !roadInfoList.Any())
            {
                return;
            }
            foreach (var v in roadInfoList)
            {

                mainContent.Children.Add(createPath(v));
            }
        }



        private Path createPath(RoadInfoModel roadInfoModel)
        {
            Path path = new Path();
            path.Name = roadInfoModel.RoadName;

            path.ToolTip = $"路段:{roadInfoModel.RoadName}\r\n状态:{ roadInfoModel.RoadStatus}\r\n开始地标:{ roadInfoModel.StartMarkPointInfoName}\r\n结束地标:{roadInfoModel.EndMarkPointInfoName}";
            //path.Data = PathGeometry.CreateFromGeometry(Geometry.Parse("M1,1 L230.67997,1 230.67997,70.67997 140.67998,70.67997 140.67998,135.68002 300.68,85.67999 C300.68,85.67999 300.68,140.68005 300.68,80.68002 300.68,20.679984 340.68002,40.679985 340.68002,40.679985 L383.18005,83.18003 383.18005,115.68004 325.68006,115.68")); ;
            path.Data = PathGeometry.CreateFromGeometry(Geometry.Parse(string.Format("M {0} {1} {2} {3}",
               markPointList.First(a => a.Id == roadInfoModel.StartMarkPointInfo_Id).X * ratioX - offsetX, markPointList.First(a => a.Id == roadInfoModel.StartMarkPointInfo_Id).Y * ratioY - offsetY,
              markPointList.First(a => a.Id == roadInfoModel.EndMarkPointInfo_Id).X * ratioX - offsetX, markPointList.First(a => a.Id == roadInfoModel.EndMarkPointInfo_Id).Y * ratioY - offsetY)));
            System.Diagnostics.Debug.Print($"名称{path.Name },坐标点{path.Data.ToString()}");

            //路线的状态颜色
            path.Stroke = Utility.Windows.ResourceHelper.FindResource("AgvRoadStatusBrush" + roadInfoModel.RoadStatus) as Brush;

            //path.Stretch = Stretch.None;
            //path.StrokeLineJoin = PenLineJoin.Round;
            path.StrokeThickness = 4;
            path.StrokeEndLineCap = PenLineCap.Triangle;
            return path;
        }
        #endregion








        //private  ObservableCollection<UserControls.AgvCar> agvCars = new ObservableCollection<UserControls.AgvCar>() {
        //    new UserControls.AgvCar() { CarName="1" },
        //    new UserControls.AgvCar() { CarName="2" },
        //    new UserControls.AgvCar() { CarName="3" },
        //    new UserControls.AgvCar() { CarName="4" },


        //};
        #region Agv车辆集合
        private ObservableCollection<UserControls.AgvCar> agvCars = new ObservableCollection<UserControls.AgvCar>();

        /// <summary>
        /// agv控件集合
        /// </summary>
        public ObservableCollection<UserControls.AgvCar> AgvCars
        {
            get { return agvCars; }
            set { Set(ref agvCars, value); }
        }

        #endregion




        private List<UserControls.AgvCar> initCar()
        {
            var carModelList = new List<AgvInfoModel>();
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = 200;
            pageRepuestParams.FilterGroup = null;
            getPageData(this.pageRepuestParams.PageIndex, pageRepuestParams.PageSize);
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<AgvInfoModel>>>(GlobalData.ServerRootUri + "AgvInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    carModelList = result.Data.Data.ToList();
                }

            }
            else
            {
                //操作失败，显示错误信息
                LogHelper.Info(result?.Message ?? "查询车辆信息失败，请联系管理员！");
            }



            List<UserControls.AgvCar> carControlList = new List<UserControls.AgvCar>();
            foreach (var v in carModelList)
            {
                Agv.UserControls.AgvCar agvCar = new UserControls.AgvCar();
                agvCar.Name = v.CarNo;
                //agvCar.carName.Text = v.CarNo ?? null;
                agvCar.CarStatus = 0;
                agvCar.Tag = v;
                Canvas.SetLeft(agvCar, offsetX);
                Canvas.SetTop(agvCar, offsetX);
                //agvCars[i].Background = Brushes.Green;// new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = 255, R = 0, G = 45, B = 86 });
                mainContent.Children.Add(agvCar);
                carControlList.Add(agvCar);
            }

            return carControlList;


            //border1 = agvCars[0];
        }







        /// <summary>
        /// 分页执行函数
        /// </summary>
        /// <param name="e"></param>
        public override void OnExecutePageChangedCommand(PageChangedEventArgs e)
        {
            Utility.LogHelper.Info(e.PageIndex.ToString() + " " + e.PageSize);
            getPageData(e.PageIndex, e.PageSize);
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
                //if (value != 0 && !double.IsNaN(value) && !double.IsInfinity(value) && !double.IsNegativeInfinity(value) && !double.IsPositiveInfinity(value))
                //{
                //    ratioX = initRatioX();
                //    ratioY = initRatioY();
                //    initPath();
                //}

                //System.Diagnostics.Debug.Print(value.ToString())

                ;
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
                //if (value != 0 && !double.IsNaN(value) && !double.IsInfinity(value) && !double.IsNegativeInfinity(value) && !double.IsPositiveInfinity(value))
                //{
                //    ratioX = initRatioX();
                //    ratioY = initRatioY();
                //    initPath();
                //}
                //System.Diagnostics.Debug.Print(value.ToString());
            }
        }
        #endregion

        /// <summary>
        /// 定位点
        /// </summary>
        //private List<Point> MarkPoints = new List<Point>() {

        //    new Point (50,200),
        //    new Point (50,800),
        //    new Point (800,800),
        //    new Point (800 ,50),
        //};

        private List<PathModel> MarkPoint = new List<PathModel>()
        {


            new PathModel(){Id= Utility.CombHelper.NewComb(),Name ="Path1",StartPoint = new Point (50,200), EndPoint =new Point (50,800),PathStatus =1 },
            new PathModel(){Id= Utility.CombHelper.NewComb(),Name ="Path2",StartPoint = new Point (50,800), EndPoint =new Point (800,800),PathStatus =1 },
            new PathModel(){Id= Utility.CombHelper.NewComb(),Name ="Path3",StartPoint = new Point (800,800), EndPoint =new Point (800,50),PathStatus =1 },

            new PathModel(){Id= Utility.CombHelper.NewComb(),Name ="Path4",StartPoint = new Point (800,800), EndPoint =new Point (1000,800),PathStatus =1 },
            new PathModel(){Id= Utility.CombHelper.NewComb(),Name ="Path5",StartPoint = new Point (1000,800), EndPoint =new Point (1000,500),PathStatus =1 },

            //new PathModel(){Id= Utility.CombHelper.NewComb(),Name ="Path5",StartPoint = new Point (50,200), EndPoint =new Point (50,800),PathStatus =1 },
            //new PathModel(){Id= Utility.CombHelper.NewComb(),Name ="Path6",StartPoint = new Point (50,200), EndPoint =new Point (50,800),PathStatus =1 },
            //new KeyValuePair<Point, Point> (new Point (50,800),new Point (800,800)),
            //new KeyValuePair<Point, Point> (new Point (50,800),new Point (800,800)),
            //new KeyValuePair<Point, Point> (new Point (800,800),new Point (80,0)),
        };



        //string pathData = "M10,10 L230.67997,1 230.67997,70.67997 140.67998,70.67997 140.67998,135.68002 300.68,85.67999 C300.68,85.67999 300.68,140.68005 300.68,80.68002 300.68,20.679984 340.68002,40.679985 340.68002,40.679985 L383.18005,83.18003 383.18005,115.68004 325.68006,115.68";
        //string pathData = "M50,50 1000,50 C 1050,50 1100,800 50,800";
        string pathData = "M50,50 1000,50 C 1050,60 1100,500 1000,800 M 1000,800 50,800 ";




        /// <summary>
        /// 分页请求参数
        /// </summary>
        //private PageRepuestParams pageRepuestMarkPointParams = new PageRepuestParams();


        //        /// <summary>
        //        /// 初始化地标点
        //        /// </summary>
        //        private void initMarkPoint()
        //        {
        //#if DEBUG
        //            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //            stopwatch.Start();
        //#endif
        //            pageRepuestMarkPointParams.SortField = "LastUpdatedTime";
        //            pageRepuestMarkPointParams.SortOrder = "desc";

        //            pageRepuestMarkPointParams.PageIndex = 1;
        //            pageRepuestMarkPointParams.PageSize = 200;

        //            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
        //            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<MarkPointInfoModel>>>(GlobalData.ServerRootUri + "MarkPointInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

        //#if DEBUG
        //            stopwatch.Stop();
        //            Utility.LogHelper.Info("获取地标信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
        //            Utility.LogHelper.Info("地标信息内容：" + Utility.JsonHelper.ToJson(result));
        //#endif

        //            if (!Equals(result, null) && result.Successed)
        //            {
        //                Application.Current.Resources["UiMessage"] = result?.Message;
        //                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
        //                if (result.Data.Data.Any())
        //                {
        //                    //TotalCounts = result.Data.Total;
        //                    //Messenger.Default.Send(LoginUser, MessengerToken.LoginSuccess);
        //                    markPointList = result.Data.Data.ToList();
        //                    //TotalCounts = result.Data.Total;
        //                }
        //                else
        //                {
        //                    markPointList?.Clear();
        //                    //TotalCounts = 0;
        //                    Application.Current.Resources["UiMessage"] = "未找到数据";
        //                }
        //            }
        //            else
        //            {
        //                //操作失败，显示错误信息
        //                markPointList = new List<MarkPointInfoModel>();
        //                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询地标信息失败，请联系管理员！";
        //            }

        //        }







        public Canvas mainContent = new Canvas() { Background = Utility.Windows.ResourceHelper.FindResource("WorkshopBackBrush1") as Brush };
        public Canvas MainContent
        {
            get { return mainContent; }
            set { Set(ref mainContent, value); }
        }
        //Border border = new Border();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orientation">0正向 1反向</param>
        /// <param name="data">路径数据</param>
        private void MatrixStory(UserControls.AgvCar agvCar, PathModel pathModel)
        {

            //border.Width = 10;
            //border.Height = 10;
            //border.Visibility = Visibility.Visible;
            //if (orientation == 0)
            //{
            //    border.Background = new SolidColorBrush(Colors.Blue);
            //}
            //else
            //{
            //    border.Background = new SolidColorBrush(Colors.Green);
            //}
            //if (!this.MainContent.Children.Contains(agvCar))
            //{
            //    this.MainContent.Children.Add(agvCar);

            //}
            Canvas.SetLeft(agvCar, -agvCar.Width / 2);
            Canvas.SetTop(agvCar, -agvCar.Height / 2);
            agvCar.RenderTransformOrigin = new Point(0.5, 0.5);

            MatrixTransform matrix = new MatrixTransform();
            TransformGroup groups = new TransformGroup();
            groups.Children.Add(matrix);
            agvCar.RenderTransform = groups;
            //NameScope.SetNameScope(this, new NameScope());
            string registname = "matrix" + Guid.NewGuid().ToString().Replace("-", "");
            agvCar.RegisterName(registname, matrix);
            MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
            //matrixAnimation.PathGeometry = PathGeometry.CreateFromGeometry(Geometry.Parse(    (  mainContent.FindChild(path.Key.ToString() + path.Value.ToString()) as Path )  .Data       ));
            matrixAnimation.PathGeometry = PathGeometry.CreateFromGeometry(mainContent.FindChild<Path>(pathModel.Name).Data);
            matrixAnimation.Duration = new Duration(TimeSpan.FromSeconds(10));
            matrixAnimation.DoesRotateWithTangent = false;//旋转
            matrixAnimation.AccelerationRatio = 0.4;
            matrixAnimation.DecelerationRatio = 0.4;
            //matrixAnimation.FillBehavior 
            //matrixAnimation.FillBehavior = FillBehavior.Stop;
            Storyboard story = new Storyboard();
            story.Children.Add(matrixAnimation);
            Storyboard.SetTargetName(matrixAnimation, registname);
            Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

            //#region 控制显示与隐藏
            //ObjectAnimationUsingKeyFrames ObjectAnimation = new ObjectAnimationUsingKeyFrames();
            //ObjectAnimation.Duration = matrixAnimation.Duration;
            //DiscreteObjectKeyFrame kf1 = new DiscreteObjectKeyFrame(Visibility.Visible, TimeSpan.FromMilliseconds(1));
            //ObjectAnimation.KeyFrames.Add(kf1);
            //story.Children.Add(ObjectAnimation);
            ////Storyboard.SetTargetName(border, border.Name);
            //Storyboard.SetTargetProperty(ObjectAnimation, new PropertyPath(UIElement.VisibilityProperty));
            //#endregion

            story.FillBehavior = FillBehavior.HoldEnd;
            agvCar.Tag = story;

            story.Begin(agvCar, true);

        }

        ~AgvAnimationViewModel()
        {
        }

        //#region OpcUa业务数据模型
        ///// <summary>
        ///// OpcUa业务数据模型
        ///// </summary>
        //private Fork commOpcUaBusinessInfo;// = new EnterpriseModel();
        ///// <summary>
        ///// OpcUa业务数据模型
        ///// </summary>
        //public Fork CommOpcUaBusinessInfo
        //{
        //    get { return commOpcUaBusinessInfo; }
        //    set { Set(ref commOpcUaBusinessInfo, value); }
        //}
        //#endregion

        //#region OpcUa业务数据模型,用于列表数据显示
        //private ObservableCollection<Fork> commOpcUaBusinessInfoList = new ObservableCollection<Fork>();

        ///// <summary>
        ///// OpcUa业务数据
        ///// </summary>
        //public ObservableCollection<Fork> CommOpcUaBusinessInfoList
        //{
        //    get { return commOpcUaBusinessInfoList; }
        //    set { Set(ref commOpcUaBusinessInfoList, value); }
        //}
        //#endregion

        #region 命令定义和初始化

        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand ConnectCommand { get; set; }

        public ICommand StopCommand { get; set; }

        public ICommand SizeChangedCommand { get; set; }

        public ICommand RefreshAllCommand { get; set; }




        private void initCommand()
        {
            ConnectCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConnectCommand, OnCanExecuteConnectCommand);
            StopCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteStopCommand);
            SizeChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSizeChangedCommand);
            RefreshAllCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshAllCommand);
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
            if (a > 6)
            {
                a = 0;
            }
            if (agvCars.Count() < 1)
            {
                return;
            }
            //int tmp = a % roadInfoList.Count()+1;

            //MatrixStory(0, mainContent.FindChild<Path>("Path" + tmp));
            //MatrixStory(this.AgvCars[1], MarkPoint[1]);
            //MatrixStory(this.AgvCars[2], MarkPoint[2]);
            //MatrixStory(this.AgvCars[3], MarkPoint[3]);
            a = a + 1;
            //if (!this.mainContent.Children.Contains(border1))
            //{
            //    this.mainContent.Children.Add(border1);
            //}
            //Run
            //Test_Matrix(mainContent.FindChild<Path>("Path" + tmp));

            //agvCars[0].Run(mainContent, mainContent.FindChild<Path>("Path" + tmp), 10);
            //AgvCars[1].Run(mainContent, mainContent.FindChild<Path>("Path" + tmp), 5);
            //AgvCars[1].MatrixStory(AgvCars[1], mainContent.FindChild<Path>("Path" + tmp));
            agvCars[0].Run(mainContent, mainContent.FindChild<Path>("路段" + a), 10);

            //mainContent.Background = Brushes.Yellow;
            System.Diagnostics.Debug.Print(mainContent.ActualHeight.ToString());
            System.Diagnostics.Debug.Print(mainContent.ActualWidth.ToString());
            //if (connectResult ^= true)
            //{
            //    //Task<bool> resultTask = client.ConnectAsync();
            //    //bool result = resultTask.Result;
            //    //Application.Current.Resources["UiMessage"] = result ? "连接成功！" : "连接失败！";

            //}
            //else
            //{
            //    Task<bool> resultTask = client.CloseAsync();
            //    bool result = resultTask.Result;
            //    Application.Current.Resources["UiMessage"] = result ? "断开连接成功！" : "断开连接失败！";
            //}
        }



        //public void MatrixStory(Solution.Desktop.AgvTest.UserControls.AgvCar agvCar, Path path)
        //{

        //    //border.Width = 10;
        //    //border.Height = 10;
        //    //border.Visibility = Visibility.Visible;
        //    //if (orientation == 0)
        //    //{
        //    //    border.Background = new SolidColorBrush(Colors.Blue);
        //    //}
        //    //else
        //    //{
        //    //    border.Background = new SolidColorBrush(Colors.Green);
        //    //}
        //    //if (!this.canvas.Children.Contains(border))
        //    //{
        //    //    this.canvas.Children.Add(border);

        //    //}

        //    Canvas.SetLeft(agvCar, -agvCar.Width / 2);
        //    Canvas.SetTop(agvCar, -agvCar.Height / 2);
        //    agvCar.RenderTransformOrigin = new Point(0.5, 0.5);

        //    MatrixTransform matrix = new MatrixTransform();
        //    TransformGroup groups = new TransformGroup();
        //    groups.Children.Add(matrix);
        //    agvCar.RenderTransform = groups;
        //    //NameScope.SetNameScope(this, new NameScope());
        //    string registname = "matrix" + Guid.NewGuid().ToString().Replace("-", "");
        //    agvCar.RegisterName(registname, matrix);
        //    MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
        //    matrixAnimation.PathGeometry = PathGeometry.CreateFromGeometry(path.Data);
        //    matrixAnimation.Duration = new Duration(TimeSpan.FromSeconds(10));
        //    matrixAnimation.DoesRotateWithTangent = true;//旋转
        //    //matrixAnimation.FillBehavior 
        //    //matrixAnimation.FillBehavior = FillBehavior.Stop;
        //    Storyboard story = new Storyboard();
        //    story.Children.Add(matrixAnimation);
        //    Storyboard.SetTargetName(matrixAnimation, registname);
        //    Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

        //    //#region 控制显示与隐藏
        //    //ObjectAnimationUsingKeyFrames ObjectAnimation = new ObjectAnimationUsingKeyFrames();
        //    //ObjectAnimation.Duration = matrixAnimation.Duration;
        //    //DiscreteObjectKeyFrame kf1 = new DiscreteObjectKeyFrame(Visibility.Visible, TimeSpan.FromMilliseconds(1));
        //    //ObjectAnimation.KeyFrames.Add(kf1);
        //    //story.Children.Add(ObjectAnimation);
        //    ////Storyboard.SetTargetName(border, border.Name);
        //    //Storyboard.SetTargetProperty(ObjectAnimation, new PropertyPath(UIElement.VisibilityProperty));
        //    //#endregion
        //    story.FillBehavior = FillBehavior.HoldEnd;
        //    story.Begin(agvCar, true);
        //}


        //AgvTest.UserControls.AgvCar border1 = new UserControls.AgvCar() { CarName = "1" };
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="orientation">0正向 1反向</param>
        ///// <param name="data">路径数据</param>
        //private void MatrixStory(int orientation, Path path)
        //{

        //    border.Width = 10;
        //    border.Height = 10;
        //    border.Visibility = Visibility.Visible;
        //    if (orientation == 0)
        //    {
        //        border.Background = new SolidColorBrush(Colors.Blue);
        //    }
        //    else
        //    {
        //        border.Background = new SolidColorBrush(Colors.Green);
        //    }
        //    if (!this.mainContent.Children.Contains(border))
        //    {
        //        this.mainContent.Children.Add(border);

        //    }

        //    Canvas.SetLeft(border, -border.Width / 2);
        //    Canvas.SetTop(border, -border.Height / 2);
        //    border.RenderTransformOrigin = new Point(0.5, 0.5);

        //    MatrixTransform matrix = new MatrixTransform();
        //    TransformGroup groups = new TransformGroup();
        //    groups.Children.Add(matrix);
        //    border.RenderTransform = groups;
        //    //NameScope.SetNameScope(this, new NameScope());
        //    string registname = "matrix" + Guid.NewGuid().ToString().Replace("-", "");
        //    //this.RegisterName(registname, matrix);
        //    this.mainContent.RegisterName(registname, matrix);
        //    MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
        //    //matrixAnimation.PathGeometry = PathGeometry.CreateFromGeometry(Geometry.Parse(data));
        //    matrixAnimation.PathGeometry = PathGeometry.CreateFromGeometry(path.Data);
        //    matrixAnimation.Duration = new Duration(TimeSpan.FromSeconds(10));
        //    matrixAnimation.DoesRotateWithTangent = true;//旋转
        //    //matrixAnimation.FillBehavior 
        //    //matrixAnimation.FillBehavior = FillBehavior.Stop;
        //    Storyboard story = new Storyboard();
        //    story.Children.Add(matrixAnimation);
        //    Storyboard.SetTargetName(matrixAnimation, registname);
        //    Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

        //    #region 控制显示与隐藏
        //    ObjectAnimationUsingKeyFrames ObjectAnimation = new ObjectAnimationUsingKeyFrames();
        //    ObjectAnimation.Duration = matrixAnimation.Duration;
        //    DiscreteObjectKeyFrame kf1 = new DiscreteObjectKeyFrame(Visibility.Visible, TimeSpan.FromMilliseconds(1));
        //    ObjectAnimation.KeyFrames.Add(kf1);
        //    story.Children.Add(ObjectAnimation);
        //    //Storyboard.SetTargetName(border, border.Name);
        //    Storyboard.SetTargetProperty(ObjectAnimation, new PropertyPath(UIElement.VisibilityProperty));
        //    #endregion
        //    story.FillBehavior = FillBehavior.HoldEnd;
        //    story.Begin(border, true);
        //}

        private void Test_Matrix(Path path)
        {
            //Canvas.SetLeft(this.border1, -this.border1.ActualWidth / 2);
            //Canvas.SetTop(this.border1, -this.border1.ActualHeight / 2);
            //this.border1.RenderTransformOrigin = new Point(0.5, 0.5);

            //MatrixTransform matrix = new MatrixTransform();
            //this.border1.RenderTransform = matrix;

            //NameScope.SetNameScope(this.mainContent, new NameScope());

            ////this.RegisterName("matrix", matrix);
            //this.mainContent.RegisterName("matrix", matrix);

            //MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
            //matrixAnimation.PathGeometry = path.Data.GetFlattenedPathGeometry();
            //matrixAnimation.Duration = new Duration(TimeSpan.FromSeconds(10));
            ////matrixAnimation.RepeatBehavior = RepeatBehavior.Forever;
            ////matrixAnimation.AutoReverse = true;
            //matrixAnimation.IsOffsetCumulative = false;// !matrixAnimation.AutoReverse;
            //matrixAnimation.DoesRotateWithTangent = true;//旋转
            //matrixAnimation.FillBehavior = FillBehavior.HoldEnd;
            //matrixAnimation.AccelerationRatio = 0.4;
            //matrixAnimation.DecelerationRatio = 0.4;
            ////matrixAnimation.SpeedRatio = 0.5;

            //Storyboard story = new Storyboard();
            //story.Children.Add(matrixAnimation);
            //Storyboard.SetTargetName(matrixAnimation, "matrix");
            //Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));
            //story.Begin(mainContent);
        }





        /// <summary>
        /// 是否可以执行新增命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteConnectCommand()
        {
            return true;
        }

        private void OnExecuteStopCommand()
        {
            //AgvCars[0].Pause();
            //AgvCars[0].CarStatus = 1;

            var clientDataEntities = ClientDataEntities.Where(a => a.NodeId == "ns=2;s=TestChannel.TestDevice.Agv_TaskStatus");

            //foreach (var v in clientDataentity)
            //{
            //    v.FunctionCode = FuncCode.Read;
            //    v.Value = Int16.MinValue;
            //}
            var clientDataentity = clientDataEntities.FirstOrDefault().Clone() as ClientDataEntity;

            List<ClientDataEntity> clientDataEntitiesSend = new List<ClientDataEntity>(1);
            clientDataentity.FunctionCode = FuncCode.Write;
            clientDataentity.Value = (UInt16)10;

            clientDataEntitiesSend.Add(clientDataentity);
            this.client.Send(GetMessage(JsonHelper.ToJson(clientDataEntitiesSend)));


        }

        private void OnExecuteSizeChangedCommand()
        {
            ratioX = initRatioX();
            ratioY = initRatioY();
            offsetX = ratioX * minX - mainContent.ActualWidth * 0.1 / 2;
            offsetY = ratioY * minY - mainContent.ActualHeight * 0.1 / 2;

            if (Equals(markPointList, null))
            {
                return;
            }
            initPath();
            this.AgvCars = new ObservableCollection<UserControls.AgvCar>(initCar());
        }

        private void OnExecuteRefreshAllCommand()
        {
            //Task.Factory.StartNew(new Action(init));
            //OnExecuteSizeChangedCommand();


            var clientDataEntities = ClientDataEntities.Where(a => a.NodeId == "ns=2;s=TestChannel.TestDevice.Agv_TaskStatus");

            //foreach (var v in clientDataentity)
            //{
            //    v.FunctionCode = FuncCode.Read;
            //    v.Value = Int16.MinValue;
            //}
            var clientDataentity = clientDataEntities.FirstOrDefault().Clone() as ClientDataEntity;

            List<ClientDataEntity> clientDataEntitiesSend = new List<ClientDataEntity>(1);
            clientDataentity.FunctionCode = FuncCode.Read;
            //clientDataentity.Value = (UInt16)10;

            clientDataEntitiesSend.Add(clientDataentity);
            this.client.Send(GetMessage(JsonHelper.ToJson(clientDataEntitiesSend)));

        }
        #endregion

        #region Tcp客户端读取OPC数据

        private bool connectResult = false;
        private SocketClientHelper client = new SocketClientHelper(Utility.ConfigHelper.GetAppSetting("ServerIp"), Convert.ToInt32(Utility.ConfigHelper.GetAppSetting("ServerPort")));

        private void SocketInit()
        {
            if (connectResult)
            {
                return;
            }

            connectResult = client.ConnectAsync().Result;
            client.Connected += OnConnect;
            client.Closed += OnClose;
            client.OnDataReceived += OnReceive;
            client.Error += OnError;
        }

        private void SubScription()
        {
            //SocketJsonParamEntity socketJsonParamEntity = new SocketJsonParamEntity();
            ////socketJsonParamEntity.KeyId = Guid.Parse("2B68E863-6F5D-E811-8BA9-F8A005475E49");
            ////socketJsonParamEntity.NodeId = "ns=2;s=TestChannel.TestDevice.word_1";
            //socketJsonParamEntity.FunctionCode = FuncCode.SubScription;
            //List<Guid> subIdList = new List<Guid>();
            //for (int i = 0; i < CommOpcUaBusinessInfoList.Count; i++)
            //{
            //    subIdList.Add(CommOpcUaBusinessInfoList[i].Id);
            //}

            //socketJsonParamEntity.SubScriptionList = subIdList;

            //client.Send(GetMessage(Utility.JsonHelper.ToJson(socketJsonParamEntity)));
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
            SubScription();
            Application.Current.Resources["UiMessage"] = "连接成功！";
        }

        private void OnClose(object sender, EventArgs args)
        {
            connectResult = false;
            Application.Current.Resources["UiMessage"] = "连接已断开！";
        }

        private void OnReceive(object sender, DataEventArgs args)
        {
            var cde = JsonHelper.FromJson<List<ClientDataEntity>>(args.PackageInfo.Data);

            LogHelper.Info(cde.FirstOrDefault()?.NodeId + "      " + cde.FirstOrDefault()?.Value?.ToString());

            foreach (var v in cde)
            {
                var tmp = ClientDataEntities.FirstOrDefault(a => a.NodeId == v.NodeId);
                if (!Equals(v.Value, null))
                {
                    tmp.OldVaule = v.OldVaule?.ToString();
                    tmp.Value = v.Value?.ToString();
                }

                tmp.StatusCode = v.StatusCode;
                LogHelper.Info(tmp.NodeId + " " + tmp.Value?.ToString());
            }

            //Task.Run(() =>
            //{
            //    String jsonStr = args.PackageInfo.Data;
            //    SocketJsonParamEntity socketJsonParamEntity = Utility.JsonHelper.FromJson<SocketJsonParamEntity>(jsonStr);
            //    switch (socketJsonParamEntity.FunctionCode)
            //    {
            //        case FuncCode.Read:
            //            break;
            //        case FuncCode.Write:
            //            break;
            //        case FuncCode.SubScription:
            //            {
            //                if (socketJsonParamEntity.StatusCode == (uint)DeviceStatusCode.SubscriptionOK)
            //                {
            //                    Application.Current.Resources["UiMessage"] = "订阅成功！";
            //                }
            //                else
            //                {
            //                    for (int i = 0; i < CommOpcUaBusinessInfoList.Count; i++)
            //                    {
            //                        if (CommOpcUaBusinessInfoList[i].NodeId.Equals(socketJsonParamEntity.KeyId))
            //                        {
            //                            CommOpcUaBusinessInfoList[i].Value = socketJsonParamEntity.Value.ToString();
            //                        }
            //                    }
            //                }
            //            }
            //            break;
            //        default:
            //            break;
            //    }

            //});
        }

        #endregion




        #region Agv信息


        #region AgvInfo模型
        /// <summary>
        /// AgvInfo模型
        /// </summary>
        private AgvInfoModel agvInfo;// = new AgvInfoModel();
        /// <summary>
        /// AgvInfo模型
        /// </summary>
        public AgvInfoModel AgvInfo
        {
            get { return agvInfo; }
            set { Set(ref agvInfo, value); }
        }
        #endregion

        #region AgvInfo模型,用于列表数据显示
        private ObservableCollection<AgvInfoModel> agvInfoList = new ObservableCollection<AgvInfoModel>();

        /// <summary>
        /// Agv信息数据
        /// </summary>
        public ObservableCollection<AgvInfoModel> AgvInfoList
        {
            get { return agvInfoList; }
            set { Set(ref agvInfoList, value); }
        }
        #endregion

        #region 分页数据查询

        /// <summary>
        /// 取得分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        private void getAgvInfoPageData()
        {
#if DEBUG
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            pageRepuestParams.SortField = GlobalData.SortField;
            pageRepuestParams.SortOrder = GlobalData.SortOrder;

            pageRepuestParams.PageIndex = 1;
            pageRepuestParams.PageSize = 200;
            pageRepuestParams.FilterGroup = null;

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Console.WriteLine(await (await _httpClient.GetAsync("/api/service/AgvInfo/Get?id='1'")).Content.ReadAsStringAsync());
            Utility.Http.HttpClientHelper.SetAccessTokenToHttpClient(GlobalData.AccessTocken);
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult<PageResult<AgvInfoModel>>>(GlobalData.ServerRootUri + "AgvInfo/PageData", Utility.JsonHelper.ToJson(pageRepuestParams));

#if DEBUG
            stopwatch.Stop();
            Utility.LogHelper.Info("获取agv小车信息用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            Utility.LogHelper.Info("agv小车信息内容：" + Utility.JsonHelper.ToJson(result));
#endif

            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                if (result.Data.Data.Any())
                {
                    AgvInfoList = new ObservableCollection<AgvInfoModel>(result.Data.Data);
                }
                else
                {
                    AgvInfoList?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                }
            }
            else
            {
                //操作失败，显示错误信息
                AgvInfoList = new ObservableCollection<AgvInfoModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询Agv信息失败，请联系管理员！";
            }
        }

        #endregion



        #endregion




        #region 报警信息模型,用于列表数据显示
        private ObservableCollection<AlarmInfoModel> alarmInfoList = new ObservableCollection<AlarmInfoModel>();

        /// <summary>
        /// AgvInfo模型信息数据
        /// </summary>
        public ObservableCollection<AlarmInfoModel> AlarmInfoList
        {
            get { return alarmInfoList; }
            set { Set(ref alarmInfoList, value); }
        }
        #endregion


        /// <summary>
        /// 根据点表变化更新界面的内容TTODO
        /// 包括车辆列表内容，报警内容，动画内容
        /// </summary>
        /// <param name="o"></param>
        //private void upUiData(SocketJsonParamEntity socketJsonParamEntity)
        //{
        //    //socketJsonParamEntity.SubScriptionList
        //    //AlarmInfoList.Add(new AlarmInfoModel() { AgvInfo_Id })
        //    //if (CommOpcUaBusinessInfoList[i].NodeId.Equals(socketJsonParamEntity.KeyId))
        //    //{
        //    //    CommOpcUaBusinessInfoList[i].Value = socketJsonParamEntity.Value.ToString();
        //    //}
        //}



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
