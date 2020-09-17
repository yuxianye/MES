using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Utility.Socket
{

    /// <summary>
    /// 数据事件参数
    /// </summary>
    public class DataEventArgs : System.EventArgs
    {
        public DataEventArgs(PackageInfo packageInfo)
        {
            PackageInfo = packageInfo;
        }

        /// <summary>
        /// 数据包
        /// </summary>
        public PackageInfo PackageInfo { get; set; }
    }
}
