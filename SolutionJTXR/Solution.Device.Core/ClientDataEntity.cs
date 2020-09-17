using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Device.Core
{
    public class ClientDataEntity : Solution.Device.Core.DeviceOutputParamEntityBase
    {
        /// <summary>
        /// 功能码
        /// </summary>
        public FuncCode FunctionCode { get; set; }

        /// <summary>
        /// 工序设备业务业务ID
        /// </summary>
        public Guid ProductionProcessEquipmentBusinessNodeMapId { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public ICollection<Solution.Device.Core.DeviceOutputParamEntityBase> Datas { get; set; }

    }

}
