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
    public class MaterialInfoInputDto : IInputDto<Guid>, IAudited
    {

        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 物料编号，不能重复
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称，不能重复
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public int? MaterialType { get; set; }


        /// <summary>
        /// 物料单位
        /// </summary>
        public int? MaterialUnit { get; set; }

        /// <summary>
        /// 满盘数量
        /// </summary>
        public int? FullPalletQuantity { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 单位重量
        /// </summary>
        public decimal? UnitWeight { get; set; }

        /// <summary>
        /// 当前库存
        /// </summary>
        public decimal? CurrentStock { get; set; }

        /// <summary>
        /// 最大库存
        /// </summary>
        public decimal? MaxStock { get; set; }

        /// <summary>
        /// 最小库存
        /// </summary>
        public decimal? MinStock { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
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
