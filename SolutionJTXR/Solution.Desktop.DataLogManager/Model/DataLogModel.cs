using Solution.Desktop.Core;
using System;

namespace Solution.Desktop.DataLogManager.Model
{
    /// <summary>
    /// 操作日志信息模型
    /// </summary>
    public class DataLogModel : ModelBase
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

        #region 类型名称

        private string _entityName;

        public string EntityName
        {
            get { return _entityName; }
            set { Set(ref _entityName, value); }
        }
        #endregion

        #region 操作用户ID

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
        #endregion

        #region 数据编号

        private string _entityKey;

        public string EntityKey
        {
            get { return _entityKey; }
            set { Set(ref _entityKey, value); }
        }
        #endregion

        #region 操作者昵称

        private string _operator_NickName;

        public string Operator_NickName
        {
            get { return _operator_NickName; }
            set { Set(ref _operator_NickName, value); }
        }
        #endregion

        #region 功能地址

        private string _operator_Ip;

        public string Operator_Ip
        {
            get { return _operator_Ip; }
            set { Set(ref _operator_Ip, value); }
        }
        #endregion

        #region 创建时间
        private DateTime? _createdTime;
        public DateTime? CreatedTime
        {
            get { return _createdTime; }
            set { Set(ref _createdTime, value); }
        }
        #endregion

        protected override void Disposing()
        {
            EntityName = null;
            Name = null;
            EntityKey = null;
            Operator_NickName = null;
            Operator_Ip = null;
            CreatedTime = null;
        }
    }
}
