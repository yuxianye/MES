using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.Models
{
    [Description("部门信息")]
    public class EntDepartmentInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        //public new Guid Id = CombHelper.NewComb();
        public EntDepartmentInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 部门名称
        /// </summary>
        [StringLength(50)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        [StringLength(50)]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 部门职责
        /// </summary>
        [StringLength(100)]
        public string DepartmentFunction { get; set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }

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

