using OSharp.Data.Entity;
using Solution.EnterpriseInformation.Models;
using System;

namespace Solution.EnterpriseInformation.ModelConfigurations
{
    public class EntAreaInfoConfigration : EntityConfigurationBase<EntAreaInfo, Guid>
    {
        public EntAreaInfoConfigration()
        {
            HasRequired(m => m.EntSite).WithMany();
        }
    }
}
