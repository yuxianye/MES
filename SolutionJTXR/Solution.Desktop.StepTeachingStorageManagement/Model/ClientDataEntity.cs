using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.StepTeachingStorageManagement.Model
{
    /// <summary>
    /// 通讯数据传输模型
    /// </summary>
    public class ClientDataEntity : ModelBase
    {
        //#region Id
        //private Guid id;
        ///// <summary>
        ///// Id
        ///// </summary>
        //public Guid Id
        //{
        //    get { return id; }
        //    set { Set(ref id, value); }
        //}
        //#endregion

        #region 功能码
        private FuncCode functionCode;

        /// <summary>
        /// 功能码
        /// </summary>
        public FuncCode FunctionCode
        {
            get { return functionCode; }
            set { Set(ref functionCode, value); }
        }
        #endregion

        #region 工序设备业务业务ID
        private Guid productionProcessEquipmentBusinessNodeMapId;

        /// <summary>
        /// 工序设备业务业务ID
        /// </summary>
        public Guid ProductionProcessEquipmentBusinessNodeMapId
        {
            get { return productionProcessEquipmentBusinessNodeMapId; }
            set { Set(ref productionProcessEquipmentBusinessNodeMapId, value); }
        }
        #endregion

        #region 状态码
        private uint statusCode;

        /// <summary>
        /// 状态码，可使用枚举或者数字，通常包含底层通讯状态码，并可以扩展自定义的状态码（注意不要与底层设备重复）
        /// </summary>
        public uint StatusCode
        {
            get { return statusCode; }
            set { Set(ref statusCode, value); }
        }
        #endregion

        #region 节点名称、编号
        private string nodeId;

        /// <summary>
        /// 节点名称、编号
        /// </summary>
        public string NodeId
        {
            get { return nodeId; }
            set { Set(ref nodeId, value); }
        }
        #endregion
        #region 业务名称

        private string _businessName;

        public string BusinessName
        {
            get { return _businessName; }
            set { Set(ref _businessName, value); }
        }
        #endregion
        #region 业务描述

        private string _description;

        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }
        #endregion
        #region 值
        private object _value;

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }
        #endregion

        #region 旧值
        private object _oldValue;

        /// <summary>
        /// 旧值
        /// </summary>
        public object OldVaule
        {
            get { return _oldValue; }
            set { Set(ref _oldValue, value); }
        }
        #endregion
        #region 数值的类型
        private Type valueType;

        /// <summary>
        /// 数值的类型
        /// </summary>
        public Type ValueType
        {
            get { return valueType; }
            set { Set(ref valueType, value); }
        }
        #endregion

        #region 消息
        private string message;

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get { return message; }
            set { Set(ref message, value); }
        }
        #endregion

        protected override void Disposing()
        {

        }



    }

    public enum FuncCode
    {
        Nono = 0,
        Read = 1, //读
        Write = 2, //写
        SubScription = 3  //订阅
    }


}
