using Solution.Utility.Socket.Server;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;
using System.IO;
using System.Net;

namespace Solution.Utility.Socket
{
    public enum SocketCloseReason
    {
        Unknown = 0,
        ServerShutdown = 1,
        ClientClosing = 2,
        ServerClosing = 3,
        ApplicationError = 4,
        SocketError = 5,
        TimeOut = 6,
        ProtocolError = 7,
        InternalError = 8
    }

    /// <summary>
    /// 客户端套接字
    /// </summary>
    public class SocketServerHelper : Disposable
    {
        /// <summary>
        /// 错误事件
        /// </summary>
        public event Action<Session, Exception> Error;

        /// <summary>
        /// 已连接事件
        /// </summary>
        public event Action<Session> Connected;

        /// <summary>
        /// 已关闭事件
        /// </summary>
        public event Action<Session, SocketCloseReason> Closed;

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event Action<Session, ServerRequestInfo> DataReceived;

        private SocketServer socketServer = new SocketServer();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">服务器的ip</param>
        /// <param name="port">服务器的端口</param>
        public SocketServerHelper(string serverIp, int serverPort, int maxConnectionNumber)
        {
            if (IPAddress.TryParse(serverIp, out IPAddress tmpServerIp))
            {
                Error?.Invoke(null, new FormatException("IP地址错误"));
            }
            if (1024 < serverPort && serverPort > 65535)
            {
                Error?.Invoke(null, new FormatException("端口号错误"));
            }
            try
            {
                var m_Config = new ServerConfig
                {
                    Ip = serverIp,
                    Port = serverPort,
                    MaxConnectionNumber = maxConnectionNumber,
                    MaxRequestLength = 10240000,
                    Mode = SocketMode.Tcp,
                    Name = "SocketServer",
                    TextEncoding = System.Text.Encoding.Default.BodyName,
                };
                socketServer = new SocketServer();
                if (socketServer.Setup(m_Config))
                {
                    socketServer.NewSessionConnected += OnConnected;
                    socketServer.SessionClosed += OnClosed;
                    socketServer.NewRequestReceived += OnReceived;
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke(null, ex);
            }
        }

        public bool Start()
        {
            bool result = false;
            try
            {
                result = socketServer.Start();
            }
            catch (Exception ex)
            {
                Error?.Invoke(null, ex);
                return false;
            }
            return result;
        }

        public void Stop()
        {
            try
            {
                socketServer?.Stop();
            }
            catch (Exception ex)
            {
                Error?.Invoke(null, ex);
            }
        }

        public void SendMessage(Session session, String message)
        {
            try
            {
                session.Send(message);
            }
            catch (Exception ex)
            {
                Error?.Invoke(session, ex);
            }
        }

        private void OnConnected(Session session)
        {
            Connected?.Invoke(session);
        }

        private void OnClosed(Session session, CloseReason reason)
        {
            SocketCloseReason closeReason = (SocketCloseReason)reason;
            Closed?.Invoke(session, closeReason);
        }

        private void OnReceived(Session session, ServerRequestInfo requestInfo)
        {
            DataReceived?.Invoke(session, requestInfo);
        }

        protected override void Disposing()
        {
            socketServer.Dispose();

        }
    }

    public class SocketServer : AppServer<Session, ServerRequestInfo>
    {
        public SocketServer()
            : base(new DefaultReceiveFilterFactory<ServerReceiveFilter, ServerRequestInfo>())
        {

        }
    }
}
