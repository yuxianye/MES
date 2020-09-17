using SuperSocket.SocketBase.Protocol;

namespace Solution.Utility.Socket
{
    /// <summary>
    /// 服务端数据包
    /// </summary>
    public class ServerRequestInfo : IRequestInfo
    {
        public ServerRequestInfo(string data)
        {
            this.Data = data;
        }
        /// <summary>
        /// 包数据
        /// </summary>
        public string Data { get; set; }

        public string Key { get; set; }
    }
}
