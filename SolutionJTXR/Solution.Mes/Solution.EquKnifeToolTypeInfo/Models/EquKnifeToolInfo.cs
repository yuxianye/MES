using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Solution.EquipKnifeToolInfo.Models
{
    [Description("装备信息")]
    public class EquKnifeToolInfo : EntityBase<Guid>, /*ILockable, IRecyclable, */IAudited
    {

        /// <summary>
        /// 刀具类别
        /// </summary>
        /// 
        public virtual KnifeToolTypeInfo Knifetooltypeinfo { get; set; }

        /// <summary>
        /// 刀具名称
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string KnifeName { get; set; }

        /// <summary>
        /// 刀具编号
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string KnifeCode { get; set; }


        /// <summary>
        ///  规格型号
        /// </summary>
        [StringLength(200)]
        public string Specifications { get; set; }

        /// <summary>
        /// 安装时间
        /// </summary>
        //public DateTime InstallTime { get; set; }
        public DateTime? InstallTime { get; set; }
        /// <summary>
        /// 使用寿命(年)
        /// </summary>
        public decimal ServiceLife { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]

        public string Remark { get; set; }

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

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
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public DateTime? LastUpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion

    }
}
