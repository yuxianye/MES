using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EquipKnifeToolInfo.Models;


namespace Solution.EquipKnifeToolInfo.Dtos
{
    public class EquKnifeToolInfoInputDto : /* ILockable, IRecyclable,*/ IInputDto<Guid>, IAudited
    {


        public Guid Id { get; set; }

        /// <summary>
        /// 刀具ID
        /// </summary>
        public Guid KnifeToolTypeInfo_Id { get; set; }

        /// <summary>
        /// 刀具ID
        /// </summary>
        /// 
        public KnifeToolTypeInfo Knifetooltypeinfo { get; set; }

        /// <summary>
        /// 刀具名称
        /// </summary>
        public string KnifeName { get; set; }

        /// <summary>
        /// 刀具编号
        /// </summary>
        public string KnifeCode { get; set; }


        /// <summary>
        ///  规格型号
        /// </summary>
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
        ///// <summary>
        ///// 是否锁定
        ///// </summary>
        //public bool IsLocked { get; set; }
        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
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
        //#region Implementation of IRecyclable
        //public bool IsDeleted { get; set; }
        //#endregion
    }
}
