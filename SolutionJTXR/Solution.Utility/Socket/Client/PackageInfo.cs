using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Utility.Socket
{

    /// <summary>
    /// 数据包
    /// </summary>
    public class PackageInfo : SuperSocket.ProtoBase.IPackageInfo
    {
        public PackageInfo(string data)
        {
            this.Data = data;
        }
        /// <summary>
        /// 包数据
        /// </summary>
        public string Data { get; set; }
    }


}
