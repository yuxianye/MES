using Solution.Desktop.Core;
using Solution.Desktop.FunctionManager.Model;
using Solution.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Solution.Desktop.ModuleManager.Model
{
    /// <summary>
    /// 模块信息模型
    /// </summary>
    public class ModuleManagerModel : ModelBase
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

        #region 模块名称

        private string _name;

        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
        #endregion

        #region 备注

        private string _ramark;

        public string Remark
        {
            get { return _ramark; }
            set { Set(ref _ramark, value); }
        }
        #endregion

        #region 排序码

        private double _orderCode;

        [Required(ErrorMessage = "排序码必填")]
        public double OrderCode
        {
            get { return _orderCode; }
            set { Set(ref _orderCode, value); }
        }
        #endregion

        #region 父节点树形路径

        private string _treePathString;

        //[Required(ErrorMessage = "父节点树形路径")]
        public string TreePathString
        {
            get { return _treePathString; }
            set { Set(ref _treePathString, value); }
        }
        #endregion

        #region 图标

        private string _icon;

        //[Required(ErrorMessage = "图标路径")]
        public string Icon
        {
            get { return _icon; }
            set { Set(ref _icon, value); }
        }
        #endregion

        #region 父节点编号 

        private int? _parentId;

        public int? ParentId
        {
            get { return _parentId; }
            set { Set(ref _parentId, value); }
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

        #region 是否被选择
        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                Set(ref _isChecked, value);
            }
        }
        #endregion

        #region 模块列表
        private ObservableCollection<ModuleManagerModel> moduleManagerModels;
        public ObservableCollection<ModuleManagerModel> ModuleManagerModels
        {
            get { return moduleManagerModels; }
            set { Set(ref moduleManagerModels, value); }
        }
        #endregion

        #region 功能列表
        private ObservableCollection<FunctionModel> functionModels;
        public ObservableCollection<FunctionModel> FunctionModels
        {
            get { return functionModels; }
            set { Set(ref functionModels, value); }
        }
        #endregion

        #region 左侧空格数
        private int _marginLeft;
        public int MarginLeft
        {
            get { return _marginLeft; }
            set { Set(ref _marginLeft, value); }
        }
        #endregion

        protected override void Disposing()
        {
            Name = null;
            Remark = null;
            TreePathString = null;
            Icon = null;
            ParentId = null;
            ModuleManagerModels = null;
            FunctionModels = null;
        }
    }

}
