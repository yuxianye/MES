using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Agv.Model
{
    /// <summary>
    /// 地标
    /// </summary>
    public class MarkPointInfoModel : ModelBase, IAudited
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

        #region 地标编号
        private string markPointNo;

        /// <summary>
        /// 地标编号
        /// </summary>
        [Required(ErrorMessage = "必填，长度小于50个字符，不能重复！"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string MarkPointNo
        {
            get { return markPointNo; }
            set { Set(ref markPointNo, value); }
        }
        #endregion

        #region 地标名称
        private string markPointName;

        /// <summary>
        /// 地标名称
        /// </summary>
        [Required(ErrorMessage = "必填，长度小于50个字符，不能重复！"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string MarkPointName
        {
            get { return markPointName; }
            set { Set(ref markPointName, value); }
        }
        #endregion

        #region 所属区域
        private int areaId;

        /// <summary>
        /// 所属区域
        /// </summary>
        public int AreaId
        {
            get { return areaId; }
            set { Set(ref areaId, value); }
        }
        #endregion

        #region 所属产线
        private int productLineId;

        /// <summary>
        /// 所属产线
        /// </summary>
        public int ProductLineId
        {
            get { return productLineId; }
            set { Set(ref productLineId, value); }
        }
        #endregion

        #region X坐标
        private float x;

        /// <summary>
        /// X坐标
        /// </summary>
        public float X
        {
            get { return x; }
            set { Set(ref x, value); }
        }
        #endregion

        #region Y坐标
        private float y;

        /// <summary>
        /// Y坐标
        /// </summary>
        public float Y
        {
            get { return y; }
            set { Set(ref y, value); }
        }
        #endregion

        #region 通行优先级
        private bool isVirtualMarkPoint;

        /// <summary>
        /// 是否虚拟地标
        /// </summary>
        public bool IsVirtualMarkPoint
        {
            get { return isVirtualMarkPoint; }
            set { Set(ref isVirtualMarkPoint, value); }
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
            MarkPointNo = null;
            MarkPointName = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
