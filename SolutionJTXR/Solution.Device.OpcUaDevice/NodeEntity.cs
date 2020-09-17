using System;
using System.Collections.Generic;
using System.Text;

namespace Solution.Device.OpcUaDevice
{
    /// <summary>
    /// OPCUA点实体
    /// </summary>
    public class OpcUaNodeEntity : OpcUaDeviceInParamEntity
    {
        /// <summary>
        /// 采集间隔,默认200毫秒
        /// </summary>
        public int Interval { get; set; } = 200;
    }
}
