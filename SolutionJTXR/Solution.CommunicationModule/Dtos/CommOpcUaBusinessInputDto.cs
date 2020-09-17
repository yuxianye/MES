using OSharp.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Dtos
{
    public class CommOpcUaBusinessInputDto : IInputDto<Guid>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Opc Ua 业务ID
        /// </summary>
        public int BusinessId { get; set; }

        /// <summary>
        /// Opc Ua 业务名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 数据点ID
        /// </summary>
        public Guid NodeId { get; set; }

        /// <summary>
        /// 数据点名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public bool IsLocked { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatorUserId { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string LastUpdatorUserId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
