using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquSparePartTypeInfo.Model
{
    /// <summary>
    /// 备件类别模型
    /// </summary>
    public class EquSparePartTypeModel : ModelBase, /*ILockable, IRecyclable,*/ IAudited
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

        #region 备件类别名称
        private string equspareparttypeName;

        /// <summary>
        /// 备件类别名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string EquSparePartTypeName
        {
            get { return equspareparttypeName; }
            set { Set(ref equspareparttypeName, value); }
        }
        #endregion

        #region 备件类别编码
        private string equspareparttypeCode;

        /// <summary>
        /// 备件类别编码
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string EquSparePartTypeCode
        {
            get { return equspareparttypeCode; }
            set { Set(ref equspareparttypeCode, value); }
        }
        #endregion


        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度小于200个字符")]
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
        }
        #endregion

        #region 是否被选中
        private bool _isChecked = false;
        public bool IsSparePartTypeChecked
        {
            get { return _isChecked; }
            set
            {
                //Set(ref _isChecked, value);
                _isChecked = value;
                // Set(ref _isChecked, value);
                if (PropertyChangedHandler != null)
                    //   base.PropertyChangedHandler(this, new PropertyChangedEventArgs("IsSparePartTypeChecked"));
                    PropertyChangedHandler(this, new PropertyChangedEventArgs("IsSparePartTypeChecked"));
            }
        }
        #endregion

        //#region 是否逻辑删除
        //private bool isDeleted;

        ///// <summary>
        ///// 是否逻辑删除
        ///// </summary>
        //public bool IsDeleted
        //{
        //    get { return isDeleted; }
        //    set { Set(ref isDeleted, value); }
        //}
        //#endregion

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
            EquSparePartTypeName = null;
            EquSparePartTypeCode = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
