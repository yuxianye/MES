using OSharp.Core.Dependency;
using System.Threading.Tasks;

namespace Solution.CommunicationService.Contracts
{
    public interface ICommunicationContract : ISingletonDependency
    {
        /// <summary>
        /// 通信初始化工作
        /// </summary>
        /// <returns></returns>
        void Initialize();

        /// <summary>
        /// 开启Socket服务
        /// </summary>
        void StartServer();

        /// <summary>
        /// 关闭Socket服务
        /// </summary>
        void StopServer();

        /// <summary>
        /// 重启服务
        /// </summary>
        void RestartServer();


        //void Read();

        //void Send();

    }
}