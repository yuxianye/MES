using Solution.Device.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Device.OpcUaDevice
{
    /// <summary>
    /// OpcUa设备操作返回参数实体基类
    /// </summary>
    public class OpcUaDeviceOutParamEntity : DeviceOutputParamEntityBase
    {
        ///// <summary>
        ///// 订阅数据列表 TODO
        ///// </summary>
        //public List<Tuple<string, object>> SubScriptionValueList { get; set; }


        protected override void Disposing()
        {
            this.NodeId = null;
            this.Value = null;
            this.ValueType = null;
            this.StatusCode = 0;
            //this.SubScriptionValueList.Clear();
            //this.SubScriptionValueList = null;
        }


        public override string ToString()
        {
            //return base.ToString();

            return $"NodeId;{this.NodeId} Value:{this.Value?.ToString()} ValueType:{this.ValueType} StatusCode:{this.StatusCode} Message:{this.Message}";

        }
    }
}
