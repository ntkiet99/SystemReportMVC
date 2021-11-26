using System.Web.Mvc;
using SystemReportMVC.Interfaces;

namespace SystemReportMVC.Controllers
{
    public class AuditLogController : BaseController
    {
        private readonly IAuditLogService _auditLogService;
        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }
        public ActionResult Index()
        {
            var data = _auditLogService.GetList();
            return View(data);
        }
    }
}