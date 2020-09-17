using OSharp.Data.Entity;
using Solution.EnterpriseInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.ModelConfigurations
{
    public class EntEmployeeInfoConfigration : EntityConfigurationBase<EntEmployeeInfo, Guid>
    {
        public EntEmployeeInfoConfigration()
        {
            HasRequired(m => m.DepartmentInfo).WithMany();
        }
    }
}
