using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;

namespace Solution.Utility.Socket
{
    /// <summary>
    /// 接收过滤器
    /// </summary>
    internal class ReceiveFilter : BeginEndMarkReceiveFilter<PackageInfo>
    {
        public ReceiveFilter() : base(Encoding.Default.GetBytes("[STX]"), Encoding.Default.GetBytes("[ETX]"))
        {

        }


        public override PackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            String infoStr = Encoding.Default.GetString(bufferStream.Buffers, 5, (int)bufferStream.Length - 10);
            return new PackageInfo(infoStr);
        }
    }


}
