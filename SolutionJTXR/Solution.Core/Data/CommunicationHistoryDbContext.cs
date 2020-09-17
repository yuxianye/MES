using OSharp.Data.Entity;

namespace Solution.Core.Data
{
    public class CommunicationHistoryDbContext : DbContextBase<CommunicationHistoryDbContext>
    {
        /// <summary>
        /// 初始化一个<see cref="CommunicationHistoryDbContext"/>类型的新实例
        /// </summary>
        public CommunicationHistoryDbContext()
        {

        }

        /// <summary>
        /// 初始化一个<see cref="CommunicationHistoryDbContext"/>类型的新实例
        /// </summary>
        public CommunicationHistoryDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }
    }
}
