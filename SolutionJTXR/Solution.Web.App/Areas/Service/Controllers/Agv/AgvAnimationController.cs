using Microsoft.AspNet.Identity;
using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility.Data;
using Solution.Agv.Contracts;
using Solution.Agv.Models;
using Solution.Core.Models.Identity;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    [Description("服务-2D调度看板")]
    public class AgvAnimationController : ServiceBaseApiController
    {


        [Description("服务-2D调度看板")]
        public IHttpActionResult Animation()
        {
            return null;
        }

    }
    [Description("服务-3D调度看板")]
    public class AgvAnimation3DController : ServiceBaseApiController
    {
        [Description("服务-3D调度看板")]
        public IHttpActionResult Animation()
        {
            return null;
        }
    }

    [Description("服务-大连调度看板")]
    public class AgvAnimationDalianController : ServiceBaseApiController
    {
        [Description("服务-大连调度看板")]
        public IHttpActionResult Animation()
        {
            return null;
        }
    }

}
