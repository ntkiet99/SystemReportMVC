using System.Web.Mvc;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Services;
using Unity;
using Unity.Mvc5;

namespace SystemReportMVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IDonViService, DonViService>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IAuditLogService, AuditLogService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}