using System;
using System.ComponentModel.DataAnnotations;

using OSharp.Core.Data;

namespace Solution.Core.Dtos.Infos
{
   public class SysDicInfoInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// 字典编号
        /// </summary>
        [StringLength(50)]
        public string DicCode { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        [StringLength(100)]
        public string DicName { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public Guid? DicParentID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DicLevel { get; set; }

        /// <summary>
        /// 字典类型(0:明细，1:类别)
        /// </summary>
        public bool DicType { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>

        [StringLength(50)]
        public string DicValue { get; set; }
        /// <summary>
        /// 字典设定值
        /// </summary>
        [StringLength(200)]
        public string DicSetValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>

        [StringLength(200)]
        public string Remark { get; set; }
        /// <summary>
        /// 字典ID
        /// </summary>
        public Guid Id { get; set; }
    }
}
