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
    [Description("员工信息")]
    public class EntEmployeeInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public EntEmployeeInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 员工姓名
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string EntEmployeeName { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string EntEmployeeCode { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string EntEmployeeSex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// 所属部门ID
        /// </summary>
        public virtual EntDepartmentInfo DepartmentInfo { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public int WorkPost { get; set; }

        /// <summary>
        /// 工种
        /// </summary>
        public int WorkBranch { get; set; }

        /// <summary>
        /// 技能
        /// </summary>
        public int Skills { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public int Education { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public int ProfessionalTitles { get; set; }

        /// <summary>
        /// 工作经历
        /// </summary>
        [StringLength(200)]
        public string WorkExperience { get; set; }

        /// <summary>
        /// 奖励记录
        /// </summary>
        [StringLength(200)]
        public string AwardRecord { get; set; }

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
