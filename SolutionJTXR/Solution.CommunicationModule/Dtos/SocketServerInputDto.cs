using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.CommunicationModule.Dtos
{
    /// <summary>
    /// 实时通讯服务器输入Dto
    /// </summary>
    public class SocketServerInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 实时通讯服务器名字
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 实时通讯服务器IP
        /// </summary>
        public string ServerIp { get; set; }

        /// <summary>
        /// 实时通讯服务器端口Port
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 实时通讯服务器最大连接数（日后可根据客户端授权数量收费）
        /// </summary>
        public int MaxConnectionNumber { get; set; }

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
