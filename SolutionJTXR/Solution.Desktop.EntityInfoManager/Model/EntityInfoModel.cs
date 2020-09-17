using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EntityInfoManager.Model
{
    /// <summary>
    /// 实体信息模型
    /// </summary>
    public class EntityInfoModel : ModelBase
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

        #region 实体类型全名

        private string _className;

        public string ClassName
        {
            get { return _className; }
            set { Set(ref _className, value); }
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

        #region 是否启用数据日志

        private bool _dataLogEnabled;

        public bool DataLogEnabled
        {
            get { return _dataLogEnabled; }
            set { Set(ref _dataLogEnabled, value); }
        }
        #endregion

        protected override void Disposing()
        {
            ClassName = null;
            Name = null;
        }
    }
}
