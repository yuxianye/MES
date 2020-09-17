using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Solution.CommunicationModule.Dtos
{
    /// <summary>
    /// 业务点表管理map输入Dto
    /// </summary>
    public class EquipmentBusinessNodeMapManageInputDto : ProductionProcessEquipmentBusinessNodeMapInputDto
    {
        /// <summary>
        /// 设备点表列表
        /// </summary>
        public List<DeviceNode> DeviceNodeList { get; set; }
    }

}
