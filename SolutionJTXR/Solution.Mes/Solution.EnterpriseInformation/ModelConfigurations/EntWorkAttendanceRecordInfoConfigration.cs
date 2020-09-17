using OSharp.Data.Entity;
using Solution.EnterpriseInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.ModelConfigurations
{
    public class EntWorkAttendanceRecordInfoConfigration : EntityConfigurationBase<EntWorkAttendanceRecordInfo, Guid>
    {
        public EntWorkAttendanceRecordInfoConfigration()
        {
            HasRequired(m => m.EntEmployeeInfo).WithMany();
        }
    }
}
