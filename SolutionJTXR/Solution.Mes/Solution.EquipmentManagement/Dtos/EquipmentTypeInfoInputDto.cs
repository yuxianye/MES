using OSharp.Core.Data;
using System;

namespace Solution.EquipmentManagement.Dtos
{
    public class EquipmentTypeInfoInputDto : /* ILockable, IRecyclable,*/ IInputDto<Guid>
    {


        public Guid Id { get; set; }

        /// <summary>
        /// 设备类别名称
        /// </summary>
        public string EquipmentTypeName { get; set; }

        /// <summary>
        /// 设备类别编号
        /// </summary>
        public string EquipmentTypeCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastUpdatorUserId { get; set; }
    }
}
