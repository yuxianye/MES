using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using static Solution.Desktop.MatWareHouseInfo.Model.WareHouseLocationTypeEnumModel;

namespace Solution.Desktop.MatWareHousAreaLocationInfo.Model
{
    /// <summary>
    /// 仓位图模型
    /// </summary>
    public class MatWareHousAreaLocationTypeModel : ModelBase, IAudited
    {

        #region 库位类型1
        private string matwarehouselocationtype1;

        // <summary>
        // 库位类型1
        // </summary>
        public string MatWareHouseLocationType1
        {
            get { return matwarehouselocationtype1; }
            set { Set(ref matwarehouselocationtype1, value); }
        }
        #endregion

        #region 库位类型2
        private string matwarehouselocationtype2;

        // <summary>
        // 库位类型2
        // </summary>
        public string MatWareHouseLocationType2
        {
            get { return matwarehouselocationtype2; }
            set { Set(ref matwarehouselocationtype2, value); }
        }
        #endregion

        #region 库位类型3
        private string matwarehouselocationtype3;

        // <summary>
        // 库位类型3
        // </summary>
        public string MatWareHouseLocationType3
        {
            get { return matwarehouselocationtype3; }
            set { Set(ref matwarehouselocationtype3, value); }
        }
        #endregion

        #region 库位类型4
        private string matwarehouselocationtype4;

        // <summary>
        // 库位类型4
        // </summary>
        public string MatWareHouseLocationType4
        {
            get { return matwarehouselocationtype4; }
            set { Set(ref matwarehouselocationtype4, value); }
        }
        #endregion

        #region 库位类型5
        private string matwarehouselocationtype5;

        // <summary>
        // 库位类型5
        // </summary>
        public string MatWareHouseLocationType5
        {
            get { return matwarehouselocationtype5; }
            set { Set(ref matwarehouselocationtype5, value); }
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
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
