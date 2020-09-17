﻿using Solution.Desktop.Core;
using System;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialUnitEnumModel;

namespace Solution.Desktop.PlanWorkListManage.Model
{
    public class PlanMaterialRequirementInfoModel : ModelBase, IAudited
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
        #region 工序需求Id
        private Guid productionProcessRequirement_Id;

        /// <summary>
        /// 工序需求Id
        /// </summary>

        public Guid ProductionProcessRequirement_Id
        {
            get { return productionProcessRequirement_Id; }
            set { Set(ref productionProcessRequirement_Id, value); }
        }

        #endregion
        #region 计划名称
        private string scheduleName;

        /// <summary>
        /// 计划名称
        /// </summary>
        public string ScheduleName
        {
            get { return scheduleName; }
            set { Set(ref scheduleName, value); }
        }

        #endregion

        #region 计划编号
        private string scheduleCode;

        /// <summary>
        /// 计划编号
        /// </summary>

        public string ScheduleCode
        {
            get { return scheduleCode; }
            set { Set(ref scheduleCode, value); }
        }

        #endregion

        #region 工序Id
        private Guid productionProcess_Id;

        /// <summary>
        /// 工序Id
        /// </summary>

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
        #region 材料Id
        private Guid material_Id;

        /// <summary>
        /// 材料Id
        /// </summary>

        public Guid Material_Id
        {
            get { return material_Id; }
            set { Set(ref material_Id, value); }
        }

        #endregion
        #region 材料名称
        private string materialName;

        /// <summary>
        /// 材料名称
        /// </summary>
        public string MaterialName
        {
            get { return materialName; }
            set { Set(ref materialName, value); }
        }

        #endregion

        #region 材料编号
        private string materialCode;

        /// <summary>
        /// 材料编号
        /// </summary>

        public string MaterialCode
        {
            get { return materialCode; }
            set { Set(ref materialCode, value); }
        }

        #endregion
        #region 物料单位
        private MaterialUnit materialUnit;

        /// <summary>
        /// 物料单位
        /// </summary>
        public MaterialUnit MaterialUnit
        {
            get { return materialUnit; }
            set { Set(ref materialUnit, value); }
        }
        #endregion
        #region 需要数量
        private decimal? requireQuantity;

        /// <summary>
        /// 需要数量
        /// </summary>

        public decimal? RequireQuantity
        {
            get { return requireQuantity; }
            set { Set(ref requireQuantity, value); }
        }

        #endregion
        #region 实际数量
        private decimal? actualQuantity;

        /// <summary>
        /// 实际数量
        /// </summary>

        public decimal? ActualQuantity
        {
            get { return actualQuantity; }
            set { Set(ref actualQuantity, value); }
        }

        #endregion
        #region 描述
        private string description;

        /// <summary>
        /// 描述
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
            ProductionProcessCode = null;
            ProductionProcessName = null;
            ProductionRuleName = null;
            ProductionRuleVersion = null;
            ScheduleCode = null;
            ScheduleName = null;
            MaterialCode = null;
            MaterialName = null;
            RequireQuantity = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}