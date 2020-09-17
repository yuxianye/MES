using Solution.Desktop.Core;
using System;

namespace Solution.Desktop.OperateLogManager.Model
{
    /// <summary>
    /// 操作日志信息模型
    /// </summary>
    public class OperateLogModel : ModelBase
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

        #region 功能名称

        private string _functionName;

        public string FunctionName
        {
            get { return _functionName; }
            set { Set(ref _functionName, value); }
        }
        #endregion

        #region 操作用户ID

        private string _operator_UserId;

        public string Operator_UserId
        {
            get { return _operator_UserId; }
            set { Set(ref _operator_UserId, value); }
        }
        #endregion

        #region 操作者

        private string _operator_Name;

        public string Operator_Name
        {
            get { return _operator_Name; }
            set { Set(ref _operator_Name, value); }
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
            FunctionName = null;
            Operator_UserId = null;
            Operator_Name = null;
            Operator_NickName = null;
            Operator_Ip = null;
            CreatedTime = null;
        }

    }

}
