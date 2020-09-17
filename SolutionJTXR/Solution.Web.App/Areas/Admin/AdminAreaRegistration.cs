using System.Web.Mvc;

using OSharp.Web.Mvc.Routing;


namespace Solution.Web.App.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowerCaseUrlRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller="Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Solution.Web.App.Areas.Admin.Controllers" }
                );
        }
    }
}