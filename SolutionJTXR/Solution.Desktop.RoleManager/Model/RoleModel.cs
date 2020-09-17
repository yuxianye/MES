using Solution.Desktop.Core;
using Solution.Desktop.ModuleManager.Model;
using Solution.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.RoleManager.Model
{
    /// <summary>
    /// 角色信息模型
    /// </summary>
    public class RoleModel : ModelBase, ILockable
    {

        #region Id
        private int id;

        /// <summary>
        /// Id
        /// </summary>

        public int Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }
        #endregion

        #region 角色名
        private string _name;
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }

        }
        #endregion

        #region 备注
        private string _remark;

        public string Remark
        {
            get { return _remark; }
            set { Set(ref _remark, value); }

        }
        #endregion

        #region 是否是是管理员
        private bool _isAdmin;

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { Set(ref _isAdmin, value); }
        }
        #endregion

        #region 是否是是默认
        private bool _isDefault;

        public bool IsDefault
        {
            get { return _isDefault; }
            set { Set(ref _isDefault, value); }
        }
        #endregion

        #region 是否是系统角色
        private bool _isSystem;

        public bool IsSystem
        {
            get { return _isSystem; }
            set { Set(ref _isSystem, value); }
        }
        #endregion 

        #region 是否被锁定
        private bool _isLocked;

        public bool IsLocked
        {
            get { return _isLocked; }
            set { Set(ref _isLocked, value); }
        }
        #endregion 

        #region 创建时间
        private DateTime _createdTime;

        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { Set(ref _createdTime, value); }
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

        private ObservableCollection<ModuleManagerModel> moduleManagerModels;
        public ObservableCollection<ModuleManagerModel> ModuleManagerModels
        {
            get { return moduleManagerModels; }
            set { Set(ref moduleManagerModels, value); }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        protected override void Disposing()
        {
            Name = null;
            Remark = null;
            ModuleManagerModels = null;
        }

    }

}
