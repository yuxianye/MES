using Solution.Desktop.Core;
using Solution.Utility;
using Solution.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Solution.Desktop.EntDepartmentInfo.Model
{
    /// <summary>
    /// 部门模型
    /// </summary>
    public class EntDepartmentInfoModel : ModelBase
    {
        #region Id
        private Guid id;

        /// <summary>
        /// Id
        /// </summary>
        [DisplayName("编号")]
        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 部门名称
        private string departmentName;

        /// <summary>
        /// 部门名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        [DisplayName("部门名称")]
        public string DepartmentName
        {
            get { return departmentName; }
            set { Set(ref departmentName, value); }
        }
        #endregion

        #region 部门编号
        private string departmentCode;

        /// <summary>
        /// 部门编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        [DisplayName("部门编号")]
        public string DepartmentCode
        {
            get { return departmentCode; }
            set { Set(ref departmentCode, value); }
        }
        #endregion

        #region 部门职责
        private string departmentFunction;

        /// <summary>
        /// 部门职责
        /// </summary>
        [DisplayName("部门职责")]
        public string DepartmentFunction
        {
            get { return departmentFunction; }
            set { Set(ref departmentFunction, value); }
        }
        #endregion

        #region 部门描述
        private string description;

        /// <summary>
        /// 部门描述
        /// </summary>
        [DisplayName("部门描述")]
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }
        #endregion

        //#region 创建时间
        //private DateTime createTime;

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //[DisplayName("创建时间")]
        //public DateTime CreateTime
        //{
        //    get { return createTime; }
        //    set { Set(ref createTime, value); }
        //}
        //#endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
        }
        #endregion

        #region 记录创建时间
        private DateTime createdTime;

        /// <summary>
        /// 记录创建时间
        /// </summary>
        [DisplayName("记录创建时间")]
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { Set(ref createdTime, value); }
        }
        #endregion

        #region 创建者编号
        private string creatorUserId;

        /// <summary>
        /// 创建者编号
        /// </summary>
        [DisplayName("创建者编号")]
        public string CreatorUserId
        {
            get { return creatorUserId; }
            set { Set(ref creatorUserId, value); }
        }
        #endregion

        #region 最后更新时间
        private DateTime? lastUpdatedTime;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DisplayName("最后更新时间")]
        public DateTime? LastUpdatedTime
        {
            get { return lastUpdatedTime; }
            set { Set(ref lastUpdatedTime, value); }
        }
        #endregion

        #region 最后更新者编号
        private string lastUpdatorUserId;

        /// <summary>
        /// 最后更新者编号
        /// </summary>
        [DisplayName("最后更新者编号")]
        public string LastUpdatorUserId
        {
            get { return lastUpdatorUserId; }
            set { Set(ref lastUpdatorUserId, value); }
        }
        #endregion

        protected override void Disposing()
        {
            DepartmentName = null;
            DepartmentCode = null;
            DepartmentFunction = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
