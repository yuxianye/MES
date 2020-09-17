using Solution.Device.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Device.OpcUaDevice
{
    /// <summary>
    /// OpcUa设备连接参数实体
    /// </summary>
    public class OpcUaDeviceConnectParamEntity : DeviceConnectParamEntityBase
    {
        protected override void Disposing()
        {
            this.DeviceUrl = null;
        }

        public override string ToString()
        {
            return $"DeviceUrl;{this.DeviceUrl} StatusCode:{this.StatusCode} Message:{this.Message}";
        }
    }
}
