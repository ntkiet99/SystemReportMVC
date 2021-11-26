using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SystemReportMVC.Helpers;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;

namespace SystemReportMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        public ActionResult Index(string error = null)
        {
            var session = (AppUser)Session[Constants.USER_SESSION];
            if (session == null)
            {
                if (error != null)
                    ViewBag.Error = error;
                return View();
            }
            else
                return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult SignIn(NguoiDung model)
        {
            try
            {
                NguoiDung user = _userService.SignIn(model);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.TaiKhoan, true);
                    var userSession = new AppUser();
                    userSession.TaiKhoan = user.TaiKhoan;
                    userSession.Id = user.Id;
                    userSession.HoTen = user.HoTen;
                    Session.Add(Constants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "NguoiDung");
                }
            }
            catch (System.Exception)
            {
            }

            return RedirectToAction("Index", "Auth", new { error = "Tài khoản hoặc mật khẩu không chính xác!" });
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index", "Auth");
        }

        public ActionResult SideBar()
        {
            var url = Request.RequestContext.HttpContext.Request.RawUrl;
            var session = (AppUser)Session[Constants.USER_SESSION];
            if (session == null)
                return RedirectToAction("Index", "Auth");
            var menus = _userService.GetMenusByUserId(session.Id, url);
            if (menus == null)
                SignOut();
            var menusCha = menus.ToList();
            foreach (var cha in menusCha)
            {
                if (cha.MenuCons.Count() > 0)
                {
                    var menucon = cha.MenuCons.Where(x => x.Active).FirstOrDefault();
                    if (menucon != null)
                        cha.Active = true;
                }
            }
            var checkMenuActive = menusCha.Where(x => x.Active).FirstOrDefault();
            if (checkMenuActive == null)
                menusCha.FirstOrDefault().Active = true;
            return View(menusCha);
        }
    }
}