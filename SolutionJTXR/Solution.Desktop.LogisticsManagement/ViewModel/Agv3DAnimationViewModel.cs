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
    public class Agv3DAnimationViewModel : VmBase
    {
        private void timercallback(object state)
        {
            //SetAgvStackerPosition();
            //SetMaterialOut();
            //SetToWorkIsland1();
            //SetIsland1On();
            //SetIsland1Working();
            //SetIsland1Off();
            //SetToWorkIsland2();
            //SetIsland2On();
            //SetIsland2Working();
            //SetIsland2Off();
            //SetProductIn();
        }
        #region 绑定AGV和堆垛机位置
        /// <summary>
        /// 绑定AGV和堆垛机位置
        /// </summary>
        private void SetAgvStackerPosition()
        {
            if (ClientDataEntities.Count > 0)
            {
                var v = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_CurrentPosition")).FirstOrDefault().Value;
                if (v != null)
                {
                    string agvposition = v.ToString();
                    int agvp = int.Parse(agvposition);
                    AgvPosition = new Point3D(agvp % 100, AgvPosition.Y, AgvPosition.Z);
                    AgvNamePosition = new Point3D(agvp % 100, AgvPosition.Y, AgvPosition.Z + 5.5);
                }
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("Stacker_CurrentPosition")).FirstOrDefault().Value;
                if (v1 != null)
                {
                    int stackp = int.Parse(v1.ToString());
                    StackerPosition = new Point3D(StackerPosition.X, stackp % 100, StackerPosition.Z);
                }
            }
        }
        #endregion
        #region 原料出库操作
        /// <summary>
        /// 原料出库操作
        /// </summary>
        private void SetMaterialOut()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 1)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineK2_Arrived")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        TransLineK2_Arrived = bool.Parse(v1.Value.ToString());
                        if (TransLineK2_Arrived && !isSend_AgvTargetSite)
                        {
                            //写 AGV_TargetSite=K2  值需要后期修改
                            WriteData("AGV_TargetSite", 200);
                            isSend_AgvTargetSite = true;
                        }
                    }
                }
                var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_K2")).FirstOrDefault();
                if (!Equals(v2, null))
                {
                    if (!Equals(v2.Value, null))
                    {
                        AGV_Arrived_K2 = bool.Parse(v2.Value.ToString());
                        if (AGV_Arrived_K2 && !isSend_TransLineK2Action)
                        {
                            //写 TransLineK2_Action=1， AGV_Roller2_Action=2
                            TransLineK2_Action = 1;
                            AGV_Roller2_Action = 2;
                            WriteData("TransLineK2_Action", TransLineK2_Action);
                            WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                            isSend_TransLineK2Action = true;
                        }
                    }
                }
                var v3 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Roller2_Arrived")).FirstOrDefault();
                if (!Equals(v3, null))
                {
                    if (!Equals(v3.Value, null))
                    {
                        AGV_Roller2_Arrived = bool.Parse(v3.Value.ToString());
                        if (AGV_Roller2_Arrived && !isSend_TransLineK2Stop)
                        {
                            //写 TransLineK2_Action=0， AGV_Roller2_Action=0   
                            TransLineK2_Action = 0;
                            AGV_Roller2_Action = 0;
                            WriteData("TransLineK2_Action", TransLineK2_Action);
                            WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                            MaterialOutStorageShowTask();  //执行原料自动出库-库存、日志等信息更新
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 执行原料出库操作
        /// </summary>
        private void MaterialOutStorageShowTask()
        {
            isSend_TransLineK2Stop = true;
            MaterialOutStorageInfoModel outStorageinfo = MaterialOutStorageInfoList.FirstOrDefault();
            outStorageinfo.MaterialBatchs = MaterialBatchInfoDataSource;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MaterialOutStorageInfo/MaterialOutStorageShowTask",
               Utility.JsonHelper.ToJson(new List<MaterialOutStorageInfoModel> { outStorageinfo }));
            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MaterialOutStorageInfoModel>(outStorageinfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
                getMaterialOutStorageInfoPageData(1, 1);
                if (MaterialOutStorageInfoList.Count > 0)
                {
                    GetPageDataMaterialBatch(1, 200);
                }
                ActionProcess = 2;
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CommandManager.InvalidateRequerySuggested();
                }));
                string tip = (isAllAction ? "全程-" : "分步-");
                Application.Current.Resources["UiMessage"] = tip + "原料出库完成，进入下一步！";
            }
            else
            {
                //操作失败，显示错误信息
                //EnterpriseInfoList = new ObservableCollection<EnterpriseModel>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            }
        }
        #endregion


        #region 转送至加工岛1操作
        /// <summary>
        /// 转送至加工岛1操作
        /// </summary>
        private void SetToWorkIsland1()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 3)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_D1")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        AGV_Arrived_D1 = bool.Parse(v1.Value.ToString());
                        if (AGV_Arrived_D1)
                        {
                            ActionProcess = 4;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CommandManager.InvalidateRequerySuggested();
                            }));
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = tip + "转运至加工岛1完成，进入下一步！";
                        }
                    }
                }
            }
        }
        #endregion
        #region 加工岛1上料操作
        /// <summary>
        /// 加工岛1上料操作
        /// </summary>
        private void SetIsland1On()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 5)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineD1_Arrived")).FirstOrDefault();
                var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_D1")).FirstOrDefault();
                if (!Equals(v2, null))
                {
                    if (!Equals(v2.Value, null))
                    {
                        AGV_Arrived_D1 = bool.Parse(v2.Value.ToString());
                    }
                }
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        TransLineD1_Arrived = bool.Parse(v1.Value.ToString());
                        if (!TransLineD1_Arrived && AGV_Arrived_D1 && !isSend_TransLineD1Action)
                        {
                            //写 AGV_Roller2_Action=1,TransLineD1_Action=2
                            AGV_Roller2_Action = 1;
                            TransLineD1_Action = 2;
                            WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                            WriteData("TransLineD1_Action", TransLineD1_Action);
                            isSend_TransLineD1Action = true;
                        }
                    }
                }
                if (TransLineD1_Arrived && isSend_TransLineD1Action && !isSend_AGVRoller2Stop)
                {
                    //写 AGV_Roller2_Action=0 
                    AGV_Roller2_Action = 0;
                    WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                    isSend_AGVRoller2Stop = true;
                    ActionProcess = 6;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CommandManager.InvalidateRequerySuggested();
                    }));
                    string tip = (isAllAction ? "全程-" : "分步-");
                    Application.Current.Resources["UiMessage"] = tip + "加工岛1上料完成，进入下一步！";
                }
            }
        }
        #endregion
        #region 加工岛1生产操作
        /// <summary>
        /// 加工岛1生产操作
        /// </summary>
        private void SetIsland1Working()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 7)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("Robot1_WorkFinished")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        Robot1_WorkFinished = bool.Parse(v1.Value.ToString());
                        if (Robot1_WorkFinished)
                        {
                            ActionProcess = 8;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CommandManager.InvalidateRequerySuggested();
                            }));
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = tip + "加工岛1生产完成，进入下一步！";
                        }
                    }
                }
            }
        }
        #endregion
        #region 加工岛1下料操作
        /// <summary>
        /// 加工岛1下料操作
        /// </summary>
        private void SetIsland1Off()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 9)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineD1_Arrived")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        TransLineD1_Arrived = bool.Parse(v1.Value.ToString());
                        if (TransLineD1_Arrived && !isSend_TransLineD1Action1)
                        {
                            //写 TransLineD1_Action=1， AGV_Roller2_Action=2
                            TransLineD1_Action = 1;
                            AGV_Roller2_Action = 2;
                            WriteData("TransLineD1_Action", TransLineD1_Action);
                            WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                            isSend_TransLineD1Action1 = true;
                        }
                    }
                }


                var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Roller2_Arrived")).FirstOrDefault();
                if (!Equals(v2, null))
                {
                    if (!Equals(v2.Value, null))
                    {
                        AGV_Roller2_Arrived = bool.Parse(v2.Value.ToString());
                        if (AGV_Roller2_Arrived && !isSend_AGVRoller2Stop1 && isSend_TransLineD1Action1)
                        {
                            //写 TransLineD1_Action=0， AGV_Roller2_Action=0,AGV_TargetSite=D2 目标地址后期修改
                            TransLineD1_Action = 0;
                            AGV_Roller2_Action = 0;
                            AGV_TargetSite = 400;
                            WriteData("TransLineD1_Action", TransLineD1_Action);
                            WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                            WriteData("AGV_TargetSite", AGV_TargetSite);
                            isSend_AGVRoller2Stop1 = true;
                        }
                    }
                }

                var v3 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_D2")).FirstOrDefault();
                if (!Equals(v3, null))
                {
                    if (!Equals(v3.Value, null))
                    {
                        AGV_Arrived_D2 = bool.Parse(v3.Value.ToString());
                    }
                }
                var v4 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineD2_Arrived")).FirstOrDefault();
                if (!Equals(v4, null))
                {
                    if (!Equals(v4.Value, null))
                    {
                        TransLineD2_Arrived = bool.Parse(v4.Value.ToString());
                    }
                }
                if (AGV_Arrived_D2 && TransLineD2_Arrived && isSend_AGVRoller2Stop1 && !isSend_TransLineD2Action)
                {
                    //写 TransLineD2_Action=1， AGV_Roller1_Action=2
                    TransLineD2_Action = 1;
                    AGV_Roller1_Action = 2;
                    WriteData("TransLineD2_Action", TransLineD2_Action);
                    WriteData("AGV_Roller1_Action", AGV_Roller1_Action);
                    isSend_TransLineD2Action = true;
                }
                var v5 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Roller1_Arrived")).FirstOrDefault();
                if (!Equals(v5, null))
                {
                    if (!Equals(v5.Value, null))
                    {
                        AGV_Roller1_Arrived = bool.Parse(v5.Value.ToString());
                        if (AGV_Roller1_Arrived && isSend_TransLineD2Action && !isSend_AGVRoller1Stop)
                        {
                            //写 TransLineD2_Action=0， AGV_Roller1_Action=0
                            TransLineD2_Action = 0;
                            AGV_Roller1_Action = 0;
                            WriteData("TransLineD2_Action", TransLineD2_Action);
                            WriteData("AGV_Roller1_Action", AGV_Roller1_Action);
                            isSend_AGVRoller1Stop = true;
                            ActionProcess = 10;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CommandManager.InvalidateRequerySuggested();
                            }));
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = tip + "加工岛1下料完成，进入下一步！";
                        }
                    }
                }

            }
        }
        #endregion
        #region 转运到加工岛2操作
        /// <summary>
        /// 转运到加工岛2操作
        /// </summary>
        private void SetToWorkIsland2()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 11)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_D3")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        AGV_Arrived_D3 = bool.Parse(v1.Value.ToString());
                        if (AGV_Arrived_D3)
                        {
                            ActionProcess = 12;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CommandManager.InvalidateRequerySuggested();
                            }));
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = tip + "转运到加工岛2完成，进入下一步！";
                        }
                    }
                }
            }
        }
        #endregion
        #region 加工岛2上料操作
        /// <summary>
        /// 加工岛2上料操作
        /// </summary>
        private void SetIsland2On()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 13)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineD3_Arrived")).FirstOrDefault();
                var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_D3")).FirstOrDefault();
                var v3 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineD4_Arrived")).FirstOrDefault();
                var v4 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_D4")).FirstOrDefault();
                if (!Equals(v2, null))
                {
                    if (!Equals(v2.Value, null))
                    {
                        AGV_Arrived_D3 = bool.Parse(v2.Value.ToString());
                    }
                }
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        TransLineD3_Arrived = bool.Parse(v1.Value.ToString());
                        if (!TransLineD3_Arrived && AGV_Arrived_D3 && !isSend_TransLineD3Action)
                        {
                            //写 AGV_Roller1_Action=1,TransLineD3_Action=2
                            AGV_Roller1_Action = 1;
                            TransLineD3_Action = 2;
                            WriteData("AGV_Roller1_Action", AGV_Roller1_Action);
                            WriteData("TransLineD3_Action", TransLineD3_Action);
                            isSend_TransLineD3Action = true;
                        }
                    }
                }
                if (TransLineD3_Arrived && isSend_TransLineD3Action && !isSend_AGVRoller1Stop1)
                {
                    //写 AGV_Roller1_Action=0 AGV_TargetSite=D4
                    AGV_Roller1_Action = 0;
                    AGV_TargetSite = 500;
                    WriteData("AGV_Roller1_Action", AGV_Roller1_Action);
                    WriteData("AGV_TargetSite", AGV_TargetSite);
                    isSend_AGVRoller1Stop1 = true;
                }
                if (!Equals(v4, null))
                {
                    if (!Equals(v4.Value, null))
                    {
                        AGV_Arrived_D4 = bool.Parse(v4.Value.ToString());
                    }
                }
                if (!Equals(v3, null))
                {
                    if (!Equals(v3.Value, null))
                    {
                        TransLineD4_Arrived = bool.Parse(v3.Value.ToString());
                        if (!TransLineD4_Arrived && AGV_Arrived_D4 && !isSend_TransLineD4Action)
                        {
                            //写 AGV_Roller2_Action=1,TransLineD4_Action=2
                            AGV_Roller2_Action = 1;
                            TransLineD4_Action = 2;
                            WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                            WriteData("TransLineD4_Action", TransLineD4_Action);
                            isSend_TransLineD4Action = true;
                        }
                    }
                }
                if (TransLineD4_Arrived && isSend_TransLineD4Action && !isSend_AGVRoller2Stop2)
                {
                    //写 AGV_Roller2_Action=0 
                    AGV_Roller2_Action = 0;
                    WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                    isSend_AGVRoller2Stop2 = true;
                    ActionProcess = 14;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CommandManager.InvalidateRequerySuggested();
                    }));
                    string tip = (isAllAction ? "全程-" : "分步-");
                    Application.Current.Resources["UiMessage"] = tip + "加工岛2上料完成，进入下一步！";
                }
            }

        }
        #endregion
        #region 加工岛2生产操作
        /// <summary>
        /// 加工岛2上料操作
        /// </summary>
        private void SetIsland2Working()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 15)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("Robot2_WorkFinished")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        Robot2_WorkFinished = bool.Parse(v1.Value.ToString());
                        if (Robot2_WorkFinished)
                        {
                            ActionProcess = 16;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CommandManager.InvalidateRequerySuggested();
                            }));
                            string tip = (isAllAction ? "全程-" : "分步-");
                            Application.Current.Resources["UiMessage"] = tip + "加工岛2生产完成，进入下一步！";
                        }
                    }
                }
            }
        }
        #endregion
        #region 加工岛2下料操作
        /// <summary>
        /// 加工岛2下料操作
        /// </summary>
        private void SetIsland2Off()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 17)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineD4_Arrived")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        TransLineD4_Arrived = bool.Parse(v1.Value.ToString());
                    }
                }

                if (TransLineD4_Arrived && !isSend_TransLineD4Action1)
                {
                    //写 TransLineD4_Action=1， AGV_Roller2_Action=2
                    TransLineD4_Action = 1;
                    AGV_Roller2_Action = 2;
                    WriteData("TransLineD4_Action", TransLineD4_Action);
                    WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                    isSend_TransLineD4Action1 = true;
                }
                var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Roller2_Arrived")).FirstOrDefault();
                if (!Equals(v2, null))
                {
                    if (!Equals(v2.Value, null))
                    {
                        AGV_Roller2_Arrived = bool.Parse(v2.Value.ToString());
                    }
                }
                if (AGV_Roller2_Arrived && !isSend_AGVRoller2Stop3 && isSend_TransLineD4Action1)
                {
                    //写 TransLineD4_Action=0， AGV_Roller2_Action=0,AGV_TargetSite=D3 目标地址后期修改
                    TransLineD4_Action = 0;
                    AGV_Roller2_Action = 0;
                    AGV_TargetSite = 600;
                    WriteData("TransLineD4_Action", TransLineD4_Action);
                    WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                    WriteData("AGV_TargetSite", AGV_TargetSite);
                    isSend_AGVRoller2Stop3 = true;
                }
                var v3 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_D3")).FirstOrDefault();
                if (!Equals(v3, null))
                {
                    if (!Equals(v3.Value, null))
                    {
                        AGV_Arrived_D3 = bool.Parse(v3.Value.ToString());
                    }
                }
                var v4 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineD3_Arrived")).FirstOrDefault();
                if (!Equals(v4, null))
                {
                    if (!Equals(v4.Value, null))
                    {
                        TransLineD3_Arrived = bool.Parse(v4.Value.ToString());
                    }
                }
                if (AGV_Arrived_D3 && TransLineD3_Arrived && isSend_AGVRoller2Stop3 && !isSend_TransLineD3Action1)
                {
                    //写 TransLineD3_Action=1， AGV_Roller1_Action=2
                    TransLineD3_Action = 1;
                    AGV_Roller1_Action = 2;
                    WriteData("TransLineD3_Action", TransLineD3_Action);
                    WriteData("AGV_Roller1_Action", AGV_Roller1_Action);
                    isSend_TransLineD3Action1 = true;
                }
                var v5 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Roller1_Arrived")).FirstOrDefault();
                if (!Equals(v5, null))
                {
                    if (!Equals(v5.Value, null))
                    {
                        AGV_Roller1_Arrived = bool.Parse(v5.Value.ToString());
                    }
                }
                if (AGV_Roller1_Arrived && isSend_TransLineD3Action1 && !isSend_AGVRoller1Stop2)
                {
                    //写 TransLineD3_Action=0， AGV_Roller1_Action=0
                    TransLineD3_Action = 0;
                    AGV_Roller1_Action = 0;
                    WriteData("TransLineD3_Action", TransLineD3_Action);
                    WriteData("AGV_Roller1_Action", AGV_Roller1_Action);
                    isSend_AGVRoller1Stop2 = true;
                    ActionProcess = 18;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CommandManager.InvalidateRequerySuggested();
                    }));
                    string tip = (isAllAction ? "全程-" : "分步-");
                    Application.Current.Resources["UiMessage"] = tip + "加工岛2下料完成，进入下一步！";
                }
            }

        }
        #endregion
        #region 成品回库操作
        /// <summary>
        /// 成品回库操作
        /// </summary>
        private void SetProductIn()
        {
            if (ClientDataEntities.Count > 0 && ActionProcess == 19)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_D2")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        AGV_Arrived_D2 = bool.Parse(v1.Value.ToString());
                    }
                }
                var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineD2_Arrived")).FirstOrDefault();
                var v3 = ClientDataEntities.Where(d => d.NodeId.Contains("AGV_Arrived_K3")).FirstOrDefault();
                var v4 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineK3_Arrived")).FirstOrDefault();
                if (!Equals(v2, null))
                {
                    if (!Equals(v2.Value, null))
                    {
                        TransLineD2_Arrived = bool.Parse(v2.Value.ToString());
                        if (AGV_Arrived_D2 && !TransLineD2_Arrived && !isSend_TransLineD2Action1)
                        {
                            //写 TransLineD2_Action=2， AGV_Roller1_Action=1
                            TransLineD2_Action = 2;
                            AGV_Roller1_Action = 1;
                            WriteData("TransLineD2_Action", TransLineD2_Action);
                            WriteData("AGV_Roller1_Action", AGV_Roller1_Action);
                            isSend_TransLineD2Action1 = true;
                        }
                        if (TransLineD2_Arrived && isSend_TransLineD2Action1 && !isSend_AGVRoller1Stop3)
                        {
                            //写 AGV_Roller1_Action=0， AGV_TargetSite=K3 目标地址后期修改
                            AGV_Roller1_Action = 0;
                            AGV_TargetSite = 300;
                            WriteData("AGV_Roller1_Action", AGV_Roller1_Action);
                            WriteData("AGV_TargetSite", AGV_TargetSite);
                            isSend_AGVRoller1Stop3 = true;
                        }
                        if (!Equals(v3, null))
                        {
                            if (!Equals(v3.Value, null))
                            {
                                AGV_Arrived_K3 = bool.Parse(v3.Value.ToString());
                                if (!Equals(v4, null))
                                {
                                    if (!Equals(v4.Value, null))
                                    {
                                        TransLineK3_Arrived = bool.Parse(v4.Value.ToString());
                                    }
                                }
                                if (AGV_Arrived_K3 && !TransLineK3_Arrived && !isSend_TransLineK3Action)
                                {
                                    //写 TransLineK3_Action=2， AGV_Roller2_Action=1
                                    TransLineK3_Action = 2;
                                    AGV_Roller2_Action = 1;
                                    WriteData("TransLineK3_Action", TransLineK3_Action);
                                    WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                                    isSend_TransLineK3Action = true;
                                }
                                if (isSend_TransLineK3Action && TransLineK3_Arrived && !isSend_AGVRoller2Stop4)
                                {
                                    //写 AGV_Roller2_Action=0
                                    AGV_Roller2_Action = 0;
                                    WriteData("AGV_Roller2_Action", AGV_Roller2_Action);
                                    isSend_AGVRoller2Stop4 = true;
                                }
                                if (isSend_AGVRoller2Stop4 && !isSend_Location)
                                {
                                    var v5 = ClientDataEntities.Where(d => d.NodeId.Contains("Pallet_Code_K3")).FirstOrDefault();
                                    if (!Equals(v5, null))
                                    {
                                        if (!Equals(v5.Value, null))
                                        {
                                            Pallet_Code_K3 = int.Parse(v5.Value.ToString());
                                            //获取成品空库位
                                            getPageDataMatWareHouseLocation(1, 1);
                                            if (MatWareHouseLocationInfoList.Count > 0)
                                            {
                                                string locationcode = MatWareHouseLocationInfoList.FirstOrDefault().WareHouseLocationCode;
                                                MatWareHouse_Location = int.Parse(locationcode.Substring(locationcode.Length - 2, 2));
                                            }
                                            else
                                            {
                                                MatWareHouse_Location = 10;
                                            }
                                            MatStorage_Type = 3;
                                            WriteData("MatWareHouse_Location", MatWareHouse_Location);
                                            WriteData("MatStorage_Type", MatStorage_Type);
                                            isSend_Location = true;
                                        }
                                    }

                                }
                                if (isSend_Location && !isUpdateProductInStorage)
                                {
                                    var v6 = ClientDataEntities.Where(d => d.NodeId.Contains("ProductInStorage_Finished")).FirstOrDefault();
                                    if (!Equals(v6, null))
                                    {
                                        if (!Equals(v6.Value, null))
                                        {
                                            ProductInStorage_Finished = bool.Parse(v6.Value.ToString());
                                            if (ProductInStorage_Finished)
                                            {
                                                //校验托盘编号与原料出库的编号是否一致 更新成品入库 库存等信息
                                                ProductInStorageShowTask();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
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
        /// <summary>
        /// 执行成品回库操作
        /// </summary>
        private void ProductInStorageShowTask()
        {
            isUpdateProductInStorage = true;
            MaterialInStorageInfoModel inStorageinfo = new MaterialInStorageInfoModel();
            inStorageinfo.PalletCode = Pallet_Code_K3.ToString();
            inStorageinfo.Quantity = MaterialOutStorageInfo.Quantity;
            inStorageinfo.PalletID = batchinfo.Pallet_Id;
            inStorageinfo.MatWareHouseLocations = MatWareHouseLocationInfoList;
            var result = Utility.Http.HttpClientHelper.PostResponse<OperationResult>(GlobalData.ServerRootUri + "MaterialInStorageInfo/ProductInStorageShowTask",
               Utility.JsonHelper.ToJson(new List<MaterialInStorageInfoModel> { inStorageinfo }));
            if (!Equals(result, null) && result.Successed)
            {
                Application.Current.Resources["UiMessage"] = result?.Message;
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
                Messenger.Default.Send<MaterialInStorageInfoModel>(inStorageinfo, MessengerToken.DataChanged);
                Messenger.Default.Send<bool>(true, MessengerToken.ClosePopup);
                InitParaMeters();
                Task.Factory.StartNew(new Action(init));
                string tip = (isAllAction ? "全程-" : "分步-");
                Application.Current.Resources["UiMessage"] = tip + "成品回库完成！";
            }
            else
            {
                //操作失败，显示错误信息
                Application.Current.Resources["UiMessage"] = result?.Message ?? "操作失败，请联系管理员！";
                LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            }
        }

        private void InitParaMeters()
        {
            TransLineK2_Arrived = false;
            MatWareHouse_Location = 0;
            MatStorage_Type = 2;
            AGV_TargetSite = 0;
            AGV_Arrived_K2 = false;
            TransLineK2_Action = 0;
            AGV_Roller2_Action = 0;
            AGV_Roller2_Arrived = false;
            isSend_AgvTargetSite = false;
            isSend_TransLineK2Action = false;
            isSend_TransLineK2Stop = false;
            AGV_Arrived_D1 = false;
            TransLineD1_Action = 0;
            TransLineD1_Arrived = false;
            isSend_TransLineD1Action = false;
            isSend_AGVRoller2Stop = false;
            Robot1_WorkFinished = false;
            AGV_Arrived_D2 = false;
            TransLineD2_Action = 0;
            TransLineD2_Arrived = false;
            AGV_Roller1_Action = 0;
            AGV_Roller1_Arrived = false;
            isSend_TransLineD1Action1 = false;
            isSend_AGVRoller2Stop1 = false;
            isSend_TransLineD2Action = false;
            isSend_AGVRoller1Stop = false;
            AGV_Arrived_D3 = false;
            TransLineD3_Action = 0;
            TransLineD3_Arrived = false;
            AGV_Arrived_D4 = false;
            TransLineD4_Action = 0;
            TransLineD4_Arrived = false;
            isSend_TransLineD3Action = false;
            isSend_AGVRoller1Stop1 = false;
            isSend_TransLineD4Action = false;
            isSend_AGVRoller2Stop2 = false;
            Robot2_WorkFinished = false;
            isSend_TransLineD4Action1 = false;
            isSend_AGVRoller2Stop3 = false;
            isSend_TransLineD3Action1 = false;
            isSend_AGVRoller1Stop2 = false;
            AGV_Arrived_K3 = false;
            TransLineK3_Action = 0;
            TransLineK3_Arrived = false;
            Pallet_Code_K3 = 0;
            ProductInStorage_Finished = false;
            isSend_TransLineD2Action1 = false;
            isSend_AGVRoller1Stop3 = false;
            isSend_TransLineK3Action = false;
            isSend_AGVRoller2Stop4 = false;
            isSend_Location = false;
            isUpdateProductInStorage = false;
            ActionProcess = 0;
            StepActionCode = "StepAction_MaterialOutStorageShow";
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
           {
               //MaterialOutStorageInfoList?.Clear();
               MaterialBatchInfoList?.Clear();
               MaterialBatchInfoDataSource?.Clear();
               MatWareHouseLocationInfoList?.Clear();
           }));
            isAllAction = false;
            isClickMaterialOutCommand = false;
            isClickToWorkIsland1Command = false;
            isClickIsland1OnCommand = false;
            isClickIsland1WorkingCommand = false;
            isClickIsland1OffCommand = false;
            isClickToWorkIsland2Command = false;
            isClickIsland2OnCommand = false;
            isClickIsland2WorkingCommand = false;
            isClickIsland2OffCommand = false;
            isClickProductInCommand = false;

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
        #endregion
        System.Threading.Timer timer;
        public Agv3DAnimationViewModel()
        {
            timer = new System.Threading.Timer(new System.Threading.TimerCallback(timercallback), null, 1000, 50);
            Task.Factory.StartNew(new Action(init));
        }
        #region 原料出库-采集数据初始化
        private bool TransLineK2_Arrived = false;
        private int MatWareHouse_Location = 0;
        private int MatStorage_Type = 2;
        private int AGV_TargetSite;
        private bool AGV_Arrived_K2 = false;
        private int TransLineK2_Action = 0;
        private int AGV_Roller2_Action = 0;
        private bool AGV_Roller2_Arrived = false;
        private bool isSend_AgvTargetSite = false;
        private bool isSend_TransLineK2Action = false;
        private bool isSend_TransLineK2Stop = false;
        #endregion
        #region 转运至加工岛1-采集数据
        private bool AGV_Arrived_D1 = false;
        #endregion
        #region 加工岛1上料-采集数据
        private int TransLineD1_Action = 0;
        private bool TransLineD1_Arrived = false;
        private bool isSend_TransLineD1Action = false;
        private bool isSend_AGVRoller2Stop = false;
        #endregion
        #region 加工岛1生产-采集数据
        private bool Robot1_WorkFinished = false;
        #endregion
        #region 加工岛1下料-采集数据
        private bool AGV_Arrived_D2 = false;
        private int TransLineD2_Action = 0;
        private bool TransLineD2_Arrived = false;
        private int AGV_Roller1_Action = 0;
        private bool AGV_Roller1_Arrived = false;
        private bool isSend_TransLineD1Action1 = false;
        private bool isSend_AGVRoller2Stop1 = false;
        private bool isSend_TransLineD2Action = false;
        private bool isSend_AGVRoller1Stop = false;
        #endregion
        #region 转运到加工岛2-采集数据
        private bool AGV_Arrived_D3 = false;
        #endregion
        #region 加工岛2上料-采集数据
        private int TransLineD3_Action = 0;
        private bool TransLineD3_Arrived = false;
        private bool AGV_Arrived_D4 = false;
        private int TransLineD4_Action = 0;
        private bool TransLineD4_Arrived = false;
        private bool isSend_TransLineD3Action = false;
        private bool isSend_AGVRoller1Stop1 = false;
        private bool isSend_TransLineD4Action = false;
        private bool isSend_AGVRoller2Stop2 = false;
        #endregion
        #region 加工岛2生产-采集数据
        private bool Robot2_WorkFinished = false;
        #endregion
        #region 加工岛2下料-采集数据
        private bool isSend_TransLineD4Action1 = false;
        private bool isSend_AGVRoller2Stop3 = false;
        private bool isSend_TransLineD3Action1 = false;
        private bool isSend_AGVRoller1Stop2 = false;
        #endregion
        #region 成品回库-采集数据
        private bool AGV_Arrived_K3 = false;
        private int TransLineK3_Action = 0;
        private bool TransLineK3_Arrived = false;
        private int Pallet_Code_K3 = 0;
        private bool ProductInStorage_Finished = false;
        private bool isSend_TransLineD2Action1 = false;
        private bool isSend_AGVRoller1Stop3 = false;
        private bool isSend_TransLineK3Action = false;
        private bool isSend_AGVRoller2Stop4 = false;
        private bool isSend_Location = false;
        private bool isUpdateProductInStorage = false;
        #endregion
        #region 全程
        private bool isAllAction = false;
        private bool isClickMaterialOutCommand = false;
        private bool isClickToWorkIsland1Command = false;
        private bool isClickIsland1OnCommand = false;
        private bool isClickIsland1WorkingCommand = false;
        private bool isClickIsland1OffCommand = false;
        private bool isClickToWorkIsland2Command = false;
        private bool isClickIsland2OnCommand = false;
        private bool isClickIsland2WorkingCommand = false;
        private bool isClickIsland2OffCommand = false;
        private bool isClickProductInCommand = false;
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
        private void init()
        {
            initCommand();
            minX = 2;
            minY = 3;
            SocketInit();
            getMaterialOutStorageInfoPageData(1, 1);
            if (MaterialOutStorageInfoList.Count > 0)
            {
                GetPageDataMaterialBatch(1, 200);
            }
            ClientDataEntities = new ObservableCollection<ClientDataEntity>(getClientDataEntities(1, 100, StepActionCode));
            this.client.Send(GetMessage(JsonHelper.ToJson(ClientDataEntities)));
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
                        ClientDataEntities.FunctionCode = FuncCode.SubScription;
                        ClientDataEntities.ProductionProcessEquipmentBusinessNodeMapId = data.Id;
                        ClientDataEntities.StatusCode = 0;
                        ClientDataEntities.NodeId = data.NodeUrl;
                        ClientDataEntities.ValueType = Type.GetType("System." + data.DataType);
                        results.Add(ClientDataEntities);
                    }
                    if (StepActionCode == "StepAction_MaterialOutStorageShow")
                    {
                        Application.Current.Resources["UiMessage"] = "准备执行原料出库！";
                    }
                    if (StepActionCode == "StepAction_ToWorkIsland1")
                    {
                        ActionProcess = 3;
                        string tip = (isAllAction ? "全程-" : "分步-");
                        Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "转运至加工岛1";
                    }
                    if (StepActionCode == "StepAction_Island1On")
                    {
                        ActionProcess = 5;
                        string tip = (isAllAction ? "全程-" : "分步-");
                        Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛1上料";
                    }
                    if (StepActionCode == "StepAction_Island1Working")
                    {
                        ActionProcess = 7;
                        string tip = (isAllAction ? "全程-" : "分步-");
                        Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛1生产";
                    }
                    if (StepActionCode == "StepAction_Island1Off")
                    {
                        ActionProcess = 9;
                        string tip = (isAllAction ? "全程-" : "分步-");
                        Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛1下料";
                    }
                    if (StepActionCode == "StepAction_ToWorkIsland2")
                    {
                        ActionProcess = 11;
                        string tip = (isAllAction ? "全程-" : "分步-");
                        Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "转运至加工岛2";
                    }
                    if (StepActionCode == "StepAction_Island2On")
                    {
                        ActionProcess = 13;
                        string tip = (isAllAction ? "全程-" : "分步-");
                        Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛2上料";
                    }
                    if (StepActionCode == "StepAction_Island2Working")
                    {
                        ActionProcess = 15;
                        string tip = (isAllAction ? "全程-" : "分步-");
                        Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛2生产";
                    }
                    if (StepActionCode == "StepAction_Island2Off")
                    {
                        ActionProcess = 17;
                        string tip = (isAllAction ? "全程-" : "分步-");
                        Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "加工岛2下料";
                    }
                    if (StepActionCode == "StepAction_ProductInStorage")
                    {
                        ActionProcess = 19;
                        string tip = (isAllAction ? "全程-" : "分步-");
                        Application.Current.Resources["UiMessage"] = "正在执行:" + tip + "成品回库";
                    }
                    return results;
                }
                else
                {
                    //ClientDataEntity?.Clear();
                    Application.Current.Resources["UiMessage"] = "未找到数据";
                    return null;

                }
            }
            else
            {
                //操作失败，显示错误信息
                //ClientDataEntity = new ObservableCollection<ClientDataEntity>();
                Application.Current.Resources["UiMessage"] = result?.Message ?? "查询数据点列表失败，请联系管理员！";
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

        string pathData = "M50,50 1000,50 C 1050,60 1100,500 1000,800 M 1000,800 50,800 ";
        public Canvas mainContent = new Canvas() { Background = Utility.Windows.ResourceHelper.FindResource("WorkshopBackBrush1") as Brush };
        public Canvas MainContent
        {
            get { return mainContent; }
            set { Set(ref mainContent, value); }
        }


        ~Agv3DAnimationViewModel()
        {
        }


        #region 命令定义和初始化

        /// <summary>
        /// 新增命令
        /// </summary>
        public ICommand ConnectCommand { get; set; }

        public ICommand StopCommand { get; set; }

        public ICommand SizeChangedCommand { get; set; }

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

        private void initCommand()
        {
            ConnectCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteConnectCommand, OnCanExecuteConnectCommand);
            StopCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteStopCommand);
            SizeChangedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteSizeChangedCommand);
            RefreshAllCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(OnExecuteRefreshAllCommand);
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
            return true;
        }

        private void WriteData(string NodeId, int value)
        {
            var clientDataEntities = ClientDataEntities.Where(a => a.NodeId.Contains(NodeId)).FirstOrDefault();
            //  var clientDataentity = clientDataEntities.FirstOrDefault().Clone() as ClientDataEntity;

            List<ClientDataEntity> clientDataEntitiesSend = new List<ClientDataEntity>(1);
            clientDataEntities.FunctionCode = FuncCode.Write;
            clientDataEntities.Value = (UInt16)value;

            clientDataEntitiesSend.Add(clientDataEntities);
            this.client.Send(GetMessage(JsonHelper.ToJson(clientDataEntitiesSend)));
        }
        private int ActionProcess = 0;
        /// <summary>
        /// 执行原料出库命令
        /// </summary>
        private void OnExecuteMaterialOutCommand()
        {
            if (MaterialOutStorageInfoList.Count > 0)
            {
                MaterialOutStorageInfo = MaterialOutStorageInfoList.FirstOrDefault();
                if (MaterialOutStorageInfo.Quantity != MaterialBatchInfoDataSource.FirstOrDefault().Quantity)
                {
                    Application.Current.Resources["UiMessage"] = "选择库位库存与出库单数量不一致！";
                    isAllAction = false;
                    isClickMaterialOutCommand = false;
                    return;
                }
                string locationcode = MaterialBatchInfoDataSource.FirstOrDefault().WareHouseLocationCode;
                MatWareHouse_Location = int.Parse(locationcode.Substring(locationcode.Length - 2, 2));
                batchinfo = MaterialBatchInfoDataSource.FirstOrDefault();
                try
                {
                    WriteData("MatWareHouse_Location", MatWareHouse_Location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                MatStorage_Type = 2;
                WriteData("MatStorage_Type", MatStorage_Type);
                ActionProcess = 1;
                Application.Current.Resources["UiMessage"] = "正在执行:原料出库";
            }
            else
            {
                isAllAction = false;
                isClickMaterialOutCommand = false;
                Application.Current.Resources["UiMessage"] = "请先填写原料出库单！";
                return;
            }
            //  MessageBox.Show("MatWareHouse_Location：" + MaterialBatchInfoDataSource.FirstOrDefault().WareHouseLocationCode);
            //写PLC  MatWareHouse_Location=选择的库位号，MatStorage_Type=2
        }
        /// <summary>
        /// 是否可以执行原料出库命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteMaterialOutCommand()
        {
            return ActionProcess == 0 && MaterialBatchInfoDataSource.Count > 0 ? true : false;
        }
        /// <summary>
        /// 执行去加工岛1命令
        /// </summary>
        private void OnExecuteToWorkIsland1Command()
        {
            StepActionCode = "StepAction_ToWorkIsland1";
            Task.Factory.StartNew(new Action(init));
            //写AGV AGV_TargetSite=D1 AGV目标地址后期需要修改
            AGV_TargetSite = 300;
            WriteData("AGV_TargetSite", AGV_TargetSite);
        }
        /// <summary>
        /// 是否可以执行去加工岛1命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteToWorkIsland1Command()
        {
            return (ActionProcess == 2 && !isAllAction) ? true : false;
        }
        /// <summary>
        /// 执行加工岛1上料命令
        /// </summary>
        private void OnExecuteIsland1OnCommand()
        {
            StepActionCode = "StepAction_Island1On";
            Task.Factory.StartNew(new Action(init));
        }
        /// <summary>
        /// 是否可以执行加工岛1上料命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland1OnCommand()
        {
            return (ActionProcess == 4 && !isAllAction) ? true : false;
        }
        /// <summary>
        /// 执行加工岛1生产命令
        /// </summary>
        private void OnExecuteIsland1WorkingCommand()
        {
            StepActionCode = "StepAction_Island1Working";
            Task.Factory.StartNew(new Action(init));
            // 给Robot1发送TransLineD1_Arrived = 1信号(加工岛1开始加工)
        }
        /// <summary>
        /// 是否可以执行加工岛1生产命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland1WorkingCommand()
        {
            return (ActionProcess == 6 && !isAllAction) ? true : false;
        }
        /// <summary>
        /// 执行加工岛1下料命令
        /// </summary>
        private void OnExecuteIsland1OffCommand()
        {

            StepActionCode = "StepAction_Island1Off";
            Task.Factory.StartNew(new Action(init));
        }
        /// <summary>
        /// 是否可以执行加工岛1下料命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland1OffCommand()
        {
            return (ActionProcess == 8 && !isAllAction) ? true : false;
        }
        /// <summary>
        /// 执行转运到加工岛2命令
        /// </summary>
        private void OnExecuteToWorkIsland2Command()
        {
            StepActionCode = "StepAction_ToWorkIsland2";
            Task.Factory.StartNew(new Action(init));
            //写AGV AGV_TargetSite=D3 AGV目标地址后期需要修改
            AGV_TargetSite = 500;
            WriteData("AGV_TargetSite", AGV_TargetSite);
        }
        /// <summary>
        /// 是否可以执行转运到加工岛2命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteToWorkIsland2Command()
        {
            return (ActionProcess == 10 && !isAllAction) ? true : false;
        }
        /// <summary>
        /// 执行加工岛2上料命令
        /// </summary>
        private void OnExecuteIsland2OnCommand()
        {
            StepActionCode = "StepAction_Island2On";
            Task.Factory.StartNew(new Action(init));
        }
        /// <summary>
        /// 是否可以执行加工岛2上料命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland2OnCommand()
        {
            return (ActionProcess == 12 && !isAllAction) ? true : false;
        }
        /// <summary>
        /// 执行加工岛2生产命令
        /// </summary>
        private void OnExecuteIsland2WorkingCommand()
        {
            StepActionCode = "StepAction_Island2Working";
            Task.Factory.StartNew(new Action(init));
        }
        /// <summary>
        /// 是否可以执行加工岛2生产命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland2WorkingCommand()
        {
            return (ActionProcess == 14 && !isAllAction) ? true : false;
        }
        /// <summary>
        /// 执行加工岛2下料命令
        /// </summary>
        private void OnExecuteIsland2OffCommand()
        {
            StepActionCode = "StepAction_Island2Off";
            Task.Factory.StartNew(new Action(init));
        }
        /// <summary>
        /// 是否可以执行加工岛2下料命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteIsland2OffCommand()
        {
            return (ActionProcess == 16 && !isAllAction) ? true : false;
        }
        /// <summary>
        /// 执行成品入库命令
        /// </summary>
        private void OnExecuteProductInCommand()
        {
            getPageDataMatWareHouseLocation(1, 200);
            if (MatWareHouseLocationInfoList.Count == 0)
            {
                Application.Current.Resources["UiMessage"] = "仓库中没有空库位的成品库位！";
                return;
            }
            StepActionCode = "StepAction_ProductInStorage";
            Task.Factory.StartNew(new Action(init));
            //写AGV AGV_TargetSite=D2 AGV目标地址后期需要修改
            AGV_TargetSite = 400;
            WriteData("AGV_TargetSite", AGV_TargetSite);
        }
        /// <summary>
        /// 是否可以执行成品入库命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteProductInCommand()
        {
            return (ActionProcess == 18 && !isAllAction) ? true : false;
        }

        /// <summary>
        /// 执行全程命令
        /// </summary>
        private void OnExecuteAllActionCommand()
        {
            isAllAction = true;
        }
        /// <summary>
        /// 是否可以执行全程命令
        /// </summary>
        /// <returns></returns>
        private bool OnCanExecuteAllActionCommand()
        {
            return ActionProcess == 0 && MaterialBatchInfoDataSource.Count > 0 ? true : false;
        }
        private void OnExecuteStopCommand()
        {

        }

        private void OnExecuteSizeChangedCommand()
        {
            ratioX = initRatioX();
            ratioY = initRatioY();
            offsetX = ratioX * minX - mainContent.ActualWidth * 0.1 / 2;
            offsetY = ratioY * minY - mainContent.ActualHeight * 0.1 / 2;
        }

        private void OnExecuteRefreshAllCommand()
        {
            Task.Factory.StartNew(new Action(init));
            OnExecuteSizeChangedCommand();

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
            if (isAllAction)
            {
                if (ActionProcess == 0 && !isClickMaterialOutCommand)
                {
                    isClickMaterialOutCommand = true;
                    OnExecuteMaterialOutCommand();
                }
                if (ActionProcess == 2 && !isClickToWorkIsland1Command)
                {
                    isClickToWorkIsland1Command = true;
                    OnExecuteToWorkIsland1Command();
                }
                if (ActionProcess == 4 && !isClickIsland1OnCommand)
                {
                    isClickIsland1OnCommand = true;
                    OnExecuteIsland1OnCommand();
                }
                if (ActionProcess == 6 && !isClickIsland1WorkingCommand)
                {
                    isClickIsland1WorkingCommand = true;
                    OnExecuteIsland1WorkingCommand();
                }
                if (ActionProcess == 8 && !isClickIsland1OffCommand)
                {
                    isClickIsland1OffCommand = true;
                    OnExecuteIsland1OffCommand();
                }
                if (ActionProcess == 10 && !isClickToWorkIsland2Command)
                {
                    isClickToWorkIsland2Command = true;
                    OnExecuteToWorkIsland2Command();
                }
                if (ActionProcess == 12 && !isClickIsland2OnCommand)
                {
                    isClickIsland2OnCommand = true;
                    OnExecuteIsland2OnCommand();
                }
                if (ActionProcess == 14 && !isClickIsland2WorkingCommand)
                {
                    isClickIsland2WorkingCommand = true;
                    OnExecuteIsland2WorkingCommand();
                }
                if (ActionProcess == 16 && !isClickIsland2OffCommand)
                {
                    isClickIsland2OffCommand = true;
                    OnExecuteIsland2OffCommand();
                }
                if (ActionProcess == 18 && !isClickProductInCommand)
                {
                    isClickProductInCommand = true;
                    OnExecuteProductInCommand();
                }
            }
            SetAgvStackerPosition();
            SetMaterialOut();
            SetToWorkIsland1();
            SetIsland1On();
            SetIsland1Working();
            SetIsland1Off();
            SetToWorkIsland2();
            SetIsland2On();
            SetIsland2Working();
            SetIsland2Off();
            SetProductIn();
        }

        #endregion




        #region Agv信息


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
                    //RoleInfoList = new ObservableCollection<RoleModel>(result.Data.Data);
                    MaterialBatchInfoList = new ObservableCollection<MaterialBatchInfoModel>();
                    foreach (var data in result.Data.Data)
                    {
                        MaterialBatchInfoModel materialbatchinfo = new MaterialBatchInfoModel();
                        materialbatchinfo = data;

                        MaterialOutStorageInfoModel outinfo = MaterialOutStorageInfoList.FirstOrDefault();
                        if (materialbatchinfo.MaterialCode == outinfo.MaterialCode &&
                            materialbatchinfo.PalletCode != null &&
                            materialbatchinfo.WareHouseLocationType == WareHouseLocationTypeEnumModel.WareHouseLocationType.MaterialWareHouseLocationType)
                        {
                            materialbatchinfo.PropertyChanged += OnPropertyChangedCommand;
                            MaterialBatchInfoList.Add(materialbatchinfo);
                        }
                    }
                    int TotalCounts = result.Data.Total;
                }
                else
                {
                    MaterialBatchInfoList?.Clear();
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
#endregion