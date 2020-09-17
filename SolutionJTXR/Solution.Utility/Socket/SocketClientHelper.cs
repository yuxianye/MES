using SuperSocket.ClientEngine;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Utility.Socket
{

    /// <summary>
    /// 客户端套接字
    /// </summary>
    public class SocketClientHelper : Disposable
    {
        /// <summary>
        /// 服务器IP，默认127.0.0.1
        /// </summary>
        public string ServerIp { get; set; } = "127.0.0.1";

        /// <summary>
        /// 服务器端口，默认13805
        /// </summary>
        public int ServerPort { get; set; } = 13805;

        /// <summary>
        /// 错误事件
        /// </summary>
        public event EventHandler<Exception> Error;

        /// <summary>
        /// 已连接事件
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// 已关闭事件
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event EventHandler<DataEventArgs> OnDataReceived;

        /// <summary>
        /// EasyClient实例
        /// </summary>
        private EasyClient easyClient = new EasyClient();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">服务器的ip</param>
        /// <param name="port">服务器的端口</param>
        public SocketClientHelper(string serverIp, int serverPort)
        {
            if (System.Net.IPAddress.TryParse(serverIp, out IPAddress tmpServerIp))
            {
                if (!Equals(null, Error))
                {
                    Error(this, new FormatException("IP地址错误"));
                }
            }
            if (1024 < serverPort && serverPort > 65535)
            {
                if (!Equals(null, Error))
                {
                    Error(this, new FormatException("端口号错误"));
                }
            }
            try
            {
                this.ServerIp = serverIp;
                this.ServerPort = serverPort;
                easyClient.NoDelay = true;
                easyClient.Closed += EasyClient_Closed;
                easyClient.Connected += EasyClient_Connected;
                easyClient.Error += EasyClient_Error;
                easyClient.Initialize<PackageInfo>(new ReceiveFilter(), HandlerPackage);

            }
            catch (Exception ex)
            {
                if (!Equals(null, Error))
                {
                    Error(this, ex);
                }
            }
        }

        private void EasyClient_Error(object sender, ErrorEventArgs e)
        {
            if (!Equals(null, Error))
                Error(this, e.Exception);
        }

        private void EasyClient_Connected(object sender, System.EventArgs e)
        {
            if (!Equals(null, Connected))
                Connected(this, e);
        }

        private void EasyClient_Closed(object sender, System.EventArgs e)
        {
            string error = e.ToString();
            if (!Equals(null, Closed))
                Closed(this, e);
        }

        public Task<bool> ConnectAsync()
        {
            return easyClient.ConnectAsync(new System.Net.IPEndPoint(IPAddress.Parse(ServerIp), ServerPort));
        }

        public Task<bool> CloseAsync()
        {
            return easyClient.Close();
        }

        private void HandlerPackage(PackageInfo packageInfo)
        {
            if (!Equals(null, OnDataReceived))
                OnDataReceived(this, new DataEventArgs(packageInfo));
        }

        public bool Send(string message)
        {
            try
            {
                easyClient.Send(Encoding.Default.GetBytes(message));
                return true;
            }
            catch (Exception ex)
            {
                if (!Equals(null, Error))
                    Error(this, ex);
                return false;
            }
        }

        protected override void Disposing()
        {
            easyClient.Socket.Dispose();
            easyClient.Close();
        }
    }


}
