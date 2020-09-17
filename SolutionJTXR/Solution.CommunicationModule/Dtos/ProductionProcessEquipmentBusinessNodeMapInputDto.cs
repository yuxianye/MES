using OSharp.Core.Data;
using Solution.CommunicationModule.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.CommunicationModule.Dtos
{
    /// <summary>
    /// 业务点表map输入Dto
    /// </summary>
    public class ProductionProcessEquipmentBusinessNodeMapInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public Guid ProductionProcessInfo_Id { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public Guid? Equipment_Id { get; set; }

        /// <summary>
        /// 业务Id
        /// </summary>
        public Guid BusinessNode_Id { get; set; }

        /// <summary>
        /// 设备点表Id
        /// </summary>
        public Guid DeviceNode_Id { get; set; }

        /// <summary>
        /// 业务点表
        /// </summary>
        public BusinessNode BusinessNode { get; set; }

        /// <summary>
        /// 设备点表
        /// </summary>
        public DeviceNode DeviceNode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastUpdatorUserId { get; set; }
    }
}
