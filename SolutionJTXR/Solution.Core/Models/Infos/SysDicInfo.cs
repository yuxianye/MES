using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Core.Data;
using OSharp.Utility.Data;

namespace Solution.Core.Models.Infos
{
    /// <summary>
    /// 实体类-字典信息
    /// </summary>
    [Description("信息-字典信息")]
    public class SysDicInfo : EntityBase<Guid>, IAudited
    {
        public SysDicInfo()
        {
            Id = Guid.Empty;
        }
        [StringLength(50)]
        public string DicCode { get; set; }

        [StringLength(100)]
        public string DicName { get; set; }

        public Guid? DicParentID { get; set; }

        public int? DicLevel { get; set; }

        public bool DicType { get; set; }


        [StringLength(50)]
        public string DicValue { get; set; }
        [StringLength(200)]
        public string DicSetValue { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }
        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime
        { get; set; }

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
