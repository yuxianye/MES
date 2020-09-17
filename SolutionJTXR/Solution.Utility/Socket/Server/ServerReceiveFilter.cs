using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using System.Text;

namespace Solution.Utility.Socket.Server
{
    public class ServerReceiveFilter : BeginEndMarkReceiveFilter<ServerRequestInfo>
    {
        private BasicRequestInfoParser m_Parser = new BasicRequestInfoParser();
        private readonly static byte[] BeginMark = Encoding.Default.GetBytes("[STX]");
        private readonly static byte[] EndMark = Encoding.Default.GetBytes("[ETX]");

        public ServerReceiveFilter() : base(BeginMark, EndMark) { }

        protected override ServerRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
        {
            var line = Encoding.Default.GetString(readBuffer, offset + 5, length - 10);
            return new ServerRequestInfo(line);
        }
    }
}
