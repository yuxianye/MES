using System;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;

using OSharp.Core.Data;
using OSharp.Core.Dependency;
using OSharp.Core.Security;
using Solution.Core.Contracts;
using Solution.Core.Models.Identity;
using OSharp.Utility.Extensions;
using OSharp.Web.Http.Extensions;
using OSharp.Web.Http.Filters;
using OSharp.Web.Http.Logging;
using Solution.Core.Identity;
using OSharp.Utility.Data;
using Solution.Core.Services;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 工业机器人实习/实训系统 webapi
    /// </summary>
    public class RobotTrainingController : ApiController
    {

        [HttpPost]
        [Description("服务-工业机器人实习/实训系统-打开数控仿真宇龙软件")]
        public IHttpActionResult OpenSimulationSoftware()
        {
            System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Kepware\\KEPServerEX 6\\" + "server_config.exe");
            return Ok(new { msg = "open SimulationSoftware" });
        }
    }
}
