using OSharp.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.Dtos
{
    /// <summary>
    /// 部门信息输入Dto
    /// </summary>
    public class EntDepartmentInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 部门职责
        /// </summary>
        public string DepartmentFunction { get; set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        public string Description { get; set; }

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }

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
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
