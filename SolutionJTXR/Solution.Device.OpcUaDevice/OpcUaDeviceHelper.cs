using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Solution.Device.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Solution.Utility.Extensions;

namespace Solution.Device.OpcUaDevice
{
    /// <summary>
    /// Opc设备帮助类
    /// </summary>
    public class OpcUaDeviceHelper : IDevice
    {
        #region Private Fields

        /// <summary>
        /// Ua应用实例
        /// </summary>
        private ApplicationInstance applicationInstance;
        /// <summary>
        /// Ua应用配置
        /// </summary>
        private ApplicationConfiguration applicationConfiguration;
        /// <summary>
        /// Ua会话
        /// </summary>
        private Session session;
        //private Dictionary<int, Subscription> subscriptionDic;
        private int reconnectPeriod = 10;   //重连状态
        private static bool autoAccept = true;
        private bool haveAppCertificate;
        private SessionReconnectHandler sessionReconnectHandler;
        private String serverUrl;
        public event DeviceNotificationEventHandler Notification;
        #endregion

        /// <summary>
        /// 名称，OpcUa/Opc/COM1
        /// </summary>
        public string Name { get; set; }

        private void OnCertificateValidatorNotification(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
            {
                e.Accept = autoAccept;
                if (autoAccept)
                {
                    //WriteLine("Accepted Certificate: {0}", e.Certificate.Subject);
                }
                else
                {
                    //WriteLine("Rejected Certificate: {0}", e.Certificate.Subject);
                }
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <typeparam name="Tin">集成自<seealso cref="DeviceConnectParamEntityBase"/>类型的连接参数</typeparam>
        /// <typeparam name="Tout">集成自<seealso cref="DeviceConnectParamEntityBase"/>类型的输出参数</typeparam>
        /// <param name="connectParam">连接参数</param>
        /// <returns></returns>
        public Task<Tout> Connect2<Tin, Tout>(Tin connectParam) where Tin : DeviceConnectParamEntityBase where Tout : DeviceConnectParamEntityBase
        {
            var connectP = connectParam as OpcUaDeviceConnectParamEntity;
            serverUrl = connectP.DeviceUrl;
            return Task.Run(() =>
            {
                try
                {
                    //实例化应用实例
                    applicationInstance = new ApplicationInstance
                    {
                        ApplicationName = "OpcUaDeviceClient",
                        ApplicationType = ApplicationType.Client,
                        ConfigSectionName = "Solution.OpcUaClient"
                    };
                    //加载配置文件

                    //var vv = System.AppDomain.CurrentDomain.RelativeSearchPath ;

                    //vv = Assembly.GetEntryAssembly().Location;
                    string configFilePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.RelativeSearchPath ?? "", "Solution.OpcUaClient.Config.xml");

                    //string configFilePath = Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.LastIndexOf('\\') + 1)
                    //+ "Solution.OpcUaClient.Config.xml";
                    if (!File.Exists(configFilePath))
                    {
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadConfigurationError, DeviceUrl = connectP.DeviceUrl, Message = $"未找到配置文件：{configFilePath}！" } as object);
                    }

                    applicationConfiguration = applicationInstance.LoadApplicationConfiguration(configFilePath, true).Result;
                    if (Equals(applicationConfiguration, null))
                    {
                        //配置文件错误
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadConfigurationError, DeviceUrl = connectP.DeviceUrl, Message = "配置文件加载失败！" } as object);
                    }
                    //检查安全性
                    haveAppCertificate = applicationInstance.CheckApplicationInstanceCertificate(true, 0).Result;
                    if (!haveAppCertificate)
                    {
                        //未授权则返回认证无效
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadCertificateInvalid, DeviceUrl = connectP.DeviceUrl } as object);
                    }
                    //认证通过,是否自动接受
                    if (haveAppCertificate)
                    {
                        applicationConfiguration.ApplicationUri = Utils.GetApplicationUriFromCertificate(applicationConfiguration.SecurityConfiguration.ApplicationCertificate.Certificate);
                        if (applicationConfiguration.SecurityConfiguration.AutoAcceptUntrustedCertificates)
                        {
                            autoAccept = true;
                        }
                        applicationConfiguration.CertificateValidator.CertificateValidation += new CertificateValidationEventHandler(OnCertificateValidatorNotification);
                    }
                    //else
                    //{

                    //    //未授权则返回认证无效
                    //    return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadCertificateInvalid } as object);
                    //    //Console.WriteLine("    WARN: missing application certificate, using unsecure connection.");

                    //}

                    //断开现有连接
                    if (session != null)
                    {
                        var sc = session.Close(10000);
                        Notification(this, new DeviceEventArgs<IDeviceParam>(new OpcUaDeviceConnectParamEntity() { DeviceUrl = connectP.DeviceUrl, StatusCode = sc.Code, Message = "" }));
                        session = null;
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadSessionNotActivated, DeviceUrl = connectP.DeviceUrl } as object);
                    }
                    if (applicationConfiguration == null)
                    {
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadConfigurationError, DeviceUrl = connectP.DeviceUrl } as object);
                    }

                    var selectedEndpoint = CoreClientUtils.SelectEndpoint(connectP.DeviceUrl, haveAppCertificate, 15000);
                    var endpointConfiguration = EndpointConfiguration.Create(applicationConfiguration);
                    var endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);
                    session = Session.Create(applicationConfiguration, endpoint, false, "OpcUaDeviceClient", 60000, new UserIdentity(new AnonymousIdentityToken()), null).Result;
                    if (session == null)
                    {
                        IsConnected = true;
                        //会话初始化失败
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadSessionNotActivated, DeviceUrl = connectP.DeviceUrl } as object);
                    }
                    else
                    {
                        // 保持连接句柄
                        session.KeepAlive += new KeepAliveEventHandler(OnKeepAliveNotification);
                        IsConnected = true;
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.Good, DeviceUrl = connectP.DeviceUrl, Message = "连接成功！" } as object);
                    }
                }
                catch (Exception ex)
                {
                    return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadUnexpectedError, DeviceUrl = connectP.DeviceUrl, Message = ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace + ex.StackTrace } as object);
                }
            });
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <typeparam name="Tin">集成自<seealso cref="DeviceConnectParamEntityBase"/>类型的连接参数</typeparam>
        /// <typeparam name="Tout">集成自<seealso cref="DeviceConnectParamEntityBase"/>类型的输出参数</typeparam>
        /// <param name="connectParam">连接参数</param>
        /// <returns></returns>
        public Task<Tout> Connect<Tin, Tout>(Tin connectParam) where Tin : DeviceConnectParamEntityBase where Tout : DeviceConnectParamEntityBase
        {
            var connectP = connectParam as OpcUaDeviceConnectParamEntity;
            serverUrl = connectP.DeviceUrl;
            return Task.Run(() =>
            {
                string configFilePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.RelativeSearchPath ?? "", "Solution.OpcUaClient.Config.xml");
                try
                {
                    var certificateValidator = new CertificateValidator();
                    certificateValidator.CertificateValidation += OnCertificateValidatorNotification;
                    applicationInstance = new ApplicationInstance
                    {
                        ApplicationType = ApplicationType.Client,
                        ConfigSectionName = "OpcUaDeviceClient",
                        ApplicationConfiguration = new ApplicationConfiguration
                        {
                            ApplicationUri = "",
                            ApplicationName = "OpcUaDeviceClient",
                            ApplicationType = ApplicationType.Client,
                            CertificateValidator = certificateValidator,
                            ServerConfiguration = new ServerConfiguration
                            {
                                MaxSubscriptionCount = 100000,
                                MaxMessageQueueSize = 100000,
                                MaxNotificationQueueSize = 1002,
                                MaxPublishRequestCount = 100000
                            },
                            SecurityConfiguration = new SecurityConfiguration
                            {
                                AutoAcceptUntrustedCertificates = true,
                            },
                            TransportQuotas = new TransportQuotas
                            {
                                OperationTimeout = 600000,
                                MaxStringLength = 1048576,
                                MaxByteStringLength = 1048576,
                                MaxArrayLength = 65535,
                                MaxMessageSize = 4194304,
                                MaxBufferSize = 65535,
                                ChannelLifetime = 600000,
                                SecurityTokenLifetime = 3600000
                            },
                            ClientConfiguration = new ClientConfiguration
                            {
                                DefaultSessionTimeout = 60000,
                                MinSubscriptionLifetime = 10000
                            },
                            DisableHiResClock = true
                        }
                    };

                    //断开现有连接
                    if (session != null)
                    {
                        var sc = session.Close(10000);
                        Notification(this, new DeviceEventArgs<IDeviceParam>(new OpcUaDeviceConnectParamEntity() { DeviceUrl = connectP.DeviceUrl, StatusCode = sc.Code, Message = "" }));
                        session = null;
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadSessionNotActivated, DeviceUrl = connectP.DeviceUrl } as object);
                    }
                    //if (applicationConfiguration == null)
                    //{
                    //    return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadConfigurationError, DeviceUrl = connectP.DeviceUrl } as object);
                    //}
                    applicationConfiguration = applicationInstance.ApplicationConfiguration;
                    var selectedEndpoint = CoreClientUtils.SelectEndpoint(connectP.DeviceUrl, haveAppCertificate, 15000);
                    var endpointConfiguration = EndpointConfiguration.Create(applicationConfiguration);
                    var endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);
                    session = Session.Create(applicationConfiguration, endpoint, false, "OpcUaDeviceClient", 60000, new UserIdentity(new AnonymousIdentityToken()), null).Result;
                    if (session == null)
                    {
                        IsConnected = true;
                        //会话初始化失败
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadSessionNotActivated, DeviceUrl = connectP.DeviceUrl } as object);
                    }
                    else
                    {
                        // 保持连接句柄
                        session.KeepAlive += new KeepAliveEventHandler(OnKeepAliveNotification);
                        IsConnected = true;
                        return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.Good, DeviceUrl = connectP.DeviceUrl, Message = "连接成功！" } as object);
                    }
                }
                catch (Exception ex)
                {
                    return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.BadUnexpectedError, DeviceUrl = connectP.DeviceUrl, Message = ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace + ex.StackTrace + configFilePath } as object);
                }
            });
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <typeparam name="Tin">集成自<seealso cref="IDeviceParam"/>类型的断开连接参数</typeparam>
        /// <typeparam name="Tout">集成自<seealso cref="IDeviceParam"/>类型的输出参数</typeparam>
        /// <param name="disConnectParam"></param>
        /// <returns></returns>
        public Task<Tout> DisConnect<Tin, Tout>(Tin disConnectParam) where Tin : IDeviceParam where Tout : IDeviceParam
        {
            return Task.Run(() =>
            {
                // stop any reconnect operation.
                if (sessionReconnectHandler != null)
                {
                    sessionReconnectHandler.Dispose();
                    sessionReconnectHandler = null;
                }
                //断开现有连接
                if (session != null)
                {
                    var sc = session.Close(10000);
                    session.Dispose();
                    session = null;
                    IsConnected = false;
                    return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = sc.Code, DeviceUrl = serverUrl, Message = "断开连接！" } as object);
                }
                else
                {
                    IsConnected = false;
                    return (Tout)(new OpcUaDeviceConnectParamEntity() { StatusCode = StatusCodes.Good, DeviceUrl = serverUrl } as object);
                }
            });
        }

        /// <summary>
        /// 向OPC服务器写入数据
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="writeParam"></param>
        /// <returns></returns>
        public Task<Tout> Write<Tin, Tout>(Tin writeParam) where Tin : IDeviceParam where Tout : IDeviceParam
        {
            return Task<Tout>.Factory.StartNew(() =>
            {
                var writeNode = writeParam as DeviceInputParamEntityBase;
                OpcUaDeviceOutParamEntity opcUaDeviceOutParamEntity = new OpcUaDeviceOutParamEntity();
                WriteValue valueToWrite = new WriteValue()
                {
                    NodeId = new NodeId(writeNode.NodeId),
                    AttributeId = Attributes.Value
                };
                valueToWrite.Value.Value = Convert.ChangeType(writeNode.Value, writeNode.ValueType ?? typeof(object));
                //valueToWrite.Value.Value = writeNode.Value;
                valueToWrite.Value.StatusCode = StatusCodes.Good;
                valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
                valueToWrite.Value.SourceTimestamp = DateTime.MinValue;
                WriteValueCollection valuesToWrite = new WriteValueCollection
                {
                    valueToWrite
                };
                try
                {
                    session.Write(null, valuesToWrite, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);
                    ClientBase.ValidateResponse(results, valuesToWrite);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite);
                    opcUaDeviceOutParamEntity.NodeId = writeNode.NodeId;
                    opcUaDeviceOutParamEntity.Value = writeNode.Value;
                    opcUaDeviceOutParamEntity.StatusCode = results[0].Code;
                    opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(results[0].Code);
                    opcUaDeviceOutParamEntity.ValueType = valueToWrite.Value.WrappedValue.TypeInfo.BuiltInType.GetTypeCode().ToType();
                }
                catch (ServiceResultException e)
                {
                    opcUaDeviceOutParamEntity.Value = e.LocalizedText;
                    opcUaDeviceOutParamEntity.StatusCode = e.StatusCode;
                    opcUaDeviceOutParamEntity.Message = e.Message + e.StackTrace;
                }
                catch (Exception ex)
                {
                    opcUaDeviceOutParamEntity.StatusCode = StatusCodes.BadUnexpectedError;
                    opcUaDeviceOutParamEntity.Value = null;
                    opcUaDeviceOutParamEntity.Message = ex.Message + ex.StackTrace;
                }
                DeviceEventArgs<IDeviceParam> args = new DeviceEventArgs<IDeviceParam>(opcUaDeviceOutParamEntity);
                Notification?.Invoke(this, args);
                return (Tout)(opcUaDeviceOutParamEntity as object);
            });
        }

        /// <summary>
        /// 从OPC服务器读取数据
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="readParam"></param>
        /// <returns></returns>
        public Task<Tout> Read<Tin, Tout>(Tin readParam) where Tin : IDeviceParam where Tout : IDeviceParam
        {
            return Task<Tout>.Factory.StartNew(() =>
            {
                var readNode = readParam as DeviceInputParamEntityBase;
                OpcUaDeviceOutParamEntity opcUaDeviceOutParamEntity = new OpcUaDeviceOutParamEntity();
                ReadValueId nodeToRead = new ReadValueId()
                {
                    NodeId = new NodeId(readNode.NodeId),
                    AttributeId = Attributes.Value
                };
                ReadValueIdCollection nodesToRead = new ReadValueIdCollection
                {
                    nodeToRead
                };
                try
                {
                    session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos);
                    ClientBase.ValidateResponse(results, nodesToRead);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);
                    opcUaDeviceOutParamEntity.StatusCode = results[0].StatusCode.Code;
                    opcUaDeviceOutParamEntity.Message = results[0].StatusCode.ToString();
                    opcUaDeviceOutParamEntity.Value = results[0].Value;
                    opcUaDeviceOutParamEntity.NodeId = readNode.NodeId;
                    opcUaDeviceOutParamEntity.ValueType = results[0].WrappedValue.TypeInfo.BuiltInType.GetTypeCode().ToType();
                }
                catch (ServiceResultException e)
                {
                    opcUaDeviceOutParamEntity.Value = e.LocalizedText;
                    opcUaDeviceOutParamEntity.StatusCode = e.StatusCode;
                    opcUaDeviceOutParamEntity.Message = e.StatusCode.ToString() + e.Message + e.StackTrace;
                }
                catch (Exception ex)
                {
                    opcUaDeviceOutParamEntity.Value = null;
                    opcUaDeviceOutParamEntity.StatusCode = StatusCodes.BadUnexpectedError;
                    opcUaDeviceOutParamEntity.Message = "BadUnexpectedError" + ex.Message + ex.StackTrace;
                }
                DeviceEventArgs<IDeviceParam> args = new DeviceEventArgs<IDeviceParam>(opcUaDeviceOutParamEntity);
                Notification?.Invoke(this, args);
                return (Tout)(opcUaDeviceOutParamEntity as object);
            });
        }

        /// <summary>
        /// 批量读取
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="writeParams"></param>
        /// <returns></returns>
        public Task<Tout[]> Writes<Tin, Tout>(Tin[] writeParams) where Tin : IDeviceParam where Tout : IDeviceParam
        {
            return Task<Tout[]>.Factory.StartNew(() =>
            {
                var writeNodes = writeParams as DeviceInputParamEntityBase[];
                OpcUaDeviceOutParamEntity[] opcUaDeviceOutParamEntitys = new OpcUaDeviceOutParamEntity[writeNodes.Count()];
                WriteValueCollection valuesToWrite = new WriteValueCollection(writeNodes.Count());
                foreach (var writeNode in writeNodes)
                {
                    WriteValue valueToWrite = new WriteValue();
                    valueToWrite.NodeId = new NodeId(writeNode.NodeId);
                    valueToWrite.AttributeId = Attributes.Value;
                    valueToWrite.Value.Value = Convert.ChangeType(writeNode.Value, writeNode.ValueType ?? typeof(object));
                    valueToWrite.Value.StatusCode = StatusCodes.Good;
                    valueToWrite.Value.ServerTimestamp = DateTime.MinValue;
                    valueToWrite.Value.SourceTimestamp = DateTime.MinValue;
                    valuesToWrite.Add(valueToWrite);
                }
                try
                {
                    session.Write(null, valuesToWrite, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);
                    ClientBase.ValidateResponse(results, valuesToWrite);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, valuesToWrite);
                    for (int i = 0; i < results.Count; i++)
                    {
                        OpcUaDeviceOutParamEntity opcUaDeviceOutParamEntity = new OpcUaDeviceOutParamEntity();
                        opcUaDeviceOutParamEntity.NodeId = writeNodes[i].NodeId;
                        opcUaDeviceOutParamEntity.Value = writeNodes[i].Value;
                        opcUaDeviceOutParamEntity.ValueType = valuesToWrite[i].Value.WrappedValue.TypeInfo.BuiltInType.GetTypeCode().ToType();
                        opcUaDeviceOutParamEntity.StatusCode = results[i].Code;
                        opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(results[i].Code);
                        opcUaDeviceOutParamEntitys[i] = opcUaDeviceOutParamEntity;
                    }
                }
                catch (ServiceResultException e)
                {
                    opcUaDeviceOutParamEntitys[0] = new OpcUaDeviceOutParamEntity()
                    {
                        Value = e.LocalizedText,
                        StatusCode = e.StatusCode,
                        Message = e.StatusCode.ToString() + e.Message + e.StackTrace,
                    };
                }
                catch (Exception ex)
                {
                    opcUaDeviceOutParamEntitys[0] = new OpcUaDeviceOutParamEntity()
                    {
                        Value = null,
                        StatusCode = StatusCodes.BadUnexpectedError,
                        Message = "BadUnexpectedError" + ex.Message + ex.StackTrace,

                    };
                }
                return (Tout[])(opcUaDeviceOutParamEntitys as object);
            });
        }

        /// <summary>
        /// 批量写入
        /// </summary>
        /// <typeparam name="Tin"></typeparam>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="readParams"></param>
        /// <returns></returns>
        public Task<Tout[]> Reads<Tin, Tout>(Tin[] readParams) where Tin : IDeviceParam where Tout : IDeviceParam
        {
            return Task<Tout[]>.Factory.StartNew(() =>
            {
                var readNodes = readParams as DeviceInputParamEntityBase[];
                OpcUaDeviceOutParamEntity[] opcUaDeviceOutParamEntitys = new OpcUaDeviceOutParamEntity[readNodes.Count()];
                ReadValueIdCollection nodesToRead = new ReadValueIdCollection(readNodes.Count());
                foreach (var readNode in readNodes)
                {
                    ReadValueId nodeToRead = new ReadValueId()
                    {
                        NodeId = new NodeId(readNode.NodeId),
                        AttributeId = Attributes.Value
                    };
                    nodesToRead.Add(nodeToRead);
                }
                try
                {
                    session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos);
                    ClientBase.ValidateResponse(results, nodesToRead);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);
                    for (int i = 0; i < results.Count; i++)
                    {
                        OpcUaDeviceOutParamEntity opcUaDeviceOutParamEntity = new OpcUaDeviceOutParamEntity();
                        opcUaDeviceOutParamEntity.Message = results[i].StatusCode.ToString();
                        opcUaDeviceOutParamEntity.NodeId = readNodes[i].NodeId;
                        opcUaDeviceOutParamEntity.Value = results[i].Value;
                        opcUaDeviceOutParamEntity.StatusCode = results[i].StatusCode.Code;
                        opcUaDeviceOutParamEntity.ValueType = results[i].WrappedValue.TypeInfo.BuiltInType.GetTypeCode().ToType();
                        opcUaDeviceOutParamEntitys[i] = opcUaDeviceOutParamEntity;
                    }
                }
                catch (ServiceResultException e)
                {
                    opcUaDeviceOutParamEntitys[0] = new OpcUaDeviceOutParamEntity()
                    {
                        Value = e.LocalizedText,
                        StatusCode = e.StatusCode,
                        Message = e.Message + e.StackTrace,
                    };
                }
                catch (Exception ex)
                {
                    opcUaDeviceOutParamEntitys[0] = new OpcUaDeviceOutParamEntity()
                    {
                        Value = null,
                        StatusCode = StatusCodes.BadUnexpectedError,
                        Message = ex.Message + ex.StackTrace,
                    };
                }
                return (Tout[])(opcUaDeviceOutParamEntitys as object);
            });
        }

        /// <summary>
        /// 处理保持连接心跳事件
        /// </summary>
        private void OnKeepAliveNotification(Session session, KeepAliveEventArgs e)
        {
            try
            {
                if (!Object.ReferenceEquals(this.session, session))
                {
                    return;
                }
                OpcUaDeviceConnectParamEntity opcUaDeviceConnectParamEntity = new OpcUaDeviceConnectParamEntity();
                if (e.Status != null && ServiceResult.IsNotGood(e.Status))
                {
                    if (reconnectPeriod <= 0)
                    {
                        opcUaDeviceConnectParamEntity.StatusCode = e.Status.Code;
                        opcUaDeviceConnectParamEntity.Message = "通讯错误！" + StatusCodes.GetBrowseName(e.Status.Code);
                        DeviceEventArgs<IDeviceParam> args = new DeviceEventArgs<IDeviceParam>(opcUaDeviceConnectParamEntity);
                        IsConnected = false;
                        Notification?.Invoke(this, args);
                        return;
                    }
                    if (sessionReconnectHandler == null)
                    {
                        opcUaDeviceConnectParamEntity.Message = $"{reconnectPeriod}重新连接中... {StatusCodes.GetBrowseName(e.Status.Code)}";
                        Notification?.Invoke(this, new DeviceEventArgs<IDeviceParam>(opcUaDeviceConnectParamEntity));
                        sessionReconnectHandler = new SessionReconnectHandler();
                        sessionReconnectHandler.BeginReconnect(session, reconnectPeriod * 1000, Server_ReconnectComplete);
                    }
                    return;
                }
                IsConnected = true;
            }
            catch (Exception ex)
            {
                Notification?.Invoke(this, new DeviceEventArgs<IDeviceParam>(new OpcUaDeviceConnectParamEntity()
                {
                    StatusCode = StatusCodes.BadUnexpectedError,
                    Message = ex.Message + ex.StackTrace
                }));
            }
        }

        /// <summary>
        /// 处理OPC UA服务器重连事件
        /// </summary>
        private void Server_ReconnectComplete(object sender, EventArgs e)
        {
            OpcUaDeviceConnectParamEntity opcUaDeviceConnectParamEntity = new OpcUaDeviceConnectParamEntity();
            opcUaDeviceConnectParamEntity.DeviceUrl = serverUrl;
            try
            {
                if (!ReferenceEquals(sender, sessionReconnectHandler))
                {
                    return;
                }
                session = sessionReconnectHandler.Session;
                sessionReconnectHandler.Dispose();
                sessionReconnectHandler = null;
                opcUaDeviceConnectParamEntity.StatusCode = StatusCodes.Good;
            }
            catch (Exception exception)
            {
                opcUaDeviceConnectParamEntity.StatusCode = StatusCodes.Bad;
                opcUaDeviceConnectParamEntity.Message = exception.Message + exception.StackTrace;
            }
            DeviceEventArgs<IDeviceParam> args = new DeviceEventArgs<IDeviceParam>(opcUaDeviceConnectParamEntity);
            Notification?.Invoke(this, args);
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnected { get; private set; } = false;

        public Task<Tout> RegisterNodes<Tout>(dynamic[] nodes) where Tout : DeviceOutputParamEntityBase
        {
            return Task.Run<Tout>(new Func<Tout>(() =>
            {
                OpcUaDeviceOutParamEntity opcUaDeviceOutParamEntity = new OpcUaDeviceOutParamEntity();
                if (session == null)
                {
                    opcUaDeviceOutParamEntity.StatusCode = StatusCodes.BadSessionIdInvalid;
                    opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode);
                    return (Tout)(opcUaDeviceOutParamEntity as object);
                }
                try
                {
                    if (Equals(null, nodes) && nodes.Count() < 1)
                    {
                        //没有数据
                        opcUaDeviceOutParamEntity.StatusCode = StatusCodes.GoodNoData;
                        opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode);
                        return (Tout)(opcUaDeviceOutParamEntity as object);
                    }
                    // 根据更新频率分组
                    var OpcUaNodeGroups = from a in nodes
                                          group a by a.Interval into g
                                          select new { Interval = g.Key, OpcUaNodes = g };
                    foreach (var OpcUaNodeGroup in OpcUaNodeGroups)
                    {
                        var subscription = session.Subscriptions.FirstOrDefault(a => a.DisplayName == OpcUaNodeGroup.Interval.ToString());
                        //未找到已经订阅的组，那么新建组并添加订阅项,并关联监视事件句柄
                        if (Equals(null, subscription))
                        {
                            #region 增加订阅
                            //创建订阅组。订阅组状态的名称是更新频率
                            //subscription = new Subscription();
                            subscription = new Subscription(session.DefaultSubscription);
                            bool isAddSession = session.AddSubscription(subscription);
                            subscription.Create();
                            subscription.PublishingInterval = OpcUaNodeGroup.Interval;
                            subscription.PublishingEnabled = true;
                            subscription.LifetimeCount = 0;
                            subscription.KeepAliveCount = 0;
                            subscription.DisplayName = OpcUaNodeGroup.Interval.ToString();
                            subscription.Priority = byte.MaxValue;
                            List<MonitoredItem> monitoredItems = new List<MonitoredItem>();

                            foreach (var v in OpcUaNodeGroup.OpcUaNodes)
                            {
                                monitoredItems.Add(new MonitoredItem()
                                {
                                    StartNodeId = new NodeId(v.NodeId),
                                });
                            }
                            //关联监视
                            //monitoredItems.ForEach(a => a.Notification += OnMonitoredNotification);
                            foreach (var monitoredItem in monitoredItems)
                            {
                                monitoredItem.Notification += OnMonitoredNotification;
                            }
                            subscription.AddItems(monitoredItems);
                            subscription.ApplyChanges();

                            if (isAddSession)
                            {
                                opcUaDeviceOutParamEntity.StatusCode = StatusCodes.Good;
                                opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode);
                                //return opcUaDeviceOutParamEntity;
                            }
                            else
                            {
                                opcUaDeviceOutParamEntity.StatusCode = StatusCodes.BadSessionIdInvalid;
                                opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode);
                                //return opcUaDeviceOutParamEntity;
                            }
                            #endregion 增加订阅
                            //subscription.FastDataChangeCallback += fastDataChangeNotificationEventHandler;
                        }
                        else//已经有订阅组，那么更新订阅项,没有的订阅移除，原有的订阅保留，新增的订阅增加
                        {
                            //查询要删除的点
                            #region 更新订阅

                            //移除
                            //查询要删除的点
                            List<MonitoredItem> deleteItems = new List<MonitoredItem>();
                            foreach (var v in subscription.MonitoredItems)
                            {
                                if (!OpcUaNodeGroup.OpcUaNodes.Any(a => a.NodeId == v.StartNodeId))
                                {
                                    deleteItems.Add(v);
                                }
                            }
                            deleteItems.ForEach(a => a.Notification -= OnMonitoredNotification);
                            System.Diagnostics.Debug.Print($"要删除的点数{deleteItems.Count }");

                            if (!Equals(null, deleteItems) && deleteItems.Count() > 0)
                            {
                                subscription.RemoveItems(deleteItems);
                                System.Diagnostics.Debug.Print($"已经删除的点数{deleteItems.Count }");
                                subscription.ApplyChanges();
                            }
                            //新增
                            //查询要新增加的点，
                            IList<OpcUaNodeEntity> addItems = new List<OpcUaNodeEntity>();
                            foreach (var v in OpcUaNodeGroup.OpcUaNodes)
                            {
                                if (!subscription.MonitoredItems.Any(a => a.StartNodeId.ToString() == v.NodeId))
                                {
                                    addItems.Add(v);
                                }
                            }
                            var addMonitoredItems = from a in addItems
                                                    select new MonitoredItem
                                                    {
                                                        StartNodeId = a.NodeId,
                                                        AttributeId = Attributes.Value,
                                                        DisplayName = a.NodeId,
                                                        SamplingInterval = OpcUaNodeGroup.Interval,
                                                        MonitoringMode = MonitoringMode.Reporting,
                                                    };
                            //关联监视
                            //addMonitoredItems.ToList().ForEach(a => a.Notification += OnMonitoredNotification);
                            foreach (var monitoredItem in addMonitoredItems)
                            {
                                monitoredItem.Notification += OnMonitoredNotification;
                            }
                            System.Diagnostics.Debug.Print($"要增加的点数{addMonitoredItems.Count() }");
                            subscription.AddItems(addMonitoredItems);
                            System.Diagnostics.Debug.Print($"已经增加的点数{addMonitoredItems.Count() }");
                            //subscription.Create();
                            subscription.ApplyChanges();
                            #endregion 增加订阅
                        }
                    }//endforeach

                    //删除订阅项后没有item了，那么删除订阅subscription
                    for (int i = session.Subscriptions.Count() - 1; i >= 0; i--)
                    {
                        var subscription = session.Subscriptions.ElementAt(i);
                        System.Diagnostics.Debug.Print($"subscription:{subscription.DisplayName}MonitoredItemCount{subscription.MonitoredItemCount}");

                        if (subscription.MonitoredItemCount < 1)
                        {
                            subscription.Delete(false);
                            session.RemoveSubscription(subscription);
                            System.Diagnostics.Debug.Print($"subscription:{subscription.DisplayName} MonitoredItemCount:{subscription.MonitoredItemCount}");
                        }
                        //订阅组组减少时，需要删除组内的监视项，释放Notification，删除，订阅项
                        if (!OpcUaNodeGroups.Any(a => a.Interval.ToString() == subscription.DisplayName))
                        {
                            if (subscription.MonitoredItemCount > 0)
                            {
                                foreach (var monitoredItem in subscription.MonitoredItems)
                                {
                                    monitoredItem.Notification -= OnMonitoredNotification;
                                }
                                System.Diagnostics.Debug.Print($"删除的点数{subscription.MonitoredItems.Count() }");
                                subscription.RemoveItems(subscription.MonitoredItems);
                            }
                            subscription.Delete(false);
                            session.RemoveSubscription(subscription);
                        }
                    }
#if DEBUG
                    System.Diagnostics.Debug.Print("调试信息");

                    foreach (var subs in session.Subscriptions)
                    {
                        System.Diagnostics.Debug.Print("订阅项：" + subs.DisplayName);
                        foreach (var mi in subs.MonitoredItems)
                        {
                            //System.Diagnostics.Debug.Print(mi.StartNodeId.ToString());
                        }
                    }
#endif
                    return (Tout)(opcUaDeviceOutParamEntity as object);
                }
                catch (ServiceResultException e)
                {
                    opcUaDeviceOutParamEntity.StatusCode = e.StatusCode;
                    opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode) + e.Message + e.StackTrace;
                    return (Tout)(opcUaDeviceOutParamEntity as object);
                }
                catch (Exception ex)
                {
                    opcUaDeviceOutParamEntity.StatusCode = StatusCodes.BadUnexpectedError;
                    opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode) + ex.Message + ex.StackTrace;
                    return (Tout)(opcUaDeviceOutParamEntity as object);
                }
            }));
        }

        /// <summary>
        /// 创建订阅
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Task<OpcUaDeviceOutParamEntity> RegisterNodes123(OpcUaNodeEntity[] nodes)
        {
            return Task.Run<OpcUaDeviceOutParamEntity>(new Func<OpcUaDeviceOutParamEntity>(() =>
            {
                OpcUaDeviceOutParamEntity opcUaDeviceOutParamEntity = new OpcUaDeviceOutParamEntity();
                if (session == null)
                {
                    opcUaDeviceOutParamEntity.StatusCode = StatusCodes.BadSessionIdInvalid;
                    opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode);
                    return opcUaDeviceOutParamEntity;
                }
                try
                {
                    if (Equals(null, nodes) && nodes.Count() < 1)
                    {
                        //没有数据
                        opcUaDeviceOutParamEntity.StatusCode = StatusCodes.GoodNoData;
                        opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode);
                        return opcUaDeviceOutParamEntity;
                    }
                    // 根据更新频率分组
                    var OpcUaNodeGroups = from a in nodes
                                          group a by a.Interval into g
                                          select new { Interval = g.Key, OpcUaNodes = g };
                    foreach (var OpcUaNodeGroup in OpcUaNodeGroups)
                    {
                        var subscription = session.Subscriptions.FirstOrDefault(a => a.DisplayName == OpcUaNodeGroup.Interval.ToString());
                        //未找到已经订阅的组，那么新建组并添加订阅项,并关联监视事件句柄
                        if (Equals(null, subscription))
                        {
                            #region 增加订阅
                            //创建订阅组。订阅组状态的名称是更新频率
                            //subscription = new Subscription();
                            subscription = new Subscription(session.DefaultSubscription);
                            bool isAddSession = session.AddSubscription(subscription);
                            subscription.Create();
                            subscription.PublishingInterval = OpcUaNodeGroup.Interval;
                            subscription.PublishingEnabled = true;
                            subscription.LifetimeCount = 0;
                            subscription.KeepAliveCount = 0;
                            subscription.DisplayName = OpcUaNodeGroup.Interval.ToString();
                            subscription.Priority = byte.MaxValue;
                            List<MonitoredItem> monitoredItems = new List<MonitoredItem>();

                            foreach (var v in OpcUaNodeGroup.OpcUaNodes)
                            {
                                monitoredItems.Add(new MonitoredItem()
                                {
                                    StartNodeId = new NodeId(v.NodeId),
                                });
                            }
                            //关联监视
                            //monitoredItems.ForEach(a => a.Notification += OnMonitoredNotification);
                            foreach (var monitoredItem in monitoredItems)
                            {
                                monitoredItem.Notification += OnMonitoredNotification;
                            }
                            subscription.AddItems(monitoredItems);
                            subscription.ApplyChanges();

                            if (isAddSession)
                            {
                                opcUaDeviceOutParamEntity.StatusCode = StatusCodes.Good;
                                opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode);
                                //return opcUaDeviceOutParamEntity;
                            }
                            else
                            {
                                opcUaDeviceOutParamEntity.StatusCode = StatusCodes.BadSessionIdInvalid;
                                opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode);
                                //return opcUaDeviceOutParamEntity;
                            }
                            #endregion 增加订阅
                            //subscription.FastDataChangeCallback += fastDataChangeNotificationEventHandler;
                        }
                        else//已经有订阅组，那么更新订阅项,没有的订阅移除，原有的订阅保留，新增的订阅增加
                        {
                            //查询要删除的点
                            #region 更新订阅

                            //移除
                            //查询要删除的点
                            List<MonitoredItem> deleteItems = new List<MonitoredItem>();
                            foreach (var v in subscription.MonitoredItems)
                            {
                                if (!OpcUaNodeGroup.OpcUaNodes.Any(a => a.NodeId == v.StartNodeId))
                                {
                                    deleteItems.Add(v);
                                }
                            }
                            deleteItems.ForEach(a => a.Notification -= OnMonitoredNotification);
                            System.Diagnostics.Debug.Print($"要删除的点数{deleteItems.Count }");

                            if (!Equals(null, deleteItems) && deleteItems.Count() > 0)
                            {
                                subscription.RemoveItems(deleteItems);
                                System.Diagnostics.Debug.Print($"已经删除的点数{deleteItems.Count }");
                                subscription.ApplyChanges();
                            }
                            //新增
                            //查询要新增加的点，
                            IList<OpcUaNodeEntity> addItems = new List<OpcUaNodeEntity>();
                            foreach (var v in OpcUaNodeGroup.OpcUaNodes)
                            {
                                if (!subscription.MonitoredItems.Any(a => a.StartNodeId.ToString() == v.NodeId))
                                {
                                    addItems.Add(v);
                                }
                            }
                            var addMonitoredItems = from a in addItems
                                                    select new MonitoredItem
                                                    {
                                                        StartNodeId = a.NodeId,
                                                        AttributeId = Attributes.Value,
                                                        DisplayName = a.NodeId,
                                                        SamplingInterval = OpcUaNodeGroup.Interval,
                                                        MonitoringMode = MonitoringMode.Reporting,
                                                    };
                            //关联监视
                            //addMonitoredItems.ToList().ForEach(a => a.Notification += OnMonitoredNotification);
                            foreach (var monitoredItem in addMonitoredItems)
                            {
                                monitoredItem.Notification += OnMonitoredNotification;
                            }
                            System.Diagnostics.Debug.Print($"要增加的点数{addMonitoredItems.Count() }");
                            subscription.AddItems(addMonitoredItems);
                            System.Diagnostics.Debug.Print($"已经增加的点数{addMonitoredItems.Count() }");
                            //subscription.Create();
                            subscription.ApplyChanges();
                            #endregion 增加订阅
                        }
                    }//endforeach

                    //删除订阅项后没有item了，那么删除订阅subscription
                    for (int i = session.Subscriptions.Count() - 1; i >= 0; i--)
                    {
                        var subscription = session.Subscriptions.ElementAt(i);
                        System.Diagnostics.Debug.Print($"subscription:{subscription.DisplayName}MonitoredItemCount{subscription.MonitoredItemCount}");

                        if (subscription.MonitoredItemCount < 1)
                        {
                            subscription.Delete(false);
                            session.RemoveSubscription(subscription);
                            System.Diagnostics.Debug.Print($"subscription:{subscription.DisplayName} MonitoredItemCount:{subscription.MonitoredItemCount}");
                        }
                        //订阅组组减少时，需要删除组内的监视项，释放Notification，删除，订阅项
                        if (!OpcUaNodeGroups.Any(a => a.Interval.ToString() == subscription.DisplayName))
                        {
                            if (subscription.MonitoredItemCount > 0)
                            {
                                foreach (var monitoredItem in subscription.MonitoredItems)
                                {
                                    monitoredItem.Notification -= OnMonitoredNotification;
                                }
                                System.Diagnostics.Debug.Print($"删除的点数{subscription.MonitoredItems.Count() }");
                                subscription.RemoveItems(subscription.MonitoredItems);
                            }
                            subscription.Delete(false);
                            session.RemoveSubscription(subscription);
                        }
                    }
#if DEBUG
                    System.Diagnostics.Debug.Print("调试信息");

                    foreach (var subs in session.Subscriptions)
                    {
                        System.Diagnostics.Debug.Print("订阅项：" + subs.DisplayName);
                        foreach (var mi in subs.MonitoredItems)
                        {
                            //System.Diagnostics.Debug.Print(mi.StartNodeId.ToString());
                        }
                    }
#endif
                    return opcUaDeviceOutParamEntity;
                }
                catch (ServiceResultException e)
                {
                    opcUaDeviceOutParamEntity.StatusCode = e.StatusCode;
                    opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode) + e.Message + e.StackTrace;
                    return opcUaDeviceOutParamEntity;
                }
                catch (Exception ex)
                {
                    opcUaDeviceOutParamEntity.StatusCode = StatusCodes.BadUnexpectedError;
                    opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(opcUaDeviceOutParamEntity.StatusCode) + ex.Message + ex.StackTrace;
                    return opcUaDeviceOutParamEntity;
                }
            }));
        }

        private void fastDataChangeNotificationEventHandler(Subscription subscription, DataChangeNotification notification, IList<string> stringTable)
        {
            System.Threading.Tasks.Parallel.ForEach<MonitoredItemNotification>(notification.MonitoredItems, (monitoredItemNotification) =>
            {

                MonitoredItem item = subscription.FindItemByClientHandle(monitoredItemNotification.ClientHandle);
                if (item == null)
                {
                    return;
                }
                //var monitoredItemNotification = item.LastValue as MonitoredItemNotification;
                OpcUaDeviceOutParamEntity opcUaDeviceOutParamEntity = new OpcUaDeviceOutParamEntity();
                opcUaDeviceOutParamEntity.NodeId = item.StartNodeId.ToString();
                opcUaDeviceOutParamEntity.StatusCode = monitoredItemNotification.Value.StatusCode.Code;
                //opcUaDeviceOutParamEntity.Message = 
                //opcUaDeviceOutParamEntity.SubScriptionValueList = new List<Tuple<string, object>>();
                opcUaDeviceOutParamEntity.Value = monitoredItemNotification.Value.Value;

                DeviceEventArgs<IDeviceParam> args = new DeviceEventArgs<IDeviceParam>(opcUaDeviceOutParamEntity);
                Notification?.BeginInvoke(this, args, new AsyncCallback((a) =>
                {
                    if (a.IsCompleted)
                    {
                        Notification?.EndInvoke(a);
                    }
                }), null);
            });
        }

        /// <summary>
        /// 处理订阅事件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e"></param>
        private void OnMonitoredNotification(MonitoredItem item, MonitoredItemNotificationEventArgs e)
        {
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            //System.Diagnostics.Debug.Print($"值变化 NodeId:{item.StartNodeId.ToString() } 值：{notification.Value.Value.ToString()}");
            //OpcUaDeviceOutParamEntity[] opcUaDeviceOutParamEntities = new OpcUaDeviceOutParamEntity[1];
            var node = e.NotificationValue as NodeId;
            OpcUaDeviceOutParamEntity opcUaDeviceOutParamEntity = new OpcUaDeviceOutParamEntity();
            opcUaDeviceOutParamEntity.StatusCode = StatusCodes.Good;// e.(uint)DeviceStatusCode.SubscriptionOK;
            //opcUaDeviceOutParamEntity.SubScriptionValueList = new List<Tuple<string, object>>();
            opcUaDeviceOutParamEntity.NodeId = item.ResolvedNodeId.ToString();
            opcUaDeviceOutParamEntity.Value = notification.Value.Value;
            opcUaDeviceOutParamEntity.Message = StatusCodes.GetBrowseName(StatusCodes.Good);
            //opcUaDeviceOutParamEntities[1] = opcUaDeviceOutParamEntity;
            //foreach (var value in item.DequeueValues())
            //{
            //opcUaDeviceOutParamEntity.SubScriptionValueList.Add(Tuple.Create(item.ResolvedNodeId.ToString(), notification.Value.Value));
            //opcUaDeviceOutParamEntity.SubScriptionValueList.Add(tupe);
            //}

            DeviceEventArgs<IDeviceParam> args = new DeviceEventArgs<IDeviceParam>(opcUaDeviceOutParamEntity);

            Notification?.BeginInvoke(this, args, new AsyncCallback((a) =>
            {
                ////(a.AsyncState as DeviceNotificationEventHandler).EndInvoke ()
                if (a.IsCompleted)
                {
                    Notification?.EndInvoke(a);
                }
            }), null);
        }

        #region IDisposable
        private bool _disposed;

        /// <summary>
        /// 释放对象，用于外部调用
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放当前对象时释放资源
        /// </summary>
        ~OpcUaDeviceHelper()
        {
            Dispose(false);
        }

        /// <summary>
        /// 重写以实现释放对象的逻辑
        /// </summary>
        /// <param name="disposing">是否要释放对象</param>
        protected virtual void Dispose(bool disposing)
        {

            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                DisConnect<IDeviceParam, IDeviceParam>(null);
                this.session = null;
                //this.subscriptionDic.Clear();
                this.applicationConfiguration = null;
                this.applicationInstance = null;
                this.serverUrl = null;
                this.sessionReconnectHandler = null;
            }
            _disposed = true;


        }

        #endregion


        public override string ToString()
        {
            return $"ServerUrl:{this.serverUrl }";
        }
    }
}
