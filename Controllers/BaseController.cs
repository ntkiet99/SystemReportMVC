using System.Web.Mvc;
using SystemReportMVC.Helpers;

namespace SystemReportMVC.Controllers
{
    public class BaseController : Controller
    {
        public static AppUser AppUser { get; set; }
        public BaseController()
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AppUser = (AppUser)Session[Constants.USER_SESSION];
            if (AppUser == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Auth", action = "Index" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}