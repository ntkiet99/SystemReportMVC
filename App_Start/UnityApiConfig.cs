using SystemReportMVC.Interfaces;
using SystemReportMVC.Services;
using Unity;

namespace SystemReportMVC.App_Start
{
    public class UnityApiConfig
    {
        public static UnityContainer Container()
        {
            var container = new UnityContainer();
            container.RegisterType<IFileService, FileService>();
            return container;
        }
    }
}