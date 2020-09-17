using OSharp.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Dtos
{
    /// <summary>
    /// 业务点输入Dto
    /// </summary>
    public class BusinessNodeInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 业务点名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

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
