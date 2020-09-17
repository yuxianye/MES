using Solution.Desktop.Core;
using Solution.Desktop.EntEmployeeInfo.EnumType;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EntEmployeeInfo.Model
{
    /// <summary>
    /// 区域模型
    /// </summary>
    public class EntEmployeeInfoModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 员工编号
        private string entEmployeeCode;
        public string EntEmployeeCode
        {
            get { return entEmployeeCode; }
            set { Set(ref entEmployeeCode, value); }
        }

        #endregion

        #region 员工名称
        private string entEmployeeName;
        public string EntEmployeeName
        {
            get { return entEmployeeName; }
            set { Set(ref entEmployeeName, value); }
        }

        #endregion

        #region 性别
        private Sex entEmployeeSex;
        public Sex EntEmployeeSex
        {
            get { return entEmployeeSex; }
            set { Set(ref entEmployeeSex, value); }
        }

        #endregion

        #region 出生日期
        private DateTime birthDate;

        public DateTime BirthDate
        {
            get { return birthDate; }
            set { Set(ref birthDate, value); }
        }

        #endregion

        #region 所属部门ID
        private Guid _departmentInfoCode;

        /// <summary>
        /// 所属部门ID
        /// </summary>
        public Guid DepartmentCode
        {
            get { return _departmentInfoCode; }
            set { Set(ref _departmentInfoCode, value); }
        }

        #endregion

        #region 所属部门名称
        private string _departmentInfoName;

        /// <summary>
        /// 所属部门名称
        /// </summary>
        public string DepartmentInfoName
        {
            get { return _departmentInfoName; }
            set { Set(ref _departmentInfoName, value); }
        }

        #endregion

        #region 职务
        private Duty workPost;

        /// <summary>
        /// 职务
        /// </summary>
        public Duty WorkPost
        {
            get { return workPost; }
            set { Set(ref workPost, value); }
        }

        #endregion

        #region 工种
        private WorkType workBranch;

        /// <summary>
        /// 工种
        /// </summary>
        public WorkType WorkBranch
        {
            get { return workBranch; }
            set { Set(ref workBranch, value); }
        }
        #endregion

        #region 技能
        private Skills skills;

        /// <summary>
        /// 技能
        /// </summary>
        //[Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        //[RegularExpression(@"^[1][3,4,5,6,7,8,9][0-9]{9}$", ErrorMessage = "无效的电话号码")]
        public Skills Skills
        {
            get { return skills; }
            set { Set(ref skills, value); }
        }
        #endregion

        #region 入职日期
        private DateTime entryDate;

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { Set(ref entryDate, value); }
        }
        #endregion

        #region 学历
        private Education education;

        /// <summary>
        /// 学历
        /// </summary>
        public Education Education
        {
            get { return education; }
            set { Set(ref education, value); }
        }
        #endregion

        #region 职称
        private PositionalTitles professionalTitles;

        /// <summary>
        /// 职称
        /// </summary>
        public PositionalTitles ProfessionalTitles
        {
            get { return professionalTitles; }
            set { Set(ref professionalTitles, value); }
        }
        #endregion

        #region 工作经历
        private string workExperience;

        /// <summary>
        /// 工作经历
        /// </summary>
        public string WorkExperience
        {
            get { return workExperience; }
            set { Set(ref workExperience, value); }
        }
        #endregion

        #region 奖励记录
        private string awardRecord;

        /// <summary>
        /// 奖励记录
        /// </summary>
        public string AwardRecord
        {
            get { return awardRecord; }
            set { Set(ref awardRecord, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
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
        public string LastUpdatorUserId
        {
            get { return lastUpdatorUserId; }
            set { Set(ref lastUpdatorUserId, value); }
        }
        #endregion

        protected override void Disposing()
        {
            EntEmployeeCode = null;
            EntEmployeeName = null;
            //EntEmployeeSex = null;
            //WorkPost = 1;
            //WorkBranch = 1;
            //Skills = 1;
            //Education = 1;
            //ProfessionalTitles = 1;
            WorkExperience = null;
            AwardRecord = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
            Remark = null;
        }

    }

}
