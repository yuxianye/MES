using OSharp.Data.Entity;
using Solution.EnterpriseInformation.Models;
using System;

namespace Solution.EnterpriseInformation.ModelConfigurations
{
    public class EntSiteInfoConfigration : EntityConfigurationBase<EntSiteInfo, Guid>
    {
        public EntSiteInfoConfigration()
        {
            HasRequired(m => m.Enterprise).WithMany();
        }
    }
}
