using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using Solution.MatWarehouseStorageManagement.Models;

namespace Solution.MatWarehouseStorageManagement.Dtos
{
    /// <summary>
    /// 仓库信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class MatWareHouseInfoInputDto : IInputDto<Guid>, IAudited
    {

        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 所属区域ID
        /// </summary>
        public Guid EntArea_Id { get; set; }

        public EntAreaInfo EntArea { get; set; }

        /// <summary>
        /// 仓库类型ID
        /// </summary>
        public Guid MatWareHouseType_Id { get; set; }

        public MatWareHouseTypeInfo MatWareHouseType { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        [StringLength(50)]
        public string WareHouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        [StringLength(50)]
        public string WareHouseName { get; set; }

        /// <summary>
        /// 仓库描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// 仓库负责人
        /// </summary>
        [StringLength(50)]
        public string Manager { get; set; }

        /// <summary>
        /// 仓库电话
        /// </summary>
        [StringLength(50)]
        public string WareHousePhone { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        #endregion

        #region Implementation of ICreatedAudited

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        [StringLength(50)]
        public string CreatorUserId { get; set; }

        #endregion

        #region Implementation of IUpdateAutited

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
