using Solution.Core.Contracts;
using System.ComponentModel;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service.Controllers
{
    /// <summary>
    /// 教学管理系统 webapi
    /// </summary>
    public class TeachingManageController : ApiController
    {
        public IIdentityContract IdentityContract { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("服务-教学系统-打开CAD软件")]
        public IHttpActionResult OpenCAD()
        {
            System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Kepware\\KEPServerEX 6\\" + "server_config.exe");
            return Ok(new { msg = "open CAD" });
        }

    }
}
