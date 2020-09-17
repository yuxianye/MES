using OSharp.Data.Entity;
using OSharp.Data.Entity.Migrations;


namespace Solution.Core.Data
{
    public class CreateDatabaseIfNotExistsWithSeed : CreateDatabaseIfNotExistsWithSeedBase<DefaultDbContext>
    {
        public CreateDatabaseIfNotExistsWithSeed()
        {
            SeedActions.Add(new CreateDatabaseSeedAction());
        }
    }
}
