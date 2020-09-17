using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquKnifeToolTypeInfo.Model
{
    /// <summary>
    /// 刀具类别信息模型
    /// </summary>
    public class KnifeToolTypeModel : ModelBase, /*ILockable, IRecyclable,*/ IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 刀具类型ID
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 刀具类别名称
        private string knifeToolTypeName;

        /// <summary>
        /// 刀具名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string KnifeToolTypeName
        {
            get { return knifeToolTypeName; }
            set { Set(ref knifeToolTypeName, value); }
        }

        #endregion

        #region 刀具类别编号
        private string knifeToolTypeCode;

        /// <summary>
        /// 刀具类别编号编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string KnifeToolTypeCode
        {
            get { return knifeToolTypeCode; }
            set { Set(ref knifeToolTypeCode, value); }
        }

        #endregion
        #region 是否被选中
        private bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                //Set(ref _isChecked, value);
                _isChecked = value;
                //  Set(ref _isChecked, value);
                if (PropertyChangedHandler != null)
                    //  base.PropertyChangedHandler(this, new PropertyChangedEventArgs("IsChecked"));
                    PropertyChangedHandler(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }
        #endregion

        //public event PropertyChangedEventHandler PropertyKnifeTypeChanged;

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        /// 
        [MaxLength(200, ErrorMessage = "长度小于200个字符")]
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
        }
        #endregion

        //#region 是否锁定
        //private bool isLocked;

        ///// <summary>
        ///// 是否锁定
        ///// </summary>
        //public bool IsLocked
        //{
        //    get { return isLocked; }
        //    set { Set(ref isLocked, value); }
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

        protected override void Disposing()
        {
            KnifeToolTypeName = null;
            KnifeToolTypeCode = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
