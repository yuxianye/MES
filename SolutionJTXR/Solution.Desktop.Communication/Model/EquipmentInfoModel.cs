using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Communication.Model
{
    /// <summary>
    /// 设备信息模型
    /// </summary>
    public class EquipmentModel : ModelBase
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 设备ID
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 设备名称
        private string equipmentName;

        /// <summary>
        /// 设备名称
        /// </summary>
        [Required(ErrorMessage = "设备名称必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string EquipmentName
        {
            get { return equipmentName; }
            set { Set(ref equipmentName, value); }
        }

        #endregion

        protected override void Disposing()
        {
            EquipmentName = null;
        }

    }

}
