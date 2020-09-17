using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.EnterpriseInformation.Models
{
    /// <summary>
    /// 企业信息
    /// </summary>
    [Description("企业信息")]
    public class EnterpriseInfo : EntityBase<Guid>, /*ILockable, IRecyclable, */IAudited
    {
        public EnterpriseInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 企业编号，不能重复
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 企业名称，不能重复
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 企业地址
        /// </summary>
        [StringLength(100)]
        public string EnterpriseAddress { get; set; }

        /// <summary>
        /// 企业电话
        /// </summary>
        [StringLength(50)]
        public string EnterprisePhone { get; set; }

        /// <summary>
        /// 企业描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        ////////////////////以下为通用字段。《其他模块复制即可使用，如有特殊实体不继承下面接口，需要删除对应属性。》

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
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion

    }
}
