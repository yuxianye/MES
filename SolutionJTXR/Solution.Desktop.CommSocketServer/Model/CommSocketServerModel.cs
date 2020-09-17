using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.CommSocketServer.Model
{
    /// <summary>
    /// Socket通信模型
    /// </summary>
    public class CommSocketServerModel : ModelBase, IAudited/*, IRecyclable, ILockable*/
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

        #region 服务器名称

        public string _serverName;

        [Required(ErrorMessage = "必填,长度小于20字符"), MaxLength(20, ErrorMessage = "长度小于50个字符")]
        public string ServerName
        {
            get { return _serverName; }
            set { Set(ref _serverName, value); }
        }
        #endregion

        #region 服务器IP地址

        public string _serverIp;

        [Required(ErrorMessage = "必填，长度小于15字符"), MaxLength(15, ErrorMessage = "长度小于15个字符")]
        [RegularExpression(@"^(?=(\b|\D))(((\d{1,2})|(1\d{1,2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{1,2})|(2[0-4]\d)|(25[0-5]))(?=(\b|\D))$", ErrorMessage = "无效的IP地址")]
        public string ServerIp
        {
            get { return _serverIp; }
            set { Set(ref _serverIp, value); }
        }
        #endregion

        #region 服务器端口号
        public int _serverPort;

        [Required(ErrorMessage = "必填，1-65535")]
        [RegularExpression(@"^([1-9]|[1-9]\d{1,3}|[1-5]\d{4}|6[0-5]{2}[0-3][0-5])$", ErrorMessage = "无效的端口号(1-65535)")]
        public int ServerPort
        {
            get { return _serverPort; }
            set { Set(ref _serverPort, value); }
        }
        #endregion

        #region 服务器最大连接数

        public int _maxConnectionNumber;

        [Required(ErrorMessage = "必填，非0正整数")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "无效的最大连接数(非0正整数)")]
        public int MaxConnectionNumber
        {
            get { return _maxConnectionNumber; }
            set { Set(ref _maxConnectionNumber, value); }
        }
        #endregion

        #region 描述 

        public string _description;
        [MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
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
        }

    }

}
