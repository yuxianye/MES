using System;

namespace Solution.Device.Core
{
    /// <summary>
    /// 设备操作输出(返回)参数实体基类
    /// </summary>
    public class DeviceOutputParamEntityBase : DeviceInputParamEntityBase
    {
        /// <summary>
        /// 状态码，可使用枚举或者数字，通常包含底层通讯状态码，并可以扩展自定义的状态码（注意不要与底层设备重复）
        /// </summary>
        public uint StatusCode { get; set; }

        /// <summary>
        /// 旧值
        /// </summary>
        public object OldVaule { get; set; }

        protected override void Disposing()
        {
            this.StatusCode = 0;
        }
    }
}
