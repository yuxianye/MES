using System.Reflection;

namespace Solution.Device.Core
{
    /// <summary>
    /// 状态码TODO
    /// </summary>
    public enum DeviceStatusCode : uint
    {
        None = 0,
        ConnOk = 1, //连接成功
        ConnFault = 2, //连接失败
        ReConnOk = 3, //重连成功
        ReConnFault = 4, //重连失败
        DisConnOk = 5, //断开成功
        DisConnFault = 6, //断开失败
        ConnClosed = 7, //连接关闭
        ReadOk = 8, //读取成功
        ReadFault = 9, //读取失败
        WriteOk = 10,//写入成功
        WriteFault = 11,//写入失败
        SubscriptionOK = 12,//订阅成功
        SubscriptionFault = 13 //订阅失败
    }

    public enum FuncCode
    {
        Read = 0, //读
        Write = 1, //写
        SubScription = 2  //订阅
    }





}
