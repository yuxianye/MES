using Solution.Device.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Device.OpcUaDevice
{
    /// <summary>
    /// OpcUa设备操作输入参数实体基类
    /// </summary>
    public class OpcUaDeviceInParamEntity : DeviceInputParamEntityBase
    {
        ///// <summary>
        ///// 服务器地址
        ///// </summary>
        protected override void Disposing()
        {
            this.NodeId = null;
            this.Value = null;
            this.ValueType = null;
            this.Message = null;
        }
    }
}
