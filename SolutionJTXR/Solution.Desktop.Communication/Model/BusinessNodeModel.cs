using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Communication.Model
{
    /// <summary>
    /// 业务点表模型
    /// </summary>
    public class BusinessNodeModel : ModelBase, IAudited/*, IRecyclable, ILockable*/
    {
        #region Id
        private Guid id = CombHelper.NewComb();

        /// <summary>
        /// Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 业务点名称

        private string businessName;
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string BusinessName
        {
            get { return businessName; }
            set { Set(ref businessName, value); }
        }

        #endregion

        #region 描述 

        private string _description;
        [MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
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
                if (PropertyChangedHandler != null)
                    PropertyChangedHandler(this, new PropertyChangedEventArgs("IsChecked"));
            }
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
            BusinessName = null;
            Description = null;
        }

    }

}
