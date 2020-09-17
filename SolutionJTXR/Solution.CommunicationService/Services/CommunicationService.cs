using Newtonsoft.Json;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Logging;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Models;
using Solution.CommunicationService.Contracts;
using Solution.Device.Core;
using Solution.Device.OpcUaDevice;
using Solution.Utility.Extensions;
using Solution.Utility.Socket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Solution.CommunicationService.Services
{
    public class CommunicationService : ICommunicationContract, IDisposable
    {

        #region Private Fields

        /// <summary>
        /// 客户端和订阅点的临时存储字典
        /// </summary>
        private ConcurrentDictionary<Session, List<ClientDataEntity>> subscriptionDic = new ConcurrentDictionary<Session, List<ClientDataEntity>>();

        /// <summary>
        /// 所有设备的通讯点
        /// </summary>
        private List<DeviceNode> allDeviceNodes = new List<DeviceNode>();

        /// <summary>
        /// socket服务端
        /// </summary>
        private SocketServerHelper appServer;

        /// <summary>
        /// 已连接设备的列表
        /// </summary>
        private List<IDevice> devices = new List<IDevice>();

        private bool IsStarted = false;

        /// <summary>
        /// socket连接到服务端的会话列表
        /// </summary>
        private IList<Session> connectedCientSession;

        /// <summary>
        /// 日志记录
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(CommunicationService));

        #endregion

        #region Public Fields
        /// <summary>
        /// 设备服务信息契约
        /// </summary>
        public IDeviceServerInfoConract DeviceServerInfoConract { get; set; }

        /// <summary>
        /// socket服务契约
        /// </summary>
        public ISocketServerContract SocketServerContract { get; set; }

        /// <summary>
        /// 设备点契约
        /// </summary>
        public IDeviceNodeContract DeviceNodeContract { get; set; }

        /// <summary>
        /// 工位设备业务点map
        /// </summary>
        public IProductionProcessEquipmentBusinessNodeMapContract ProductionProcessEquipmentBusinessNodeMapContract { get; set; }

        #endregion

        /// <summary>
        /// 初始化操作：（1）从数据库读取配置文件，（2）查询出所有OPC服务器并建立连接，（3）启动Socket服务
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            try
            {
                foreach (var deviceServer in DeviceServerInfoConract.DeviceServerInfos.Unlocked())
                {
                    try
                    {
                        IDevice device = System.Reflection.Assembly.Load(deviceServer.DeviceDriveAssemblyName)
                            .CreateInstance(deviceServer.DeviceDriveClassName) as IDevice;
                        Logger.Info($"初始化{deviceServer.DeviceServerName} {deviceServer.DeviceServerName} 设备驱动完成！");

                        DeviceConnectParamEntityBase deviceConnectParamEntity =
                            System.Reflection.Assembly.Load(deviceServer.DeviceDriveAssemblyName)
                            .CreateInstance($"{deviceServer.DeviceDriveAssemblyName}.{deviceServer.DeviceServerName}DeviceConnectParamEntity")
                            as DeviceConnectParamEntityBase;
                        deviceConnectParamEntity.DeviceUrl = deviceServer.DeviceUrl;
                        device.Notification += Device_Notification;
                        device.Name = deviceServer.DeviceServerName;
                        var connectResult = device.Connect<DeviceConnectParamEntityBase, DeviceConnectParamEntityBase>(deviceConnectParamEntity).Result;
                        Logger.Info($"连接{deviceServer.DeviceServerName},{deviceServer.DeviceDriveAssemblyName},{deviceServer.DeviceDriveClassName}设备完成！{connectResult.ToString()}");
                        devices.Add(device);
                        var deviceNodes = DeviceNodeContract.DeviceNodes.Where(a => a.DeviceServerInfo.Id == deviceServer.Id).ToArray();

                        //注册点
                        dynamic[] dynamics = new dynamic[deviceNodes.Count()];
                        for (int i = 0; i < dynamics.Count(); i++)
                        {
                            var nodeEntity = Assembly.Load(deviceServer.DeviceDriveAssemblyName).
                            CreateInstance($"{deviceServer.DeviceDriveAssemblyName}.{deviceServer.DeviceServerName}NodeEntity") as dynamic;

                            nodeEntity.NodeId = deviceNodes[i].NodeUrl;//..n $"ns=2;s=通道 1.设备 1.标记 {(i + 1).ToString().PadLeft(5, '0')}";
                            nodeEntity.ValueType = ((TypeCode)deviceNodes[i].DataType).ToType();
                            nodeEntity.Interval = deviceNodes[i].Interval;
                            dynamics[i] = nodeEntity;
                        }
                        var registerResult = device.RegisterNodes<DeviceOutputParamEntityBase>(dynamics).Result;
                        allDeviceNodes.AddRange(deviceNodes);
                        Logger.Info($"注册点表{deviceServer.DeviceServerName},{deviceServer.DeviceDriveAssemblyName},{deviceServer.DeviceDriveClassName} 完成！{registerResult.ToString()}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"初始化{deviceServer.DeviceServerName}设备驱动错误！", ex);
                    }
                }

                //SubScripttionDoWork();

                Logger.Debug($"CommunicationService初始化操作完成");

                //启动服务
                StartServer();
            }
            catch (Exception ex)
            {
                Logger.Error($"初始化操作异常！ + \n {ex.ToString()}");
            }
        }

        /// <summary>
        /// 设备变化通知
        /// </summary>
        /// <param name="device"></param>
        /// <param name="deviceEventArgs"></param>
        private void Device_Notification(IDevice device, DeviceEventArgs<IDeviceParam> deviceEventArgs)
        {
            var deviceConnectParamEntityBase = deviceEventArgs.Data as DeviceConnectParamEntityBase;
            if (!Equals(deviceConnectParamEntityBase, null))
            {
                //if (deviceConnectParamEntityBase.StatusCode != 0)
                Logger.Info($"设备：{device.ToString()} 连接状态{deviceConnectParamEntityBase.ToString()}");

            }
            var deviceOutputParamEntityBase = deviceEventArgs.Data as DeviceOutputParamEntityBase;

            if (!Equals(deviceOutputParamEntityBase, null))
            {
#if DEBUG
                Logger.Debug($"设备输出数据{device.ToString()}  {deviceEventArgs.Data.ToString()}");
#endif
                //向客户端发送数据
                if (subscriptionDic.Any())
                {
                    sendDataToClient(device, deviceOutputParamEntityBase);
                }
            }
        }

        /// <summary>
        /// 向订阅了数据点的客户端发送变化的数据
        /// </summary>
        /// <param name="device"></param>
        /// <param name="deviceOutputParamEntityBase"></param>
        private async void sendDataToClient(IDevice device, DeviceOutputParamEntityBase deviceOutputParamEntityBase)
        {
            try
            {
                foreach (var v in subscriptionDic)
                {
                    var nodes = v.Value.Where(a => a.NodeId == deviceOutputParamEntityBase.NodeId);
                    if (nodes.Any())
                    {
                        var node = nodes.FirstOrDefault();
                        node.OldVaule = node.Value;
                        node.Value = deviceOutputParamEntityBase.Value;
                        node.StatusCode = deviceOutputParamEntityBase.StatusCode;
                        await SendAsync(v.Key, GetMessage(JsonConvert.SerializeObject(nodes)));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"异步发送订阅数据到客户端异常sendDataToClient！ + \n {ex.ToString()}");
            }
        }

        /// <summary>
        /// 开启Socket服务
        /// </summary>
        public void StartServer()
        {
            try
            {
                if (appServer == null && !IsStarted)
                {

                    var socketServerInfo = SocketServerContract.SocketServerInfos.FirstOrDefault();
                    if (Equals(socketServerInfo, null))
                    {
                        Logger.Info($"CommunicationService.StartServer,未找到SocketServerInfo的信息");
                        return;
                    }
                    //最大客户端数预留，将来根据客户端数量授权收费。
                    appServer = new SocketServerHelper(socketServerInfo.ServerIp, socketServerInfo.ServerPort, socketServerInfo.MaxConnectionNumber);
                    connectedCientSession = new List<Session>(socketServerInfo.MaxConnectionNumber);
                    appServer.Connected += OnConnected;
                    appServer.Closed += OnClosed;
                    appServer.DataReceived += OnReceived;
                    IsStarted = appServer.Start();
                    string result = IsStarted ? "成功！" : "失败！";
                    Logger.Info($"开启Socket服务{result}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"开启Socket服务异常！ + \n {ex.ToString()}");
            }
        }

        /// <summary>
        /// 停止Socket服务
        /// </summary>
        public void StopServer()
        {
            try
            {
                devices.ForEach((device) => { device.DisConnect<IDeviceParam, IDeviceParam>(null); });
                devices.Clear();
                if (appServer != null && IsStarted)
                {
                    appServer.Connected -= OnConnected;
                    appServer.Closed -= OnClosed;
                    appServer.DataReceived -= OnReceived;
                    appServer.Stop();
                    IsStarted = false;
                    Logger.Info($"Socket服务已停止！");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"停止Socket服务异常！ + \n {ex.ToString()}");
            }
        }

        /// <summary>
        /// 读取数据值（异步）,并将结果发送给客户端
        /// </summary>
        /// <param name="clientDataEntity">需要封装NodeID,ValueType</param>
        /// <returns></returns>
        private async Task ReadAsync(Session session, ClientDataEntity clientDataEntity)
        {
            try
            {
                DeviceInputParamEntityBase deviceInputParamEntityBase = new DeviceInputParamEntityBase();
                deviceInputParamEntityBase.NodeId = clientDataEntity.NodeId;
                deviceInputParamEntityBase.ValueType = clientDataEntity.ValueType;
                //OpcUaDeviceHelper opcUaDevice = GetOpcUaDevice(clientDataEntity.KeyId);
                var deviceName = ProductionProcessEquipmentBusinessNodeMapContract.BusinessNodeMaps
                    .FirstOrDefault(a => a.Id == clientDataEntity.ProductionProcessEquipmentBusinessNodeMapId)
                    .DeviceNode.DeviceServerInfo.DeviceServerName;
                var opcUaDeviceOutParamEntity = await devices.FirstOrDefault(a => a.Name == deviceName).Read<DeviceInputParamEntityBase, DeviceOutputParamEntityBase>(deviceInputParamEntityBase);
                clientDataEntity.Value = opcUaDeviceOutParamEntity.Value;
                clientDataEntity.StatusCode = opcUaDeviceOutParamEntity.StatusCode;
            }
            catch (Exception ex)
            {
                Logger.Error($"读取数据值（异步）,并将结果发送给客户端异常！ + \n {ex.ToString()}");
            }
            await SendAsync(session, GetMessage(JsonConvert.SerializeObject(new ClientDataEntity[] { clientDataEntity })));
        }

        /// <summary>
        /// 写入数据值（异步）,并将结果发送给客户端
        /// </summary>
        /// <param name="opcUaDeviceInputParamEntity">需要封装NodeID，Value</param>
        /// <returns></returns>
        private async Task WriteAsync(Session session, ClientDataEntity clientDataEntity)
        {
            try
            {
                DeviceInputParamEntityBase deviceInputParamEntityBase = new DeviceInputParamEntityBase();
                deviceInputParamEntityBase.NodeId = clientDataEntity.NodeId;
                deviceInputParamEntityBase.Value = clientDataEntity.Value;
                deviceInputParamEntityBase.ValueType = clientDataEntity.ValueType;
                var deviceName = ProductionProcessEquipmentBusinessNodeMapContract.BusinessNodeMaps
                   .FirstOrDefault(a => a.Id == clientDataEntity.ProductionProcessEquipmentBusinessNodeMapId)
                   .DeviceNode.DeviceServerInfo.DeviceServerName;
                var opcUaDeviceOutParamEntity = await devices.FirstOrDefault(a => a.Name == deviceName)
                    .Write<DeviceInputParamEntityBase, DeviceOutputParamEntityBase>(deviceInputParamEntityBase);
                clientDataEntity.StatusCode = opcUaDeviceOutParamEntity.StatusCode;
            }
            catch (Exception ex)
            {
                Logger.Error($"写入数据值（异步）,并将结果发送给客户端异常！ + \n {ex.ToString()}");
            }
            await SendAsync(session, GetMessage(JsonConvert.SerializeObject(new ClientDataEntity[] { clientDataEntity })));
        }
        #region Socket 服务端事件函数

        /// <summary>
        /// 处理Socket客户端连接事件
        /// </summary>
        /// <param name="session"></param>
        private void OnConnected(Session session)
        {
            connectedCientSession.Add(session);
            Logger.Debug($"客户端：Session RemoteEndPoint：{session.RemoteEndPoint}已连接! ");
        }

        /// <summary>
        /// 处理Socket客户端断开事件
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reason"></param>
        private void OnClosed(Session session, SocketCloseReason reason)
        {
            //List<Guid> guids = new List<Guid>();
            //bool removeResult = SubscriptionDic.TryRemove(session, out guids);
            connectedCientSession.Remove(session);
            //string result = removeResult ? "成功！" : "失败！";
            //Logger.Debug($"移除客户端{session.SessionID}订阅信息{result}");
            Logger.Debug($"客户端：Session RemoteEndPoint：{session.RemoteEndPoint}已断开! ");
        }

        /// <summary>
        /// 处理Socket发送数据事件
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        private void OnReceived(Session session, ServerRequestInfo requestInfo)
        {
            String jsonStr = requestInfo.Data;
            List<ClientDataEntity> clientDataEntities = JsonConvert.DeserializeObject<List<ClientDataEntity>>(jsonStr);
            foreach (var clientDataEntity in clientDataEntities)
            {
                switch (clientDataEntity.FunctionCode)
                {
                    case FuncCode.Read:
                        {

                            ReadAsync(session, clientDataEntity).Wait();
                            break;
                        }
                    case FuncCode.Write:
                        {
                            WriteAsync(session, clientDataEntity).Wait();
                            break;
                        }
                    case FuncCode.SubScription:
                        {
                            SetSubScriptionAsync(session, clientDataEntities).Wait();
                            break;
                        }
                    default:
                        break;
                }
            }

        }
        #endregion Socket 服务端事件函数

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="session"></param>
        /// <param name="msgBody"></param>
        /// <returns></returns>
        private async Task<bool> SendAsync(Session session, String msgBody)
        {
            bool result = false;
            await Task.Run(() =>
            {
                if (IsStarted)
                {
                    try
                    {
                        session.Send(msgBody);
#if DEBUG
                        Logger.Debug($"发送消息到SessionId {session.SessionID} IP is {session.Config.Ip} Port is {session.Config.Port}。消息： {msgBody} 。");
#endif
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        Logger.Error($"Socket服务SendAsync错误！", ex);

                    }
                }
                else
                {
                    Logger.Debug($"Socket服务已经关闭，不能发送消息。");
                    result = false;
                }
            });
            return result;
        }

        /// <summary>
        /// 设置客户端订阅数据
        /// </summary>
        /// <param name="session"></param>
        /// <param name="socketJsonParamEntity"></param>
        /// <returns></returns>
        private async Task SetSubScriptionAsync(Session session, List<ClientDataEntity> clientDataEntities)
        {
            await Task.Run(() =>
            {
                try
                {
                    subscriptionDic.AddOrUpdate(
                                                  key: session,
                                                  addValue: clientDataEntities,
                                                  updateValueFactory: (oldkey, oldvalue) => clientDataEntities);
                    //clientDataEntities.Select (a=>a.StatusCode = );
                    foreach (var v in clientDataEntities)
                    {
                        v.StatusCode = StatusCodes.Good;
                    }
                }
                catch (ArgumentNullException ex)
                {
                    //socketJsonParamEntity.StatusCode = (uint)DeviceStatusCode.SubscriptionFault;
                    foreach (var v in clientDataEntities)
                    {
                        v.StatusCode = StatusCodes.Bad;
                    }
                    Logger.Error($"设置客户端订阅数据异常！ + \n {ex.ToString()}");
                }
                catch (OverflowException ex)
                {
                    foreach (var v in clientDataEntities)
                    {
                        v.StatusCode = StatusCodes.Bad;
                    }
                    Logger.Error($"设置客户端订阅数据异常！ + \n {ex.ToString()}");
                }

            });

            await SendAsync(session, GetMessage(JsonConvert.SerializeObject(clientDataEntities)));
        }

        /// <summary>
        /// 取得消息的完整内容，增加开始结束符
        /// </summary>
        /// <param name="msgBody"></param>
        /// <returns></returns>
        private String GetMessage(String msgBody)
        {
            return "[STX]" + msgBody + "[ETX]";
        }

        public void RestartServer()
        {
            allDeviceNodes.Clear();
            devices.Clear();
            this.Initialize();

        }

        public void Dispose()
        {
            allDeviceNodes.Clear();
            devices.Clear();
            this.appServer.Stop();
        }
    }
}
