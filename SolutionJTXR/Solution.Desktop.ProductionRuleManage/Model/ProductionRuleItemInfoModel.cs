﻿using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.ProductionRuleManage.Model
{
    public class ProductionRuleItemInfoModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        ///配方明细 Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion
        #region 配方ID
        private Guid productionRule_Id;
        public Guid ProductionRule_Id
        {
            get { return productionRule_Id; }
            set { Set(ref productionRule_Id, value); }
        }

        #endregion
        #region 配方名称
        private string productionRuleName;

        /// <summary>
        /// 配方名称
        /// </summary>
        public string ProductionRuleName
        {
            get { return productionRuleName; }
            set { Set(ref productionRuleName, value); }
        }

        #endregion

        #region 配方版本号
        private string productionRuleVersion;

        /// <summary>
        /// 配方版本号
        /// </summary>

        public string ProductionRuleVersion
        {
            get { return productionRuleVersion; }
            set { Set(ref productionRuleVersion, value); }
        }

        #endregion
        #region 工序ID
        private Guid productionProcess_Id;
        public Guid ProductionProcess_Id
        {
            get { return productionProcess_Id; }
            set { Set(ref productionProcess_Id, value); }
        }

        #endregion
        #region 工序名称
        private string productionProcessName;

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProductionProcessName
        {
            get { return productionProcessName; }
            set { Set(ref productionProcessName, value); }
        }

        #endregion
        #region 工序编号
        private string productionProcessCode;

        /// <summary>
        /// 工序编号
        /// </summary>
        public string ProductionProcessCode
        {
            get { return productionProcessCode; }
            set { Set(ref productionProcessCode, value); }
        }

        #endregion
        #region 工序排序
        private int? productionProcessOrder;

        /// <summary>
        /// 工序排序
        /// </summary>
        public int? ProductionProcessOrder
        {
            get { return productionProcessOrder; }
            set { Set(ref productionProcessOrder, value); }
        }
        #endregion

        #region 工序时长
        private decimal? duration;

        /// <summary>
        /// 工序时长
        /// </summary>
        public decimal? Duration
        {
            get { return duration; }
            set { Set(ref duration, value); }
        }
        #endregion

        #region 时长单位
        private int? durationUnit;

        /// <summary>
        /// 时长单位
        /// </summary>
        public int? DurationUnit
        {
            get { return durationUnit; }
            set { Set(ref durationUnit, value); }
        }
        #endregion
        #region 时长单位名称
        private string durationUnitName;

        /// <summary>
        /// 时长单位名称
        /// </summary>
        public string DurationUnitName
        {
            get { return durationUnitName; }
            set { Set(ref durationUnitName, value); }
        }
        #endregion

        #region 配方明细描述
        private string description;

        /// <summary>
        /// 配方明细描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
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
            ProductionProcessCode = null;
            ProductionProcessName = null;
            ProductionRuleVersion = null;
            ProductionRuleName = null;
            ProductionProcessOrder = null;
            Duration = null;
            DurationUnit = null;
            DurationUnitName = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
