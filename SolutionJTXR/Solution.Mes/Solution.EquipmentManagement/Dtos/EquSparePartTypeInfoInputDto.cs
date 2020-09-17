using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;

namespace Solution.EquipmentManagement.Dtos
{
    /// <summary>
    /// 企业信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class EquSparePartTypeInfoInputDto :/* ILockable, IRecyclable,*/ IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 主键
        /// </summary>

        public Guid Id { get; set; }

        /// <summary>
        /// 备件类别名称
        /// </summary>


        public string EquSparePartTypeName { get; set; }

        /// <summary>
        /// 备件类别编号，不能重复
        /// </summary>

        public string EquSparePartTypeCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }



        //#region Implementation of ILockable

        ///// <summary>
        ///// 获取设置 是否锁定
        ///// </summary>
        //public bool IsLocked { get; set; }

        //#endregion

        //#region Implementation of IRecyclable

        ///// <summary>
        ///// 是否逻辑删除
        ///// </summary>
        //public bool IsDeleted { get; set; }

        //#endregion

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
