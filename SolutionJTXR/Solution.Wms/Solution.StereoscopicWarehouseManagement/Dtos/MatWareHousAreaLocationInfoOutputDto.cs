using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.MatWarehouseStorageManagement.Models;
using System.Collections.Generic;

namespace Solution.StereoscopicWarehouseManagement.Dtos
{
    /// <summary>
    /// 仓位图信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class MatWareHousAreaLocationInfoOutputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 所属货区ID
        /// </summary>
        public Guid MatWareHouseArea_Id { get; set; }
        public MatWareHouseAreaInfo MatWareHouseArea { get; set; }

        public string WareHouseCode { get; set; }

        public string WareHouseName { get; set; }


        public string WareHouseAreaCode { get; set; }

        public string WareHouseAreaName { get; set; }

        public string LayerNumber { get; set; }

        public int ColumnNumber { get; set; }

        public List<string> WareHouseColumns { get; set; }
        

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
