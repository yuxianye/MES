using OSharp.Data.Entity;
using Solution.EnterpriseInformation.Models;
using System;

namespace Solution.EnterpriseInformation.ModelConfigurations
{
    public class EntTeamMapInfoConfigration : EntityConfigurationBase<EntTeamMapInfo, Guid>
    {
        public EntTeamMapInfoConfigration()
        {
            HasRequired(m => m.EntTeamInfo).WithMany();
            HasRequired(m => m.EntEmployeeInfo).WithMany();
        }
    }
}
