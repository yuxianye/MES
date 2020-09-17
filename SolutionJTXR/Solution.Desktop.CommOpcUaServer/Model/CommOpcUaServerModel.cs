using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.CommOpcUaServer.Model
{
    /// <summary>
    /// Opc Ua服务器数据模型
    /// </summary>
    public class CommOpcUaServerModel : ModelBase, IAudited/*, IRecyclable, ILockable*/
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

        [Required(ErrorMessage = "必填,长度小于20个字符"), MaxLength(20, ErrorMessage = "长度小于20个字符")]
        public string ServerName
        {
            get { return _serverName; }
            set { Set(ref _serverName, value); }
        }
        #endregion

        #region 服务器地址

        public string _url;

        [Required(ErrorMessage = "必填,长度小于200个字符"), MaxLength(200, ErrorMessage = "长度小于200个字符")]
        public string Url
        {
            get { return _url; }
            set { Set(ref _url, value); }
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
