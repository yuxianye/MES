using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Agv.Model
{
    /// <summary>
    /// 路段模型
    /// </summary>
    public class RoadInfoModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 路段编号
        private string roadNo;

        /// <summary>
        /// 路段编号
        /// </summary>
        [Required(ErrorMessage = "必填，长度小于50个字符，不能重复！"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string RoadNo
        {
            get { return roadNo; }
            set { Set(ref roadNo, value); }
        }
        #endregion

        #region 路段名称
        private string roadName;

        /// <summary>
        /// 路段名称
        /// </summary>
        [Required(ErrorMessage = "必填，长度小于50个字符，不能重复！"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string RoadName
        {
            get { return roadName; }
            set { Set(ref roadName, value); }
        }
        #endregion

        #region 开始地标点
        private Guid startMarkPointInfo_Id;

        /// <summary>
        /// 开始地标点
        /// </summary>
        [Required(ErrorMessage = "必选！")]
        public Guid StartMarkPointInfo_Id
        {
            get { return startMarkPointInfo_Id; }
            set { Set(ref startMarkPointInfo_Id, value); }
        }
        #endregion

        #region 结束地标点
        private Guid endMarkPointInfo_Id;

        /// <summary>
        /// 结束地标点
        /// </summary>
        [Required(ErrorMessage = "必选！")]
        public Guid EndMarkPointInfo_Id
        {
            get { return endMarkPointInfo_Id; }
            set { Set(ref endMarkPointInfo_Id, value); }
        }
        #endregion

        #region 开始地标点
        private string startMarkPointInfoName;

        /// <summary>
        /// 开始地标点
        /// </summary>
        public string StartMarkPointInfoName
        {
            get { return startMarkPointInfoName; }
            set { Set(ref startMarkPointInfoName, value); }
        }
        #endregion

        #region 结束地标点
        private string endMarkPointInfoName;

        /// <summary>
        /// 结束地标点
        /// </summary>
        public string EndMarkPointInfoName
        {
            get { return endMarkPointInfoName; }
            set { Set(ref endMarkPointInfoName, value); }
        }
        #endregion

        #region 距离
        private float distance;

        /// <summary>
        /// 距离
        /// </summary>
        public float Distance
        {
            get { return distance; }
            set { Set(ref distance, value); }
        }
        #endregion

        #region 状态
        private int roadStatus;

        /// <summary>
        /// 状态
        /// </summary>
        public int RoadStatus
        {
            get { return roadStatus; }
            set { Set(ref roadStatus, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
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
            RoadNo = null;
            RoadName = null;
            StartMarkPointInfoName = null;
            EndMarkPointInfoName = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
