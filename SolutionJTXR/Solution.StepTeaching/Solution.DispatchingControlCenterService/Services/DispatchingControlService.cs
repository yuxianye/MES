using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using OSharp.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Solution.CommunicationModule.Contracts;
using Solution.Utility.Socket;
using Solution.TakeOutWarehouseManagement.Contracts;
using Solution.TakeOutWarehouseManagement.Dtos;
using Solution.TakeOutWarehouseManagement.Models;
using Solution.StoredInWarehouseManagement.Models;
using Solution.StoredInWarehouseManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Contracts;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Dtos;
using Solution.StereoscopicWarehouseManagement.Models;
using Solution.StereoscopicWarehouseManagement.Dtos;
using Solution.StereoscopicWarehouseManagement.Contracts;

namespace Solution.DispatchingControlCenterService
{
    /// <summary>
    /// 调度控制中心服务
    /// </summary>
    public class DispatchingControlService : IDispatchingControlContract
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(DispatchingControlService));

        /// <summary>
        /// 1、初始化socket客户端。
        /// 2、从业务点表查询本业务需要的点表，通过socket客户端注册相关的通讯点。
        /// 3、根据业务点变化控制/调度车辆、仓储库房出入库业务、设备Equipment资源状态等。
        /// </summary>
        public void Initialize()
        {
            bool socketClientResult = initSocketClient();
            Logger.Info($"DispatchingControlService.socketClientHelper连接结果：{socketClientResult}");

            bool registerNodeResult = registerNode();
            Logger.Info($"DispatchingControlService.registerNode：{registerNodeResult}");
            //  InitParaMeters();
            ActionProcess = 0;
            //isAllAction = false;
            WriteData("Storage_ActionProcess", ActionProcess);
            //WriteBoolData("StepAction_IsWholeCourse", isAllAction);

        }
        /// <summary>
        /// 业务点表契约
        /// </summary>
        public IProductionProcessEquipmentBusinessNodeMapContract ProductionProcessEquipmentBusinessNodeMapContract { get; set; }
        /// <summary>
        /// 出库契约
        /// </summary>
        public IMaterialOutStorageInfoContract MaterialOutStorageInfoContract { get; set; }
        public IMaterialBatchInfoContract MaterialBatchInfoContract { get; set; }
        public IMatWareHouseLocationInfoContract MatWareHouseLocationInfoContract { get; set; }
        public IMaterialInStorageInfoContract MaterialInStorageInfoContract { get; set; }
        public IMatStorageMoveInfoContract MatStorageMoveInfoContract { get; set; }
        public ISocketServerContract socketServerContract { get; set; }

        private Utility.Socket.SocketClientHelper socketClientHelper;
        /// <summary>
        /// 初始化socket客户端服务
        /// </summary>
        private bool initSocketClient()
        {
            var socketServerInfo = socketServerContract.SocketServerInfos.FirstOrDefault();

            if (Equals(socketServerInfo, null))
            {
                Logger.Info($"DispatchingControlService.initSocketClient,未找到SocketServerInfo的信息");

                return false;
            }
            socketClientHelper = new Utility.Socket.SocketClientHelper(socketServerInfo.ServerIp, socketServerInfo.ServerPort);
            var result = socketClientHelper.ConnectAsync().Result;
            socketClientHelper.Connected += SocketClientHelper_Connected;
            socketClientHelper.Closed += SocketClientHelper_Closed;
            socketClientHelper.OnDataReceived += SocketClientHelper_OnDataReceived;
            socketClientHelper.Error += SocketClientHelper_Error;
            return result;
        }

        List<ClientDataEntity> ClientDataEntities = new List<ClientDataEntity>();
        #region 物流调度
        private int ActionProcess = 0;
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
        private Guid? productPalletID;
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

        /// <summary>
        /// 注册需要监视的点
        /// </summary>
        /// <returns></returns>
        private bool registerNode()
        {
            //string businessName = Utility.ConfigHelper.GetAppSetting("Business_Agv");
            string businessName = System.Web.Configuration.WebConfigurationManager.AppSettings["Business_Dispatch"];
            var businessNodes = ProductionProcessEquipmentBusinessNodeMapContract.BusinessNodeMaps.
                Where(a => a.BusinessNode.BusinessName.StartsWith(businessName));
            bool result = false;
            //LogHelper.Info(Application.Current.Resources["UiMessage"].ToString());
            if (!(Equals(businessNodes, null) && businessNodes.Any()))
            {
                //ClientDataEntities?.Clear();
                foreach (var data in businessNodes)
                {
                    ClientDataEntity clientDataEntity = new ClientDataEntity();
                    clientDataEntity.FunctionCode = FuncCode.SubScription;
                    clientDataEntity.ProductionProcessEquipmentBusinessNodeMapId = data.Id;
                    //clientDataEntity=data.BusinessNode.BusinessName
                    clientDataEntity.StatusCode = 0;
                    clientDataEntity.NodeId = data.DeviceNode.NodeUrl;
                    clientDataEntity.ValueType = Utility.Extensions.TypeExtensions.TypeCodeToTypeDic[(TypeCode)data.DeviceNode.DataType];
                    ClientDataEntities.Add(clientDataEntity);
                }
                if (ClientDataEntities.Count > 0)
                {
                    result = this.socketClientHelper.Send(addMessageSplit(JsonHelper.ToJson(ClientDataEntities)));
                }
                else
                {

                    result = false;
                }
                Logger.Info($"DispatchingControlService.registerNode ClientDataEntities数量={ClientDataEntities.Count}");

            }
            return result;
        }

        /// <summary>
        /// 增加消息分隔符 开始结束符
        /// </summary>
        /// <param name="msgBody"></param>
        /// <returns></returns>
        private String addMessageSplit(String msgBody)
        {
            return "[STX]" + msgBody + "[ETX]";
        }
        private void SocketClientHelper_Connected(object sender, EventArgs e)
        {
            Logger.Info($"DispatchingControlService.SocketClientHelper_Connected{socketClientHelper.ServerIp}");

        }
        private void SocketClientHelper_Closed(object sender, EventArgs e)
        {
            Logger.Info($"DispatchingControlService.SocketClientHelper_Closed{socketClientHelper.ServerIp}");
        }

        private void SocketClientHelper_OnDataReceived(object sender, Utility.Socket.DataEventArgs e)
        {

            var cde = JsonHelper.FromJson<List<ClientDataEntity>>(e.PackageInfo.Data);
            if (!Equals(cde, null) && cde.Any())
            {
                foreach (var v in cde)
                {
                    var tmp = ClientDataEntities.FirstOrDefault(a => a.NodeId == v.NodeId);
                    if (!Equals(v.Value, null))
                    {
                        tmp.Value = v.Value?.ToString();
                    }
                    tmp.StatusCode = v.StatusCode;
                    // Logger.Debug(tmp.NodeId + " " + tmp.Value?.ToString());
                }
            }
            LogisticsDispatchDataTreat();
            StorageDispatchDataTreat();
        }
        /// <summary>
        /// 物流调度数据处理
        /// </summary>
        private void LogisticsDispatchDataTreat()
        {
            if (ClientDataEntities.Count > 0)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("StepAction_IsWholeCourse")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        isAllAction = bool.Parse(v1.Value.ToString());
                        var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("StepAction_ActionProcess")).FirstOrDefault();
                        if (!Equals(v2, null))
                        {
                            if (!Equals(v2.Value, null))
                            {
                                ActionProcess = int.Parse(v2.Value.ToString());
                                if (isAllAction)
                                {
                                    switch (ActionProcess)
                                    {
                                        case 1:
                                            if (!isClickMaterialOutCommand)
                                            {
                                                isClickMaterialOutCommand = true;
                                                ExecuteMaterialOutCommand();
                                            }
                                            break;
                                        case 2:
                                            if (!isClickToWorkIsland1Command)
                                            {
                                                isClickToWorkIsland1Command = true;
                                                ExecuteToWorkIsland1Command();
                                            }
                                            break;
                                        case 6:
                                            if (!isClickIsland1WorkingCommand)
                                            {
                                                isClickIsland1WorkingCommand = true;
                                                ExecuteIsland1WorkingCommand();
                                            }
                                            break;
                                        case 10:
                                            if (!isClickToWorkIsland2Command)
                                            {
                                                isClickToWorkIsland2Command = true;
                                                ExecuteToWorkIsland2();
                                            }
                                            break;
                                        case 14:
                                            if (!isClickIsland2WorkingCommand)
                                            {
                                                isClickIsland2WorkingCommand = true;
                                                ExecuteIsland2WorkingCommand();
                                            }
                                            break;
                                        case 18:
                                            if (!isClickProductInCommand)
                                            {
                                                isClickProductInCommand = true;
                                                ExecuteProductInCommand();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                if (!isAllAction)
                                {
                                    switch (ActionProcess)
                                    {
                                        case 1:
                                            if (!isClickMaterialOutCommand)
                                            {
                                                isClickMaterialOutCommand = true;
                                                ExecuteMaterialOutCommand();
                                            }
                                            break;
                                        case 3:
                                            if (!isClickToWorkIsland1Command)
                                            {
                                                isClickToWorkIsland1Command = true;
                                                ExecuteToWorkIsland1Command();
                                            }
                                            break;
                                        case 7:
                                            if (!isClickIsland1WorkingCommand)
                                            {
                                                isClickIsland1WorkingCommand = true;
                                                ExecuteIsland1WorkingCommand();
                                            }
                                            break;
                                        case 11:
                                            if (!isClickToWorkIsland2Command)
                                            {
                                                isClickToWorkIsland2Command = true;
                                                ExecuteToWorkIsland2();
                                            }
                                            break;
                                        case 15:
                                            if (!isClickIsland2WorkingCommand)
                                            {
                                                isClickIsland2WorkingCommand = true;
                                                ExecuteIsland2WorkingCommand();
                                            }
                                            break;
                                        case 19:
                                            if (!isClickProductInCommand)
                                            {
                                                isClickProductInCommand = true;
                                                ExecuteProductInCommand();
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
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
                        }
                    }

                }
            }
        }

        private MaterialOutStorageInfo materialOutStorageInfo = new MaterialOutStorageInfo();
        private MaterialOutStorageInfoInputDto materialOutStorageInfoDto = new MaterialOutStorageInfoInputDto();
        private MaterialBatchInfo materialBatchinfo = new MaterialBatchInfo();
        private MatWareHouseLocationInfo productWareHouseLocationInfo = new MatWareHouseLocationInfo();
        /// <summary>
        /// 原料出库操作
        /// </summary>
        private void ExecuteMaterialOutCommand()
        {
            materialOutStorageInfo = MaterialOutStorageInfoContract.MaterialOutStorageTrackInfos.Where(x => x.OutStorageType == 4 && x.OutStorageStatus == 1).OrderByDescending(x => x.CreatedTime).FirstOrDefault();
            if (!Equals(materialOutStorageInfo, null))
            {
                materialBatchinfo = MaterialBatchInfoContract.MaterialTrackBatchInfos.Where(x => x.Material.Id == materialOutStorageInfo.MaterialID && x.MatWareHouseLocation.WareHouseLocationType == 1 && x.Quantity == materialOutStorageInfo.Quantity).OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                if (!Equals(materialBatchinfo, null))
                {
                    string locationcode = materialBatchinfo.MatWareHouseLocation.WareHouseLocationCode;
                    MatWareHouse_Location = int.Parse(locationcode.Substring(locationcode.Length - 2, 2));
                    productPalletID = materialBatchinfo.MatWareHouseLocation.PalletID;
                    List<MaterialBatchInfo> batchList = new List<MaterialBatchInfo>();
                    batchList.Add(materialBatchinfo);
                    materialOutStorageInfoDto.MaterialBatchs = batchList;
                    materialOutStorageInfoDto.Id = materialOutStorageInfo.Id;
                    materialOutStorageInfoDto.CreatedTime = materialOutStorageInfo.CreatedTime;
                    materialOutStorageInfoDto.CreatorUserId = materialOutStorageInfo.CreatorUserId;
                    materialOutStorageInfoDto.LastUpdatorUserId = materialOutStorageInfo.LastUpdatorUserId;
                    materialOutStorageInfoDto.LastUpdatedTime = DateTime.Now;
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
                    WriteData("StepAction_ActionProcess", ActionProcess);
                }
            }
            else
            {
                isAllAction = false;
                WriteBoolData("StepAction_IsWholeCourse", isAllAction);
                isClickMaterialOutCommand = false;
                return;
            }
        }
        /// <summary>
        /// 转运至加工岛1操作
        /// </summary>
        private void ExecuteToWorkIsland1Command()
        {
            //写AGV AGV_TargetSite=D1 AGV目标地址后期需要修改
            AGV_TargetSite = 300;
            WriteData("AGV_TargetSite", AGV_TargetSite);
        }
        private void ExecuteIsland1WorkingCommand()
        {
            // 给Robot1发送TransLineD1_Arrived = 1信号(加工岛1开始加工)
        }
        /// <summary>
        /// 转运至加工岛2操作
        /// </summary>
        private void ExecuteToWorkIsland2()
        {
            //写AGV AGV_TargetSite=D3 AGV目标地址后期需要修改
            AGV_TargetSite = 500;
            WriteData("AGV_TargetSite", AGV_TargetSite);
        }
        private void ExecuteIsland2WorkingCommand()
        {
            // 给Robot2发送TransLineD3_Arrived = 1信号(加工岛1开始加工)
        }
        /// <summary>
        /// 成品入库操作
        /// </summary>
        private void ExecuteProductInCommand()
        {
            //分配成品空库位
            //写AGV AGV_TargetSite=D2 AGV目标地址后期需要修改
            AGV_TargetSite = 400;
            WriteData("AGV_TargetSite", AGV_TargetSite);
        }
        private String GetMessage(String msgBody)
        {
            return "[STX]" + msgBody + "[ETX]";
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
                    this.socketClientHelper.Send(GetMessage(JsonHelper.ToJson(clientDataEntitiesSend)));
                    System.Threading.Thread.Sleep(200);
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
                    this.socketClientHelper.Send(GetMessage(JsonHelper.ToJson(clientDataEntitiesSend)));
                    System.Threading.Thread.Sleep(200);
                }
            }
        }
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
            var result = MaterialOutStorageInfoContract.MaterialOutStorageShowTask(materialOutStorageInfoDto);
            if (result.Result.Successed)
            {
                ActionProcess = 2;
                WriteData("StepAction_ActionProcess", ActionProcess);
                isSend_TransLineK2Stop = true;
            }
            else
            {
                ActionProcess = 0;
                WriteData("StepAction_ActionProcess", ActionProcess);
                InitParaMeters();
                Console.WriteLine("原料出库不成功！");
                return;
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
                            WriteData("StepAction_ActionProcess", ActionProcess);
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
                    ActionProcess = 6;
                    WriteData("StepAction_ActionProcess", ActionProcess);
                    isSend_AGVRoller2Stop = true;
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
                            WriteData("StepAction_ActionProcess", ActionProcess);
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
                            ActionProcess = 10;
                            WriteData("StepAction_ActionProcess", ActionProcess);
                            isSend_AGVRoller1Stop = true;
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
                            WriteData("StepAction_ActionProcess", ActionProcess);
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
                    ActionProcess = 14;
                    WriteData("StepAction_ActionProcess", ActionProcess);
                    isSend_AGVRoller2Stop2 = true;
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
                            WriteData("StepAction_ActionProcess", ActionProcess);
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
                    ActionProcess = 18;
                    WriteData("StepAction_ActionProcess", ActionProcess);
                    isSend_AGVRoller1Stop2 = true;
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
                                            productWareHouseLocationInfo = MatWareHouseLocationInfoContract.MatWareHouseTrackLocationInfos.Where(m => (m.PalletID == null || m.PalletID == Guid.Empty) && m.WareHouseLocationType == 2 && m.IsUse).OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                                            if (!Equals(productWareHouseLocationInfo, null))
                                            {
                                                string locationcode = productWareHouseLocationInfo.WareHouseLocationCode;
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
        private MaterialInStorageInfoInputDto materialInStorageInfoInputDto = new MaterialInStorageInfoInputDto();
        /// <summary>
        /// 执行成品回库操作
        /// </summary>
        private void ProductInStorageShowTask()
        {
            isUpdateProductInStorage = true;
            materialInStorageInfoInputDto.PalletCode = Pallet_Code_K3.ToString();
            materialInStorageInfoInputDto.Quantity = materialOutStorageInfo.Quantity;
            //var loca = materialBatchinfo.MatWareHouseLocation;
            //materialInStorageInfoInputDto.PalletID = (loca != null) ? loca.PalletID : Guid.Empty;
            materialInStorageInfoInputDto.PalletID = productPalletID;
            materialInStorageInfoInputDto.CreatedTime = DateTime.Now;
            materialInStorageInfoInputDto.CreatorUserId = materialBatchinfo.CreatorUserId;
            materialInStorageInfoInputDto.LastUpdatedTime = DateTime.Now;
            materialInStorageInfoInputDto.LastUpdatedTime = DateTime.Now;
            List<MatWareHouseLocationInfo> locationList = new List<MatWareHouseLocationInfo>();
            locationList.Add(productWareHouseLocationInfo);
            materialInStorageInfoInputDto.MatWareHouseLocations = locationList;
            var result = MaterialInStorageInfoContract.ProductInStorageShowTask(materialInStorageInfoInputDto);
            if (result.Result.Successed)
            {
                ActionProcess = 20;
                WriteData("StepAction_ActionProcess", ActionProcess);
                InitParaMeters();
            }
            else
            {
                ActionProcess = 0;
                WriteData("StepAction_ActionProcess", ActionProcess);
                InitParaMeters();
                Console.WriteLine("成品回库不成功！");
                return;
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
            isAllAction = false;
            WriteBoolData("StepAction_IsWholeCourse", isAllAction);
        }
        #endregion
        private void SocketClientHelper_Error(object sender, Exception e)
        {
            Logger.Error($"DispatchingControlService.SocketClientHelper_Error：{socketClientHelper.ServerIp}", e);
        }
        #endregion
        #region 智能仓储调度

        private int Storage_ActionProcess = 0;
        private int StorageMatWareHouse_Location = 0;
        private int StorageMatStorage_Type = 1;
        private bool isClickStorageMaterialInCommand = false;
        private bool isClickStorageProductOutCommand = false;
        private bool isClickStorageMoveCommand = false;
        private bool isClickStorageEmptyPalletInCommand = false;
        private bool isUpdateStorageMaterialIn = false;
        private bool TransLineK1_Arrived = false;
        private int Pallet_Code_K1 = 0;
        private int TransLineK1_Action = 0;
        private bool MaterialInStorage_Finished = false;
        private bool TransLineK4_Arrived = false;
        private int MatWareHouse_OriginalLocation = 0;
        private bool MaterialMoveStorage_Finished1 = false;
        private bool MaterialMoveStorage_Finished2 = false;
        private bool PalletInStorage_Finished = false;
        private bool isStorageSend_TransLineK1Action = false;
        private bool isStorageSend_TransLineK1Action1 = false;
        private bool isUpdateStorageEmptyPalletIn = false;
        private bool isUpdateStorageProductOut = false;
        private bool isStorageSend_MoveToLocation = false;
        private bool isUpdateStorageMoveInfo = false;
        private MaterialInStorageInfo StorageMaterialInStorageInfo = new MaterialInStorageInfo();
        private MaterialInStorageInfoInputDto StorageMaterialInStorageInfoDto = new MaterialInStorageInfoInputDto();
        private MaterialOutStorageInfoInputDto StorageProductOutStorageInfoDto = new MaterialOutStorageInfoInputDto();
        private MaterialBatchInfo StorageProductBatchinfo = new MaterialBatchInfo();
        private MaterialOutStorageInfo StorageProductOutStorageInfo = new MaterialOutStorageInfo();
        private MatWareHouseLocationInfo StorageMaterialWareHouseLocationInfo = new MatWareHouseLocationInfo();
        private MatWareHouseLocationInfo StorageProductWareHouseLocationInfo = new MatWareHouseLocationInfo();
        private MatWareHouseLocationInfo StorageOriginalWareHouseLocationInfo = new MatWareHouseLocationInfo();
        private MatWareHouseLocationInfo StorageMoveToWareHouseLocationInfo = new MatWareHouseLocationInfo();
        private MatStorageMoveInfo StorageMatMoveInfo = new MatStorageMoveInfo();
        private MatStorageMoveInfoInputDto StorageMoveInfoInputDto = new MatStorageMoveInfoInputDto();
        /// <summary>
        /// 智能仓储调度数据处理
        /// </summary>
        private void StorageDispatchDataTreat()
        {
            if (ClientDataEntities.Count > 0)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("Storage_ActionProcess")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        Storage_ActionProcess = int.Parse(v1.Value.ToString());
                        switch (Storage_ActionProcess)
                        {
                            case 1:
                                if (!isClickStorageMaterialInCommand)
                                {
                                    isClickStorageMaterialInCommand = true;
                                    ExecuteStorageMaterialInCommand();
                                }
                                break;
                            case 4:
                                if (!isClickStorageProductOutCommand)
                                {
                                    isClickStorageProductOutCommand = true;
                                    ExecuteStorageProductOutCommand();
                                }
                                break;
                            case 6:
                                if (!isClickStorageMoveCommand)
                                {
                                    isClickStorageMoveCommand = true;
                                    ExecuteStorageMoveCommand();
                                }
                                break;
                            case 8:
                                if (!isClickStorageEmptyPalletInCommand)
                                {
                                    isClickStorageEmptyPalletInCommand = true;
                                    ExecuteStorageEmptyPalletInCommand();
                                }
                                break;
                            default:
                                break;
                        }
                        StorageSetMaterialIn();
                        StorageSetEmptyPalletIn();
                        StorageSetMove();
                        StorageSetProductOut();
                    }
                }
            }
        }
        /// <summary>
        /// 原料入库-自动分配库位
        /// </summary>
        private void ExecuteStorageMaterialInCommand()
        {
            StorageMaterialInStorageInfo = MaterialInStorageInfoContract.MaterialInStorageTrackInfos.Where(x => x.InStorageType == 1 && x.InStorageStatus == 1 && x.PalletQuantity == 1).OrderByDescending(x => x.CreatedTime).FirstOrDefault();
            if (!Equals(StorageMaterialInStorageInfo, null))
            {
                var locationlist = MatWareHouseLocationInfoContract.MatWareHouseTrackLocationInfos.Where(x => x.PalletID != null && x.PalletID != Guid.Empty && x.WareHouseLocationType == 1);
                List<MatWareHouseLocationInfo> materialLocationlist = new List<MatWareHouseLocationInfo>();
                foreach (var item in locationlist)
                {
                    var list1 = MaterialBatchInfoContract.MaterialTrackBatchInfos.Where(m => m.MatWareHouseLocation.Id == item.Id && m.Quantity >= 0);
                    decimal? quantity = -1;
                    if (list1.Count() > 0)
                    {
                        quantity = list1.Sum(m => m.Quantity);
                    }
                    if (quantity == 0 || list1.Count() == 0)
                    {
                        materialLocationlist.Add(item);
                    }
                }
                if (materialLocationlist.Count > 0)
                {
                    StorageMaterialWareHouseLocationInfo = materialLocationlist.OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                    string locationcode = StorageMaterialWareHouseLocationInfo.WareHouseLocationCode;
                    StorageMatWareHouse_Location = int.Parse(locationcode.Substring(locationcode.Length - 2, 2));
                    StorageMaterialInStorageInfoDto = new MaterialInStorageInfoInputDto();
                    StorageMaterialInStorageInfoDto.Id = StorageMaterialInStorageInfo.Id;
                    StorageMaterialInStorageInfoDto.CreatedTime = StorageMaterialInStorageInfo.CreatedTime;
                    StorageMaterialInStorageInfoDto.CreatorUserId = StorageMaterialInStorageInfo.CreatorUserId;
                    StorageMaterialInStorageInfoDto.LastUpdatorUserId = StorageMaterialInStorageInfo.LastUpdatorUserId;
                }
                else
                {
                    Storage_ActionProcess = 0;
                    WriteData("Storage_ActionProcess", Storage_ActionProcess);
                    isClickStorageMaterialInCommand = false;
                    Console.WriteLine("原料入库：没有空托盘的原料库位！");
                    return;
                }
                try
                {
                    WriteData("MatWareHouse_Location", StorageMatWareHouse_Location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Storage_ActionProcess = 0;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                isClickStorageMaterialInCommand = false;
                Console.WriteLine("原料入库：没有原料入库单！");
                return;
            }
        }
        /// <summary>
        /// 原料入库操作
        /// </summary>
        private void StorageSetMaterialIn()
        {
            if (ClientDataEntities.Count > 0 && (Storage_ActionProcess == 1 || Storage_ActionProcess == 2))
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineK1_Arrived")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    if (!Equals(v1.Value, null))
                    {
                        TransLineK1_Arrived = bool.Parse(v1.Value.ToString());
                    }
                }
                var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("Pallet_Code_K1")).FirstOrDefault();
                var v3 = ClientDataEntities.Where(d => d.NodeId.Contains("Storage_ActionProcess")).FirstOrDefault();
                if (!Equals(v2, null))
                {
                    if (!Equals(v2.Value, null))
                    {
                        Pallet_Code_K1 = int.Parse(v2.Value.ToString());
                        if (!Equals(v3, null))
                        {
                            if (!Equals(v3.Value, null))
                            {
                                Storage_ActionProcess = int.Parse(v3.Value.ToString());
                                if (TransLineK1_Arrived && Storage_ActionProcess == 2 && !isStorageSend_TransLineK1Action)
                                {
                                    TransLineK1_Action = 2;
                                    WriteData("TransLineK1_Action", TransLineK1_Action);
                                    WriteData("MatWareHouse_Location", StorageMatWareHouse_Location);
                                    StorageMatStorage_Type = 1;
                                    WriteData("MatStorage_Type", StorageMatStorage_Type);
                                    isStorageSend_TransLineK1Action = true;
                                }

                                if (isStorageSend_TransLineK1Action && !isUpdateStorageMaterialIn)
                                {
                                    var v4 = ClientDataEntities.Where(d => d.NodeId.Contains("MaterialInStorage_Finished")).FirstOrDefault();
                                    if (!Equals(v4, null))
                                    {
                                        if (!Equals(v4.Value, null))
                                        {
                                            MaterialInStorage_Finished = bool.Parse(v4.Value.ToString());
                                            if (MaterialInStorage_Finished)
                                            {
                                                //校验托盘编号与原料出库的编号是否一致 更新原料入库 库存等信息
                                                StorageMaterialInStorageShowTask();
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
        /// <summary>
        /// 原料入库任务
        /// </summary>
        private void StorageMaterialInStorageShowTask()
        {
            isUpdateStorageMaterialIn = true;
            StorageMaterialInStorageInfoDto.PalletCode = Pallet_Code_K1.ToString();
            StorageMaterialInStorageInfoDto.Quantity = StorageMaterialInStorageInfo.Quantity;
            StorageMaterialInStorageInfoDto.PalletID = (StorageMaterialWareHouseLocationInfo != null) ? StorageMaterialWareHouseLocationInfo.PalletID : Guid.Empty;
            StorageMaterialInStorageInfoDto.InStorageType = StorageMaterialInStorageInfo.InStorageType;
            StorageMaterialInStorageInfoDto.AuditStatus = StorageMaterialInStorageInfo.AuditStatus;
            StorageMaterialInStorageInfoDto.LastUpdatedTime = DateTime.Now;
            StorageMaterialInStorageInfoDto.UserName = StorageMaterialInStorageInfo.CreatorUserId;
            StorageMaterialInStorageInfoDto.MaterialID = StorageMaterialInStorageInfo.MaterialID;
            StorageMaterialInStorageInfoDto.MatSupplierID = StorageMaterialInStorageInfo.MatSupplierID;
            StorageMaterialInStorageInfoDto.Remark = StorageMaterialInStorageInfo.Remark;
            StorageMaterialInStorageInfoDto.PalletQuantity = StorageMaterialInStorageInfo.PalletQuantity;
            StorageMaterialInStorageInfoDto.InStorageStatus = StorageMaterialInStorageInfo.InStorageStatus;
            StorageMaterialInStorageInfoDto.InStorageBillCode = StorageMaterialInStorageInfo.InStorageBillCode;
            List<MatWareHouseLocationInfo> locationList = new List<MatWareHouseLocationInfo>();
            locationList.Add(StorageMaterialWareHouseLocationInfo);
            StorageMaterialInStorageInfoDto.MatWareHouseLocations = locationList;
            var result = MaterialInStorageInfoContract.AddTask(StorageMaterialInStorageInfoDto);
            if (result.Result.Successed)
            {
                Storage_ActionProcess = 3;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                InitStorageParaMeters();
            }
            else
            {
                Storage_ActionProcess = 0;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                InitStorageParaMeters();
                Console.WriteLine("原料入库不成功！");
                return;
            }
        }

        private void InitStorageParaMeters()
        {
            isClickStorageMaterialInCommand = false;
            isClickStorageProductOutCommand = false;
            isClickStorageMoveCommand = false;
            isClickStorageEmptyPalletInCommand = false;
            isUpdateStorageMaterialIn = false;
            Pallet_Code_K1 = 0;
            TransLineK1_Action = 0;
            MaterialInStorage_Finished = false;
            TransLineK4_Arrived = false;
            MatWareHouse_OriginalLocation = 0;
            MaterialMoveStorage_Finished1 = false;
            MaterialMoveStorage_Finished2 = false;
            PalletInStorage_Finished = false;
            isStorageSend_TransLineK1Action = false;
            isStorageSend_TransLineK1Action1 = false;
            isUpdateStorageEmptyPalletIn = false;
            isUpdateStorageProductOut = false;
            TransLineK1_Arrived = false;
            isStorageSend_MoveToLocation = false;
            isUpdateStorageMoveInfo = false;
            StorageMaterialInStorageInfo = new MaterialInStorageInfo();
        }
        private void ExecuteStorageEmptyPalletInCommand()
        {
            StorageMaterialInStorageInfo = MaterialInStorageInfoContract.MaterialInStorageTrackInfos.Where(x => x.InStorageType == 4 && x.InStorageStatus == 1).OrderByDescending(x => x.CreatedTime).FirstOrDefault();
            if (!Equals(StorageMaterialInStorageInfo, null))
            {
                var locationlist = MatWareHouseLocationInfoContract.MatWareHouseTrackLocationInfos.Where(x => (x.PalletID == null || x.PalletID == Guid.Empty) && x.WareHouseLocationType == 1);
                if (locationlist.Count() > 0)
                {
                    StorageMaterialInStorageInfoDto = new MaterialInStorageInfoInputDto();
                    StorageMaterialWareHouseLocationInfo = locationlist.OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                    string locationcode = StorageMaterialWareHouseLocationInfo.WareHouseLocationCode;
                    StorageMatWareHouse_Location = int.Parse(locationcode.Substring(locationcode.Length - 2, 2));
                    StorageMaterialInStorageInfoDto.Id = StorageMaterialInStorageInfo.Id;
                    StorageMaterialInStorageInfoDto.CreatedTime = StorageMaterialInStorageInfo.CreatedTime;
                    StorageMaterialInStorageInfoDto.CreatorUserId = StorageMaterialInStorageInfo.CreatorUserId;
                    StorageMaterialInStorageInfoDto.LastUpdatorUserId = StorageMaterialInStorageInfo.LastUpdatorUserId;
                }
                else
                {
                    Storage_ActionProcess = 0;
                    WriteData("Storage_ActionProcess", Storage_ActionProcess);
                    isClickStorageMaterialInCommand = false;
                    Console.WriteLine("原料入库：没有空的原料库位！");
                    return;
                }
                try
                {
                    WriteData("MatWareHouse_Location", StorageMatWareHouse_Location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Storage_ActionProcess = 0;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                isClickStorageMaterialInCommand = false;
                Console.WriteLine("原料入库：没有空托盘入库单！");
                return;
            }
        }
        /// <summary>
        /// 空托盘入库操作
        /// </summary>
        private void StorageSetEmptyPalletIn()
        {
            if (ClientDataEntities.Count > 0 && Storage_ActionProcess == 8)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("Pallet_Code_K1")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    Pallet_Code_K1 = int.Parse(v1.Value.ToString());
                    if (!isStorageSend_TransLineK1Action1)
                    {
                        TransLineK1_Action = 2;
                        WriteData("TransLineK1_Action", TransLineK1_Action);
                        WriteData("MatWareHouse_Location", StorageMatWareHouse_Location);
                        StorageMatStorage_Type = 6;
                        WriteData("MatStorage_Type", StorageMatStorage_Type);
                        isStorageSend_TransLineK1Action1 = true;
                    }

                    if (isStorageSend_TransLineK1Action1 && !isUpdateStorageEmptyPalletIn)
                    {
                        var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("PalletInStorage_Finished")).FirstOrDefault();
                        if (!Equals(v2, null))
                        {
                            if (!Equals(v2.Value, null))
                            {
                                PalletInStorage_Finished = bool.Parse(v2.Value.ToString());
                                if (PalletInStorage_Finished)
                                {
                                    //校验托盘编号与原料出库的编号是否一致 更新原料入库 库存等信息
                                    StorageEmptyPalletInStorageShowTask();
                                }
                            }
                        }
                    }
                }
            }

        }
        /// <summary>
        /// 空托盘入库任务
        /// </summary>
        private void StorageEmptyPalletInStorageShowTask()
        {
            isUpdateStorageEmptyPalletIn = true;
            StorageMaterialInStorageInfoDto.PalletCode = Pallet_Code_K1.ToString();
            StorageMaterialInStorageInfoDto.PalletID = StorageMaterialInStorageInfo.PalletID;
            StorageMaterialInStorageInfoDto.InStorageType = StorageMaterialInStorageInfo.InStorageType;
            StorageMaterialInStorageInfoDto.AuditStatus = StorageMaterialInStorageInfo.AuditStatus;
            StorageMaterialInStorageInfoDto.LastUpdatedTime = DateTime.Now;
            StorageMaterialInStorageInfoDto.UserName = StorageMaterialInStorageInfo.CreatorUserId;
            StorageMaterialInStorageInfoDto.MatSupplierID = StorageMaterialInStorageInfo.MatSupplierID;
            StorageMaterialInStorageInfoDto.Remark = StorageMaterialInStorageInfo.Remark;
            StorageMaterialInStorageInfoDto.InStorageStatus = StorageMaterialInStorageInfo.InStorageStatus;
            StorageMaterialInStorageInfoDto.InStorageBillCode = StorageMaterialInStorageInfo.InStorageBillCode;
            List<MatWareHouseLocationInfo> locationList = new List<MatWareHouseLocationInfo>();
            locationList.Add(StorageMaterialWareHouseLocationInfo);
            StorageMaterialInStorageInfoDto.MatWareHouseLocations = locationList;
            var result = MaterialInStorageInfoContract.AddTask(StorageMaterialInStorageInfoDto);
            if (result.Result.Successed)
            {
                Storage_ActionProcess = 9;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                InitStorageParaMeters();
            }
            else
            {
                Storage_ActionProcess = 0;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                InitStorageParaMeters();
                Console.WriteLine("空托盘入库不成功！");
                return;
            }
        }
        /// <summary>
        /// 移库
        /// </summary>
        private void ExecuteStorageMoveCommand()
        {
            StorageMatMoveInfo = new MatStorageMoveInfo();
            StorageMatMoveInfo = MatStorageMoveInfoContract.MatStorageTrackMoveInfos.Where(x => x.StorageMoveState == 1).OrderByDescending(x => x.CreatedTime).FirstOrDefault();
            if (!Equals(StorageMatMoveInfo, null))
            {

                if (!Equals(StorageMatMoveInfo.FromLocationID, null) && !Equals(StorageMatMoveInfo.ToLocationID, null) && !Equals(StorageMatMoveInfo.FromLocationID, Guid.Empty) && !Equals(StorageMatMoveInfo.ToLocationID, Guid.Empty))
                {
                    StorageOriginalWareHouseLocationInfo = MatWareHouseLocationInfoContract.MatWareHouseTrackLocationInfos.Where(x => x.Id == StorageMatMoveInfo.FromLocationID).FirstOrDefault();
                    StorageMoveToWareHouseLocationInfo = MatWareHouseLocationInfoContract.MatWareHouseTrackLocationInfos.Where(x => x.Id == StorageMatMoveInfo.ToLocationID).FirstOrDefault();
                    string locationcode = StorageOriginalWareHouseLocationInfo.WareHouseLocationCode;
                    MatWareHouse_OriginalLocation = int.Parse(locationcode.Substring(locationcode.Length - 2, 2));
                    string locationcode1 = StorageMoveToWareHouseLocationInfo.WareHouseLocationCode;
                    StorageMatWareHouse_Location = int.Parse(locationcode1.Substring(locationcode1.Length - 2, 2));
                    WriteData("MatWareHouse_OriginalLocation", MatWareHouse_OriginalLocation);
                    StorageMatStorage_Type = 5;
                    WriteData("MatStorage_Type", StorageMatStorage_Type);
                    StorageMoveInfoInputDto = new MatStorageMoveInfoInputDto();
                    StorageMoveInfoInputDto.Id = StorageMatMoveInfo.Id;
                    StorageMoveInfoInputDto.FromLocationID = StorageMatMoveInfo.FromLocationID;
                    StorageMoveInfoInputDto.ToLocationID = StorageMatMoveInfo.ToLocationID;
                    StorageMoveInfoInputDto.StorageMoveCode = StorageMatMoveInfo.StorageMoveCode;
                    StorageMoveInfoInputDto.StorageMoveReason = StorageMatMoveInfo.StorageMoveReason;
                    StorageMoveInfoInputDto.Remark = StorageMatMoveInfo.Remark;
                    StorageMoveInfoInputDto.StorageMoveState = StorageMatMoveInfo.StorageMoveState;
                    StorageMoveInfoInputDto.Operator = StorageMatMoveInfo.Operator;
                    StorageMoveInfoInputDto.CreatedTime = StorageMatMoveInfo.CreatedTime;
                    StorageMoveInfoInputDto.CreatorUserId = StorageMatMoveInfo.CreatorUserId;
                    StorageMoveInfoInputDto.LastUpdatorUserId = StorageMatMoveInfo.LastUpdatorUserId;
                }
                else
                {
                    Storage_ActionProcess = 0;
                    WriteData("Storage_ActionProcess", Storage_ActionProcess);
                    isClickStorageMoveCommand = false;
                    Console.WriteLine("移库：原库位或目标库位数据异常！");
                    return;
                }
                try
                {
                    WriteData("MatWareHouse_Location", StorageMatWareHouse_Location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                Storage_ActionProcess = 0;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                isClickStorageMoveCommand = false;
                Console.WriteLine("移库：没有移库单！");
                return;
            }
        }
        /// <summary>
        /// 移库操作
        /// </summary>
        private void StorageSetMove()
        {
            if (ClientDataEntities.Count > 0 && Storage_ActionProcess == 6)
            {

                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("MaterialMoveStorage_Finished1")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    MaterialMoveStorage_Finished1 = bool.Parse(v1.Value.ToString());
                    if (MaterialMoveStorage_Finished1 && !isStorageSend_MoveToLocation)
                    {
                        WriteData("MatWareHouse_Location", StorageMatWareHouse_Location);
                        StorageMatStorage_Type = 5;
                        WriteData("MatStorage_Type", StorageMatStorage_Type);
                        isStorageSend_MoveToLocation = true;
                    }

                    if (isStorageSend_MoveToLocation && !isUpdateStorageMoveInfo)
                    {
                        var v2 = ClientDataEntities.Where(d => d.NodeId.Contains("MaterialMoveStorage_Finished2")).FirstOrDefault();
                        if (!Equals(v2, null))
                        {
                            if (!Equals(v2.Value, null))
                            {
                                MaterialMoveStorage_Finished2 = bool.Parse(v2.Value.ToString());
                                if (MaterialMoveStorage_Finished2)
                                {
                                    StorageMoveShowTask();
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 移库任务-更新移库库存等信息
        /// </summary>
        private void StorageMoveShowTask()
        {
            isUpdateStorageMoveInfo = true;
            StorageMoveInfoInputDto.LastUpdatedTime = DateTime.Now;
            StorageMoveInfoInputDto.UserName = StorageMatMoveInfo.CreatorUserId;
            var result = MatStorageMoveInfoContract.AddTask(StorageMoveInfoInputDto);
            if (result.Result.Successed)
            {
                Storage_ActionProcess = 7;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                InitStorageParaMeters();
            }
            else
            {
                Storage_ActionProcess = 0;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                InitStorageParaMeters();
                Console.WriteLine("移库不成功，请检查库位是否正确！");
                return;
            }
        }
        /// <summary>
        /// 成品出库
        /// </summary>
        private void ExecuteStorageProductOutCommand()
        {
            StorageProductOutStorageInfo = MaterialOutStorageInfoContract.MaterialOutStorageTrackInfos.Where(x => x.OutStorageType == 2 && x.OutStorageStatus == 1 && x.PalletQuantity == 1).OrderByDescending(x => x.CreatedTime).FirstOrDefault();
            if (!Equals(StorageProductOutStorageInfo, null))
            {
                StorageProductBatchinfo = MaterialBatchInfoContract.MaterialTrackBatchInfos.Where(x => x.Material.Id == StorageProductOutStorageInfo.MaterialID && x.MatWareHouseLocation.WareHouseLocationType == 2 && x.Quantity == StorageProductOutStorageInfo.Quantity).OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                if (!Equals(StorageProductBatchinfo, null))
                {
                    string locationcode = StorageProductBatchinfo.MatWareHouseLocation.WareHouseLocationCode;
                    MatWareHouse_Location = int.Parse(locationcode.Substring(locationcode.Length - 2, 2));
                    List<MaterialBatchInfo> batchList = new List<MaterialBatchInfo>();
                    batchList.Add(StorageProductBatchinfo);
                    StorageProductOutStorageInfoDto = new MaterialOutStorageInfoInputDto();
                    StorageProductOutStorageInfoDto.MaterialBatchs = batchList;
                    StorageProductOutStorageInfoDto.Id = StorageProductOutStorageInfo.Id;
                    StorageProductOutStorageInfoDto.CreatedTime = StorageProductOutStorageInfo.CreatedTime;
                    StorageProductOutStorageInfoDto.CreatorUserId = StorageProductOutStorageInfo.CreatorUserId;
                    StorageProductOutStorageInfoDto.LastUpdatorUserId = StorageProductOutStorageInfo.LastUpdatorUserId;
                    try
                    {
                        WriteData("MatWareHouse_Location", MatWareHouse_Location);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    MatStorage_Type = 4;
                    WriteData("MatStorage_Type", MatStorage_Type);
                }
            }
            else
            {
                isClickStorageProductOutCommand = false;
                Console.WriteLine("成品出库：没有成品出库单！");
                return;
            }
        }
        /// <summary>
        /// 成品出库操作
        /// </summary>
        private void StorageSetProductOut()
        {
            if (ClientDataEntities.Count > 0 && Storage_ActionProcess == 4)
            {
                var v1 = ClientDataEntities.Where(d => d.NodeId.Contains("TransLineK4_Arrived")).FirstOrDefault();
                if (!Equals(v1, null))
                {
                    TransLineK4_Arrived = bool.Parse(v1.Value.ToString());
                    if (TransLineK4_Arrived && !isUpdateStorageProductOut)
                    {
                        StorageProductOutStorageShowTask();

                    }

                }
            }
        }

        private void StorageProductOutStorageShowTask()
        {
            isUpdateStorageProductOut = true;
            StorageProductOutStorageInfoDto.OutStorageType = StorageProductOutStorageInfo.OutStorageType;
            StorageProductOutStorageInfoDto.MaterialID = StorageProductOutStorageInfo.MaterialID;
            StorageProductOutStorageInfoDto.AuditStatus = StorageProductOutStorageInfo.AuditStatus;
            StorageProductOutStorageInfoDto.LastUpdatedTime = DateTime.Now;
            StorageProductOutStorageInfoDto.UserName = StorageProductOutStorageInfo.CreatorUserId;
            StorageProductOutStorageInfoDto.Remark = StorageProductOutStorageInfo.Remark;
            StorageProductOutStorageInfoDto.OutStorageStatus = StorageProductOutStorageInfo.OutStorageStatus;
            StorageProductOutStorageInfoDto.OutStorageBillCode = StorageProductOutStorageInfo.OutStorageBillCode;
            StorageProductOutStorageInfoDto.Quantity = StorageProductOutStorageInfo.Quantity;
            StorageProductOutStorageInfoDto.PalletQuantity = StorageProductOutStorageInfo.PalletQuantity;
            var result = MaterialOutStorageInfoContract.AddTask(StorageProductOutStorageInfoDto);
            if (result.Result.Successed)
            {
                Storage_ActionProcess = 5;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                InitStorageParaMeters();
            }
            else
            {
                Storage_ActionProcess = 0;
                WriteData("Storage_ActionProcess", Storage_ActionProcess);
                InitStorageParaMeters();
                Console.WriteLine("成品出库不成功！");
                return;
            }
        }
        #endregion
    }
}


