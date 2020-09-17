using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Utility.Socket
{
    public class Session : AppSession<Session, ServerRequestInfo>
    {
        public override void Send(string message)
        {
            base.Send(message);
        }
    }
}
