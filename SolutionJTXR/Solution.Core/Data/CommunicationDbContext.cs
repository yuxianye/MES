using OSharp.Data.Entity;

namespace Solution.Core.Data
{
    public class CommunicationDbContext : DbContextBase<CommunicationDbContext>
    {
        /// <summary>
        /// 初始化一个<see cref="CommunicationDbContext"/>类型的新实例
        /// </summary>
        public CommunicationDbContext()
        {

        }

        /// <summary>
        /// 初始化一个<see cref="CommunicationDbContext"/>类型的新实例
        /// </summary>
        public CommunicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }
    }
}
