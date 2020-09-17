using Solution.Desktop.Core;
using System;

namespace Solution.Desktop.DataLogItemManager.Model
{
    /// <summary>
    /// 操作日志明细信息模型
    /// </summary>
    public class DataLogItemModel : ModelBase
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

        #region 字段
        private string _field;

        public string Field
        {
            get { return _field; }
            set { Set(ref _field, value); }
        }
        #endregion

        #region 字段名称
        private string _fieldName;

        public string FieldName
        {
            get { return _fieldName; }
            set { Set(ref _fieldName, value); }
        }
        #endregion

        #region 旧值
        private string _originalValue;

        public string OriginalValue
        {
            get { return _originalValue; }
            set { Set(ref _originalValue, value); }
        }
        #endregion

        #region 新值
        private string _newValue;

        public string NewValue
        {
            get { return _newValue; }
            set { Set(ref _newValue, value); }
        }
        #endregion

        #region 数据类型
        private string _dataType;

        public string DataType
        {
            get { return _dataType; }
            set { Set(ref _dataType, value); }
        }
        #endregion

        #region 类型名称
        private string _entityName;
        public string EntityName
        {
            get { return _entityName; }
            set { Set(ref _entityName, value); }
        }
        #endregion

        #region 实体名称
        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
        #endregion

        protected override void Disposing()
        {
            Field = null;
            FieldName = null;
            OriginalValue = null;
            NewValue = null;
            DataType = null;
            EntityName = null;
            Name = null;
        }
    }
}
