using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.Core.Enum
{
    /// <summary>
    /// 表示客户端类型的枚举
    /// </summary>
    public enum OAuthClientType
    {
        /// <summary>
        /// 网站
        /// </summary>
        [Description("网站")]
        网站 = 1,
        /// <summary>
        /// 桌面/移动 客户端程序
        /// </summary>
        [Description("客户端")]
        桌面或移动客户端 = 2
    }
}
