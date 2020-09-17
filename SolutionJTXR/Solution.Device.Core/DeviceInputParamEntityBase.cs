using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Device.Core
{
    /// <summary>
    /// 设备操作输入参数实体基类
    /// </summary>
    public class DeviceInputParamEntityBase : DeviceParamEntityBase
    {
        /// <summary>
        /// 节点名称、编号
        /// </summary>
        public string NodeId { get; set; }

        protected override void Disposing()
        {
            this.NodeId = null;
        }

        public override string ToString()
        {
            return $"NodeId;{this.NodeId} ValueType:{this.ValueType} Message:{this.Message}";
        }
    }

}
