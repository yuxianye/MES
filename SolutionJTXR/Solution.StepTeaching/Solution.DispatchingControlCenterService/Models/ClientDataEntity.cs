using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.DispatchingControlCenterService
{
    /// <summary>
    /// 通讯数据传输模型
    /// </summary>
    public class ClientDataEntity : Utility.Disposable
    {

        public FuncCode FunctionCode { get; set; }

        public Guid ProductionProcessEquipmentBusinessNodeMapId { get; set; }

        public uint StatusCode { get; set; }

        public string NodeId { get; set; }

        public object Value { get; set; }

        public Type ValueType { get; set; }

        public string Message { get; set; }

        protected override void Disposing()
        {

        }

    }

    /// <summary>
    /// 功能码
    /// </summary>
    public enum FuncCode
    {
        /// <summary>
        /// None
        /// </summary>
        Nono = 0,

        /// <summary>
        /// 读
        /// </summary>
        Read = 1,

        /// <summary>
        /// 写
        /// </summary>
        Write = 2,

        /// <summary>
        /// 订阅
        /// </summary>
        SubScription = 3
    }


}
